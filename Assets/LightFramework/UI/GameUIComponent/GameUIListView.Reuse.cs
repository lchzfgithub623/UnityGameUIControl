using System.Collections.Generic;
using UnityEngine;

namespace LightFramework.UI
{
    /// <summary>
    /// GameUIListView 复用列表
    /// </summary>
    partial class GameUIListView
    {
        #region 私有字段

        /// <summary>
        /// [复用模式] 复用项大小, 复用模版项改变时重新计算
        /// </summary>
        private Vector2 reuseCellSize = Vector2.one;

        /// <summary>
        /// [复用模式] 复用项大小加上间隔的大小, 复用模版项改变时重新计算
        /// </summary>
        private Vector2 cellSizeWithSpace = Vector2.one;

        /// <summary>
        /// [复用模式] 可显示的第一个索引值
        /// </summary>
        private int firstShowIndex = -1;

        /// <summary>
        /// [复用模式] 视图内允许显示的最后一个索引值
        /// </summary>
        private int lastShowIndex = 0;
        #endregion

        #region 公有属性

        /// <summary>
        /// [复用模式] 模版列表项
        /// </summary>
        public GameUIListItem TemplateItem { get; protected set; }

        /// <summary>
        /// [复用模式] 可以复用的最大个数, 复用模版项改变时重新计算
        /// </summary>
        public int MaxShowCount { get; protected set; }

        /// <summary>
        /// [复用模式] 单行行或单列中列表项个数
        /// </summary>
        public int LineNumber { get; protected set; }

        /// <summary>
        /// [复用模式] 可显示的第一个索引值
        /// </summary>
        public int FirstShowIndex { get { return firstShowIndex; } }

        /// <summary>
        /// [复用模式] 可显示的最后一个索引值
        /// </summary>
        public int LastShowIndex { get { return lastShowIndex; } }

        #endregion

        #region 公有方法 复用模式

        /// <summary>
        /// [复用模式] 初始化列表项的组件实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comp"></param>
        public void InitReuseItem<T>(Component comp) where T : GameUIListItem, new()
        {
            if (comp == null || comp.gameObject == null)
            {
                Debug.LogWarning("listview reuse gameobject is null : " + typeof(T));
                return;
            }

            if (!isReuse)
            {
                Debug.LogError("need 'isReuse' is true");
                return;
            }

            TemplateItem = comp.gameObject.GetComponent<T>() ?? comp.gameObject.AddComponent<T>();
            comp.gameObject.SetActive(false);

            // 获取格子大小
            reuseCellSize = TemplateItem.GetRectTrans().GetSizeWithScale();
            if (reuseCellSize.x == 0)
                reuseCellSize.x = 1;
            if (reuseCellSize.y == 0)
                reuseCellSize.y = 1;
            // 计算复用模式需要的参数
            CaculateReuseParam();
        }

        /// <summary>
        /// [复用模式] 初始化列表项的组件实例, 使用默认的 GameUIListItem 为列表项的组件
        /// </summary>
        /// <param name="comp"></param>
        public void InitReuseItemWithDefalut(Component comp)
        {
            InitReuseItem<GameUIListItem>(comp);
        }

        /// <summary>
        /// [复用模式] 设置复用数据的个数
        /// </summary>
        /// <param name="dataCount">数据的个数</param>
        public void SetReuseDataCount(int dataCount)
        {
            DataCount = dataCount;

            AdjustReuseContentSize();
        }

        /// <summary>
        /// [复用模式] 添加一个复用的列表项
        /// </summary>
        public void AddReuseItem()
        {
            if (!isReuse)
            {
                Debug.LogError("'isReuse' is false, check it and set true, 请勾选选项'可以复用列表项'");
                return;
            }

            int curIndex = DataCount;
            SetReuseDataCount(++DataCount);
            RefreshFirstShowIndex();

            AddReuseItemByIndex(curIndex);
        }

        /// <summary>
        /// [复用模式] 刷新所有的复用项
        /// </summary>
        public void RefreshAllReuseItems()
        {
            firstShowIndex = -1;
            AdjustAllReuseItemsPosition(true);
        }
        #endregion

        #region 受保护方法

        /// <summary>
        /// [复用模式] 计算listItem大小以及每列或每行最多显示几个listItem
        /// </summary>
        protected void CaculateReuseParam()
        {
            if (!isReuse)
            {
                return;
            }

            ResetContent();

            Vector2 viewSize = viewport.rect.size;
            cellSizeWithSpace = reuseCellSize + space;
            // 计算单行单列可排最大个数
            if (vertical)
            {
                // 可用宽度 = 内容固定宽度 - 左右边距 + 间隔
                float canUseWidth = contentMaxSize.x - padding.left - padding.right + space.x;
                // 单行最大数量
                LineNumber = Mathf.FloorToInt(canUseWidth / cellSizeWithSpace.x);
                LineNumber = LineNumber > 0 ? LineNumber : 1;
                // 可显示最大行数
                int canShowLine = Mathf.CeilToInt(viewSize.y / cellSizeWithSpace.y) + 1;
                MaxShowCount = canShowLine * LineNumber;
            }
            else
            {
                // 可用高度 = 内容固定高度 - 上下边距 + 间隔
                float canUseHeight = contentMaxSize.y - padding.top - padding.bottom + space.x;
                // 单列最大数量
                LineNumber = Mathf.FloorToInt(canUseHeight / cellSizeWithSpace.y);
                LineNumber = LineNumber > 0 ? LineNumber : 1;
                // 可显示最大列数
                int canShowLine = Mathf.CeilToInt(viewSize.x / cellSizeWithSpace.x) + 1;
                MaxShowCount = canShowLine * LineNumber;
            }
        }

        /// <summary>
        /// [复用模式] 调整 content 实际大小
        /// </summary>
        protected void AdjustReuseContentSize()
        {
            int lineOrColumnNum = Mathf.CeilToInt((float)DataCount / LineNumber);
            if (vertical)
            {
                contentMaxSize.y = padding.top + lineOrColumnNum * cellSizeWithSpace.y - (lineOrColumnNum > 0 ? space.y : 0) + padding.bottom;
            }
            else
            {
                contentMaxSize.x = padding.left + lineOrColumnNum * cellSizeWithSpace.x - (lineOrColumnNum > 0 ? space.x : 0) + padding.right;
            }
            contentSize = contentMaxSize;
        }

        /// <summary>
        /// [复用模式] 获取显示中的第一个列表项索引值 和最后一个列表项索引值
        /// </summary>
        /// <returns>true:索引值有改变,false:没有改变</returns>
        protected bool RefreshFirstShowIndex()
        {
            if (!isReuse)
            {
                return false;
            }

            Vector2 contentPos = content.anchoredPosition;
            int line = 0;
            if (vertical)
            {
                line = Mathf.FloorToInt((contentPos.y - padding.top) / cellSizeWithSpace.y);
            }
            else
            {
                line = Mathf.FloorToInt((-contentPos.x - padding.left) / cellSizeWithSpace.x);
            }

            int tempShowIndex = (line <= 0) ? 0 : (line * LineNumber);
            bool isChange = tempShowIndex != firstShowIndex;
            if(isChange)
            {
                firstShowIndex = tempShowIndex;
            }
            lastShowIndex = firstShowIndex + MaxShowCount - 1;
            if (lastShowIndex >= DataCount)
            {
                lastShowIndex = DataCount - 1;
            }
            return isChange;
        }

        /// <summary>
        /// [复用模式] 添加列表项, 根据index
        /// </summary>
        /// <param name="index">索引值</param>
        protected void AddReuseItemByIndex(int index)
        {
            if (index < firstShowIndex || index > lastShowIndex)
            {
                return;
            }
            GameUIListItem item = InstantiateReuseItem();
            AddDisplayItem(item, index);
        }


        /// <summary>
        /// [复用模式] 实例一个模版项
        /// </summary>
        /// <returns></returns>
        protected GameUIListItem InstantiateReuseItem()
        {
            if (TemplateItem == null)
            {
                Debug.LogError("TemplateItem is null, need call InitItemForReuse first");
                return null;
            }

            GameUIListItem item = GetOneIdleItem();
            if (item == null)
            {
                item = GetOneUnuseItem();
            }
            if (item == null)
            {
                GameObject go = Instantiate(TemplateItem.gameObject);
                item = go.GetComponent<GameUIListItem>() ?? go.AddComponent<GameUIListItem>();

                return item;
            }

            return item;
        }


        /// <summary>
        /// [复用模式] 调整单个列表项位置
        /// </summary>
        /// <param name="item"></param>
        protected void AdjustReuseItemPostion(GameUIListItem item)
        {
            int index = item.Index;

            int line, column;
            Vector2 itemPos, itemSize;
            CalculateReuseItemTransformInfo(index, out line, out column, out itemPos, out itemSize);

            item.LineIndex = line;
            item.ColumnIndex = column;
            if (item.OnBeforeSetPosition(itemPos))
            {
                item.GetRectTrans().anchoredPosition = itemPos;
            }
        }

        /// <summary>
        /// [复用模式] 调整所有列表项位置
        /// </summary>
        /// <param name="adjustNoChangeItem">没有索引值变化的列表项是否重新调整位置, true:重新调整</param>
        protected void AdjustAllReuseItemsPosition(bool adjustNoChangeItem = false)
        {
            if (!RefreshFirstShowIndex())
            {
                return;
            }

            // 暂时把不在范围之内的列表项放在空闲列表中
            RemoveOverDisplayedList();

            // 添加没有显示的项到ShowingList中
            GameUIListItem item;
            for (int i = firstShowIndex; i <= lastShowIndex; i++)
            {
                // 没有列表项的, 根据index 添加一个
                if (!displayItems.TryGetValue(i, out item))
                {
                    AddReuseItemByIndex(i);
                }
                else if (adjustNoChangeItem)
                {
                    AdjustReuseItemPostion(item);
                }
            }
            RecycleIdleItems();
        }

        /// <summary>
        /// [复用模式] 获取一个复用列表项的位置,大小,行列值 信息
        /// </summary>
        /// <param name="index"></param>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        protected void CalculateReuseItemTransformInfo(int index, out int line, out int column, out Vector2 pos, out Vector2 size)
        {
            size = reuseCellSize;
            pos = Vector2.zero;
            // 判断index所在行列
            line = Mathf.FloorToInt(index / LineNumber);
            column = index % LineNumber;
            if (horizontal)
            {
                int temp = line;
                line = column;
                column = temp;
            }
            pos.x = padding.left + (column * (size.x + space.x));
            pos.y = -padding.top - (line * (size.y + space.y));
        }

        /// <summary>
        /// [复用模式] 根据开始索引值和最大显示数量, 把不在显示区域的项移除
        /// </summary>
        protected void RemoveOverDisplayedList()
        {
            tempDisplayItems.Clear();
            foreach (KeyValuePair<int, GameUIListItem> it in displayItems)
            {
                int itemIndex = it.Key;
                GameUIListItem item = it.Value;
                if (itemIndex < firstShowIndex || itemIndex > lastShowIndex)
                {
                    CacheDisplayItemToIdleList(item);
                }
                else
                {
                    tempDisplayItems.Add(item);
                }
            }

            RemoveAllSelectableItems();
            displayItems.Clear();
            
            for (int i = 0; i < tempDisplayItems.Count; i++)
            {
                GameUIListItem item = tempDisplayItems[i];
                displayItems.Add(item.Index, item);
                AddSelectableItem(item);
            }
            tempDisplayItems.Clear();
        }


        #endregion

    }
}
