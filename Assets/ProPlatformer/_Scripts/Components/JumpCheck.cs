﻿namespace Myd.Platform
{
    /// <summary>
    /// 跳跃检查组件
    /// </summary>
    public class JumpCheck
    {
        private float timer;

        private PlayerController controller;
        public float Timer => timer;
        private bool jumpGrace;
        public JumpCheck(PlayerController playerController, bool jumpGrace)
        {
            controller = playerController;
            ResetTime();
            this.jumpGrace = jumpGrace;
        }

        public void ResetTime()
        {
            timer = 0;
        }

        public void Update(float deltaTime)
        {
            //Jump Grace
            if (controller.OnGround)
            {
                //dreamJump = false;
                timer = Constants.JumpGraceTime;
            }
            else
            {
                if (timer > 0)
                {
                    timer -= deltaTime;
                }
            }
        }

        public bool AllowJump()
        {
            return jumpGrace ? timer > 0 : controller.OnGround;
        }
    }
}