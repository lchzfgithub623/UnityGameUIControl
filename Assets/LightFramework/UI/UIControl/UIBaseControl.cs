using System.Collections.Generic;

using UnityEngine;

namespace LightFramework.UI
{
    /// <summary>
    /// 组件管理器
    /// </summary>
    public class UIBaseControl : UIMono
    {
        #region 私有字段

        private Dictionary<string, GameUIBaseComponent> ctrlDic;

        // (Clone)
        private static readonly char[] cloneCharArr = { '(', 'C', 'l', 'o', 'n', 'e', ')' };

        #endregion

        #region 公有属性

        /// <summary>
        /// 是否初始化组件
        /// </summary>
        public bool IsInitControl
        {
            get; protected set;
        }

        /// <summary>
        /// 获取ui组件
        /// </summary>
        /// <param name="uiCtrlName"></param>
        /// <returns></returns>
        public GameUIBaseComponent this[string uiCtrlName]
        {
            get
            {
                if (!IsInitControl)
                {
                    InitControls(false);
                }
                GameUIBaseComponent ctrlBase;
                ctrlDic.TryGetValue(uiCtrlName, out ctrlBase);
                return ctrlBase;
            }
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 根据名字获取组件对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameUIBaseComponent GetCtrl(string name)
        {
            return this[name];
        }

        /// <summary>
        /// 根据名字获取对应类型的组件对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetCtrl<T>(string name) where T : GameUIBaseComponent
        {
            GameUIBaseComponent uiCtrl = this[name];
            if (uiCtrl == null)
            {
                return null;
            }
            return uiCtrl as T;
        }

        /// <summary>
        /// 设置组件对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="t"></param>
        public void SetCtrl<T>(string name, ref T t) where T:GameUIBaseComponent
        {
            t = GetCtrl<T>(name);
        }

        /// <summary>
        /// 重置初始化
        /// </summary>
        public virtual void ResetInit()
        {
            if (IsApplicationQuit)
            {
                return;
            }

            IsInitControl = false;
            if (ctrlDic != null)
            {
                ctrlDic.Clear();
            }
            InitControls(true);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 初始化组件
        /// </summary>
        /// <param name="bReset"></param>
        protected virtual void InitControls(bool bReset)
        {
            IsInitControl = true;

            GameUIBaseComponent[] uiCtrls = GetComponentsInChildren<GameUIBaseComponent>(true);
            for (int i = 0; i < uiCtrls.Length; i++)
            {
                GameUIBaseComponent item = uiCtrls[i];
                string ctrlName = item.name;
                AddCtrl(ctrlName, item, bReset);
            }

            GameUIBaseComponent selfCtrl = gameObject.GetComponent<GameUIBaseComponent>();
            if(selfCtrl != null)
            {
                AddCtrl(selfCtrl.name.TrimEnd(cloneCharArr), selfCtrl, bReset);
            }
        }

        /// <summary>
        /// 添加一个ui组件对象
        /// </summary>
        /// <param name="uiCtrl"></param>
        /// <param name="bRest"></param>
        private void AddCtrl(string ctrlName, GameUIBaseComponent uiCtrl, bool bRest)
        {
            if (ctrlDic == null)
            {
                ctrlDic = new Dictionary<string, GameUIBaseComponent>();
            }
            if (ctrlDic.ContainsKey(ctrlName))
            {
                Debug.LogWarning("check prefab: has duplicate control name [" + ctrlName + "] in " + name);
            }
            ctrlDic[ctrlName] = uiCtrl;
            if (bRest)
            {
                uiCtrl.ResetComponent();
            }
        }
        #endregion
    }
}


