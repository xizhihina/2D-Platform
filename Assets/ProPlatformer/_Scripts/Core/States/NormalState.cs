using System;
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
            ctx.HopWaitX = 0;
        }

        public override EActionState Update(float deltaTime)
        {
            //Dashing
            if (ctx.CanDash)
            {
                return ctx.Dash();
            }

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
            }

            return state;
        }
    }


}
