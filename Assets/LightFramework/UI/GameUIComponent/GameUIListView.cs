using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LightFramework.UI
{
    [AddComponentMenu("UI/GameUI/GameUIListView", 37)]
    public partial class GameUIListView : GameUIBaseComponent
    {
        #region 私有字段
        /// <summary>
        /// 可以复用列表项
        /// </summary>
        [SerializeField]
        private bool isReuse = false;

        [SerializeField]
        private Vector2 space = new Vector2(0, 0);

        [SerializeField]
        private RectOffset padding = new RectOffset();

        /// <summary>
        /// 列表项可选
        /// </summary>
        [SerializeField]
        private bool isOptional = false;

        /// <summary>
        /// 正在显示的列表项
        /// </summary>
        private SortedList<int, GameUIListItem> displayItems = new SortedList<int, GameUIListItem>();


        /// <summary>
        /// 从显示列表中暂时分离出来的新的显示列表项
        /// </summary>
        private List<GameUIListItem> tempDisplayItems = new List<GameUIListItem>();

        private ScrollRect _scrollRect;
        private RectTransform _content;
        private Image bgImage;


        /// <summary>
        /// 滑动内容变化时的最大宽高
        /// </summary>
        private Vector2 contentMaxSize = Vector2.zero;

        #endregion

        #region 公有字段

        /// <summary>
        /// 列表项在显示列表时 index 发生变化的 回调
        /// </summary>
        public UnityAction<GameUIListItem> onUpdateItem;

        /// <summary>
        /// 滑动数值发生变化
        /// </summary>
        public UnityAction<Vector2> onValueChanged;

        #endregion

        #region 受保护属性

        /// <summary>
        /// ScrollRect 组件
        /// </summary>
        protected ScrollRect scrollRect
        {
            get
            {
                if (_scrollRect == null)
                {
                    _scrollRect = GetComponent<ScrollRect>();
                    if (_scrollRect == null)
                    {
                        Debug.LogError("the 'Scroll Rect' not found, name:" + (gameObject != null ? gameObject.name : ""));
                    }
                }
                return _scrollRect;
            }
        }

        #endregion

        #region 公有属性

        /// <summary>
        /// ScrollRect 组件
        /// </summary>
        public ScrollRect Scroll
        {
            get { return scrollRect; }
        }

        /// <summary>
        /// LightScrollRect 组件
        /// </summary>
        public LightScrollRect LightScroll
        {
            get { return scrollRect as LightScrollRect; }
        }

        /// <summary>
        /// 列表大小
        /// </summary>
        public Vector2 scrollSize
        {
            get { return scrollRect.GetRectTrans().rect.size; }
            set { scrollRect.GetRectTrans().sizeDelta = value; }
        }


        /// <summary>
        /// 背景图片 image 组件
        /// </summary>
        public Image BgImage
        {
            get
            {
                if (bgImage == null)
                {
                    bgImage = GetComponent<Image>();
                    if (bgImage == null)
                    {
                        Debug.LogError("the ScrollRect 'Image' not found, name:" + (gameObject != null ? gameObject.name : ""));
                    }
                }
                return bgImage;
            }
        }


        /// <summary>
        /// 可视区域组件
        /// </summary>
        public RectTransform viewport
        {
            get { return scrollRect.viewport; }
            set { scrollRect.viewport = value; }
        }

        /// <summary>
        /// 可视区域组件的大小, 这个和scrollSize有一定的区别
        /// </summary>
        public Vector2 viewSize
        {
            get { return viewport.rect.size; }
            set { viewport.sizeDelta = value; }
        }


        /// <summary>
        /// 内容组件
        /// </summary>
        public RectTransform content
        {
            get
            {
                if (_content == null)
                {
                    ResetContent();
                }
                return _content;
            }
            set { scrollRect.content = value; }
        }

        /// <summary>
        /// 内容组件的大小
        /// </summary>
        public Vector2 contentSize
        {
            get { return content.rect.size; }
            set
            {
                if (!content.sizeDelta.Equals(value))
                {
                    // 计算content组件的最大距离
                    CalculateContenFarPos(value);
                    // 重新设置content组件的位置,防止超出边界
                    SetContentPosition(contentPos);
                    content.sizeDelta = value;
                }
            }
        }

        /// <summary>
        /// 内容组件的位置
        /// </summary>
        public Vector2 contentPos
        {
            get { return content.anchoredPosition; }
            protected set { content.anchoredPosition = value; }
        }

        /// <summary>
        /// 内容组件最远距离的位置
        /// </summary>
        public Vector2 contentFarPos
        {
            get; protected set;
        }


        /// <summary>
        /// 是否水平的
        /// </summary>
        public bool horizontal
        {
            get { return scrollRect.horizontal; }
            set
            {
                if (scrollRect.horizontal != value)
                {
                    scrollRect.horizontal = value;
                    ResetContent();
                }
            }
        }

        /// <summary>
        /// 是否竖直的
        /// </summary>
        public bool vertical
        {
            get { return scrollRect.vertical; }
            set
            {
                if (scrollRect.vertical != value)
                {
                    scrollRect.vertical = value;
                    ResetContent();
                }
            }
        }

        /// <summary>
        /// 拖拽是否区分方向
        /// </summary>
        public bool ConsiderDirection
        {
            get
            {
                if (scrollRect is IGameUIDrag)
                {
                    return (scrollRect as IGameUIDrag).ConsiderDirection;
                }
                return false;
            }

            set
            {
                if (scrollRect is IGameUIDrag)
                {
                    (scrollRect as IGameUIDrag).ConsiderDirection = value;
                }
                else
                {
                    Debug.LogWarning("the scrollrect has not inherited IGameUIDrag interface");
                }
            }
        }

        /// <summary>
        /// 是否拖拽中
        /// </summary>
        public bool Dragging
        {
            get
            {
                if (scrollRect is IGameUIDrag)
                {
                    return (scrollRect as IGameUIDrag).Dragging;
                }
                return false;
            }
        }


        /// <summary>
        /// 超出边界时的移动类型
        /// </summary>
        public ScrollRect.MovementType movementType
        {
            get { return scrollRect.movementType; }
            set { scrollRect.movementType = value; }
        }

        /// <summary>
        /// 超出边界时的回弹速度, (movementType == ScrollRect.MovementType.Elastic) 有效
        /// </summary>
        public float elasticity
        {
            get { return scrollRect.elasticity; }
            set { scrollRect.elasticity = value; }
        }

        /// <summary>
        /// 是否有移动惯性, true:滑动离开后会惯性想目标方向移动一小段距离
        /// </summary>
        public bool inertia
        {
            get { return scrollRect.inertia; }
            set { scrollRect.inertia = value; }
        }

        /// <summary>
        /// 减速速率
        /// </summary>
        public float decelerationRate
        {
            get { return scrollRect.decelerationRate; }
            set { scrollRect.decelerationRate = value; }
        }

        /// <summary>
        /// 滑动敏感度, 或移动速度(类似鼠标滚轮在scrollRect上滑动)
        /// </summary>
        public float scrollSensitivity
        {
            get { return scrollRect.scrollSensitivity; }
            set { scrollRect.scrollSensitivity = value; }
        }

        /// <summary>
        /// 水平滚动条
        /// </summary>
        public Scrollbar horizontalScrollbar
        {
            get { return scrollRect.horizontalScrollbar; }
            set { scrollRect.horizontalScrollbar = value; }
        }

        /// <summary>
        /// 水平滚动条显示类型
        /// </summary>
        public ScrollRect.ScrollbarVisibility horizontalScrollbarVisibility
        {
            get { return scrollRect.horizontalScrollbarVisibility; }
            set { scrollRect.horizontalScrollbarVisibility = value; }
        }

        /// <summary>
        /// 竖直滚动条
        /// </summary>
        public Scrollbar verticalScrollbar
        {
            get { return scrollRect.verticalScrollbar; }
            set { scrollRect.verticalScrollbar = value; }
        }

        /// <summary>
        /// 竖直滚动条显示类型
        /// </summary>
        public ScrollRect.ScrollbarVisibility verticalScrollbarVisibility
        {
            get { return scrollRect.verticalScrollbarVisibility; }
            set { scrollRect.verticalScrollbarVisibility = value; }
        }

        /// <summary>
        /// content位置比例 [0-1]
        /// </summary>
        public Vector2 normalizedPosition
        {
            get { return scrollRect.normalizedPosition; }
            set { scrollRect.normalizedPosition = value; }
        }

        /// <summary>
        /// content的水平位置比例, 0在左边
        /// </summary>
        public float horizontalNormalizedPosition
        {
            get { return scrollRect.horizontalNormalizedPosition; }
            set { scrollRect.horizontalNormalizedPosition = value; }
        }

        /// <summary>
        /// content的竖直位置比例, 0在上边
        /// </summary>
        public float verticalNormalizedPosition
        {
            get { return scrollRect.verticalNormalizedPosition; }
            set { scrollRect.verticalNormalizedPosition = value; }
        }

        /// <summary>
        /// 是否复用
        /// </summary>
        public bool IsReuse
        {
            get { return isReuse; }
            set
            {
                isReuse = value;
                if (DataCount > 0)
                {
                    Debug.LogWarning("ListView有列表项存在时, 不能切换复用模式");
                }
            }
        }

        /// <summary>
        /// 列表项的间隔
        /// </summary>
        public Vector2 Space
        {
            get { return space; }
            set
            {
                if (PropertySetUtility.SetStruct(ref space, value))
                {
                    SetDirty();
                }
            }
        }

        /// <summary>
        /// 列表项在内容组件中的边距
        /// </summary>
        public RectOffset Padding
        {
            get { return padding; }
            set
            {
                if (PropertySetUtility.SetClass(ref padding, value))
                {
                    SetDirty();
                }
            }
        }

        /// <summary>
        /// 正在使用的列表项
        /// </summary>
        public SortedList<int, GameUIListItem> Items
        {
            get { return displayItems; }
        }

        /// <summary>
        /// 数据项个数
        /// </summary>
        public int DataCount
        {
            get; protected set;
        }

        #endregion

        #region 虚方法

        public virtual void OnValueChanged(Vector2 percent)
        {
            if (isReuse)
            {
                AdjustAllReuseItemsPosition();
            }
            if (onValueChanged != null)
            {
                onValueChanged.Invoke(percent);
            }
        }


        protected override void OnEnable()
        {
            SetDirty();
            scrollRect.onValueChanged.AddListener(OnValueChanged);
        }

        protected override void OnDisable()
        {
            scrollRect.onValueChanged.RemoveListener(OnValueChanged);
        }

        protected override void Reset()
        {
            base.Reset();
            _scrollRect = null;
            _content = null;
            bgImage = null;
            RemoveAll();
            RemoveUnusePool();
        }

        #endregion

        #region 公有方法 通用

        /// <summary>
        /// 清空显示列表
        /// </summary>
        public void RemoveAll()
        {
            DataCount = 0;
            firstShowIndex = -1;
            ClearSelectableData();
            RecycleDisplayItems();
            RecycleIdleItems();
            ResetContent();
        }

        /// <summary>
        /// 刷新所有列表项的位置
        /// </summary>
        /// <remarks>
        /// 复用模式下, 该方法会重新计算content组件相关数值,例如: content 大小, 复用项大小,以及view中最多显示多少个复用项等
        /// </remarks>
        public void SetDirty()
        {
            if (isActiveAndEnabled)
            {
                RefreshItems();
            }
        }

        /// <summary>
        /// 强制立即重建 content 的自适应大小
        /// </summary>
        public void ForceLayout()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        }

        #endregion

        #region 受保护方法

        /// <summary>
        /// 重置 content 位置 以及基础的大小增量
        /// </summary>
        protected void ResetContent()
        {
            if(_content == null)
            {
                _content = scrollRect.content;
            }
            if(vertical != horizontal)
            {
                Vector2 size = ResetAnchorAndPivot(_content, false);
                if (vertical)
                {
                    size.y = 0;
                }
                else
                {
                    size.x = 0;
                }
                content.sizeDelta = size;
                _content.anchoredPosition = Vector2.zero;
            }
            ResetContentMaxSize();
        }

        /// <summary>
        /// 重置内容大小增量数值为初始值
        /// </summary>
        protected void ResetContentMaxSize()
        {
            if (vertical)
            {
                contentMaxSize = new Vector2(content.rect.width, 0);
            }
            else
            {
                contentMaxSize = new Vector2(0, content.rect.height);
            }
        }


        /// <summary>
        /// 添加一个实例化的 ListItem 到可显示列表
        /// </summary>
        /// <param name="item"> listItem </param>
        /// <param name="index"> 对应的索引值 </param>
        /// <returns></returns>
        protected void AddDisplayItem(GameUIListItem item, int index)
        {
            item.SetActive(true);
            item.transform.SetParent(content, false);
            RectTransform rectTrf = item.transform as RectTransform;
            ResetAnchorAndPivot(item.transform as RectTransform, true);
            ChangeItemIndex(item, index);
            item.IsAdded = true;
        }

        /// <summary>
        /// 设置添加物体的锚点和对齐锚点
        /// </summary>
        /// <param name="rectTrf"></param>
        /// <param name="bResetContent"></param>
        /// <returns></returns>
        protected Vector2 ResetAnchorAndPivot(RectTransform rectTrf, bool bResetContent)
        {
            Vector2 size = rectTrf.rect.size;
            rectTrf.pivot = new Vector2(0, 1);
            rectTrf.anchorMin = new Vector2(0, 1);
            rectTrf.anchorMax = new Vector2(0, 1);
            if(bResetContent)
            {
                rectTrf.sizeDelta = size;
            }
            return size;
        }

        /// <summary>
        /// 改变列表项 index
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        protected void ChangeItemIndex(GameUIListItem item, int index)
        {
            if(item.Index != index)
            {
                item.Index = index;
                item.transform.SetSiblingIndex(index);
                GameUIListItem replaceItem = InsertItem(item);
                
                // 顺序改变, 改变位置
                AdjustItemPosition(item);

                if (onUpdateItem != null)
                {
                    onUpdateItem.Invoke(item);
                }

                if (replaceItem != null)
                {
                    ChangeItemIndex(replaceItem, index + 1);
                }
            }
            else
            {
                InsertItem(item);
                // 刷新一下位置
                AdjustItemPosition(item);
            }
        }


        /// <summary>
        /// 根据 index 从 mShowingItems 中获取一个列表项实例
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected GameUIListItem GetItemByIndex(int index)
        {
            GameUIListItem item;
            displayItems.TryGetValue(index, out item);
            return item;
        }

        /// <summary>
        /// 插入一个列表项实例 到 mShowingItems
        /// </summary>
        /// <param name="listItem"></param>
        protected GameUIListItem InsertItem(GameUIListItem listItem)
        {
            int index = listItem.Index;
            if (!displayItems.ContainsKey(index))
            {
                displayItems.Add(index, listItem);
                AddSelectableItem(listItem);
                return null;
            }
            GameUIListItem oldItem = displayItems[index];
            displayItems[index] = listItem;
            AddSelectableItem(listItem);
            return oldItem;
        }


        /// <summary>
        /// 调整列表项的位置
        /// </summary>
        /// <param name="item"></param>
        protected void AdjustItemPosition(GameUIListItem item)
        {
            if(isReuse)
            {
                AdjustReuseItemPostion(item);
            }
            else
            {
                AdjustUnReuseItemPosition(item);
            }
        }

        /// <summary>
        /// 获取一个列表项的位置,大小,行列值 信息
        /// </summary>
        /// <param name="index"></param>
        /// <param name="line">行 从0开始的</param>
        /// <param name="column">列 从0开始的</param>
        /// <param name="pos">位置</param>
        /// <param name="size">大小</param>
        protected void CalculateItemTransformInfo(int index, out int line, out int column, out Vector2 pos, out Vector2 size)
        {
            if(isReuse)
            {
                CalculateReuseItemTransformInfo(index, out line, out column, out pos, out size);
            }
            else
            {
                CalculateUnReuseItemTransformInfo(index, out line, out column, out pos, out size);
            }
        }

        /// <summary>
        /// 刷新所有列表项的位置
        /// </summary>
        protected void RefreshItems()
        {
            ResetContentMaxSize();
            if (isReuse)
            {
                CaculateReuseParam();
                AdjustReuseContentSize();
                firstShowIndex = -1;
                AdjustAllReuseItemsPosition(true);
            }
            else
            {
                AdjustAllUnReuseItemsPostion();
            }
        }

        #endregion

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (!IsActive())
                return;

            StartCoroutine(DelayedSetDirty());
        }
        IEnumerator DelayedSetDirty()
        {
            yield return null;
            if(Application.isPlaying)
            {
                Vector2 anchorTemp = new Vector2(0, 1);
                if (content.anchorMin != anchorTemp || content.anchorMax != anchorTemp)
                {
                    ResetContent();
                }
                RefreshItems();
            }
        }
#endif
    }
}

