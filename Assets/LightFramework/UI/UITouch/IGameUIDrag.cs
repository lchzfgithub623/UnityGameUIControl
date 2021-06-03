
namespace LightFramework.UI
{
    /// <summary>
    /// 接口 拖拽
    /// </summary>
    interface IGameUIDrag
    {
        /// <summary>
        /// 拖拽时考虑方向, true:移动向量的y距离大于等于x距离时 表示可以上下滑动, 反之可以水平滑动
        /// </summary>
        bool ConsiderDirection
        {
            get; set;
        }

        /// <summary>
        /// 拖拽中
        /// </summary>
        bool Dragging
        {
            get; set;
        }
    }
}

