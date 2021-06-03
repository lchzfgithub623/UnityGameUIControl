using System;
using System.Collections.Generic;

using UnityEngine;

namespace LightFramework.UI
{
    /// <summary>
    /// GameUIListView 列表项的缓存池
    /// </summary>
    partial class GameUIListView
    {
        #region 私有字段

        /// <summary>
        /// 缓存池组件 Transform
        /// </summary>

        private Transform _poolTrf;
        private const string Pool_Transform_Name = "UnuseItemPool";

        /// <summary>
        /// 放在缓存池中未使用的列表项
        /// </summary>
        private List<GameUIListItem> unuseItems = new List<GameUIListItem>();

        /// <summary>
        /// 从显示列表中暂时移除出来的空闲列表项, 还没有放在缓存池中
        /// </summary>
        private List<GameUIListItem> idleItems = new List<GameUIListItem>();

        #endregion

        #region 公有属性

        /// <summary>
        /// 缓存池 Transform
        /// </summary>
        public Transform poolTrf
        {
            get
            {
                if (_poolTrf == null)
                {
                    _poolTrf = new GameObject(Pool_Transform_Name).transform;
                    _poolTrf.gameObject.SetActive(false);
                    _poolTrf.SetParent(transform, false);
                }
                return _poolTrf;
            }
        }

        /// <summary>
        /// 未使用的列表项
        /// </summary>
        public List<GameUIListItem> UnuseItems
        {
            get { return unuseItems; }
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 清除所有缓存的列表选项
        /// </summary>
        public void ClearUnusePool()
        {
            unuseItems.Clear();
            if (_poolTrf != null)
            {
                _poolTrf.DestroyAllChildren();
            }
        }

        #endregion

        #region 受保护方法 回收列表项

        /// <summary>
        /// 把一个列表项放到缓存池中
        /// </summary>
        /// <param name="item"></param>
        protected void RecycleListItem(GameUIListItem item)
        {
            item.IsAdded = false;
            item.Index = -1;
            item.LineIndex = 0;
            item.ColumnIndex = 0;

            unuseItems.Add(item);
            item.transform.SetParent(poolTrf, false);
        }

        /// <summary>
        /// 把显示中的列表项全部放到缓存池中 
        /// </summary>
        protected void RecycleDisplayItems()
        {
            if (displayItems.Count <= 0)
            {
                return;
            }

            for (int i = 0; i < displayItems.Count; i++)
            {
                RecycleListItem(displayItems.Values[i]);
            }

            // 清空 显示列表
            RemoveAllSelectableItems();
            displayItems.Clear();
        }

        /// <summary>
        /// 把空闲中的列表项全部放到缓存池中
        /// </summary>
        protected void RecycleIdleItems()
        {
            for (int i = 0; i < idleItems.Count; i++)
            {
                RecycleListItem(idleItems[i]);
            }

            // 清空 空闲列表
            idleItems.Clear();
        }

        /// <summary>
        /// 把显示中的列表项缓存到空闲列表中
        /// </summary>
        /// <param name="item"></param>
        protected void CacheDisplayItemToIdleList(GameUIListItem item)
        {
            item.IsAdded = false;
            idleItems.Add(item);
        }

        #endregion

        #region 受保护方法 获取一个列表项

        /// <summary>
        /// 根据类型获取一个未使用的列表项
        /// </summary>
        /// <param name="requieType"></param>
        /// <returns></returns>
        protected GameUIListItem GetOneUnuseItem(Type requieType)
        {
            if (unuseItems.Count <= 0)
            {
                return null;
            }
            int index = unuseItems.Count - 1;
            while (index >= 0)
            {
                GameUIListItem item = unuseItems[index];
                if (item.GetType() == requieType)
                {
                    unuseItems.RemoveAt(index);
                    return item;
                }
                index--;
            }
            return null;
        }

        /// <summary>
        /// 获取一个未使用的列表项
        /// </summary>
        /// <returns></returns>
        protected GameUIListItem GetOneUnuseItem()
        {
            if (unuseItems.Count <= 0)
            {
                return null;
            }
            int index = unuseItems.Count - 1;
            GameUIListItem item = unuseItems[index];
            unuseItems.RemoveAt(index);
            return item;
        }

        /// <summary>
        /// 获取一个空闲的列表项
        /// </summary>
        /// <returns></returns>
        protected GameUIListItem GetOneIdleItem()
        {
            if (idleItems.Count <= 0)
            {
                return null;
            }
            int index = idleItems.Count - 1;
            GameUIListItem item = idleItems[index];
            idleItems.RemoveAt(index);
            return item;
        }

        #endregion

        #region 受保护方法

        /// <summary>
        /// 移除选项列表
        /// </summary>
        protected void RemoveUnusePool()
        {
            ClearUnusePool();
            _poolTrf = null;
            Transform unusePool = transform.Find(Pool_Transform_Name);
            if (unusePool != null)
            {
                Destroy(unusePool.gameObject);
            }
        }

        #endregion
    }
}
