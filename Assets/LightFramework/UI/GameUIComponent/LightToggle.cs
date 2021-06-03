using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace LightFramework.UI
{
    /// <summary>
    /// 封装的开关组件
    /// </summary>
    [AddComponentMenu("UI/GameUI/LightToggle", 31)]
    public class LightToggle : Toggle
    {
        #region 公有字段

        /// <summary>
        /// 切换选择状态前, 检测是否可以进行切换的函数
        /// 参数: 当前的 Toggle 组件, 返回值:true可以切换, false:不可以切换
        /// </summary>
        public Func<Toggle, bool> CheckCanToggleFunc;

        #endregion

        #region 公有方法

        /// <summary>
        /// 设置选中
        /// 该逻辑会在切换选择状态前执行检测函数 CheckCanToggleFunc
        /// </summary>
        /// <param name="isOn">true:选中, false:非选中</param>
        /// <returns>false:不能执行操作, true:执行操作成功</returns>
        public bool SetIsOn(bool isOn)
        {
            bool ret = true;
            if (CheckCanToggleFunc != null)
            {
                ret = CheckCanToggleFunc(this);
            }
            if (ret)
            {
                this.isOn = isOn;
            }
            return ret;
        }

        #endregion

        #region 重写方法

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (CheckCanToggle())
            {
                InternalToggleCustom();
            }
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            if (CheckCanToggle())
            {
                InternalToggleCustom();
            }
        }

        #endregion

        #region 受保护方法

        /// <summary>
        /// 检测是否可以切换
        /// </summary>
        /// <returns>true:可以; false:不可以</returns>
        protected bool CheckCanToggle()
        {
            if (!IsActive() || !IsInteractable())
                return false;
            bool curIsOn = isOn;
            if (CheckCanToggleFunc != null)
            {
                return CheckCanToggleFunc(this);
            }
            return true;
        }

        /// <summary>
        /// 反置 isOn
        /// </summary>
        protected void InternalToggleCustom()
        {
            isOn = !isOn;
        }

        #endregion
    }
}

