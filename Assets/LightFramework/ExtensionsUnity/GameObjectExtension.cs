using System;
using UnityEngine;

namespace LightFramework
{
    /// <summary>
    /// GameObject扩展
    /// </summary>
    public static partial class GameObjectExtension
    {
        /// <summary>
        /// 通过泛型获取并添加组件
        /// </summary>
        /// <typeparam name="T">组件类</typeparam>
        /// <param name="go">对象的 GameObject </param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T t = go.GetComponent<T>();
            if (t == null)
            {
                t = go.AddComponent<T>();
            }
            return t;
        }

        /// <summary>
        /// 通过类型获取并添加组件
        /// </summary>
        /// <param name="go">对象的 GameObject </param>
        /// <param name="type">组件类型</param>
        /// <returns></returns>
        public static Component GetOrAddComponent(this GameObject go, Type type)
        {
            Component t = go.GetComponent(type);
            if (t == null)
            {
                t = go.AddComponent(type);
            }
            return t;
        }

        /// <summary>
        /// 获取RectTransform
        /// </summary>
        /// <param name="go">对象的 GameObject </param>
        /// <returns></returns>
        public static RectTransform GetRectTrans(this GameObject go)
        {
            return go.transform as RectTransform;
        }

        /// <summary>
        /// 是否某一个图层
        /// </summary>
        /// <param name="go">对象的 GameObject </param>
        /// <param name="layer">层级id</param>
        /// <returns></returns>
        public static bool IsLayer(this GameObject go, int layer)
        {
            if (go == null)
            {
                return false;
            }
            return go.layer == layer;
        }

        /// <summary>
        /// 是否某一个图层
        /// </summary>
        /// <param name="go">对象的 GameObject </param>
        /// <param name="layerName">图层名字</param>
        /// <returns></returns>
        public static bool IsLayer(this GameObject go, string layerName)
        {
            if (go == null)
            {
                return false;
            }
            return go.layer == LayerMask.NameToLayer(layerName);
        }

        /// <summary>
        /// 设置目标层级, 可以指定数值的层级进行设置
        /// </summary>
        /// <param name="go">对象的 GameObject </param>
        /// <param name="layer">层级id</param>
        /// <param name="filterValue">筛选层级数值, 如果有值, 则符合这些层级的才能修改为目标层级</param>
        public static void SetLayerInChildren(this GameObject go, int layer, params int[] filterLayers)
        {
            if (go == null)
            {
                return;
            }
            go.layer = layer;
            foreach (Transform children in go.transform)
            {
                GameObject childGo = children.gameObject;
                SetLayerInChildren(children.gameObject, layer, filterLayers);
                // 如果有筛选值, 但是不符合直接跳过
                if (filterLayers != null && (Array.FindIndex(filterLayers, t => t == childGo.layer) == -1))
                {
                    continue;
                }
                childGo.layer = layer;
            }
        }

        /// <summary>
        /// 设置目标层级, 可以指定数值的层级进行设置
        /// </summary>
        /// <param name="go">对象的 GameObject </param>
        /// <param name="layer">层级id</param>
        /// <param name="filterLayer1">筛选层级数值, 如果大于-1, 则符合这个层级的才能修改为目标层级</param>
        public static void SetLayerInChildren(this GameObject go, int layer, int filterLayer1 = -1)
        {
            if (go == null)
            {
                return;
            }
            go.layer = layer;
            foreach (Transform children in go.transform)
            {
                GameObject childGo = children.gameObject;
                SetLayerInChildren(children.gameObject, layer, filterLayer1);
                if (childGo.layer == filterLayer1)
                {
                    continue;
                }
                childGo.layer = layer;
            }
        }
    }
}


