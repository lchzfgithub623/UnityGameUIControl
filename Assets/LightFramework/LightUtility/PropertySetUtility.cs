using System.Collections.Generic;
using UnityEngine;

namespace LightFramework
{
    /// <summary>
    /// 设置值实用类
    /// </summary>
    public static partial class PropertySetUtility
    {
        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="currentValue">被改的变量</param>
        /// <param name="newValue">修改后的值</param>
        /// <returns>true:有变化, false:没有变化</returns>
        public static bool SetColor(ref Color currentValue, Color newValue)
        {
            if (currentValue.r == newValue.r && currentValue.g == newValue.g && currentValue.b == newValue.b && currentValue.a == newValue.a)
                return false;

            currentValue = newValue;
            return true;
        }

        /// <summary>
        /// 设置结构体
        /// </summary>
        /// <typeparam name="T">模版</typeparam>
        /// <param name="currentValue">被改的变量</param>
        /// <param name="newValue">修改后的值</param>
        /// <returns>true:有变化, false:没有变化</returns>
        public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
                return false;

            currentValue = newValue;
            return true;
        }

        /// <summary>
        /// 设置类值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentValue">被改的变量</param>
        /// <param name="newValue">修改后的值</param>
        /// <returns>true:有变化, false:没有变化</returns>
        public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
        {
            if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
                return false;

            currentValue = newValue;
            return true;
        }
    }
}

