﻿using System;
using UnityEngine;

namespace Myd.Platform
{
    /// <summary>
    /// 记录PlayerController关于碰撞相关功能
    /// 
    /// //每个接触面之间留0.01精度的缝隙.
    /// </summary>
    public partial class PlayerController
    {
        const float STEP = 0.1f;  //碰撞检测步长，对POINT检测用
        const float DEVIATION = 0.02f;  //碰撞检测误差

        private readonly Rect normalHitBox = new(0, -0.25f, 0.8f, 1.1f);

        private Rect collider;


        //碰撞检测
        public Collider2D CollideCheck(Vector2 position, Vector2 dir, float dist = 0)
        {
            Vector2 origin = position + collider.position;
            // 如果当前检测位置有物体，直接返回当前检测位置的物体，之后再考虑误差（保证电梯场景下能检测到电梯）
            if (Physics2D.OverlapBox(origin + dir * dist, collider.size, 0, GroundMask))
            {
                return Physics2D.OverlapBox(origin + dir * dist, collider.size, 0, GroundMask);
            }
            //返回Collider2D 与该盒体重叠的碰撞体。GroundMask="Ground"
            return Physics2D.OverlapBox(origin + dir * (dist + DEVIATION), collider.size, 0, GroundMask);
        }

        //根据碰撞调整X轴上的最终移动距离
        protected void UpdateCollideX(float distX)
        {
            //使用校正
            float distance = distX;
            int correctTimes = 1;
            while (true)
            {
                float moved = MoveXStepWithCollide(distance);
                //无碰撞退出循环
                Position += Vector2.right * moved;
                if (moved == distance || correctTimes == 0) //无碰撞，且校正次数为0
                {
                    break;
                }
                float tempDist = distance - moved;
                correctTimes--;
                if (!CorrectX(tempDist))
                {
                    Speed.x = 0;//未完成校正，则速度清零

                    //Speed retention
                    if (wallSpeedRetentionTimer <= 0)
                    {
                        wallSpeedRetained = Speed.x;
                    }
                    break;
                }
                distance = tempDist;
            }
        }

        protected void UpdateCollideY(float distY)
        {
            //使用校正
            float distance = distY;
            int correctTimes = 1; //默认可以迭代位置10次
            bool collided = true;
            float speedY = Mathf.Abs(Speed.y);
            while (true)
            {
                float moved = MoveYStepWithCollide(distance);
                //无碰撞退出循环
                Position += Vector2.up * moved;
                if (moved == distance || correctTimes == 0) //无碰撞，且校正次数为0
                {
                    collided = false;
                    break;
                }
                float tempDist = distance - moved;
                correctTimes--;
                if (!CorrectY(tempDist))
                {
                    Speed.y = 0;//未完成校正，则速度清零
                    break;
                }
                distance = tempDist;
            }

            //落地时候，进行缩放
            if (collided && distY < 0)
            {
                if (stateMachine.State != (int)EActionState.Climb)
                {
                    PlayLandEffect(SpritePosition, speedY);
                }
            }
        }
        private bool CheckGround()
        {
            return CheckGround(Vector2.zero);
        }
        //针对横向,进行碰撞检测.如果发生碰撞,
        private bool CheckGround(Vector2 offset)
        {
            Vector2 origion = Position + collider.position + offset;
            RaycastHit2D hit = Physics2D.BoxCast(origion, collider.size, 0, Vector2.down, DEVIATION, GroundMask);
            if (hit && hit.normal == Vector2.up)
            {
                return true;
            }
            return false;
        }

        //单步移动，参数和返回值都带方向，表示Y轴
        private float MoveYStepWithCollide(float distY)
        {
            Vector2 moved = Vector2.zero;
            Vector2 direct = Math.Sign(distY) > 0 ? Vector2.up : Vector2.down;
            Vector2 origion = Position + collider.position;
            RaycastHit2D hit = Physics2D.BoxCast(origion, collider.size, 0, direct, Mathf.Abs(distY) + DEVIATION, GroundMask);
            if (hit && hit.normal == -direct)
            {
                //如果发生碰撞,则移动距离
                moved += direct * Mathf.Max((hit.distance - DEVIATION), 0);
            }
            else
            {
                moved += Vector2.up * distY;
            }
            return moved.y;
        }

        private float MoveXStepWithCollide(float distX)
        {
            Vector2 moved = Vector2.zero;
            Vector2 direct = Math.Sign(distX) > 0 ? Vector2.right : Vector2.left;
            Vector2 origion = Position + collider.position;
            RaycastHit2D hit = Physics2D.BoxCast(origion, collider.size, 0, direct, Mathf.Abs(distX) + DEVIATION, GroundMask);
            if (hit && hit.normal == -direct)
            {
                //如果发生碰撞,则移动距离
                moved += direct * Mathf.Max((hit.distance - DEVIATION), 0);
            }
            else
            {
                moved += Vector2.right * distX;
            }
            return moved.x;
        }

        private bool CorrectX(float distX)
        {
            Vector2 origion = Position + collider.position;
            Vector2 direct = Math.Sign(distX) > 0 ? Vector2.right : Vector2.left;

            if ((stateMachine.State == (int)EActionState.Dash))
            {
                if (Speed.y == 0 && Speed.x!=0)
                {
                    for (int i = 1; i <= Constants.DashCornerCorrection; i++)
                    {
                        for (int j = 1; j >= -1; j -= 2)
                        {
                            if (!CollideCheck(Position + new Vector2(0, j * i * 0.1f), direct, Mathf.Abs(distX)))
                            {
                                Position += new Vector2(distX, j * i * 0.1f);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private bool CorrectY(float distY)
        {
            Vector2 origion = Position + collider.position;
            Vector2 direct = Math.Sign(distY) > 0 ? Vector2.up : Vector2.down;
            
            if (Speed.y < 0)
            {
                if ((stateMachine.State == (int)EActionState.Dash) && !DashStartedOnGround)
                {
                    if (Speed.x <= 0)
                    {
                        for (int i = -1; i >= -Constants.DashCornerCorrection; i--)
                        {
                            float step = (Mathf.Abs(i * 0.1f) + DEVIATION);
                            
                            if (!CheckGround(new Vector2(-step, 0)))
                            {
                                Position += new Vector2(-step, distY);
                                return true;
                            }
                        }
                    }

                    if (Speed.x >= 0)
                    {
                        for (int i = 1; i <= Constants.DashCornerCorrection; i++)
                        {
                            float step = (Mathf.Abs(i * 0.1f) + DEVIATION);
                            if (!CheckGround(new Vector2(step, 0)))
                            {
                                Position += new Vector2(step, distY);
                                return true;
                            }
                        }
                    }
                }
            }
            //向上移动
            else if (Speed.y > 0)
            {
                //Y轴向上方向的Corner Correction
                {
                    if (Speed.x <= 0)
                    {
                        for (int i = 1; i <= Constants.UpwardCornerCorrection; i++)
                        {
                            RaycastHit2D hit = Physics2D.BoxCast(origion + new Vector2(-i * 0.1f, 0), collider.size, 0, direct, Mathf.Abs(distY) + DEVIATION, GroundMask);
                            if (!hit)
                            {
                                Position += new Vector2(-i * 0.1f, 0);
                                return true;
                            }
                        }
                    }

                    if (Speed.x >= 0)
                    {
                        for (int i = 1; i <= Constants.UpwardCornerCorrection; i++)
                        {
                            RaycastHit2D hit = Physics2D.BoxCast(origion + new Vector2(i * 0.1f, 0), collider.size, 0, direct, Mathf.Abs(distY) + DEVIATION, GroundMask);
                            if (!hit)
                            {
                                Position += new Vector2(i * 0.1f, 0);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
