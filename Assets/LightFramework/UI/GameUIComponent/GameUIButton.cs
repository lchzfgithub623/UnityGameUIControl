using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace LightFramework.UI
{
    /// <summary>
    /// 封装的按钮控件
    /// </summary>
    [AddComponentMenu("UI/GameUI/GameUIButton", 30)]
    public partial class GameUIButton : GameUIBaseGrapha
    {
        #region 私有字段

        private Button _button;

        private UnityAction clickAction;
        #endregion

        #region 公有字段

        /// <summary>
        /// 按钮文本Text
        /// </summary>
        public Text Label;

        #endregion

        #region 受保护属性

        /// <summary>
        /// 按钮组件, 限当前类和子类访问
        /// </summary>
        protected Button button
        {
            get
            {
                if (_button == null)
                {
                    _button = GetComponent<Button>();
                    if (_button == null)
                    {
                        Debug.LogError("the 'Button' not found, name:" + (gameObject != null ? gameObject.name : ""));
                    }
                }
                return _button;
            }
        }

        /// <summary>
        /// 图片组件, 限当前类和子类访问 
        /// </summary>
        protected Image buttonImage
        {
            get { return button.image; }
            set { button.image = value; }
        }

        /// <summary>
        /// 图形组件, 限当前类和子类访问
        /// </summary>
        protected override Graphic graphic
        {
            get
            {
                return button.image;
            }
        }

        #endregion

        #region 公有属性

        /// <summary>
        /// 按钮组件, 外部访问
        /// </summary>
        public Button Button
        {
            get { return button; }
        }

        /// <summary>
        /// 按钮图片组件, 外部访问
        /// </summary>
        public Image ButtonImage
        {
            get { return buttonImage; }
            set { buttonImage = value; }
        }

        /// <summary>
        /// button.interactable
        /// </summary>
        public bool interactable
        {
            get { return button.interactable; }
            set { button.interactable = value; }
        }

        /// <summary>
        /// button.onClick
        /// </summary>
        public Button.ButtonClickedEvent onClick
        {
            get { return button.onClick; }
            set { button.onClick = value; }
        }

        /// <summary>
        /// 按钮是否置灰
        /// </summary>
        public bool IsGray
        {
            get; private set;
        }

        /// <summary>
        /// 是否可以点击  CanTouch 
        /// </summary>
        public override bool CanTouch
        {
            get
            {
                return button.IsInteractable();
            }

            set
            {
                button.interactable = value;
            }
        }

        #endregion

        #region 公有方法 事件监听

        /// <summary>
        /// 获取值变化事件的监听个数
        /// </summary>
        public int GetClickListenerCount()
        {
            if (clickAction != null)
            {
                return clickAction.GetInvocationList().Length;
            }
            return 0;
        }

        /// <summary>
        /// 添加点击事件监听, 如果按钮只有点击事件就使用这个
        /// </summary>
        /// <remarks>
        /// 方法和 RemoveClickListener 对应
        /// 同一个action委托不会重复添加
        /// </remarks>
        public void AddClickListener(UnityAction action)
        {
            if(clickAction == null)
            {
                button.onClick.RemoveListener(OnButtonClick);
                button.onClick.AddListener(OnButtonClick);
            }

            clickAction -= action;
            clickAction += action;
        }

        /// <summary>
        /// 移除点击事件监听, 如果按钮只有点击事件就使用这个
        /// </summary>
        /// <param name="action"></param>
        /// <remarks>
        /// 方法和 AddClickListener 对应
        /// </remarks>
        public void RemoveClickListener(UnityAction action)
        {
            clickAction -= action;
            if (clickAction == null)
            {
                button.onClick.RemoveListener(OnButtonClick);
            }
        }

        /// <summary>
        /// 移除所有点击事件监听
        /// </summary>
        public void RemoveAllClickListeners()
        {
            clickAction = null;
            button.onClick.RemoveListener(OnButtonClick);
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 设置按钮是否置灰, 是否可以点击
        /// </summary>
        /// <param name="bGray">是否灰色</param>
        /// <param name="canTouch">是否可以点击</param>
        public void SetGray(bool bGray, bool canTouch)
        {
            IsGray = bGray;
            buttonImage.SetGray(bGray);
            CanTouch = canTouch;
        }

        /// <summary>
        /// 设置按钮文本
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="bAutoFindLab">如果Label为空, true:自动查找按钮下的text进行设置文本, false:不做处理</param>
        public void SetButtonLab(string content, bool bAutoFindLab = true)
        {
            if (Label != null)
            {
                Label.text = content;
            }
            else if (bAutoFindLab)
            {
                Text t = GetComponentInChildren<Text>();
                if (t != null)
                {
                    t.text = content;
                }
            }
        }

        #endregion

        #region 私有方法 事件监听

        private void OnButtonClick()
        {
            if (clickAction != null)
            {
                clickAction.Invoke();
            }
        }

        #endregion

        #region 重写方法

        protected override void Reset()
        {
            base.Reset();
            RemoveAllClickListeners();
            _button = null;
        }

        #endregion
    }
}


