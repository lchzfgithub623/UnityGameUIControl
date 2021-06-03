using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace LightFramework.UI
{
    /// <summary>
    /// 封装的开关控件
    /// </summary>
    [AddComponentMenu("UI/GameUI/GameUIToggle", 31)]
    public class GameUIToggle : GameUIBaseGrapha
    {
        #region 私有函数

        private LightToggle _lightToggle;

        [Tooltip("非选中状态下的显示节点")]
        [SerializeField]
        private GameObject normalStateNode;

        [Tooltip("选中状态下的显示节点")]
        [SerializeField]
        private GameObject checkedStateNode;

        [SerializeField]
        private Text label;

        [Tooltip("状态切换时是否显示或隐藏对应状态的节点")]
        [SerializeField]
        private bool switchStateVisible;

        private UnityAction<bool> valueChangedActions;
        private bool enableValueChangeNotify = true;
        #endregion

        #region 受保护属性

        /// <summary>
        /// 自定义开关组件, 限当前类和子类访问
        /// </summary>
        protected LightToggle lightToggle
        {
            get
            {
                if (_lightToggle == null)
                {
                    _lightToggle = GetComponent<LightToggle>();
                    if (_lightToggle == null)
                    {
                        Debug.LogError("the 'LightToggle' not found, name:" + (gameObject != null ? gameObject.name : ""));
                    }
                }

                return _lightToggle;
            }
        }

        /// <summary>
        /// 图形组件, 限当前类和子类访问
        /// </summary>
        protected override Graphic graphic
        {
            get
            {
                return lightToggle.graphic;
            }
        }

        #endregion

        #region 公有属性

        /// <summary>
        /// 自定义开关组件, 外部访问
        /// </summary>
        public LightToggle Toggle
        {
            get { return lightToggle; }
        }

        /// <summary>
        /// Toggle.interactable
        /// </summary>
        public bool interactable
        {
            get { return lightToggle.interactable; }
            set { lightToggle.interactable = value; }
        }

        /// <summary>
        /// Toggle.isOn, Return or set whether the Toggle is on or not.
        /// 直接切换开关
        /// </summary>        
        public bool isOn
        {
            get { return lightToggle.isOn; }
            set { lightToggle.isOn = value; }
        }

        /// <summary>
        /// Toggle.group Group the toggle belongs to.
        /// </summary>        
        public ToggleGroup group
        {
            get { return lightToggle.group; }
            set { lightToggle.group = value; }
        }

        /// <summary>
        /// 切换选择状态前, 检测是否可以进行切换函数
        /// 参数: 当前的 Toggle 组件, 返回值:true可以切换, false:不可以切换
        /// </summary>
        /// <example>
        /// 点击 toggle 后, 先执行该函数判断是否可以切换状态
        /// </example>
        public Func<Toggle, bool> CheckCanToggleFunc
        {
            get { return lightToggle.CheckCanToggleFunc; }
            set { lightToggle.CheckCanToggleFunc = value; }
        }

        /// <summary>
        /// 正常状态节点
        /// </summary>
        public GameObject NormalStateNode
        {
            get { return normalStateNode; }
            set { normalStateNode = value; }
        }

        /// <summary>
        /// 勾选状态节点
        /// </summary>
        public GameObject CheckedStateNode
        {
            get { return checkedStateNode; }
            set { checkedStateNode = value; }
        }

        /// <summary>
        /// 切换状态时是否显示或隐藏对应的状态
        /// </summary>
        public bool SwitchStateVisible
        {
            get { return switchStateVisible; }
            set { switchStateVisible = value; }
        }

        /// <summary>
        /// 开关标签, 可能为null
        /// </summary>
        public Text Label
        {
            get { return label; }
            set { label = value; }
        }

        #endregion

        #region 公有方法 事件监听

        /// <summary>
        /// 获取值变化事件的监听个数
        /// </summary>
        public int GetValueChangedListenerCount()
        {
            if (valueChangedActions != null)
            {
                return valueChangedActions.GetInvocationList().Length;
            }
            return 0;
        }

        /// <summary>
        /// 添加值变化事件委托
        /// </summary>
        /// <param name="action"></param>
        public void AddValueChangedListener(UnityAction<bool> action)
        {
            if (valueChangedActions == null)
            {
                lightToggle.onValueChanged.RemoveListener(OnBaseValueChanged);
                lightToggle.onValueChanged.AddListener(OnBaseValueChanged);
            }

            valueChangedActions -= action;
            valueChangedActions += action;
        }

        /// <summary>
        /// 移除值变化事件委托
        /// </summary>
        /// <param name="action"></param>
        public void RemoveValueChangedListener(UnityAction<bool> action)
        {
            valueChangedActions -= action;
            if (valueChangedActions == null)
            {
                lightToggle.onValueChanged.RemoveListener(OnBaseValueChanged);
            }
        }

        /// <summary>
        /// 移除所有值变化事件委托
        /// </summary>
        public void RemoveAllValueChangedListeners()
        {
            valueChangedActions = null;
            lightToggle.onValueChanged.RemoveListener(OnBaseValueChanged);
        }

        #endregion

        #region 公有方法
        
        /// <summary>
        /// 设置选中
        /// 该逻辑会在切换选择状态前执行检测函数 CheckCanToggleFunc
        /// </summary>
        /// <param name="isOn">true:选中, false:非选中</param>
        /// <returns>false:不能执行操作, true:执行操作成功</returns>
        public bool SetIsOn(bool isOn)
        {
            return lightToggle.SetIsOn(isOn);
        }

        /// <summary>
        /// 设置滑块数值，但不调用 ValueChanged 事件监听。
        /// 该逻辑会在切换选择状态前执行检测函数 CheckCanToggleFunc
        /// </summary>
        /// <param name="isOn">true:选中, false:非选中</param>
        /// <returns>false:不能执行操作, true:执行操作成功</returns>
        public bool SetIsOnWithoutNotify(bool isOn)
        {
            bool temp = enableValueChangeNotify;
            enableValueChangeNotify = false;
            bool ret = SetIsOn(isOn);
            enableValueChangeNotify = temp;
            return ret;
        }

        /// <summary>
        /// 刷新开关状态, 前提 switchStateVisible 为true
        /// </summary>
        private void RefreshCheckState()
        {
            if (switchStateVisible)
            {
                if (normalStateNode != null)
                {
                    normalStateNode.SetActive(!isOn);
                }
                if (checkedStateNode != null)
                {
                    checkedStateNode.SetActive(isOn);
                }
            }
        }
        #endregion

        #region 私有方法 事件监听

        /// <summary>
        /// 封装的值变化事件监听
        /// </summary>
        /// <param name="arg"></param>
        private void OnBaseValueChanged(bool arg)
        {
            RefreshCheckState();
            if (enableValueChangeNotify && valueChangedActions != null)
            {
                valueChangedActions.Invoke(arg);
            }
        }

        #endregion

        #region 重写方法

        protected override void OnEnable()
        {
            RefreshCheckState();
        }

        protected override void Reset()
        {
            base.Reset();
            _lightToggle = null;
        }

        #endregion
    }
}

