using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace LightFramework.UI
{
    /// <summary>
    /// 封装的输入框控件
    /// </summary>
    [AddComponentMenu("UI/GameUI/GameUIInputField", 31)]
    public class GameUIInputField : GameUIBaseGrapha
    {
        #region 私有字段

        private InputField _inputField;

        #endregion

        #region 受保护属性

        /// <summary>
        /// 输入框组件, 限当前类和子类访问
        /// </summary>
        protected InputField inputField
        {
            get
            {
                if (_inputField == null)
                {
                    _inputField = GetComponent<InputField>();
                    if (_inputField == null)
                    {
                        Debug.LogError("the 'InputField' not found, name:" + (gameObject != null ? gameObject.name : ""));
                    }
                }

                return _inputField;
            }
        }

        /// <summary>
        /// 图形组件, 限当前类和子类访问
        /// </summary>
        protected override Graphic graphic
        {
            get
            {
                return inputField.targetGraphic;
            }
        }

        #endregion

        #region 公有属性

        /// <summary>
        /// 输入框组件, 外部访问
        /// </summary>
        public InputField InputField
        {
            get { return inputField; }
        }

        /// <summary>
        /// inputField.interactable
        /// </summary>
        public bool interactable
        {
            get { return inputField.interactable; }
            set { inputField.interactable = value; }
        }

        /// <summary>
        /// inputField.textComponent, 输入框主显示文本组件
        /// </summary>
        public Text textComponent
        {
            get { return inputField.textComponent; }
            set { inputField.textComponent = value; }
        }

        /// <summary>
        /// inputField.placeholder, 输入框文本提示组件, 可以是Text, 也可以是image等
        /// </summary>
        public Graphic placeholder
        {
            get { return inputField.placeholder; }
            set { inputField.placeholder = value; }
        }

        /// <summary>
        /// inputField.text, 显示文本
        /// </summary>
        public string text
        {
            get { return inputField.text; }
            set { inputField.text = value; }
        }

        /// <summary>
        /// inputField.placeholder, 提示文本
        /// </summary>
        public string placeholderText
        {
            get
            {
                if(inputField.placeholder is Text)
                {
                    return (inputField.placeholder as Text).text;
                }
                return null;
            }
            set
            {
                if (inputField.placeholder is Text)
                {
                    (inputField.placeholder as Text).text = value;
                }
            }
        }


        /// <summary>
        /// inputField.characterLimit, 字符数限制, 默认值0:不限制个数
        /// </summary>
        public int characterLimit
        {
            get { return inputField.characterLimit; }
            set { inputField.characterLimit = value; }
        }

        /// <summary>
        /// inputField.contentType, 显示文本文类型
        /// </summary>
        public InputField.ContentType contentType
        {
            get { return inputField.contentType; }
            set { inputField.contentType = value; }
        }

        /// <summary>
        /// inputField.lineType, 显示文本行类型
        /// </summary>
        public InputField.LineType lineType
        {
            get { return inputField.lineType; }
            set { inputField.lineType = value; }
        }

        /// <summary>
        /// inputField.inputType, 输入框类型
        /// </summary>
        public InputField.InputType inputType
        {
            get { return inputField.inputType; }
            set { inputField.inputType = value; }
        }

        /// <summary>
        /// inputField.keyboardType, 键盘类型
        /// </summary>
        public TouchScreenKeyboardType keyboardType
        {
            get { return inputField.keyboardType; }
            set { inputField.keyboardType = value; }
        }

        /// <summary>
        /// inputField.characterValidation, 字符验证类型
        /// </summary>
        public InputField.CharacterValidation characterValidation
        {
            get { return inputField.characterValidation; }
            set { inputField.characterValidation = value; }
        }

        /// <summary>
        /// inputField.selectionFocusPosition, 插入符位置
        /// </summary>
        public int selectionFocusPosition
        {
            get { return inputField.selectionFocusPosition; }
            set { inputField.selectionFocusPosition = value; }
        }

        /// <summary>
        /// inputField.selectionAnchorPosition, 选择的起点
        /// </summary>
        public int selectionAnchorPosition
        {
            get { return inputField.selectionAnchorPosition; }
            set { inputField.selectionAnchorPosition = value; }
        }

        /// <summary>
        /// inputField.caretBlinkRate, 插入符闪烁频率
        /// </summary>
        public float caretBlinkRate
        {
            get { return inputField.caretBlinkRate; }
            set { inputField.caretBlinkRate = value; }
        }

        /// <summary>
        /// inputField.caretWidth, 插入符宽度
        /// </summary>
        public int caretWidth
        {
            get { return inputField.caretWidth; }
            set { inputField.caretWidth = value; }
        }

        /// <summary>
        /// inputField.customCaretColor, 是否设置自定义插入符颜色
        /// </summary>
        public bool customCaretColor
        {
            get { return inputField.customCaretColor; }
            set { inputField.customCaretColor = value; }
        }
        /// <summary>
        /// inputField.caretColor, 插入符颜色
        /// </summary>
        public Color caretColor
        {
            get { return inputField.caretColor; }
            set { inputField.caretColor = value; }
        }

        /// <summary>
        /// inputField.caretPosition, 插入符位置
        /// </summary>
        public int caretPosition
        {
            get { return inputField.caretPosition; }
            set { inputField.caretPosition = value; }
        }

        /// <summary>
        /// inputField.selectionColor, 选中颜色
        /// </summary>
        public Color selectionColor
        {
            get { return inputField.selectionColor; }
            set { inputField.selectionColor = value; }
        }

        /// <summary>
        /// inputField.asteriskChar, 密码输入是的替代符
        /// </summary>
        public char asteriskChar
        {
            get { return inputField.asteriskChar; }
            set { inputField.asteriskChar = value; }
        }

        /// <summary>
        /// inputField.readOnly, 是否只读
        /// </summary>
        public bool readOnly
        {
            get { return inputField.readOnly; }
            set { inputField.readOnly = value; }
        }

        /// <summary>
        /// inputField.shouldHideMobileInput, 隐藏移动键盘输入
        /// </summary>
        public bool shouldHideMobileInput
        {
            get { return inputField.shouldHideMobileInput; }
            set { inputField.shouldHideMobileInput = value; }
        }

        /// <summary>
        /// inputField.wasCanceled, 输入是否取消
        /// </summary>
        public bool wasCanceled
        {
            get { return inputField.wasCanceled; }
        }

        /// <summary>
        /// inputField.isFocused, 是否有焦点并且可以处理事件
        /// </summary>
        public bool isFocused
        {
            get { return inputField.isFocused; }
        }

        /// <summary>
        /// inputField.multiLine, 是否支持多行显示
        /// </summary>
        public bool multiLine
        {
            get { return inputField.multiLine; }
        }
        #endregion

        #region 公有方法 事件监听

        /// <summary>
        /// 添加值变化事件委托
        /// </summary>
        /// <param name="action"></param>
        public void AddChangedListener(UnityAction<string> action)
        {
            inputField.onValueChanged.AddListener(action);
        }

        /// <summary>
        /// 移除值变化事件委托
        /// </summary>
        /// <param name="action"></param>
        public void RemoveChangeListener(UnityAction<string> action)
        {
            inputField.onValueChanged.RemoveListener(action);
        }

        /// <summary>
        /// 移除所有值变化事件委托
        /// </summary>
        public void RemoveAllChangeListeners()
        {
            inputField.onValueChanged.RemoveAllListeners();
        }

        /// <summary>
        /// 添加结束输入事件委托
        /// </summary>
        /// <param name="action"></param>
        public void AddEndEditListener(UnityAction<string> action)
        {
            inputField.onEndEdit.AddListener(action);
        }

        /// <summary>
        /// 移除结束输入事件委托
        /// </summary>
        /// <param name="action"></param>
        public void RemoveEndEditListener(UnityAction<string> action)
        {
            inputField.onEndEdit.RemoveListener(action);
        }

        /// <summary>
        /// 移除所有结束输入事件委托
        /// </summary>
        public void RemoveAllEndEditListeners()
        {
            inputField.onEndEdit.RemoveAllListeners();
        }

        /// <summary>
        /// 添加输入验证委托
        /// </summary>
        /// <param name="action"></param>
        public void AddValidateListener(InputField.OnValidateInput action)
        {
            inputField.onValidateInput = action;
        }

        /// <summary>
        /// 替换输入验证委托
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public InputField.OnValidateInput ReplaceValidateListener(InputField.OnValidateInput action)
        {
            InputField.OnValidateInput temp = inputField.onValidateInput;
            inputField.onValidateInput = action;
            return temp;
        }

        /// <summary>
        /// 移除输入验证委托
        /// </summary>
        /// <param name="action"></param>
        public void RemoveValidateListener()
        {
            inputField.onValidateInput = null;
        }

        #endregion

        #region 公有方法 显示或者隐藏输入框

        /// <summary>
        /// inputField.ActivateInputField, 激活输入框
        /// </summary>
        public void ActivateInputField()
        {
            inputField.ActivateInputField();
        }

        /// <summary>
        /// inputField.DeactivateInputField, 取消激活输入框
        /// </summary>
        public void DeactivateInputField()
        {
            inputField.DeactivateInputField();
        }

        #endregion

        #region 公有方法 移动光标

        /// <summary>
        /// inputField.MoveTextStart, 移动到文本开始处
        /// </summary>
        /// <param name="shift">只移动选择位置</param>
        public void MoveTextStart(bool shift)
        {
            inputField.MoveTextStart(shift);
        }
        /// <summary>
        /// inputField.MoveTextEnd, 移动到文本结尾处
        /// </summary>
        /// <param name="shift">只移动选择位置</param>
        public void MoveTextEnd(bool shift)
        {
            inputField.MoveTextEnd(shift);
        }

        #endregion

        #region 重写方法

        protected override void Reset()
        {
            base.Reset();
            _inputField = null;
        }

        #endregion
    }
}

