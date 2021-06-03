using System;

using UnityEngine.Events;

namespace LightFramework.UI
{
    /// <summary>
    /// 可选项 组件基类
    /// </summary>
    public class UISelectableBaseItem : UIBaseControl
    {
        #region 受保护字段

        protected int selectableSign = -1;
        protected bool isSelected = false;

        #endregion

        #region 公有字段

        /// <summary>
        /// 选项的选择状态已经切换.
        /// [参数] true: 选中; false: 未选中.
        /// </summary>
        public UnityAction<bool> onSelectStateChanged;

        /// <summary>
        /// 切换选择状态前的回调函数，用来判断是否可以进行切换操作.
        /// [参数] 可选项 本身 UISelectableBaseItem.
        /// [返回值] true: 可以切换选择状态; false: 不可以切换选择状态.
        /// </summary>
        public Func<UISelectableBaseItem, bool> onBeforeToggleSelectState;

        #endregion

        #region 公有属性

        /// <summary>
        /// 选项的标识值，具有唯一性
        /// </summary>
        public virtual int SelectableSign
        {
            get { return selectableSign; }
            set { selectableSign = value; }
        }

        /// <summary>
        /// 是否被选中
        /// </summary>
        public virtual bool IsSelected
        {
            get { return isSelected; }
            set
            {
                OnSelectStateChanged(value);
                isSelected = value;
            }
        }


        /// <summary>
        /// 可选项处理器，需要赋值才可以执行选择处理程序
        /// </summary>
        public virtual UISelectableHandler SelectableHandler
        {
            get; set;
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 外部或者内部执行选择逻辑
        /// </summary>
        /// <param name="bSelected">true: 被选中; false: 未选中.</param>
        /// <returns>true: 执行选择逻辑成功; false: 失败.</returns>
        public virtual SelectableResultType DoSelect(bool bSelected)
        {
            if (bSelected)
            {
                bool canSelect = OnBeforeToggleSelectState();
                if (!canSelect)
                {
                    return SelectableResultType.CannotBeSelected;
                }
            }

            if (SelectableHandler != null)
            {
                return SelectableHandler.ChangeSelectState(SelectableSign, bSelected, true);
            }

            return SelectableResultType.NoHandler;
        }

        #endregion

        #region 受保护方法

        /// <summary>
        ///是否可以进行选择状态切换
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnBeforeToggleSelectState()
        {
            if (onBeforeToggleSelectState != null)
            {
                return onBeforeToggleSelectState.Invoke(this);
            }
            return true;
        }

        /// <summary>
        /// 列表项被选中
        /// </summary>
        protected virtual void OnSelectStateChanged(bool bSelected)
        {
            if (onSelectStateChanged != null)
            {
                onSelectStateChanged.Invoke(bSelected);
            }
        }

        #endregion
    }
}
