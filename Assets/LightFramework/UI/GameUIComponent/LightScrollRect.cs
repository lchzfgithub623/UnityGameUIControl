using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace LightFramework.UI
{
    /// <summary>
    /// 封装的自定义 ScrollRect 组件
    /// </summary>
    [AddComponentMenu("UI/GameUI/LightScrollRect", 37)]
    public class LightScrollRect : ScrollRect, IGameUIDrag
    {
        #region 私有字段

        [HideInInspector]
        [SerializeField]
        private bool considerDirection;

        [SerializeField]
        private ScrollRectEvent onSizeChanged = new ScrollRectEvent();

        [SerializeField]
        private ScrollRectEvent onDragEnd = new ScrollRectEvent();

        #endregion

        #region 受保护属性

        /// <summary>
        /// 父 ScrollRect 组件, 可能为null
        /// </summary>
        protected ScrollRect parentScroll
        {
            get; set;
        }

        #endregion

        #region 公有属性 ScrollRect 事件

        /// <summary>
        /// 事件:ScrollRect 大小发生改变
        /// </summary>
        public ScrollRectEvent OnSizeChanged
        {
            get { return onSizeChanged; }
            set { onSizeChanged = value; }
        }

        /// <summary>
        /// 事件:ScrollRect 拖拽结束
        /// </summary>
        public ScrollRectEvent OnDragEnd
        {
            get { return onDragEnd; }
            set { onDragEnd = value; }
        }

        #endregion

        #region 公有属性

        /// <summary>
        /// 拖拽时考虑方向, true:判断方向, 移动向量的y距离大于等于x距离时 表示可以上下滑动, 反之可以水平滑动
        /// </summary>
        public bool ConsiderDirection
        {
            get { return considerDirection; }

            set { considerDirection = value; }
        }

        /// <summary>
        /// 拖拽中
        /// </summary>
        public bool Dragging
        {
            get; set;
        }

        /// <summary>
        /// 滑动框大小
        /// </summary>
        public Vector2 scrollSize
        {
            get; protected set;
        }

        #endregion

        #region 重写方法

        public override void OnInitializePotentialDrag(PointerEventData eventData)
        {
            base.OnInitializePotentialDrag(eventData);
            Transform parent = transform.parent;
            if (parent != null)
            {
                parentScroll = parent.GetComponentInParent<ScrollRect>();
            }
            else
            {
                parentScroll = null;
            }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            Dragging = true;
            if (!ConsiderDirection)
            {
                base.OnBeginDrag(eventData);
                return;
            }
            bool isVerticalDir = false;
            if (Mathf.Abs(eventData.delta.y) >= Mathf.Abs(eventData.delta.x))
            {
                isVerticalDir = true;
            }
            
            if (vertical && !isVerticalDir)
            {
                Dragging = false;
            }
            else if (horizontal && isVerticalDir)
            {
                Dragging = false;
            }

            if (Dragging)
            {
                base.OnBeginDrag(eventData);
            }
            else if (parentScroll != null)
            {
                parentScroll.OnBeginDrag(eventData);
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (!ConsiderDirection || Dragging)
            {
                base.OnDrag(eventData);
            }
            else if (parentScroll != null)
            {
                parentScroll.OnDrag(eventData);
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (!ConsiderDirection || Dragging)
            {
                base.OnEndDrag(eventData);
                onDragEnd.Invoke(normalizedPosition);
            }
            else if (parentScroll != null)
            {
                parentScroll.OnEndDrag(eventData);
            }
            Dragging = false;
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            if(!isActiveAndEnabled)
            {
                scrollSize = (transform as RectTransform).rect.size;
                return;
            }
            Vector2 newSize = (transform as RectTransform).rect.size;
            if(!scrollSize.Equals(newSize))
            {
                scrollSize = newSize;
                onSizeChanged.Invoke(newSize);
            }
        }

        #endregion
    }
}

