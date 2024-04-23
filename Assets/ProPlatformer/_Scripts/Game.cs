using System.Collections;
using UnityEngine;

namespace Myd.Platform
{
    enum EGameState
    {
        Load,   //加载中
        Play,   //游戏中
        Pause,  //游戏暂停
        Fail,   //游戏失败
    }
    public class Game : MonoBehaviour
    {
        public static Game Instance;

        [SerializeField]
        public Level level;
        //场景特效管理器
        [SerializeField]
        public SceneEffectManager sceneEffectManager;
        [SerializeField]
        private MyCamera gameCamera;
        //玩家
        // Player player;

        EGameState gameState;

        void Awake()
        {
            Instance = this;

            gameState = EGameState.Load;

            // player = new Player(this);
            Player.Instance.gameContext = this;
        }

        IEnumerator Start()
        {
            yield return null;

            //加载玩家
            Player.Instance.Reload(level.Bounds, level.StartPosition);
            gameState = EGameState.Play;
            
            //设置倒计时
            GameUIManager.Instance.timeCountDown=60f;
            yield return null;
        }

        public void Update()
        {
            float deltaTime = Time.unscaledDeltaTime;//从上一帧到当前帧的独立于 timeScale 的时间间隔（以秒为单位）（只读）。与 deltaTime 不同，该值不受 timeScale 的影响。
            if (UpdateTime(deltaTime))
            {
                if (gameState == EGameState.Play)
                {
                    //更新按键状态
                    GameInput.Update(deltaTime);
                    //更新玩家逻辑数据
                    Player.Instance.Update(deltaTime);
                    //更新摄像机
                    // gameCamera.SetCameraPosition(player.GetCameraPosition());
                    //倒计时减少
                    GameUIManager.Instance.timeCountDown -= deltaTime;
                }
            }
        }

        #region 冻帧
        private float freezeTime;

        //更新顿帧数据，如果不顿帧，返回true
        public bool UpdateTime(float deltaTime)
        {
            if (freezeTime > 0f)
            {
                freezeTime = Mathf.Max(freezeTime - deltaTime, 0f);
                return false;
            }
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            return true;
        }

        //冻帧
        public void Freeze(float freezeTime)
        {
            this.freezeTime = Mathf.Max(this.freezeTime, freezeTime);
            if (this.freezeTime > 0)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
        #endregion
        public void CameraShake(Vector2 dir, float duration)
        {
            gameCamera.Shake(dir, duration);
        }
    }

}
