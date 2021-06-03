using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace LightFramework.UI
{
    /// <summary>
    /// 封装的滑块控件
    /// </summary>
    [AddComponentMenu("UI/GameUI/GameUISlider", 33)]
    public class GameUISlider : GameUIBaseGrapha
    {
        #region 私有字段

        private Slider _slider;

        private UnityAction<float> valueChangedActions;
        private bool enableValueChangeNotify = true;

        #endregion

        #region 受保护属性

        /// <summary>
        /// 滑块组件, 限当前类和子类访问
        /// </summary>
        protected Slider slider
        {
            get
            {
                if (_slider == null)
                {
                    _slider = GetComponent<Slider>();
                    if (_slider == null)
                    {
                        Debug.LogError("the 'Slider' not found, name:" + (gameObject != null ? gameObject.name : ""));
                    }
                }
                return _slider;
            }
        }

        /// <summary>
        /// 图形组件, 限当前类和子类访问
        /// </summary>
        protected override Graphic graphic
        {
            get
            {
                return slider.image;
            }
        }

        #endregion

        #region 公有属性

        /// <summary>
        /// 滑块组件, 外部访问
        /// </summary>
        public Slider Slider
        {
            get { return slider; }
        }

        /// <summary>
        /// 滑块背景Image组件
        /// </summary>
        public Image Background
        {
            get
            {
                return transform.FindComponent<Image>("Background");
            }
        }

        /// <summary>
        /// 滑块填充Image
        /// </summary>
        public Image Fill
        {
            get
            {
                if(slider.fillRect != null)
                {
                    return slider.fillRect.GetComponent<Image>();
                }
                return null;
            }
        }

        /// <summary>
        /// 滑块Image组件
        /// </summary>
        public Image Handle
        {
            get
            {
                if(slider.handleRect != null)
                {
                    return slider.handleRect.GetComponent<Image>();
                }
                return null;
            }
        }

        /// <summary>
        /// slider.interactable
        /// </summary>
        public bool interactable
        {
            get { return slider.interactable; }
            set { slider.interactable = value; }
        }

        /// <summary>
        /// slider.direction, 设置滑动方向
        /// </summary>
        public Slider.Direction direction
        {
            get { return slider.direction; }
            set { slider.direction = value; }
        }

        /// <summary>
        /// slider.minValue, 最小变化值(包含)
        /// </summary>
        public float minValue
        {
            get { return slider.minValue; }
            set { slider.minValue = value; }
        }

        /// <summary>
        /// slider.maxValue, 最大变化值(包含)
        /// </summary>
        public float maxValue
        {
            get { return slider.maxValue; }
            set { slider.maxValue = value; }
        }

        /// <summary>
        /// slider.wholeNumbers, 值变动时是否为整数变动, 
        /// true:整数变动  false:正常变动
        /// </summary>
        public bool wholeNumbers
        {
            get { return slider.wholeNumbers; }
            set { slider.wholeNumbers = value; }
        }

        /// <summary>
        /// slider.value, 设置滑块数值,
        /// 该值变动时滑块位置也会变化
        /// 会派发值变动消息
        /// </summary>
        public float value
        {
            get { return slider.value; }
            set { slider.value = value; }
        }

        /// <summary>
        /// 是否可以点击
        /// </summary>
        public override bool CanTouch
        {
            get
            {
                return slider.IsInteractable();
            }

            set
            {
                slider.interactable = value;
            }
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
        public void AddValueChangedListener(UnityAction<float> action)
        {
            if (valueChangedActions == null)
            {
                slider.onValueChanged.RemoveListener(OnBaseValueChanged);
                slider.onValueChanged.AddListener(OnBaseValueChanged);
            }

            valueChangedActions -= action;
            valueChangedActions += action;
        }

        /// <summary>
        /// 移除值变化事件委托
        /// </summary>
        /// <param name="action"></param>
        public void RemoveValueChangedListener(UnityAction<float> action)
        {
            valueChangedActions -= action;
            if (valueChangedActions == null)
            {
                slider.onValueChanged.RemoveListener(OnBaseValueChanged);
            }
        }

        /// <summary>
        /// 移除所有值变化事件委托
        /// </summary>
        public void RemoveAllValueChangedListeners()
        {
            valueChangedActions = null;
            slider.onValueChanged.RemoveListener(OnBaseValueChanged);
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 设置滑块数值
        /// </summary>
        /// <param name="input">数值</param>
        public void SetValue(int input)
        {
            slider.value = input;
        }

        /// <summary>
        /// 设置滑块数值，但不调用 ValueChanged 事件监听。
        /// </summary>
        /// <param name="input">当前所选内容的新索引。</param>
        public void SetValueWithoutNotify(int input)
        {
            bool temp = enableValueChangeNotify;
            enableValueChangeNotify = false;
            slider.value = input;
            enableValueChangeNotify = temp;
        }

        /// <summary>
        /// 设置滑块, 滑动方向
        /// </summary>
        /// <param name="direction">方向</param>
        /// <param name="includeRectLayouts">true:整个滑块也会根据方向改为水平或者竖直</param>
        public void SetDirection(Slider.Direction direction, bool includeRectLayouts)
        {
            slider.SetDirection(direction, includeRectLayouts);
        }

        #endregion

        #region 私有方法 事件监听

        /// <summary>
        /// 封装的值变化事件监听
        /// </summary>
        /// <param name="arg"></param>
        private void OnBaseValueChanged(float arg)
        {
            if (enableValueChangeNotify && valueChangedActions != null)
            {
                valueChangedActions.Invoke(arg);
            }
        }

        #endregion

        #region 重写方法

        protected override void Reset()
        {
            base.Reset();
            RemoveAllValueChangedListeners();
            enableValueChangeNotify = true;
            _slider = null;
        }

        #endregion
    }
}

