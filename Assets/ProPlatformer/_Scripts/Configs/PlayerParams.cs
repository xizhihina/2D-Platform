using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Myd.Platform
{
    [CreateAssetMenu(fileName = "PlayerParams", menuName = "Pro Platformer/Player Param", order = 1 )]
    public class PlayerParams : ScriptableObject
    {
        [Header("启用功能【墙壁下滑】")]
        public bool EnableWallSlide;
        [Header("启用功能【土狼时间】")]
        public bool EnableJumpGrace;
        [Header("启用功能【WallBoost】")]
        [Tooltip("一个可以不消耗耐力的技巧")]
        public bool EnableWallBoost;

        [Header("水平方向参数")]
        [Tooltip("最大水平速度")]
        public int MaxRun;
        [Tooltip("水平方向加速度")]
        public int RunAccel;
        [Tooltip("水平方向减速度")]
        public int RunReduce = 40;
        [Space]
        [Header("竖直方向参数")]
        [Tooltip("重力加速度")]
        public float Gravity = 90f; //重力
        [Tooltip("当速度小于该阈值时，重力减半。值越小，滞空时间越长，0表示关闭")]
        [Range(0, 9)]
        public float HalfGravThreshold = 4f;
        [Tooltip("下落的最大速度（带方向，向上为正）")]
        public float MaxFall = -16; //普通最大下落速度
        [Tooltip("急速下落的最大速度（带方向，向上为正）")]
        public float FastMaxFall = -24f;  //快速最大下落速度
        [Tooltip("下落->急速下坠加速度")]
        public float FastMaxAccel = 30f; //快速下落加速度

        [Space]
        [Header("跳跃参数")]
        [Tooltip("最大跳跃速度")]
        public float JumpSpeed = 10.5f;
        [Tooltip("跳跃持续时间（跳起时,会持续响应跳跃按键[VarJumpTime]秒,影响跳跃的最高高度）")]
        public float VarJumpTime = 0.2f;
        [Tooltip("跳跃水平方向，水平方向减速度")]
        public float JumpHBoost = 4f;
        [Tooltip("土狼时间（离开平台时，还能响应跳跃的时间）")]
        public float JumpGraceTime = 0.1f;
        [Tooltip("向上运动遇到障碍的左右校正像素")]
        public int UpwardCornerCorrection = 4;

        [Header("Dash冲刺参数")]
        [Tooltip("开始冲刺初速度")]
        public float DashSpeed = 24f;          //冲刺速度
        [Tooltip("结束冲刺后速度")]
        public float EndDashSpeed = 16f;        //结束冲刺速度
        [Tooltip("Y轴向上冲刺的衰减系数")]
        public float EndDashUpMulti = .75f;       //如果向上冲刺，阻力。
        [Tooltip("冲刺时间")]
        public float DashTime = .15f;            //冲刺时间
        [Tooltip("冲刺冷却时间")]
        public float DashCooldown = .2f;         //冲刺冷却时间，
        [Tooltip("冲刺重新装填时间")]
        public float DashRefillCooldown = .1f;   //冲刺重新装填时间
        [Tooltip("Dash水平或者竖直方向位置校正的像素值")]
        public int DashCornerCorrection = 4;     //Dash时，遇到阻挡物的可纠正距离，单位0.1米
        [Tooltip("最大Dash次数")]
        public int MaxDashes = 1;    // 最大Dash次数


        [Header("攀爬参数")]
        [Tooltip("攀爬水平方向射线检测像素")]
        public int ClimbCheckDist = 2;           //攀爬检查像素值
        [Tooltip("攀爬竖直方向射线检测像素")]
        public int ClimbUpCheckDist = 2;         //向上攀爬检查像素值
        [Tooltip("攀爬无法移动时间")]
        public float ClimbNoMoveTime = .1f;
        [Tooltip("向上攀爬速度")]
        public float ClimbUpSpeed = 4.5f;        //上爬速度
        [Tooltip("向下攀爬速度")]
        public float ClimbDownSpeed = -8f;       //下爬速度
        [Tooltip("攀爬下滑速度")]
        public float ClimbSlipSpeed = -3f;       //下滑速度
        [Tooltip("攀爬下滑加速度")]
        public float ClimbAccel = 90f;          //下滑加速度
        [Tooltip("攀爬开始时，对原Y轴速度的衰减")]
        public float ClimbGrabYMulti = .2f;       //攀爬时抓取导致的Y轴速度衰减

        [Header("Hop参数（边缘登陆）")]
        [Tooltip("Hop的Y轴速度")]
        public float ClimbHopY = 12f;          //Hop的Y轴速度
        [Tooltip("Hop的X轴速度")]
        public float ClimbHopX = 10f;           //Hop的X轴速度
        [Tooltip("Hop时间")]
        public float ClimbHopForceTime = .2f;    //Hop时间
        [Tooltip("WallBoost时间")]
        public float ClimbJumpBoostTime = .2f;   //WallBoost时间
        [Tooltip("Wind情况下,Hop会无风0.3秒")]
        public float ClimbHopNoWindTime = .3f;   //Wind情况下,Hop会无风0.3秒

        public float DuckFriction = 50f;
        public float DuckSuperJumpXMulti = 1.25f;
        public float DuckSuperJumpYMulti = 0.5f;

        private Action reloadCallback;
        public void SetReloadCallback(Action onReload)
        {
            reloadCallback = onReload;
        }
        // OnValidate方法是一个仅限编辑器的函数，在Unity加载脚本或检查器中的值更改时调用。它的调用时机非常特殊，这里总结一下。
        // 1. OnValidate不受播放模式影响，只要其值发生变化，在非播放状态下也会被调用（可以用于非播放模式修改参数后更新）。
        // 2. 不受enabled状态影响，即使其所在的脚本被禁用，修改值时也会被正常调用。
        // 3. 更改脚本enabled状态时，会调用一次OnValidate。如果在Play Mode，OnValidated的调用时机在OnDisable或者OnEnable前。
        // 4. 更改GameObject active状态不会调用OnValidate，只有OnDisable和OnEnable会被调用。
        // 5. 初始加载时，无论enabled状态和active状态如何，都会被调用多次。其调用时机在Awake之前。
        public void OnValidate()
        {
            ReloadParams();
        }

        //将设置的参数重新加载到全局参数Constants中
        public void ReloadParams()
        {
            Debug.Log("=======更新所有Player配置参数");
            Constants.MaxRun = MaxRun;//最大水平速度
            Constants.RunAccel = RunAccel;//水平方向加速度
            Constants.RunReduce = RunReduce;//水平方向减速度
            Constants.Gravity = Gravity; //重力
            Constants.HalfGravThreshold = HalfGravThreshold;//当速度小于该阈值时，重力减半。值越小，滞空时间越长，0表示关闭
            Constants.MaxFall = MaxFall; //普通最大下落速度
            Constants.FastMaxFall = FastMaxFall;  //快速最大下落速度
            Constants.FastMaxAccel = FastMaxAccel; //快速下落加速度

            Constants.UpwardCornerCorrection = UpwardCornerCorrection;

            Constants.JumpSpeed = JumpSpeed;
            Constants.VarJumpTime = VarJumpTime;
            Constants.JumpHBoost = JumpHBoost;
            Constants.JumpGraceTime = JumpGraceTime;

            Constants.DashSpeed = DashSpeed;          //冲刺速度
            Constants.EndDashSpeed = EndDashSpeed;        //结束冲刺速度
            Constants.EndDashUpMulti = EndDashUpMulti;       //如果向上冲刺，阻力。
            Constants.DashTime = DashTime;            //冲刺时间
            Constants.DashCooldown = DashCooldown;         //冲刺冷却时间，
            Constants.DashRefillCooldown = DashRefillCooldown;   //冲刺重新装填时间
            Constants.DashCornerCorrection = DashCornerCorrection;     //水平Dash时，遇到阻挡物的可纠正像素值
            Constants.MaxDashes = MaxDashes;    // 最大Dash次数

            Constants.ClimbCheckDist = ClimbCheckDist;           //攀爬检查像素值
            Constants.ClimbUpCheckDist = ClimbUpCheckDist;         //向上攀爬检查像素值
            Constants.ClimbNoMoveTime = ClimbNoMoveTime;
            Constants.ClimbUpSpeed = ClimbUpSpeed;        //上爬速度
            Constants.ClimbDownSpeed = ClimbDownSpeed;       //下爬速度
            Constants.ClimbSlipSpeed = ClimbSlipSpeed;       //下滑速度
            Constants.ClimbAccel = ClimbAccel;          //下滑加速度
            Constants.ClimbGrabYMulti = ClimbGrabYMulti;       //攀爬时抓取导致的Y轴速度衰减
            Constants.ClimbHopY = ClimbHopY;          //Hop的Y轴速度 
            Constants.ClimbHopX = ClimbHopX;           //Hop的X轴速度
            Constants.ClimbHopForceTime = ClimbHopForceTime;    //Hop时间
            Constants.ClimbJumpBoostTime = ClimbJumpBoostTime;   //WallBoost时间
            Constants.ClimbHopNoWindTime = ClimbHopNoWindTime;   //Wind情况下,Hop会无风0.3秒

            Constants.WallJumpHSpeed = MaxRun + JumpHBoost;
            Constants.SuperJumpSpeed = JumpSpeed;
            Constants.SuperWallJumpH = MaxRun + JumpHBoost * 2;

            Constants.DashCornerCorrection = DashCornerCorrection;

            Constants.DuckFriction = DuckFriction;
            Constants.DuckSuperJumpXMulti = DuckSuperJumpXMulti;
            Constants.DuckSuperJumpYMulti = DuckSuperJumpYMulti;

            Constants.EnableWallSlide = EnableWallSlide; //启用墙壁下滑功能
            Constants.EnableJumpGrace = EnableJumpGrace; //土狼时间
            Constants.EnableWallBoost = EnableWallBoost; //WallBoost

            reloadCallback?.Invoke();
        }
        /// <summary>
        /// 切换为老年态
        /// </summary>
        public void ChangeToOld()
        {
            if (Player.Instance.playerController.childOrOld == ChildOrOld.Old) return;
            Debug.Log("=======切换为老年态");
            Constants.MaxRun = MaxRun / 2;
            Player.Instance.playerController.childOrOld = ChildOrOld.Old;
        }
        /// <summary>
        /// 切换为年轻态
        /// </summary>
        public void ChangeToChild()
        {
            if (Player.Instance.playerController.childOrOld==ChildOrOld.Child) return;
            Debug.Log("=======切换为年轻态");
            Constants.MaxRun = MaxRun;
            Player.Instance.playerController.childOrOld = ChildOrOld.Child;
        }
    }
}