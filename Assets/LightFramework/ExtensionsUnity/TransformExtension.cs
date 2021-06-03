using System;
using UnityEngine;

namespace LightFramework
{
    /// <summary>
    /// transform扩展
    /// </summary>
    public static partial class TransformExtension
    {
        /// <summary>
        /// 递归获取子目标(不检测transform自身的名字)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Transform FindRecursive(this Transform transform, string name)
        {
            if(transform == null)
            {
                return null;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform trf = transform.GetChild(i);
                if(trf.name == name)
                {
                    return trf;
                }
                trf = FindRecursive(trf, name);
                if (trf != null && trf.name == name)
                {
                    return trf;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取一个符合名字的节点上的目标组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="transform"></param>
        /// <param name="name">节点名字</param>
        /// <param name="bAddWhenNo">如果找到的对象上没目标组件是否添加一个</param>
        /// <returns></returns>
        public static T FindComponent<T>(this Transform transform, string name, bool bAddWhenNo = false) where T : Component
        {
            Transform trf = FindRecursive(transform, name);
            if(trf == null)
            {
                return default(T);
            }
            T t = trf.gameObject.GetComponent<T>();
            if(bAddWhenNo && t == null)
            {
                t = trf.gameObject.AddComponent<T>();
            }
            return t;
        }
        /// <summary>
        /// 获取一个符合名字的节点上的目标组件
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="name">节点名字</param>
        /// <param name="type">组件类型</param>
        /// <param name="bAddWhenNo">如果找到的对象上没目标组件是否添加一个</param>
        /// <returns></returns>
        public static Component FindComponent(this Transform transform, string name, Type type, bool bAddWhenNo = false)
        {
            Transform trf = FindRecursive(transform, name);
            if (trf == null)
            {
                return null;
            }
            Component t = trf.gameObject.GetComponent(type);
            if (bAddWhenNo && t == null)
            {
                t = trf.gameObject.AddComponent(type);
            }
            return t;
        }

        /// <summary>
        /// 销毁所有子对象
        /// </summary>
        /// <param name="transform"></param>
        public static void DestroyAllChildren(this Transform transform)
        {
            int childIndex = transform.childCount - 1;
            if(childIndex < 0)
            {
                return;
            }
            while (childIndex >= 0)
            {
                Transform child = transform.GetChild(childIndex);
                GameObject.Destroy(child.gameObject);
                childIndex--;
            }
        }
    }
}

