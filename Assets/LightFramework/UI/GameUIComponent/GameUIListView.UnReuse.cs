using System.Collections.Generic;
using UnityEngine;

namespace LightFramework.UI
{
    /// <summary>
    /// GameUIListView 非复用列表
    /// </summary>
    partial class GameUIListView
    {

        #region 公有方法 非复用模式

        /// <summary>
        /// [非复用模式] 根据 T 添加一个不复用的列表项
        /// 想对于 函数: AddItemForUnReuse<T>(GameObject obj) 
        /// 它只是 参数少加 一个 gameobject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns>返回带有 T 的 组件 </returns>
        public T AddUnReuseItem<T>(Component comp, int index = -1) where T : GameUIListItem, new()
        {
            if (comp == null || comp.gameObject == null)
            {
                Debug.LogWarning("listview unreuse gameobject is null : " + typeof(T));
                return null;
            }

            if (isReuse)
            {
                Debug.LogError("need 'isReuse' is false");
                return null;
            }

            int curIndex = DataCount;
            if (index >= 0)
            {
                curIndex = index;
            }
            DataCount++;
            T t = InstantiateUnReuseItem<T>(comp.gameObject);
            AddDisplayItem(t, curIndex);
            // 刷新content组件宽高
            contentSize = contentMaxSize;
            return t;
        }

        /// <summary>
        /// [非复用模式] 添加一个不复用的列表项, 使用默认的 GameUIListItem 为列表项的组件
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public GameUIListItem AddUnReuseItemWithDefault(Component comp, int index = -1)
        {
            return AddUnReuseItem<GameUIListItem>(comp, index);
        }

        /// <summary>
        /// [非复用模式] 移除一条列表项
        /// </summary>
        /// <param name="index"></param>
        /// <remarks>
        /// 删除一个列表项实例 适合非复用模式, 复用模式可以先删除数据源中的数据项然后再刷新列表
        /// </remarks>
        public void RemoveUnReuseItem(int index)
        {
            if (isReuse)
            {
                Debug.LogWarning("删除一个列表项实例 适合非复用模式, 复用模式可以先删除数据源中的数据项然后再刷新列表");
                return;
            }

            if (!displayItems.ContainsKey(index))
            {
                return;
            }

            tempDisplayItems.Clear();
            foreach (KeyValuePair<int, GameUIListItem> it in displayItems)
            {
                if (it.Key == index)
                {
                    RecycleListItem(it.Value);
                }
                else
                {
                    tempDisplayItems.Add(it.Value);
                }
            }
            RemoveAllSelectableItems();
            displayItems.Clear();

            for (int i = 0; i < tempDisplayItems.Count; i++)
            {
                ChangeItemIndex(tempDisplayItems[i], i);
            }
            tempDisplayItems.Clear();

            DataCount--;

            // 刷新content组件宽高
            if(DataCount <= 0)
            {
                ResetContentMaxSize();
            }
            contentSize = contentMaxSize;
        }

        /// <summary>
        /// [非复用模式] 移除目标项
        /// </summary>
        /// <param name="listItem"></param>
        public void RemoveUnReuseItem(GameUIListItem listItem)
        {
            RemoveUnReuseItem(listItem.Index);
        }

        #endregion

        #region 受保护方法 非复用模式

        /// <summary>
        /// [非复用模式] 实例化复制一份目标gameobject为列表项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected T InstantiateUnReuseItem<T>(GameObject obj) where T : GameUIListItem, new()
        {
            if (obj == null)
            {
                Debug.LogWarning("listview unreuse param gameobject is null : " + typeof(T));
                return null;
            }
            if (obj.activeSelf)
            {
                obj.gameObject.SetActive(false);
            }

            GameUIListItem item = GetOneUnuseItem(typeof(T));
            if (item == null)
            {
                GameObject go = Instantiate(obj);
                T t = go.GetComponent<T>() ?? go.AddComponent<T>();

                return t;
            }

            return item as T;
        }

        #endregion

        #region 受保护方法 非复用模式

        /// <summary>
        /// [非复用模式] 调整列表项位置
        /// </summary>
        /// <param name="item"></param>
        protected void AdjustUnReuseItemPosition(GameUIListItem item)
        {
            if (vertical)
            {
                AdjustUnReuseItemPositionVertical(item);
            }
            else
            {
                AdjustUnReuseItemPositionHorizontal(item);
            }
        }

        /// <summary>
        /// [非复用模式] 调整所有列表项的位置
        /// </summary>
        protected void AdjustAllUnReuseItemsPostion()
        {
            foreach (GameUIListItem item in displayItems.Values)
            {
                AdjustItemPosition(item);
            }
            contentSize = contentMaxSize;
        }

        /// <summary>
        /// [非复用模式] 竖直向滑动的列表, 调整列表项位置
        /// </summary>
        /// <param name="item"></param>
        protected void AdjustUnReuseItemPositionVertical(GameUIListItem item)
        {
            int index = item.Index;
            RectTransform rectTrf = item.GetRectTrans();
            Vector2 itemSize = rectTrf.GetSizeWithScale();

            int line, column;
            Vector2 prePos, preSize;
            CalculateItemTransformInfo(index - 1, out line, out column, out prePos, out preSize);

            Vector2 itemPos = Vector2.zero;
            // 已经使用的宽度=前一个项位置+大小+间距 //
            float usedWidth = prePos.x + preSize.x + space.x + padding.left;
            // 剩余空间 //
            float remainSpace = contentMaxSize.x - usedWidth - padding.right;
            if (index > 0 && remainSpace >= itemSize.x)
            {
                itemPos.Set(usedWidth, prePos.y);
                column++;
            }
            else if (index == 0)
            {
                itemPos.Set(padding.left, -padding.top);
            }
            else
            {
                itemPos.Set(padding.left, prePos.y - preSize.y - space.y);
                line++;
                column = 0;
            }
            float maxHeight = -itemPos.y + itemSize.y + padding.bottom;
            contentMaxSize.y = maxHeight;
            item.LineIndex = line;
            item.ColumnIndex = column;
            if (item.OnBeforeSetPosition(itemPos))
            {
                rectTrf.anchoredPosition = itemPos;
            }
        }

        /// <summary>
        /// [非复用模式] 水平向滑动的列表, 调整列表项位置
        /// </summary>
        /// <param name="item"></param>
        protected void AdjustUnReuseItemPositionHorizontal(GameUIListItem item)
        {
            int index = item.Index;
            RectTransform rectTrf = item.GetRectTrans();
            Vector2 itemSize = rectTrf.GetSizeWithScale();

            int line, column;
            Vector2 prePos, preSize;
            CalculateItemTransformInfo(index - 1, out line, out column, out prePos, out preSize);

            Vector2 itemPos = Vector2.zero;
            float usedHeight = -prePos.y + preSize.y + space.y + padding.top;
            float remainSpace = contentMaxSize.y - usedHeight - padding.bottom;
            if (index > 0 && remainSpace >= itemSize.y)
            {
                itemPos.Set(prePos.x, -usedHeight);
                line++;
            }
            else if (index == 0)
            {
                itemPos.Set(padding.left, -padding.top);
            }
            else
            {
                itemPos.Set(contentMaxSize.x + space.x - padding.left, -padding.top);
                line = 0;
                column++;
            }
            float maxWidth = itemPos.x + itemSize.x + padding.right;
            contentMaxSize.x = maxWidth;

            item.LineIndex = line;
            item.ColumnIndex = column;
            if (item.OnBeforeSetPosition(itemPos))
            {
                rectTrf.anchoredPosition = itemPos;
            }
        }


        /// <summary>
        /// [非复用模式] 获取一个非复用列表项的位置,大小,行列值 信息
        /// </summary>
        /// <param name="index"></param>
        /// <param name="line">行 从0开始的</param>
        /// <param name="column">列 从0开始的</param>
        /// <param name="pos">位置</param>
        /// <param name="size">大小</param>
        protected void CalculateUnReuseItemTransformInfo(int index, out int line, out int column, out Vector2 pos, out Vector2 size)
        {
            if(index >= 0)
            {
                GameUIListItem item;
                if (displayItems.TryGetValue(index, out item))
                {
                    RectTransform rectTrf = item.GetRectTrans();
                    line = item.LineIndex;
                    column = item.ColumnIndex;
                    pos = rectTrf.anchoredPosition;
                    size = rectTrf.GetSizeWithScale();
                    return;
                }
            }
            
            line = 0;
            column = 0;
            pos = Vector2.zero;
            size = Vector2.zero;
        }
        #endregion

    }
}
