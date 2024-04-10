﻿using UnityEngine;

namespace Myd.Platform
{
    /// <summary>
    /// 玩家类：包含
    /// 1、玩家显示器
    /// 2、玩家控制器（核心控制器）
    /// 并允许两者在内部进行交互
    /// </summary>
    public class Player
    {
        private PlayerRenderer playerRenderer;
        private PlayerController playerController;

        private Game gameContext;

        public Player(Game gameContext)
        {
            this.gameContext = gameContext;
        }

        //加载玩家实体
        public void Reload(Bounds bounds, Vector2 startPosition)
        {
            playerRenderer = Object.Instantiate(Resources.Load<PlayerRenderer>("PlayerRenderer"));
            //this.playerRenderer = AssetHelper.Create<PlayerRenderer>("Assets/ProPlatformer/_Prefabs/PlayerRenderer.prefab");
            //初始化
            playerController = new PlayerController(playerRenderer, gameContext.sceneEffectManager);
            playerController.Init(bounds, startPosition);

            PlayerParams playerParams = Resources.Load<PlayerParams>("PlayerParam");
            //PlayerParams playerParams = AssetHelper.LoadObject<PlayerParams>("Assets/ProPlatformer/PlayerParam.asset");
            playerParams.SetReloadCallback(() => playerController.RefreshAbility());
            playerParams.ReloadParams();
        }

        public void Update(float deltaTime)
        {
            playerController.Update(deltaTime);
            Render();
        }

        /// <summary>
        /// 更新玩家渲染器的位置、缩放和面向方向。
        /// </summary>
        private void Render()
        {
            playerRenderer.Render(Time.deltaTime);

            Vector2 scale = playerRenderer.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (int)playerController.Facing;
            playerRenderer.transform.localScale = scale;
            playerRenderer.transform.position = playerController.Position;
        }

        public Vector2 GetCameraPosition()
        {
            if (playerController == null)
            {
                return Vector3.zero;
            }
            return playerController.GetCameraPosition();
        }
    }

}
