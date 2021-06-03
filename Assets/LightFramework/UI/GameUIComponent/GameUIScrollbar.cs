using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace LightFramework.UI
{
    /// <summary>
    /// 封装的滚动条控件
    /// </summary>
    [AddComponentMenu("UI/GameUI/GameUIScrollbar", 34)]
    public class GameUIScrollbar : GameUIBaseGrapha
    {
        #region 私有字段

        private Scrollbar _scrollbar;

        private UnityAction<float> valueChangedActions;
        private bool enableValueChangeNotify = true;

        #endregion

        #region 受保护属性

        /// <summary>
        /// 滚动条组件, 限当前类和子类访问
        /// </summary>
        protected Scrollbar scrollbar
        {
            get
            {
                if (_scrollbar == null)
                {
                    _scrollbar = GetComponent<Scrollbar>();
                    if (_scrollbar == null)
                    {
                        Debug.LogError("the 'Scrollbar' not found, name:" + (gameObject != null ? gameObject.name : ""));
                    }
                }
                return _scrollbar;
            }
        }

        /// <summary>
        /// 图形组件, 限当前类和子类访问
        /// </summary>
        protected override Graphic graphic
        {
            get
            {
                return scrollbar.image;
            }
        }

        #endregion

        #region 公有属性

        /// <summary>
        /// 滚动条组件, 外部访问
        /// </summary>
        public Scrollbar Scrollbar
        {
            get { return scrollbar; }
        }

        /// <summary>
        /// 滚动条背景Image组件
        /// </summary>
        public Image Background
        {
            get
            {
                return transform.FindComponent<Image>("Background");
            }
        }

        /// <summary>
        /// 滚动条滑块Image组件
        /// </summary>
        public Image Handle
        {
            get
            {
                if(scrollbar.handleRect != null)
                {
                    return scrollbar.handleRect.GetComponent<Image>();
                }
                return null;
            }
        }

        /// <summary>
        /// scrollbar.interactable
        /// </summary>
        public bool interactable
        {
            get { return scrollbar.interactable; }
            set { scrollbar.interactable = value; }
        }

        /// <summary>
        /// scrollbar.direction, 设置滑动方向
        /// </summary>
        public Scrollbar.Direction direction
        {
            get { return scrollbar.direction; }
            set { scrollbar.direction = value; }
        }

        /// <summary>
        /// scrollbar.value, 设置滚动条 数值,
        /// [0-1]
        /// 该值变动时滑块位置也会变化
        /// 会派发值变动消息
        /// </summary>
        public float value
        {
            get { return scrollbar.value; }
            set { scrollbar.value = value; }
        }

        /// <summary>
        /// scrollbar.size, 设置滚动滑块尺寸相对于滚动条尺寸的比例值
        /// [0-1]
        /// </summary>
        public float size
        {
            get { return scrollbar.size; }
            set { scrollbar.size = value; }
        }

        /// <summary>
        /// scrollbar.numberOfSteps, 要用于该值的步骤数。值为0将禁用步骤的使用
        /// 大于0时,将滑块条分为1-10段
        /// [0-11]
        /// </summary>
        public int numberOfSteps
        {
            get { return scrollbar.numberOfSteps; }
            set { scrollbar.numberOfSteps = value; }
        }

        /// <summary>
        /// 是否可以点击
        /// </summary>
        public override bool CanTouch
        {
            get
            {
                return scrollbar.IsInteractable();
            }

            set
            {
                scrollbar.interactable = value;
            }
        }

        /// <summary>
        /// 开启 值变化时是否通知
        /// true: 下拉框值更改时, 会调用 ValueChanged 的事件监听;
        /// false: 下拉框值更改时, 不会调用 ValueChanged 的事件监听;
        /// </summary>
        public bool EnableValueChangeNotify
        {
            get
            {
                return enableValueChangeNotify;
            }
            set
            {
                enableValueChangeNotify = value;
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
                scrollbar.onValueChanged.RemoveListener(OnBaseValueChanged);
                scrollbar.onValueChanged.AddListener(OnBaseValueChanged);
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
            if(valueChangedActions == null)
            {
                scrollbar.onValueChanged.RemoveListener(OnBaseValueChanged);
            }
        }

        /// <summary>
        /// 移除所有值变化事件委托
        /// </summary>
        public void RemoveAllValueChangedListeners()
        {
            valueChangedActions = null;
            scrollbar.onValueChanged.RemoveListener(OnBaseValueChanged);
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 设置滚动条数值
        /// </summary>
        /// <param name="input"> 数值 </param>
        public void SetValue(int input)
        {
            scrollbar.value = input;
        }

        /// <summary>
        /// 设置滚动条数值，但不调用 ValueChanged 事件监听。
        /// </summary>
        /// <param name="input">当前所选内容的新索引。</param>
        public void SetValueWithoutNotify(int input)
        {
            bool temp = enableValueChangeNotify;
            enableValueChangeNotify = false;
            scrollbar.value = input;
            enableValueChangeNotify = temp;
        }

        /// <summary>
        /// 设置滚动条, 滑动方向
        /// </summary>
        /// <param name="direction">方向</param>
        /// <param name="includeRectLayouts">true:整个滚动条也会根据方向改为水平或者竖直</param>
        public void SetDirection(Scrollbar.Direction direction, bool includeRectLayouts)
        {
            scrollbar.SetDirection(direction, includeRectLayouts);
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

        #region 重写 事件
        protected override void Reset()
        {
            base.Reset();
            RemoveAllValueChangedListeners();
            enableValueChangeNotify = true;
            _scrollbar = null;
        }
        #endregion
    }
}

