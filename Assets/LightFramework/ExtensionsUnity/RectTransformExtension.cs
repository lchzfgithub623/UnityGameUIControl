using UnityEngine;

namespace LightFramework
{
    /// <summary>
    /// RectTransform扩展
    /// </summary>
    public static partial class RectTransformExtension 
    {
        /// <summary>
        /// 获取带有缩放的大小
        /// </summary>
        /// <param name="rectTrf">对象的RectTransform </param>
        /// <returns></returns>
        public static Vector2 GetSizeWithScale(this RectTransform rectTrf)
        {
            if(rectTrf == null)
            {
                return Vector2.zero;
            }
            return rectTrf.rect.size * rectTrf.localScale;
        }
    }
}

