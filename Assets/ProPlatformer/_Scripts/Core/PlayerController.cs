using System;
using Myd.Common;
using UnityEngine;

namespace Myd.Platform
{
    /// <summary>
    /// 玩家操作控制器
    /// </summary>
    public partial class PlayerController
    {
        private readonly int GroundMask;

        float varJumpTimer;
        float varJumpSpeed; //
        int moveX;
        private float maxFall;
        private float fastMaxFall;

        private float dashCooldownTimer;                //冲刺冷却时间计数器，为0时，可以再次冲刺
        private float dashRefillCooldownTimer;          //
        public int dashes;
        public int lastDashes;
        private float wallSpeedRetentionTimer; // If you hit a wall, start this timer. If coast is clear within this timer, retain h-speed
        private float wallSpeedRetained;

        private bool onGround;
        private bool wasOnGround;
        
        public bool DashStartedOnGround { get; set; }

        public int ForceMoveX { get; set; }
        public float ForceMoveXTimer { get; set; }

        public int HopWaitX;   // If you climb hop onto a moving solid, snap to beside it until you get above it
        public float HopWaitXSpeed;

        public bool launched;
        public float launchedTimer;
        public JumpCheck JumpCheck { get; set; }    //土狼时间
        private FiniteStateMachine<BaseActionState> stateMachine;

        public PlayerRenderer SpriteControl { get; private set; }
        //特效控制器
        public SceneEffectManager EffectControl { get; private set; } 
        // public MyCamera camera { get; private set; }
        public PlayerController(PlayerRenderer spriteControl, SceneEffectManager effectControl)
        {
            SpriteControl = spriteControl;
            EffectControl = effectControl;

            stateMachine = new FiniteStateMachine<BaseActionState>((int)EActionState.Size);
            stateMachine.AddState(new NormalState(this));
            stateMachine.AddState(new DashState(this));
            GroundMask = LayerMask.GetMask("Ground");

            Facing  = Facings.Right;
            LastAim = Vector2.right;
        }

        public void RefreshAbility()
        {
            JumpCheck = new JumpCheck(this, Constants.EnableJumpGrace);
        }

        public void Init(Bounds bounds, Vector2 startPosition)
        {
            //根据进入的方式,决定初始状态
            stateMachine.State = (int)EActionState.Normal;
            lastDashes = dashes = 1;
            Position = startPosition;
            collider = normalHitBox;

            SpriteControl.SetSpriteScale(NORMAL_SPRITE_SCALE);

            this.bounds = bounds;
            cameraPosition = CameraTarget;
            //TODO 初始化尾巴颜色
            //Color color = NormalHairColor;
            //Gradient gradient = new Gradient();
            //gradient.SetKeys(
            //    new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f) },
            //    new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 0.6f), new GradientAlphaKey(0, 1.0f) }
            //);

            //this.player.SetTrailColor(gradient);

        }

        public void Update(float deltaTime)
        {
            //更新各个组件中变量的状态
            {
                //Get ground
                wasOnGround = onGround;
                if (Speed.y <= 0)
                {
                    onGround = CheckGround();//碰撞检测地面
                }
                else
                {
                    onGround = false;
                }
                
                //跳跃检查
                JumpCheck?.Update(deltaTime);

                //Dash
                {
                    if (dashCooldownTimer > 0)
                        dashCooldownTimer -= deltaTime;
                    if (dashRefillCooldownTimer > 0)
                    {
                        dashRefillCooldownTimer -= deltaTime;
                    }
                    else if (onGround)
                    {
                        RefillDash();
                    }
                }

                //Var Jump
                if (varJumpTimer > 0)
                {
                    varJumpTimer -= deltaTime;
                }

                //Force Move X
                if (ForceMoveXTimer > 0)
                {
                    ForceMoveXTimer -= deltaTime;
                    moveX = ForceMoveX;
                }
                else
                {
                    //输入
                    moveX = Math.Sign(Input.GetAxisRaw("Horizontal"));//返回-1,0,1
                }

                //Facing
                if (moveX != 0 && stateMachine.State != (int)EActionState.Climb)
                {
                    Facing = (Facings)moveX;
                }
                //Aiming
                LastAim = GameInput.GetAimVector(Facing);

                //撞墙以后的速度保持，Wall Speed Retention，用于撞开
                if (wallSpeedRetentionTimer > 0)
                {
                    if (Math.Sign(Speed.x) == -Math.Sign(wallSpeedRetained))
                        wallSpeedRetentionTimer = 0;
                    else if (!CollideCheck(Position, Vector2.right * Math.Sign(wallSpeedRetained)))
                    {
                        Speed.x = wallSpeedRetained;
                        wallSpeedRetentionTimer = 0;
                    }
                    else
                        wallSpeedRetentionTimer -= deltaTime;
                }

                //Hop Wait X
                if (HopWaitX != 0)
                {
                    if (Math.Sign(Speed.x) == -HopWaitX || Speed.y < 0)
                        HopWaitX = 0;
                    else if (!CollideCheck(Position, Vector2.right * HopWaitX))
                    {
                        Speed.x = HopWaitXSpeed;
                        HopWaitX = 0;
                    }
                }

                //Launch Particles
                if (launched)
                {
                    var sq = Speed.SqrMagnitude();
                    if (sq < Constants.LaunchedMinSpeedSq)
                        launched = false;
                    else
                    {
                        var was = launchedTimer;
                        launchedTimer += deltaTime;

                        if (launchedTimer >= .5f)
                        {
                            launched = false;
                            launchedTimer = 0;
                        }
                        else if (Calc.OnInterval(launchedTimer, was, 0.15f))
                        {
                            EffectControl.SpeedRing(Position, Speed.normalized);
                        }
                    }
                }
                else
                    launchedTimer = 0;

            }

            //状态机更新逻辑
            stateMachine.Update(deltaTime);
            //更新位置
            UpdateCollideX(Speed.x * deltaTime);
            UpdateCollideY(Speed.y * deltaTime);

            UpdateHair(deltaTime);

            UpdateCamera(deltaTime);
        }

        //处理跳跃,跳跃时候，会给跳跃前方一个额外的速度
        public void Jump()
        {
            GameInput.Jump.ConsumeBuffer();
            JumpCheck?.ResetTime();
            varJumpTimer = Constants.VarJumpTime;
            Speed.x += Constants.JumpHBoost * moveX;
            Speed.y = Constants.JumpSpeed;
            //Speed += LiftBoost;
            varJumpSpeed = Speed.y;
            
            PlayJumpEffect(SpritePosition, Vector2.up);
        }
        
        //在墙边情况下的，跳跃。主要需要考虑当前跳跃朝向

        public void ClimbJump()
        {
            Jump();
        }
        
        public bool RefillDash()
        {
            if (dashes < Constants.MaxDashes)
            {
                dashes = Constants.MaxDashes;
                return true;
            }

            return false;
        }
        

        public bool CanDash => GameInput.Dash.Pressed() && dashCooldownTimer <= 0 && dashes > 0;

        public Vector2 Speed;

        public object Holding => null;

        public bool OnGround => onGround;

        public Vector2 Position { get; set; }
        //表示进入爬墙状态有0.1秒时间,不发生移动，为了让玩家看清发生了爬墙的动作
        public float ClimbNoMoveTimer { get; set; }
        public float VarJumpSpeed => varJumpSpeed;

        public float VarJumpTimer
        {
            get => varJumpTimer;
            set => varJumpTimer = value;
        }

        public int MoveX => moveX;
        public int MoveY => Math.Sign(Input.GetAxisRaw("Vertical"));

        public float MaxFall { get => maxFall; set => maxFall = value; }
        public float DashCooldownTimer { get => dashCooldownTimer; set => dashCooldownTimer = value; }
        public float DashRefillCooldownTimer { get => dashRefillCooldownTimer; set => dashRefillCooldownTimer = value; }
        public Vector2  LastAim { get; set; }
        public Facings Facing { get; set; }  //当前朝向
        public EActionState Dash()
        {
            //wasDashB = Dashes == 2;
            dashes = Math.Max(0, dashes - 1);
            GameInput.Dash.ConsumeBuffer();
            return EActionState.Dash;
        }
        public void SetState(int state)
        {
            stateMachine.State = state;
        }
        
    }

}