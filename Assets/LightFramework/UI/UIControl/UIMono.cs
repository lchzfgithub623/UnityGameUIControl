using UnityEngine;

namespace LightFramework.UI
{
    public class UIMono : GameMonoBehaviour
    {
        #region 公有属性

        /// <summary>
        /// 是否Awake
        /// </summary>
        public bool IsAwake { get; private set; }

        /// <summary>
        /// 是否Start
        /// </summary>
        public bool IsStart { get; private set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        bool IsShow { get; set; }

        /// <summary>
        /// 是否Enable
        /// </summary>
        public bool IsEnable { get; set; }

        #endregion

        #region 虚函数

        /// <summary>
        /// awake初始化: 获取控件, 初始化变量或游戏状态
        /// </summary>
        /// <remarks>
        /// OnInit 总是在 OnStart 前调用
        /// </remarks>
        protected virtual void OnInit()
        {
        }

        /// <summary>
        /// start初始化: 一般用来给变量赋值
        /// </summary>
        protected virtual void OnStart()
        {
        }

        /// <summary>
        /// 显示
        /// </summary>
        protected virtual void OnShow()
        {
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        protected virtual void OnHide()
        {
        }

        #endregion

        #region mono方法

        private void Awake()
        {
            IsAwake = true;
            OnInit();
        }

        private void Start()
        {
            IsStart = true;
            OnStart();
            if (IsEnable)
            {
                IsShow = true;
                OnShow();
            }
        }

        private void OnEnable()
        {
            IsEnable = true;
            if(IsStart)
            {
                IsShow = true;
                OnShow();
            }
        }

        private void OnDisable()
        {
            IsEnable = false;
            if(IsStart)
            {
                IsShow = false;
                OnHide();
            }
        }
        
        #endregion
    }
}
