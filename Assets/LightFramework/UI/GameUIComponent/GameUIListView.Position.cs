
using UnityEngine;
using UnityEngine.UI;

namespace LightFramework.UI
{
    /// <summary>
    /// 列表项定位到视图中 以及 内容组件的位置
    /// </summary>
    partial class GameUIListView
    {
        /// <summary>
        /// 列表项在视图中的对齐方式, 用于定位列表项显示在视图内
        /// </summary>
        public enum ListItemInViewAlign
        {
            /// <summary>
            /// 只要全部显示在视图内就行
            /// </summary>
            InView,

            /// <summary>
            /// 全部显示在视图内, 并且列表项要靠在视图最左边 或 最顶部
            /// </summary>
            LeftOrTop,

            /// <summary>
            /// 全部显示在视图内, 并且列表项要靠在视图最右边 或 最底部
            /// </summary>
            RightOrBottom,
        }

        #region 公有方法 定位列表选项到视图区域

        /// <summary>
        /// 停止滑动列表移动
        /// </summary>
        public void StopMovement()
        {
            scrollRect.StopMovement();
        }

        /// <summary>
        /// 设置content内容组件的位置
        /// </summary>
        /// <param name="pos">要设置的位置</param>
        /// <param name="hasElasticMovementType">true:是否有回弹动作</param>
        public void SetContentPosition(Vector2 pos, bool hasElasticMovementType = false)
        {
            ScrollRect.MovementType tempMovementType = movementType;
            if (!hasElasticMovementType && tempMovementType == ScrollRect.MovementType.Elastic)
            {
                movementType = ScrollRect.MovementType.Clamped;
            }
            Vector2 fitPos = FitContentPos(pos);
            contentPos = fitPos;

            movementType = tempMovementType;
        }

        /// <summary>
        /// 移动列表项到显示视图区域内
        /// </summary>s
        /// <param name="index">列表项index</param>
        /// <param name="alignType">列表项对齐类型 参考 枚举 EItemAlign </param>
        /// <returns>true: 有移动; false;没有移动</returns>
        public bool MoveItemInView(int index, ListItemInViewAlign alignType = ListItemInViewAlign.InView)
        {
            if (index < 0 || index >= DataCount)
            {
                return false;
            }
            bool needMove = false;
            Vector2 newContentPos = Vector2.zero;
            Vector2 leftUpPos;
            Vector2 rightDownPos;
            MatchItemContentPosition(index, out leftUpPos, out rightDownPos);
            if (vertical)
            {
                needMove = (alignType == ListItemInViewAlign.LeftOrTop && contentPos.y != leftUpPos.y) 
                    || (alignType == ListItemInViewAlign.InView && contentPos.y > leftUpPos.y);
                if (needMove)
                {
                    newContentPos.y = leftUpPos.y;
                }
                else
                {
                    needMove = (alignType == ListItemInViewAlign.RightOrBottom && contentPos.y != rightDownPos.y) 
                        || (alignType == ListItemInViewAlign.InView && contentPos.y < rightDownPos.y);
                    if (needMove)
                    {
                        newContentPos.y = rightDownPos.y;
                    }
                }
            }
            else
            {
                needMove = (alignType == ListItemInViewAlign.LeftOrTop && contentPos.x != leftUpPos.x) 
                    || (alignType == ListItemInViewAlign.InView && contentPos.x < leftUpPos.x);
                if (needMove)
                {
                    newContentPos.x = leftUpPos.x;
                }
                else
                {
                    needMove = (alignType == ListItemInViewAlign.RightOrBottom && contentPos.x != rightDownPos.x) 
                        || (alignType == ListItemInViewAlign.InView && contentPos.x > rightDownPos.x);
                    if (needMove)
                    {
                        newContentPos.x = rightDownPos.x;
                    }
                }
            }

            if (needMove)
            {
                contentPos = newContentPos;
            }
            return needMove;
        }

        /// <summary>
        /// 根据index 获取列表项在视图内可全部显示的最大和最小位置
        /// </summary>
        /// <param name="index"></param>
        /// <param name="leftUpPos">竖直:顶部位置, 水平:左侧位置</param>
        /// <param name="rightDownPos">竖直:底部位置, 水平:右侧位置</param>
        /// <returns>列表项 当前的位置</returns>
        public Vector2 MatchItemContentPosition(int index, out Vector2 leftUpPos, out Vector2 rightDownPos)
        {
            int line, column;
            Vector2 pos, size;
            CalculateItemTransformInfo(index, out line, out column, out pos, out size);
            Vector2 zero = Vector2.zero;
            if (vertical)
            {
                Vector2 upPos = new Vector2(pos.x, -pos.y);
                leftUpPos = upPos.Between(zero, contentFarPos);

                Vector2 downPos = new Vector2(pos.x, (-pos.y + size.y) - viewSize.y);
                rightDownPos = downPos.Between(zero, contentFarPos);
            }
            else
            {
                Vector2 leftPos = new Vector2(-pos.x, pos.y);
                leftUpPos = leftPos.Between(contentFarPos, zero);

                Vector2 rightPos = new Vector2(viewSize.x - (pos.x + size.x), pos.y);
                rightDownPos = rightPos.Between(contentFarPos, zero);
            }
            return pos;
        }

        /// <summary>
        /// 根据要设置的内容组件位置 适配一个正确合理的内容组件位置
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Vector2 FitContentPos(Vector2 pos)
        {
            if (vertical)
            {
                if (pos.y < 0)
                {
                    pos.y = 0;
                }
                else if (pos.y > contentFarPos.y)
                {
                    pos.y = contentFarPos.y;
                }
            }
            else
            {
                if (pos.x > 0)
                {
                    pos.x = 0;
                }
                else if (pos.x < contentFarPos.x)
                {
                    pos.x = contentFarPos.x;
                }
            }
            return pos;
        }

        /// <summary>
        /// content组件的位置是否在正确的位置范围内
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool IsInRightPosition(Vector2 pos)
        {
            if (vertical)
            {
                return (pos.y >= 0 && pos.y <= contentFarPos.y);
            }
            else
            {
                return (pos.x <= 0 && pos.x >= contentFarPos.x);
            }
        }

        #endregion

        #region 受保护方法

        /// <summary>
        /// 计算内容组件的最长距离
        /// </summary>
        protected void CalculateContenFarPos(Vector2 contentMaxSize)
        {
            if (vertical)
            {
                contentFarPos = new Vector2(0, contentMaxSize.y > viewSize.y ? contentMaxSize.y - viewSize.y : 0);
            }
            else
            {
                contentFarPos = new Vector2(contentMaxSize.x > viewSize.x ? viewSize.x - contentMaxSize.x : 0, 0);
            }
        }

        #endregion

    }
}
