using System;

using UnityEngine;
using UnityEngine.Events;

namespace LightFramework.UI
{
    /// <summary>
    /// 列表项 组件
    /// </summary>
    public class GameUIListItem : UISelectableBaseItem
    {

        #region 私有字段

        private int index = -1;

        private bool added = false;

        #endregion

        #region 公有字段

        /// <summary>
        /// 列表项被添加显示列表或者从显示列表中移除
        /// 参数1: bAdd, true添加, false移除
        /// </summary>
        public UnityAction<bool> onItemAddOrRemove;

        /// <summary>
        /// 列表项的索引值被改变
        /// 参数1: index, 新的索引值 
        /// 参数2: preIndex, 改变前的索引值
        /// </summary>
        public UnityAction<int, int> onItemIndexChanged;

        /// <summary>
        /// 传入字符串, 进行列表项匹配, 符合信息返回true
        /// 参数1: matchStr, 校验参数字符串
        /// 返回值: 是否匹配成功
        /// </summary>
        public Func<string, bool> onMatchItem;

        /// <summary>
        /// 列表项被设置位置前的逻辑
        /// 参数1:要设置的位置
        /// 返回值: true:可以设置位置, false: 通过Position 进行自定义位置逻辑
        /// </summary>
        /// <example>
        /// 做列表飞入动作, 则可以执行动作后把位置设在指定的Position位置,  这时返回值返回false, 列表就不会自动设置该列表项的位置
        /// </example>
        public Func<Vector2, bool> onBeforeSetItemPosition;

        #endregion

        #region 公有属性

        /// <summary>
        /// 索引值
        /// </summary>
        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                if (index != value)
                {
                    int preIndex = index;
                    index = value;
                    OnIndexChanged(value, preIndex);
                }
            }
        }

        /// <summary>
        /// 选项可选标识
        /// </summary>
        public override int SelectableSign
        {
            get
            {
                return index;
            }
            set
            {
                selectableSign = value;
            }
        }

        /// <summary>
        /// 添加在 listview的显示列表中, true:添加,false:移除
        /// </summary>
        public bool IsAdded
        {
            get { return added; }
            set
            {
                if (added != value)
                {
                    added = value;
                    OnAddOrRemove(value);
                }
            }
        }

        /// <summary>
        /// 该列表项所处在的行索引, 从0开始
        /// </summary>
        public int LineIndex
        {
            get;set;
        }

        /// <summary>
        /// 该列表项所处在的列索引, 从0开始
        /// </summary>
        public int ColumnIndex
        {
            get;set;
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 根据传入的参数, 校验列表项是否符合参数要求
        /// 常用来 从listView中获取符合某个条件的列表项实例对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual bool MatchData(string data)
        {
            if (onMatchItem != null)
            {
                return onMatchItem.Invoke(data);
            }
            return false;
        }

        /// <summary>
        /// 设置位置之前, 可以通过保存的Position参数来进行飞入等动作
        /// </summary>
        /// <returns>返回false: 使用自己的设置列表位置逻辑, 返回false:使用默认的设置位置逻辑</returns>
        public virtual bool OnBeforeSetPosition(Vector2 pos)
        {
            if(onBeforeSetItemPosition != null)
            {
                return onBeforeSetItemPosition.Invoke(pos);
            }
            return true;
        }

        #endregion

        #region 受保护方法
        /// <summary>
        /// 列表项被添加到list或者被移除
        /// </summary>
        /// <param name="bAdd"></param>
        protected virtual void OnAddOrRemove(bool bAdd)
        {
            if (onItemAddOrRemove != null)
            {
                onItemAddOrRemove.Invoke(added);
            }
        }

        /// <summary>
        /// index被改变, 包括第一次index赋值
        /// </summary>
        protected virtual void OnIndexChanged(int index, int preIndex)
        {
            if (onItemIndexChanged != null)
            {
                onItemIndexChanged.Invoke(index, preIndex);
            }
        }

        #endregion
    }
}


