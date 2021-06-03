using UnityEngine;

namespace LightFramework.UI
{
    /// <summary>
    /// 封装MonoBehaviour为ui组件的基础类
    /// </summary>
    public class GameUIBaseComponent : MonoBehaviour, IGameUITouchHandle
    {
        #region 公有属性

        /// <summary>
        /// 是否可以触摸
        /// </summary>
        public virtual bool CanTouch
        {
            get; set;
        }

        /// <summary>
        /// 触摸 处理器
        /// </summary>
        public virtual GameUITouch TouchMgr
        {
            get
            {
                GameUITouch touchMgr = gameObject.GetOrAddComponent<GameUITouch>();
                touchMgr.RegisterHandle(this);
                return touchMgr;
            }
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 设置 是否可以触摸
        /// </summary>
        /// <param name="canTouch">是否可以触摸</param>
        public virtual void SetCanTouch(bool canTouch)
        {
            CanTouch = canTouch;
        }

        /// <summary>
        /// 设置触摸管理enable
        /// </summary>
        /// <param name="enabled"></param>
        public virtual void SetTouchMgrEnabled(bool enabled)
        {
            GameUITouch touchMgr = gameObject.GetComponent<GameUITouch>();
            if (touchMgr != null)
            {
                touchMgr.enabled = enabled;
            }
        }

        /// <summary>
        /// 设置尺寸
        /// </summary>
        /// <param name="size"></param>
        public void SetSizeDelta(Vector2 size)
        {
            this.GetRectTrans().sizeDelta = size;
        }

        /// <summary>
        /// 重置该脚本
        /// </summary>
        public void ResetComponent()
        {
            Reset();
        }

        #endregion

        #region 公有虚函数

        /// <summary>
        /// Returns true if the GameObject and the Component are active.
        /// </summary>
        public virtual bool IsActive()
        {
            return isActiveAndEnabled;
        }

        #endregion

        #region 受保护虚函数

        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }

        protected virtual void Reset()
        {

        }
        #endregion
    }
}


