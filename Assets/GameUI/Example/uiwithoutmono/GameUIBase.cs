using UnityEngine;

namespace LightFramework.UI
{
    public class GameUIBase : IUIBase
    {
        public GameUIBase() { }

        #region 公有属性
        /// <summary>
        /// 绑定的mono
        /// </summary>
        public GameUIMono UIMono { get; set; }

        /// <summary>
        /// 是否Awake
        /// </summary>
        public bool IsAwake { get; set; }

        /// <summary>
        /// 是否Start
        /// </summary>
        public bool IsStart { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }

        #endregion

        #region 公有方法

        public RectTransform GetRectTrans()
        {
            if(UIMono != null)
            {
                return (UIMono.transform as RectTransform);
            }

            return null;
        }

        #endregion

        #region 虚函数

        /// <summary>
        /// 绑定 Mono
        /// </summary>
        /// <param name="bind"></param>
        public virtual void OnBindMono(bool bind)
        {
        }

        /// <summary>
        /// awake初始化: 获取控件, 初始化变量或游戏状态
        /// </summary>
        /// <remarks>
        /// OnInit 总是在 OnStart 前调用
        /// </remarks>
        public virtual void OnInit()
        {
        }
        /// <summary>
        /// start初始化: 一般用来给变量赋值
        /// </summary>
        public virtual void OnStart()
        {
        }

        /// <summary>
        /// 显示
        /// </summary>
        public virtual void OnShow()
        {
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public virtual void OnHide()
        {
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void OnDestroy()
        {
        }

        /// <summary>
        /// 尺寸发生变化
        /// </summary>
        public virtual void OnRectTransformDimensionsChange()
        {
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="pause"></param>
        public virtual void OnApplicationPause(bool pause)
        {
        }

        /// <summary>
        /// 退出应用
        /// </summary>
        public virtual void OnApplicationQuit()
        {
        }

        /// <summary>
        /// 重置
        /// </summary>
        public virtual void Reset()
        {
            IsAwake = false;
            IsStart = false;
            IsShow = false;
        }
        #endregion

    }
}
