﻿using System;
using System.Collections;
using UnityEngine;

namespace Myd.Platform
{
    public class NormalState : BaseActionState
    {
        public NormalState(PlayerController controller):base(EActionState.Normal, controller)//设置state和ctx初值
        {
        }
        
        public override IEnumerator Coroutine()
        {
            throw new NotImplementedException();
        }
        
        public override bool IsCoroutine()
        {
            return false;
        }
        
        public override void OnBegin()
        {
            ctx.MaxFall = Constants.MaxFall;
        }

        public override void OnEnd()
        {
            ctx.WallBoost?.ResetTime();
            ctx.WallSpeedRetentionTimer = 0;
            ctx.HopWaitX = 0;
        }

        public override EActionState Update(float deltaTime)
        {
            //Climb
            if (GameInput.Grab.Checked() && !ctx.Ducking)
            {
                //Climbing
                if (ctx.Speed.y <= 0 && Math.Sign(ctx.Speed.x) != -(int)ctx.Facing)
                {
                    if (ctx.ClimbCheck((int)ctx.Facing))
                    {
                        ctx.Ducking = false;
                        return EActionState.Climb;
                    }
                    //非下坠情况，需要考虑向上攀爬吸附
                    if (ctx.MoveY > -1)
                    {
                        bool snapped = ctx.ClimbUpSnap();
                        if (snapped)
                        {
                            ctx.Ducking = false;
                            return EActionState.Climb;
                        }
                    }
                }
            }

            //Dashing
            if (ctx.CanDash)
            {
                return ctx.Dash();
            }

            //Ducking
            if (ctx.Ducking)
            {
                if (ctx.OnGround && ctx.MoveY != -1)
                {
                    if (ctx.CanUnDuck)
                    {
                        ctx.Ducking = false;
                    }
                    else if (ctx.Speed.x == 0)
                    {
                        //根据角落位置，进行挤出操作
                    }
                }
            }
            else if (ctx.OnGround && ctx.MoveY == -1 && ctx.Speed.y <= 0)
            {
                ctx.Ducking = true;
                ctx.PlayDuck(true);
            }

            //水平面上移动,计算阻力
            if (ctx.Ducking && ctx.OnGround)
            {
                ctx.Speed.x = Mathf.MoveTowards(ctx.Speed.x, 0, Constants.DuckFriction * deltaTime);
            }
            else
            {
                float mult = ctx.OnGround ? 1 : Constants.AirMulti;
                //计算水平速度
                float max = ctx.Holding == null ? Constants.MaxRun : Constants.HoldingMaxRun;
                if (Math.Abs(ctx.Speed.x) > max && Math.Sign(ctx.Speed.x) == ctx.MoveX)
                {
                    //同方向加速
                    ctx.Speed.x = Mathf.MoveTowards(ctx.Speed.x, max * ctx.MoveX, Constants.RunReduce * mult * Time.deltaTime);
                }
                else
                {
                    //反方向减速
                    ctx.Speed.x = Mathf.MoveTowards(ctx.Speed.x, max * ctx.MoveX, Constants.RunAccel * mult * Time.deltaTime);
                }
            }
            //计算竖直速度
            {
                //计算最大下落速度
                {
                    float maxFallSpeed = Constants.MaxFall;
                    float fastMaxFallSpeed = Constants.FastMaxFall;

                    if (ctx.MoveY == -1 && ctx.Speed.y <= maxFallSpeed)
                    {
                        ctx.MaxFall = Mathf.MoveTowards(ctx.MaxFall, fastMaxFallSpeed, Constants.FastMaxAccel * deltaTime);

                        //处理表现
                        ctx.PlayFallEffect(ctx.Speed.y);
                    }
                    else
                    {
                        ctx.MaxFall = Mathf.MoveTowards(ctx.MaxFall, maxFallSpeed, Constants.FastMaxAccel * deltaTime);
                    }
                }

                if (!ctx.OnGround)
                {
                    float max = ctx.MaxFall;//最大下落速度
                    //Wall Slide
                    if ((ctx.MoveX == (int)ctx.Facing || (ctx.MoveX == 0 && GameInput.Grab.Checked())) && ctx.MoveY != -1)
                    {
                        //判断是否向下做Wall滑行
                        if (ctx.Speed.y <= 0 && ctx.WallSlideTimer > 0 && ctx.ClimbBoundsCheck((int)ctx.Facing) && ctx.CollideCheck(ctx.Position, Vector2.right * (int)ctx.Facing) && ctx.CanUnDuck)
                        {
                            ctx.Ducking = false;
                            ctx.WallSlideDir = (int)ctx.Facing;
                        }

                        if (ctx.WallSlideDir != 0)
                        {
                            //if (ctx.WallSlideTimer > Constants.WallSlideTime * 0.5f && ClimbBlocker.Check(level, this, Position + Vector2.UnitX * wallSlideDir))
                            //    ctx.WallSlideTimer = Constants.WallSlideTime * .5f;

                            max = Mathf.Lerp(Constants.MaxFall, Constants.WallSlideStartMax, ctx.WallSlideTimer / Constants.WallSlideTime);
                            if ((ctx.WallSlideTimer / Constants.WallSlideTime) > .65f)
                            {
                                //播放滑行特效
                                ctx.PlayWallSlideEffect(Vector2.right * ctx.WallSlideDir);
                            }
                        }
                    }

                    float mult = (Math.Abs(ctx.Speed.y) < Constants.HalfGravThreshold && (GameInput.Jump.Checked())) ? .5f : 1f;
                    //空中的情况,需要计算Y轴速度
                    ctx.Speed.y = Mathf.MoveTowards(ctx.Speed.y, max, Constants.Gravity * mult * deltaTime);
                }

                //处理跳跃
                if (ctx.VarJumpTimer > 0)
                {
                    if (GameInput.Jump.Checked())
                    {
                        //如果按住跳跃，则跳跃速度不受重力影响。
                        ctx.Speed.y = Math.Max(ctx.Speed.y, ctx.VarJumpSpeed);
                    }
                    else
                        ctx.VarJumpTimer = 0;
                }
            }

            if (GameInput.Jump.Pressed())
            {
                //土狼时间范围内,允许跳跃
                if (ctx.JumpCheck.AllowJump())
                {
                    ctx.Jump();
                }
                else if (ctx.CanUnDuck)
                {
                    //如果右侧有墙
                    if (ctx.WallJumpCheck(1))
                    {
                        if (ctx.Facing == Facings.Right && GameInput.Grab.Checked())
                            ctx.ClimbJump();
                        else
                            ctx.WallJump(-1);
                    }
                    //如果左侧有墙
                    else if (ctx.WallJumpCheck(-1))
                    {
                        if (ctx.Facing == Facings.Left && GameInput.Grab.Checked())
                            ctx.ClimbJump();
                        else
                            ctx.WallJump(1);
                    }
                }
            }

            return state;
        }
    }


}
