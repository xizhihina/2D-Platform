using UnityEngine;

namespace Myd.Platform
{
    public enum Facings
    {
        Right = 1,
        Left = -1
    }
    ///<summary>
    /// 虚拟摇杆
    /// </summary>
    public struct VirtualJoystick
    {
        public Vector2 Value { get => new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));}//返回值是{-1，0，1}
    }
    /// <summary>
    /// 虚拟按键
    /// key，用于存储关联的按键；
    /// consumed，表示按键是否已被消耗；
    /// bufferTime，用于存储按键缓冲时间；
    /// bufferCounter，用于存储缓冲计时器。
    /// Pressed()和Checked()方法来检查按键是否被按下（按下或刚刚按过，保证跳跃等操作的持续时间）或持续按下，并有一个Update(float deltaTime)方法来更新状态。
    /// </summary>
    public struct VisualButton
    {
        private KeyCode key;
        private float bufferTime;
        private float bufferCounter;
        public VisualButton(KeyCode key) : this(key, 0) {
        }

        public VisualButton(KeyCode key, float bufferTime)
        {
            this.key = key;
            this.bufferTime = bufferTime;
            bufferCounter = 0f;
        }
        public void ConsumeBuffer()
        {
            bufferCounter = 0f;
        }
        //按下或刚刚按过，保证跳跃等操作的持续时间
        public bool Pressed()
        {
            //老年态禁用
            if (Player.Instance.playerController.childOrOld==ChildOrOld.Old)
            {
                return false;
            }
            return Input.GetKeyDown(key)||bufferCounter > 0f;
        }

        public bool Checked()
        {
            //老年态禁用
            if (Player.Instance.playerController.childOrOld == ChildOrOld.Old)
            {
                return false;
            }
            return Input.GetKey(key);
        }

        public void Update(float deltaTime)
        {
            bufferCounter -= deltaTime;
            bool flag = false;
            if (Input.GetKeyDown(key))
            {
                bufferCounter = bufferTime;
                flag = true;
            }
            else if (Input.GetKey(key))
            {
                flag = true;
            }
            if (!flag)
            {
                bufferCounter = 0f;
            }
        }
    }
    public static class GameInput
    {
        public static VisualButton Jump = new(KeyCode.Space, 0.08f);
        public static VisualButton Dash = new(KeyCode.K, 0.08f);
        public static VirtualJoystick Aim;
        public static Vector2 LastAim;

        ///<summary>
        /// 根据当前朝向,决定移动方向.
        /// 如果没有输入,则返回当前player朝向的向量
        /// </summary>
        public static Vector2 GetAimVector(Facings defaultFacing = Facings.Right)
        {
            Vector2 value = Aim.Value;
            //TODO 考虑辅助模式

            //TODO 考虑摇杆
            if (value == Vector2.zero)
            {
                LastAim = Vector2.right * (int)defaultFacing;
            }
            else
            {
                LastAim = value;
            }
            return LastAim.normalized;
        }

        public static void Update(float deltaTime)
        {
            Jump.Update(deltaTime);
            Dash.Update(deltaTime);
        }
    }




}
