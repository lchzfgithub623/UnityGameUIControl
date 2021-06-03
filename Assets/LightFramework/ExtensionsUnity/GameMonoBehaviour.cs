using UnityEngine;

namespace LightFramework.UI
{
    public class GameMonoBehaviour : MonoBehaviour
    {
        #region 公有属性

        /// <summary>
        /// 是否销毁
        /// </summary>
        public bool IsDestroy { get; set; }

        /// <summary>
        /// 是否暂停
        /// </summary>
        public static bool IsApplicationPause { get; set; }

        /// <summary>
        /// 是否应用退出, 此处不一定是真的退出了应用
        /// </summary>
        public static bool IsApplicationQuit { get; set; }

        /// <summary>
        /// 是否应用退出
        /// </summary>
        public static bool IsQuitting { get; private set; }

        #endregion


        #region mono 虚方法

        /// <summary>
        /// 销毁
        /// </summary>
        protected virtual void OnDestroy()
        {
            IsDestroy = true;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="pause"></param>
        protected virtual void OnApplicationPause(bool pause)
        {
            IsApplicationPause = pause;
        }

        /// <summary>
        /// 退出应用
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            IsApplicationQuit = true;
        }

        #endregion

        #region initialize

        [RuntimeInitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            Application.quitting -= OnQuitting;
            Application.quitting += OnQuitting;
        }
        private static void OnQuitting()
        {
            IsQuitting = true;
        }

        #endregion

    }
}
