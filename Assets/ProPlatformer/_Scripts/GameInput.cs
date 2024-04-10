using UnityEngine;

namespace Myd.Platform
{
    public enum Facings
    {
        Right = 1,
        Left = -1
    }

    public struct VirtualIntegerAxis
    {

    }
    public struct VirtualJoystick
    {
        public Vector2 Value { get => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));}
    }
    public struct VisualButton
    {
        private KeyCode key;
        private float bufferTime;
        private bool consumed;
        private float bufferCounter;
        public VisualButton(KeyCode key) : this(key, 0) {
        }

        public VisualButton(KeyCode key, float bufferTime)
        {
            this.key = key;
            this.bufferTime = bufferTime;
            consumed = false;
            bufferCounter = 0f;
        }
        public void ConsumeBuffer()
        {
            bufferCounter = 0f;
        }

        public bool Pressed()
        {
            return Input.GetKeyDown(key)||(!consumed && (bufferCounter > 0f));
        }

        public bool Checked()
        {
            return Input.GetKey(key);
        }

        public void Update(float deltaTime)
        {
            consumed = false;
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
        public static VisualButton Jump = new VisualButton(KeyCode.Space, 0.08f);
        public static VisualButton Dash = new VisualButton(KeyCode.K, 0.08f);
        public static VisualButton Grab = new VisualButton(KeyCode.J);
        public static VirtualJoystick Aim;
        public static Vector2 LastAim;

        //根据当前朝向,决定移动方向.
        public static Vector2 GetAimVector(Facings defaultFacing = Facings.Right)
        {
            Vector2 value = Aim.Value;
            //TODO 考虑辅助模式

            //TODO 考虑摇杆
            if (value == Vector2.zero)
            {
                LastAim = Vector2.right * ((int)defaultFacing);
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
