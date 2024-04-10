using UnityEngine;

namespace Myd.Platform
{

    /// <summary>
    /// 这里是Unity下实现玩家表现接口
    /// </summary>
    public class PlayerRenderer : MonoBehaviour
    {
        [SerializeField]
        public SpriteRenderer spriteRenderer;//玩家角色的主要精灵渲染器

        [SerializeField]
        public ParticleSystem vfxDashFlux;//玩家冲刺时的特效。
        [SerializeField]
        public ParticleSystem vfxWallSlide;//玩家墙面滑动时的特效

        [SerializeField]
        public TrailRenderer hair;//玩家头发的拖尾效果。

        [SerializeField]
        public SpriteRenderer hairSprite01;
        [SerializeField]
        public SpriteRenderer hairSprite02;

        private Vector2 scale;
        private Vector2 currSpriteScale;

        public Vector3 SpritePosition { get => spriteRenderer.transform.position; }

        public void Render(float deltaTime)
        {
            float tempScaleX = Mathf.MoveTowards(scale.x, currSpriteScale.x, 1.75f * deltaTime);
            float tempScaleY = Mathf.MoveTowards(scale.y, currSpriteScale.y, 1.75f * deltaTime);
            scale = new Vector2(tempScaleX, tempScaleY);
            spriteRenderer.transform.localScale = scale;
        }

        public void Trail(int face)
        {
            SceneEffectManager.Instance.Add(spriteRenderer, face, Color.white);
        }

        public void Scale(Vector2 scale)
        {
            this.scale = scale;
        }

        public void SetSpriteScale(Vector2 scale)
        {
            currSpriteScale = scale;
        }

        public void WallSlide(Color color, Vector2 dir)
        {
            vfxWallSlide.transform.rotation = Quaternion.FromToRotation(Vector2.up, dir);
            var main = vfxWallSlide.main;
            main.startColor = color;
            vfxWallSlide.Emit(1);
        }

        public void DashFlux(Vector2 dir, bool play)
        {
            if (play)
            {
                vfxDashFlux.transform.rotation = Quaternion.FromToRotation(Vector2.up, dir);
                vfxDashFlux.Play();
            }
            else
            {
                vfxDashFlux.transform.parent = transform;
                vfxDashFlux.Stop();
            }
        }

        public void SetHairColor(Color color)
        {
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new[] { new GradientColorKey(color, 0.0f), new GradientColorKey(Color.black, 1.0f) },
                new[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 0.6f), new GradientAlphaKey(0, 1.0f) }
            );
            hair.colorGradient = gradient;
            hairSprite01.color = color;
            hairSprite02.color = color;
        }
    }
}
