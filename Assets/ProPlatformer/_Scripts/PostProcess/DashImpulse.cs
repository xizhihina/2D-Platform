using Cinemachine;
using UnityEngine;

namespace Myd.Platform {
    public class DashImpulse : MonoBehaviour
    {
        private CinemachineImpulseSource source;

        void Awake()
        {
            source = GetComponent<CinemachineImpulseSource>();
        }

        public void Shake(Vector2 dir)
        {
            source.m_DefaultVelocity = dir;
            source.GenerateImpulseWithForce(0.1f);
        }
    }
}