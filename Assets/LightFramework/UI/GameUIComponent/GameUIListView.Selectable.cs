
namespace LightFramework.UI
{
    partial class GameUIListView
    {
        #region 私有字段

        private UISelectableHandler selectableHandler;

        #endregion

        #region 公有属性

        /// <summary>
        /// 列表项是否可选
        /// </summary>
        public bool IsOptional
        {
            get { return isOptional; }
            set
            {
                if (isOptional != value)
                {
                    isOptional = value;

                    RefreshSelectable();
                }
            }
        }

        /// <summary>
        /// 可选项辅助器
        /// IsOptional = true 有效
        /// </summary>
        public UISelectableHandler SelectHander
        {
            get
            {
                if(isOptional && selectableHandler == null)
                {
                    selectableHandler = gameObject.GetOrAddComponent<UISelectableHandler>();
                    InitSelectableItems();
                }
                return selectableHandler;
            }
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 刷新列表项是否可选
        /// </summary>
        public void RefreshSelectable()
        {
            if (isOptional)
            {
                SelectHander.enabled = true;
            }
            else if (SelectHander != null)
            {
                SelectHander.enabled = false;
            }
        }

        #endregion

        #region 受保护方法 添加或删除选项
        /// <summary>
        /// 初始化现有的可选项
        /// </summary>
        protected void InitSelectableItems()
        {
            if (IsOptional)
            {
                SelectHander.ClearAll();
                for (int i = 0; i < displayItems.Values.Count; i++)
                {
                    SelectHander.AddSelectableItem(displayItems.Values[i], false);
                }
            }
        }

        /// <summary>
        /// 添加选项
        /// </summary>
        /// <param name="item"></param>
        protected void AddSelectableItem(GameUIListItem item)
        {
            if (IsOptional)
            {
                SelectHander.AddSelectableItem(item);
            }
        }

        /// <summary>
        /// 移除选项
        /// </summary>
        /// <param name="item"></param>
        protected void RemoveSelectableItem(GameUIListItem item)
        {
            if (IsOptional)
            {
                SelectHander.RemoveSelectableItem(item, !isReuse);
            }
        }

        /// <summary>
        /// 移除所有选项
        /// </summary>
        protected void RemoveAllSelectableItems()
        {
            if (IsOptional)
            {
                SelectHander.RemoveAllSelectableItems(!isReuse);
            }
        }
        #endregion

        /// <summary>
        /// 清除可选项数据
        /// </summary>
        protected void ClearSelectableData()
        {
            if (IsOptional)
            {
                SelectHander.ClearAll();
            }
        }
    }
}
