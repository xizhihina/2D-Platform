using UnityEngine;

namespace Myd.Platform
{
    public class Level : MonoBehaviour
    {
        public int levelId;

        public Bounds Bounds;

        public Vector2 StartPosition;

        //脚本附件的物体背选择时调用，仅在编辑器中可见，游戏运行时不会调用
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(Bounds.center, Bounds.size);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(StartPosition, 0.5f);
        }
    }
}
