﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Myd.Platform.Demo
{
    public class DashState : BaseActionState
    {
        private Vector2 DashDir;
        private Vector2 beforeDashSpeed; 
        public DashState(PlayerController context) : base(EActionState.Dash, context)
        {
        }

        public override void OnBegin()
        {
            ctx.DashCooldownTimer = Constants.DashCooldown;
            beforeDashSpeed = ctx.Speed;
            ctx.Speed = Vector2.zero;
            DashDir = Vector2.zero;
        }

        public override void OnEnd()
        {
            //CallDashEvents();
        }

        public override EActionState Update(float deltaTime)
        {
            //Trail
            //Grab Holdables
            //Super Jump
            //Wall Super Jump
            //Wall Jump

            return state;
        }

        public override IEnumerator Coroutine()
        {
            yield return null;
            //
            var dir = ctx.LastAim;
            var newSpeed = dir * Constants.DashSpeed;
            if (Math.Sign(beforeDashSpeed.x) == Math.Sign(newSpeed.x) && Math.Abs(beforeDashSpeed.x) > Math.Abs(newSpeed.x))
                newSpeed.x = beforeDashSpeed.x;
            ctx.Speed = newSpeed;

            DashDir = dir; 

            yield return Constants.DashTime;

            this.ctx.SetState((int)EActionState.Normal);
        }

        public override bool IsCoroutine()
        {
            return true;
        }
    }
}