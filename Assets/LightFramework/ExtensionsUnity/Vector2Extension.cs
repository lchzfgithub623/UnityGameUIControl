using UnityEngine;

namespace LightFramework
{
    public static partial class Vector2Extension
    {
        /// <summary>
        /// 根据源位置, 匹配 最小和最大位置之间的位置
        /// </summary>
        /// <param name="matchPos">源位置</param>
        /// <param name="min">最小位置</param>
        /// <param name="max">最大位置</param>
        /// <returns>匹配值</returns>
        public static Vector2 Between(this Vector2 source, Vector2 min, Vector2 max)
        {
            Vector2 matchPos = new Vector2(source.x, source.y);
            if(matchPos.x < min.x)
            {
                matchPos.x = min.x;
            }
            else if(matchPos.x > max.x)
            {
                matchPos.x = max.x;
            }
            if (matchPos.y < min.y)
            {
                matchPos.y = min.y;
            }
            else if (matchPos.y > max.y)
            {
                matchPos.y = max.y;
            }
            return matchPos;
        }
    }
}

