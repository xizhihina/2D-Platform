using UnityEngine;

namespace Myd.Platform
{
    
    public class RippleEffect : MonoBehaviour
    {
        //强度
        public int Intensity;
        public int WaveSpeed;
        public float TotalTime;
        public Material material;
        //起始缩放比率
        private Vector2 scale1;
        //结束缩放比率
        private Vector2 scale2;

        private float liveTime;
        private void Awake()
        {
            scale1 = Vector2.one;
            scale2 = Vector2.one * 2;
            material = GetComponent<SpriteRenderer>().material;
        }

        public void Update()
        {
            if (liveTime >= TotalTime)
            {
                gameObject.SetActive(false);
            }
            liveTime += Time.deltaTime;
            transform.localScale = transform.localScale + Vector3.one * WaveSpeed * Time.deltaTime;
            material.SetFloat("_DistortIntensity", (1 - Mathf.Clamp(liveTime / TotalTime, 0, 1)) * Intensity);
        }

        public void Ripple(Vector3 position)
        {
            gameObject.SetActive(true);
            transform.localScale = scale1;
            transform.position = position;
            liveTime = 0;
        }
    }
}