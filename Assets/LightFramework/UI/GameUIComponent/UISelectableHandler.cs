using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace LightFramework.UI
{
    /// <summary>
    /// 可选项 处理器
    /// </summary>
    public class UISelectableHandler : GameUIBaseComponent
    {
        #region 私有字段

        /// <summary>
        /// [最大可选中个数] 0: 不能选中; 1: 单选; 大于1: 多选.
        /// </summary>
        [SerializeField]
        private int maxSelectNum = 1;

        /// <summary>
        /// 允许可以取消最后一个选中
        /// </summary>
        [SerializeField]
        private bool allowOffLast = true;

        /// <summary>
        /// 选项实体字典
        /// </summary>
        protected Dictionary<int, UISelectableBaseItem> selectableItemDic = new Dictionary<int, UISelectableBaseItem>();

        protected List<int> selectedSigns = new List<int>();
        #endregion

        #region 公有字段

        /// <summary>
        /// 选项选择状态发生变化 事件回调
        /// </summary>
        public UnityAction onSelectStateChanged;

        #endregion

        #region 公有属性

        /// <summary>
        /// 最大可选中个数, 0: 不能选中; 1: 单选; 大于1: 多选.
        /// </summary>
        public int MaxSelectNum
        {
            get { return maxSelectNum; }
            set { maxSelectNum = value; }
        }

        /// <summary>
        /// 是否可以取消最后一个选中
        /// </summary>
        public bool AllowOffLast
        {
            get { return allowOffLast; }
            set { allowOffLast = value; }
        }

        /// <summary>
        /// 选中的标识列表, 实际以该数据为准
        /// </summary>
        public List<int> SelectedSigns
        {
            get { return selectedSigns; }
        }

        /// <summary>
        /// 选中的选项
        /// </summary>
        public Dictionary<int, UISelectableBaseItem> SelectableItems
        {
            get { return selectableItemDic ; }
        }

        /// <summary>
        /// 选中的选项数量是否已达到上限
        /// </summary>
        public bool IsMaxSelectNum
        {
            get { return MaxSelectNum <= 0 || selectedSigns.Count >= MaxSelectNum; }
        }

        #endregion

        #region 公有方法 添加或移除选项

        /// <summary>
        /// 添加选项
        /// </summary>
        /// <param name="item">要添加的选项</param>
        /// <param name="initSelectedState">是否初始化选择状态</param>
        public void AddSelectableItem(UISelectableBaseItem item, bool initSelectedState = true)
        {
            if (initSelectedState)
            {
                item.IsSelected = IsSelectedBySign(item.SelectableSign);
            }

            item.SelectableHandler = this;
            selectableItemDic[item.SelectableSign] = item;
        }

        /// <summary>
        /// 移除选项, 但不改变选项本身的选中状态
        /// </summary>
        /// <param name="item">要移除的选项</param>
        /// <param name="removeSelectedState">移除item的同时, 是否移除对应保存的选中状态</param>
        public void RemoveSelectableItem(UISelectableBaseItem item, bool removeSelectedState = true)
        {
            if (removeSelectedState && item.IsSelected)
            {
                RemoveSelectedSign(item.SelectableSign);
            }

            item.SelectableHandler = null;
            selectableItemDic.Remove(item.SelectableSign);
        }

        /// <summary>
        /// 移除所有选项, 但不改变选项本身的选中状态
        /// </summary>
        /// <param name="removeSelectedState">移除item的同时, 是否移除对应保存的选中状态</param>
        public void RemoveAllSelectableItems(bool removeSelectedState = true)
        {
            if (removeSelectedState)
            {
                RemoveAllSelectedSigns();
            }

            selectableItemDic.Clear();

        }
        #endregion

        #region 公有方法 添加或移除选项标识

        /// <summary>
        /// 根据 选项标识值 判断是否选中
        /// </summary>
        /// <param name="selectableSign">可选项标识</param>
        /// <returns></returns>
        public bool IsSelectedBySign(int selectableSign)
        {
            if (selectedSigns.Count == 0)
            {
                return false;
            }

            return selectedSigns.Contains(selectableSign);
        }

        /// <summary>
        /// 设置已选项标识
        /// </summary>
        /// <param name="selectedSign">已选项标识</param>
        /// <param name="notifyChange">选中项变动是否通知 选择状态变更函数 </param>
        /// <returns></returns>
        public SelectableResultType SetSelectedSign(int selectedSign, bool notifyChange = false)
        {
            return ChangeSelectState(selectedSign, true, notifyChange);
        }

        /// <summary>
        /// 设置一组已选项标识
        /// </summary>
        /// <param name="selectedSignList">已选项标识 列表</param>
        /// <param name="notifyChange">选中项变动是否通知 选择状态变更函数 </param>
        public SelectableResultType SetSelectedSigns(List<int> selectedSignList, bool notifyChange = false)
        {
            if(selectedSignList == null || selectedSignList.Count <= 0)
            {
                return SelectableResultType.None;
            }

            SelectableResultType result = SelectableResultType.Success;
            for (int i = 0; i < selectedSignList.Count; i++)
            {
                SelectableResultType tempRet = ChangeSelectState(selectedSignList[i], true, notifyChange);
                if (tempRet != SelectableResultType.Success)
                {
                    result = tempRet;
                }
            }

            return result;
        }

        /// <summary>
        /// 设置一组已选项标识
        /// </summary>
        /// <param name="selectedSignArray">已选项标识 数组</param>
        /// <param name="notifyChange">选中项变动是否通知 选择状态变更函数 </param>
        public SelectableResultType SetSelectedSigns(int[] selectedSignArray, bool notifyChange = false)
        {
            if (selectedSignArray == null || selectedSignArray.Length <= 0)
            {
                return SelectableResultType.None;
            }

            SelectableResultType result = SelectableResultType.Success;
            for (int i = 0; i < selectedSignArray.Length; i++)
            {
                SelectableResultType tempRet = ChangeSelectState(selectedSignArray[i], true, notifyChange);
                if (tempRet != SelectableResultType.Success)
                {
                    result = tempRet;
                }
            }

            return result;
        }

        /// <summary>
        /// 根据选项标识, 尝试选中或者取消一个选项
        /// </summary>
        /// <param name="selectableSign">可选项标识</param>
        /// <param name="selected">是否选中</param>
        /// <param name="notifyChange">选中项变动是否通知 选择状态变更函数</param>
        /// <returns></returns>
        public SelectableResultType ChangeSelectState(int selectableSign, bool selected, bool notifyChange)
        {
            if (selected && MaxSelectNum <= 0)
            {
                return SelectableResultType.MaxSelectNumZero;
            }

            SelectableResultType ret;
            // 选中前的公用检测逻辑
            if (MaxSelectNum == 1)
            {
                ret = BeforeSelectOne(selectableSign, selected);
            }
            else
            {
                ret = BeforeSelectMulti(selectableSign, selected);
            }

            if (ret != SelectableResultType.Success)
            {
                return ret;
            }

            ret = AddOrRemoveSelectableSign(selectableSign, selected);
            if (ret == SelectableResultType.Success && notifyChange)
            {
                InvokeSelectChangeAction();
            }

            return ret;
        }

        /// <summary>
        /// 移除已选项标识, 但不改变选项本身的选中状态
        /// </summary>
        /// <param name="selectedSign">已选项标识</param>
        public void RemoveSelectedSign(int selectedSign)
        {
            selectedSigns.Remove(selectedSign);
        }

        /// <summary>
        /// 移除所有已选项标识, 但不改变选项本身的选中状态
        /// </summary>
        public void RemoveAllSelectedSigns()
        {
            selectedSigns.Clear();
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 清除所有选项以及保存的选中标识, 但不取消所有选项的选中状态
        /// </summary>
        public void ClearAll()
        {
            selectableItemDic.Clear();
            selectedSigns.Clear();
        }

        /// <summary>
        /// 取消所有选项的选中状态
        /// </summary>
        /// <param name="notifyChange">选中项变动是否通知 选择状态变更函数</param>
        public void CancelAllSelectedItems(bool notifyChange = false)
        {
            int selectedCount = selectedSigns.Count;
            if (selectedCount <= 0)
            {
                return;
            }

            UISelectableBaseItem item = null;
            for (int i = 0; i < selectedCount; i++)
            {
                if (selectableItemDic.TryGetValue(selectedSigns[i], out item))
                {
                    item.IsSelected = false;
                }
            }

            selectedSigns.Clear();

            if (notifyChange)
            {
                InvokeSelectChangeAction();
            }
        }

        /// <summary>
        /// 取消选中状态, 除 excludeSign 之外的所有选项
        /// </summary>
        /// <param name="excludeSign">要排除的选项标识</param>
        /// <param name="notifyChange">选中项变动是否通知 选择状态变更函数</param>
        public void CancelSelectedItemsWithoutSpecifiedSign(int excludeSign, bool notifyChange = false)
        {
            int selectedCount = selectedSigns.Count;
            if (selectedCount <= 0)
            {
                return;
            }

            bool hasExcludeSign = false;
            UISelectableBaseItem item = null;
            for (int i = 0; i < selectedCount; i++)
            {
                if (selectedSigns[i] == excludeSign)
                {
                    hasExcludeSign = true;
                    continue;
                }

                if (selectableItemDic.TryGetValue(selectedSigns[i], out item))
                {
                    item.IsSelected = false;
                }
            }

            selectedSigns.Clear();
            if (hasExcludeSign)
            {
                selectedSigns.Add(excludeSign);
            }

            // 有选择状态变化: selectedCount 大于1, 或者 selectedCount==1时, 不是要排除的选项标识
            if (notifyChange && (!hasExcludeSign || selectedCount > 1))
            {
                InvokeSelectChangeAction();
            }
        }

        #endregion

        #region 受保护方法

        /// <summary>
        /// 单选:根据索选项标识值, 选中或取消一个选项前的公用检测逻辑
        /// </summary>
        /// <param name="selectableSign">可选项标识</param>
        /// <param name="selected">是否选中</param>
        /// <returns></returns>
        protected SelectableResultType BeforeSelectOne(int selectableSign, bool selected)
        {
            if (!selected)
            {
                // 是否允许取消选中最后一个项
                if (!allowOffLast && selectedSigns.Count == 1)
                {
                    return SelectableResultType.CannotUnselectLast;
                }
                CancelAllSelectedItems();
                return SelectableResultType.Success;
            }

            if (selectedSigns.Count == 0)
            {
                return SelectableResultType.Success;
            }

            CancelAllSelectedItems();

            return SelectableResultType.Success;
        }

        /// <summary>
        /// 多选:根据索选项标识值, 选中或取消一个选项前的公用检测逻辑
        /// </summary>
        /// <param name="selectableSign">可选项标识</param>
        /// <param name="selected">是否选中</param>
        /// <returns></returns>
        protected SelectableResultType BeforeSelectMulti(int selectableSign, bool selected)
        {
            if (!selected)
            {
                // 是否允许取消选中最后一个项
                if (!allowOffLast && selectedSigns.Count == 1)
                {
                    return SelectableResultType.CannotUnselectLast;
                }
                return SelectableResultType.Success;
            }

            if (selectedSigns.Count == 0)
            {
                return SelectableResultType.Success;
            }

            if (selectedSigns.Count >= MaxSelectNum)
            {
                return SelectableResultType.OverMaxSelectNum;
            }

            return SelectableResultType.Success;
        }

        /// <summary>
        /// 选中或者取消一个选项, 会改变选项的选中状态
        /// </summary>
        /// <param name="selectableSign">可选项标识</param>
        /// <param name="bAdd">是否添加</param>
        /// <returns></returns>
        protected SelectableResultType AddOrRemoveSelectableSign(int selectableSign, bool bAdd)
        {
            UISelectableBaseItem item = null;
            if (!bAdd)
            {
                if (selectedSigns.Remove(selectableSign))
                {
                    if (selectableItemDic.TryGetValue(selectableSign, out item))
                    {
                        item.IsSelected = false;
                    }
                }
                return SelectableResultType.Success;
            }

            if (selectedSigns.Contains(selectableSign))
            {
                return SelectableResultType.RepeatedSelect;
            }

            selectedSigns.Add(selectableSign);
            if (selectableItemDic.TryGetValue(selectableSign, out item))
            {
                item.IsSelected = true;
            }

            return SelectableResultType.Success;
        }

        /// <summary>
        /// 派发选项更改消息
        /// </summary>
        protected void InvokeSelectChangeAction()
        {
            if (onSelectStateChanged != null)
            {
                onSelectStateChanged.Invoke();
            }
        }

        protected override void Reset()
        {
            selectableItemDic.Clear();
            selectedSigns.Clear();
        }
        #endregion
    }
}

