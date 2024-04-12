namespace Myd.Platform
{
    //这里涉及坐标的数值需要/10, 除时间类型
    public static class Constants
    {

        public static bool EnableWallSlide = true;
        public static bool EnableJumpGrace = true;
        public static bool EnableWallBoost = true;

        public static float Gravity = 90f; //重力

        public static float HalfGravThreshold = 4f; //滞空时间阈值
        public static float MaxFall = -16; //普通最大下落速度
        public static float FastMaxFall = -24f;  //快速最大下落速度
        public static float FastMaxAccel = 30f; //快速下落加速度
        //最大移动速度
        public static float MaxRun = 9f;
        //Hold情况下的最大移动速度
        public static float HoldingMaxRun = 7f;
        //空气阻力
        public static float AirMulti = 0.65f;
        //移动加速度
        public static float RunAccel = 100f;
        //移动减速度
        public static float RunReduce = 40f;
        //
        public static float JumpSpeed = 10.5f;  //最大跳跃速度
        public static float VarJumpTime = 0.2f; //跳跃持续时间(跳起时,会持续响应跳跃按键[VarJumpTime]秒,影响跳跃的最高高度);
        public static float JumpHBoost = 4f; //退离墙壁的力
        public static float JumpGraceTime = 0.1f;//土狼时间

        #region Dash相关参数
        public static float DashSpeed = 24f;           //冲刺速度
        public static float EndDashSpeed = 16f;        //结束冲刺速度
        public static float EndDashUpMulti = .75f;       //如果向上冲刺，阻力。
        public static float DashTime = .15f;            //冲刺时间
        public static float DashCooldown = .2f;         //冲刺冷却时间，
        public static float DashRefillCooldown = .1f;   //冲刺重新装填时间
        public static int DashHJumpThruNudge = 6;       //
        public static int DashCornerCorrection = 4;     //水平Dash时，遇到阻挡物的可纠正像素值
        public static int DashVFloorSnapDist = 3;       //DashAttacking下的地面吸附像素值
        public static float DashAttackTime = .3f;       //
        public static int MaxDashes = 1;
        #endregion

        #region Duck参数
        public static float DuckFriction = 50f;
        public static float DuckSuperJumpXMulti = 1.25f;
        public static float DuckSuperJumpYMulti = .5f;
        #endregion

        #region Corner Correct
        public static int UpwardCornerCorrection = 4; //向上移动，X轴上边缘校正的最大距离
        #endregion

        public static float LaunchedMinSpeedSq = 196;
    }
}
