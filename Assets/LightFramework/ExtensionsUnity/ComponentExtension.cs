using System;
using UnityEngine;

namespace LightFramework
{
    /// <summary>
    /// Component扩展
    /// </summary>
    public static partial class ComponentExtension
    {

        /// <summary>
        /// 通过泛型获取并添加组件
        /// </summary>
        /// <typeparam name="T">组件类</typeparam>
        /// <param name="co">组件对象</param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this Component co) where T : Component
        {
            T t = co.GetComponent<T>();
            if (t == null && co.gameObject != null)
            {
                t = co.gameObject.AddComponent<T>();
            }
            return t;
        }

        /// <summary>
        /// 通过类型获取并添加组件
        /// </summary>
        /// <param name="co">组件对象</param>
        /// <param name="type">组件类型</param>
        /// <returns></returns>
        public static Component GetOrAddComponent(this Component co, Type type)
        {
            Component t = co.GetComponent(type);
            if (t == null && co.gameObject != null)
            {
                t = co.gameObject.AddComponent(type);
            }
            return t;
        }

        /// <summary>
        /// 获取RectTransform
        /// </summary>
        /// <param name="co">组件对象</param>
        /// <returns></returns>
        public static RectTransform GetRectTrans(this Component co)
        {
            return co.transform as RectTransform;
        }

        /// <summary>
        /// 设置所在的组件自身的显示和隐藏
        /// </summary>
        /// <param name="co">组件对象</param>
        /// <param name="bShow">true:显示, false:隐藏</param>
        public static void SetActive(this Component co, bool bShow)
        {
            if (co == null || co.gameObject == null)
            {
                return;
            }
            co.gameObject.SetActive(bShow);
        }

        /// <summary>
        /// 设置父节点
        /// </summary>
        /// <param name="co">组件对象</param>
        /// <param name="worldPositionStays">true:保持世界坐标, false:不保持世界坐标</param>
        public static void SetParent(this Component co, Transform parent, bool worldPositionStays = false)
        {
            if(co == null || co.transform == null)
            {
                return;
            }
            co.transform.SetParent(parent, worldPositionStays);
        }
    }
}


