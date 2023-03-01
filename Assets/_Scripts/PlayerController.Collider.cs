﻿using Myd.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Myd.Platform.Demo
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

        private readonly Rect normalHitbox = new Rect(0, -0.25f, 0.8f, 1.1f);
        private readonly Rect duckHitbox = new Rect(0, -0.5f, 0.8f, 0.6f);
        private readonly Rect normalHurtbox = new Rect(0f, -0.15f, 0.8f, 0.9f);
        private readonly Rect duckHurtbox = new Rect(8f, 4f, 0.8f, 0.4f);

        private Rect collider;

        //碰撞检测
        public bool CollideCheck(Vector2 position, Vector2 dir, float dist = 0)
        {
            Vector2 origion = this.Position + collider.position;
            return Physics2D.OverlapBox(origion + dir * (dist + DEVIATION), collider.size, 0, GroundMask);
        }

        public bool OverlapPoint(Vector2 position)
        {
            return Physics2D.OverlapPoint(position, GroundMask);
        }

        //攀爬检查
        public bool ClimbCheck(int dir, int yAdd = 0)
        {
            //检查在关卡范围内
            //if (!this.ClimbBoundsCheck(dir))
            //    return false;

            //且前面两个单元没有ClimbBlock
            //if (ClimbBlocker.Check(base.Scene, this, this.Position + Vector2.UnitY * (float)yAdd + Vector2.UnitX * 2f * (float)this.Facing))
            //    return false;

            //获取当前的碰撞体
            if (Physics2D.OverlapBox(this.Position + Vector2.up * (float)yAdd + Vector2.right * dir * DEVIATION , collider.size, 0, GroundMask))
            {
                return true;
            }
            return false;
        }

        //根据碰撞调整X轴上的最终移动距离
        protected void UpdateCollideX(float distX)
        {
            if (distX == 0)
                return;
            //目标位置
            Vector2 direct = Math.Sign(distX) > 0 ? Vector2.right : Vector2.left;
            Vector2 targetPosition = this.Position;

            Vector2 origion = this.Position + collider.position;

            RaycastHit2D hit = Physics2D.BoxCast(origion, collider.size, 0, direct, Mathf.Abs(distX) + DEVIATION, GroundMask);
            if (hit)
            {
                //如果发生碰撞,则移动距离
                targetPosition += direct * (hit.distance - DEVIATION);
                //Speed retention
                //if (wallSpeedRetentionTimer <= 0)
                //{
                //    wallSpeedRetained = this.speed.x;
                //    wallSpeedRetentionTimer = Constants.WallSpeedRetentionTime;
                //}
                this.Speed.x = 0;
            }
            else
            {
                targetPosition += Vector2.right * distX;
            }
            this.Position = targetPosition;
        }

        protected void UpdateCollideY(float distY)
        {
            Vector2 targetPosition = this.Position;
            //使用校正
            float distance = distY;
            int correctTimes = 10; // int correctTimes = controllerParams.UseCornerCorrection ? 10 : 0;  //默认可以迭代位置10次
            bool collided = true;
            float speedY = Mathf.Abs(this.Speed.y);
            while (true)
            {
                float moved = MoveYStepWithCollide(distance);
                //无碰撞退出循环
                this.Position += Vector2.up * moved;
                if (moved == distance || correctTimes == 0) //无碰撞，且校正次数为0
                {
                    collided = false;
                    break;
                }
                float tempDist = distance - moved;
                correctTimes--;
                if (!Correct(tempDist))
                {
                    this.Speed.y = 0;//校正失败，则速度清零
                    break;
                }
                distance = tempDist;
            }

            //落地时候，进行缩放
            if (collided && distY < 0)
            {
                if (this.stateMachine.State != (int)EActionState.Climb)
                {
                    EventManager.Get().FireOnFallLand(speedY);
                }
            }
        }

        //针对横向,进行碰撞检测.如果发生碰撞,
        private bool CheckGround()
        {
            Vector2 origion = this.Position + collider.position;
            Collider2D hit = Physics2D.OverlapBox(origion + Vector2.down * DEVIATION, collider.size, 0, GroundMask);
            if (hit)
            {
                return true;
            }
            return false;
        }

        //根据整个关卡的边缘框进行检测,确保人物在关卡的框内.
        public bool ClimbBoundsCheck(int dir)
        {
            return true;
            //return base.Left + (float)(dir * 2) >= (float)this.level.Bounds.Left && base.Right + (float)(dir * 2) < (float)this.level.Bounds.Right;
        }

        //墙壁上跳检测
        public bool WallJumpCheck(int dir)
        {
            return ClimbBoundsCheck(dir) && this.CollideCheck(Position, Vector2.right * dir, Constants.WallJumpCheckDist);
        }

        public RaycastHit2D ClimbHopSolid { get; set; }
        public RaycastHit2D CollideClimbHop(int dir)
        {
            Vector2 origion = this.Position + collider.position;
            RaycastHit2D hit = Physics2D.BoxCast(Position, collider.size, 0, Vector2.right * dir, DEVIATION, GroundMask);
            return hit;
            //if (hit && hit.normal.x == -dir)
            //{

            //}
        }

        public bool SlipCheck(float addY = 0)
        {
            int direct = Facing == Facings.Right ? 1 : -1;
            Vector2 origin = this.Position + collider.position + Vector2.up * collider.size.y / 2f + Vector2.right * direct * (collider.size.x / 2f + STEP);
            Vector2 point1 = origin + Vector2.up * (-0.4f + addY);

            if(Physics2D.OverlapPoint(point1, GroundMask))
            {
                return false;
            }
            Vector2 point2 = origin + Vector2.up * (0.4f + addY);
            if (Physics2D.OverlapPoint(point2, GroundMask))
            {
                return false;
            }
            return true;
        }

        public bool ClimbHopBlockedCheck()
        {
            return false;
        }

        //单步移动，参数和返回值都带方向，表示Y轴
        private float MoveYStepWithCollide(float distY)
        {
            Vector2 moved = Vector2.zero;
            Vector2 direct = Math.Sign(distY) > 0 ? Vector2.up : Vector2.down;
            Vector2 origion = this.Position + collider.position;
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

        private bool Correct(float distY)
        {
            Vector2 origion = this.Position + collider.position;
            Vector2 direct = Math.Sign(distY) > 0 ? Vector2.up : Vector2.down;
            //向上移动
            if (this.Speed.y > 0)
            {
                //Corner Correction
                {
                    if (this.Speed.x <= 0)
                    {
                        for (int i = 1; i <= Constants.UpwardCornerCorrection; i++)
                        {
                            RaycastHit2D hit = Physics2D.BoxCast(origion + new Vector2(-i * 0.1f, 0), collider.size, 0, direct, Mathf.Abs(distY) + DEVIATION, GroundMask);
                            if (!hit)
                            {
                                this.Position += new Vector2(-i * 0.1f, 0);
                                return true;
                            }
                        }
                    }

                    if (this.Speed.x >= 0)
                    {
                        for (int i = 1; i <= Constants.UpwardCornerCorrection; i++)
                        {
                            RaycastHit2D hit = Physics2D.BoxCast(origion + new Vector2(i * 0.1f, 0), collider.size, 0, direct, Mathf.Abs(distY) + DEVIATION, GroundMask);
                            if (!hit)
                            {
                                this.Position += new Vector2(i * 0.1f, 0);
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
