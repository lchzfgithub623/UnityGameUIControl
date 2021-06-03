using UnityEngine;

namespace LightFramework.UI
{
    public class GameUIMono : MonoBehaviour
    {
        #region 公有属性
        /// <summary>
        /// 绑定的Ui
        /// </summary>
        public IUIBase UIBase { get; private set; }

        /// <summary>
        /// 是否Awake
        /// </summary>
        public bool IsAwake { get; private set; }

        /// <summary>
        /// 是否Start
        /// </summary>
        public bool IsStart { get; private set; }


        /// <summary>
        /// 是否Enable
        /// </summary>
        public bool IsEnable { get; set; }

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



        #region 绑定

        /// <summary>
        /// 绑定ui
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T BindUI<T>(Component comp) where T : IUIBase, new()
        {
            T t;
            GameUIMono mono = comp.GetComponent<GameUIMono>();
            if(mono != null)
            {
                t = mono.GetBindUI<T>();
                if(t == null)
                {
                    t = mono.BindUI<T>();
                }
                return t;
            }

            mono = comp.gameObject.AddComponent<GameUIMono>();
            t = mono.BindUI<T>();

            return t;
        }

        /// <summary>
        /// 绑定ui
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T BindUI<T>() where T : IUIBase, new()
        {
            T t = new T();
            BindUI(t);

            return t;
        }

        public void BindUI(IUIBase ui)
        {
            if (UIBase != null)
            {
                UIBase.OnBindMono(false);
                UIBase.Reset();
                UIBase = null;
            }

            UIBase = ui;

            UIBase.UIMono = this;
            UIBase.OnBindMono(true);
            CheckUIState();
        }

        /// <summary>
        /// 获取绑定类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetBindUI<T>() where T : IUIBase
        {
            if(UIBase == null)
            {
                return default(T);
            }
            return (T)UIBase;
        }

        #endregion

        /// <summary>
        /// 检测ui状态
        /// </summary>
        private void CheckUIState()
        {
            if (IsAwake && !UIBase.IsAwake)
            {
                UIBase.IsAwake = true;
                UIBase.OnInit();
            }

            if (IsStart && !UIBase.IsStart)
            {
                IsEnable = isActiveAndEnabled;
                UIBase.IsStart = true;
                UIBase.OnStart();
                if (IsEnable)
                {
                    UIBase.OnShow();
                }
            }
        }

        #region mono方法

        private void Awake()
        {
            IsAwake = true;
            if (UIBase != null && !UIBase.IsAwake)
            {
                UIBase.IsAwake = true;
                UIBase.OnInit();
            }
        }

        private void Start()
        {
            IsStart = true;
            if (UIBase != null && !UIBase.IsStart)
            {
                UIBase.IsStart = true;
                UIBase.OnStart();
                if (IsEnable)
                {
                    UIBase.OnShow();
                }
            }
        }

        private void OnEnable()
        {
            IsEnable = true;
            if (UIBase != null && UIBase.IsStart)
            {
                UIBase.OnShow();
            }
        }

        private void OnDisable()
        {
            IsEnable = false;
            if (IsQuitting) // 如果是应用退出, 这里不必要执行 UIBase.OnDestroy
            {
                return;
            }
            if (UIBase != null && UIBase.IsStart)
            {
                UIBase.OnHide();
            }
        }
        
        private void OnDestroy()
        {
            IsDestroy = true;
            if (IsQuitting) // 如果是应用退出, 这里不必要执行 UIBase.OnDestroy
            {
                return;
            }

            if (UIBase != null)
            {
                UIBase.OnDestroy();
            }
        }

        private void OnRectTransformDimensionsChange()
        {
            if (UIBase != null)
            {
                UIBase.OnRectTransformDimensionsChange();
            }
        }

        private void OnApplicationPause(bool pause)
        {
            IsApplicationPause = pause;
            if (UIBase != null)
            {
                UIBase.OnApplicationPause(pause);
            }
        }

        private void OnApplicationQuit()
        {
            IsApplicationQuit = true;
            if (UIBase != null)
            {
                UIBase.OnApplicationQuit();
            }
        }
        #endregion


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
    }
}
