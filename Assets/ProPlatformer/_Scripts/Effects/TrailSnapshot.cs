using System;
using Myd.Common;
using UnityEngine;

namespace Myd.Platform
{
    public class TrailSnapshot : MonoBehaviour
    {
        public Vector2 SpriteScale;
        public int Index;
        public Color Color;
        public float Percent;
        public float Duration;
        public bool Drawn;
        public bool UseRawDeltaTime;

        private SpriteRenderer spriteRenderer;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private Action onRemoved;
        public void Init(int index, Vector2 position, Sprite sprite, Vector2 scale, Color color, 
            float duration, int depth, bool frozenUpdate, bool useRawDeltaTime, Action onRemoved)
        {
            Index = index;
            SpriteScale = scale;
            Color = color;
            Percent = 0.0f;
            Duration = duration;
            spriteRenderer.sortingOrder = depth;
            Drawn = false;
            UseRawDeltaTime = useRawDeltaTime;
            spriteRenderer.color = color;
            spriteRenderer.sprite = sprite;
            transform.position = position;
            transform.localScale = scale;
            this.onRemoved = onRemoved;
        }

        private void Update()
        {
            OnUpdate();
            OnRender();
        }

        private void OnUpdate()
        {
            if (Duration <= 0.0)
            {
                if (!Drawn)
                    return;
                Removed();
            }
            else
            {
                if (Percent >= 1.0)
                {
                    Removed();
                }
                Percent += Time.deltaTime / Duration;
            }
        }

        private void OnRender()
        {
            float num = Duration > 0.0 ? (float)(0.75 * (1.0 - Ease.CubeOut(Percent))) : 1f;
            spriteRenderer.color = Color * num;
        }

        private void Removed()
        {
            onRemoved?.Invoke();
            Destroy(gameObject);
        }
    }
}