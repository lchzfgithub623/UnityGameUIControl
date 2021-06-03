using UnityEngine;
using UnityEngine.UI;

namespace LightFramework.UI
{
    /// <summary>
    /// 封装的文本框控件
    /// </summary>
    [AddComponentMenu("UI/GameUI/GameUIText", 10)]
    public class GameUIText : GameUIBaseGrapha
    {
        #region 私有字段

        private Text _label;

        private string _text;

        #endregion

        #region 公有字段

        [Tooltip("字符串格式化文本")]
        public string FormatStr;

        /// <summary>
        /// 不间断空格
        /// </summary>
        public static readonly string No_Breaking_Space = "\u00A0";

        /// <summary>
        /// 是否需要把半角空格替换为不间断(不换行)空格
        /// </summary>
        public static bool NeedNoBreakSpace = true;

        #endregion

        #region 受保护属性

        /// <summary>
        /// 文本框组件, 限当前类和子类访问
        /// </summary>
        protected Text label
        {
            get
            {
                if (_label == null)
                {
                    _label = GetComponent<Text>();
                    if (_label == null)
                    {
                        Debug.LogError("the 'Text' not found, name:" + (gameObject != null ? gameObject.name : ""));
                    }
                    //mLabel.RegisterDirtyVerticesCallback(delegate() {
                    //    if (mLabel.text.Contains(" "))
                    //    {
                    //        mLabel.text = mLabel.text.Replace(" ", No_Breaking_Space);
                    //    }
                    //});
                }
                return _label;
            }
        }

        /// <summary>
        /// 图形组件, 限当前类和子类访问
        /// </summary>
        protected override Graphic graphic
        {
            get
            {
                return label;
            }
        }

        #endregion

        #region 公有属性

        /// <summary>
        /// 文本框组件, 外部访问
        /// </summary>
        public Text Label
        {
            get { return label; }
        }

        /// <summary>
        /// 文本框内容
        /// 赋值时, 会把半角空格替换为不间断(不换行)空格
        /// </summary>
        public string text
        {
            get { return _text; }
            set
            {
                _text = value;

                if (_text != null && NeedNoBreakSpace && _text.Contains(" "))
                {
                    label.text = _text.Replace(" ", No_Breaking_Space);
                }
                else
                {
                    label.text = _text;
                }
            }
        }

        /// <summary>
        /// Text.fontStyle, 字体风格
        /// </summary>
        public FontStyle fontStyle
        {
            get { return label.fontStyle; }
            set { label.fontStyle = value; }
        }

        /// <summary>
        /// Text.fontSize, 字体大小
        /// </summary>
        public int fontSize
        {
            get { return label.fontSize; }
            set { label.fontSize = value; }
        }

        /// <summary>
        /// Text.lineSpacing, 行间距
        /// </summary>
        public float lineSpacing
        {
            get { return label.lineSpacing; }
            set { label.lineSpacing = value; }
        }

        /// <summary>
        /// Text.supportRichText, 是否支持富文本
        /// </summary>
        public bool supportRichText
        {
            get { return label.supportRichText; }
            set { label.supportRichText = value; }
        }

        /// <summary>
        /// Text.alignment, 设置文本对齐方式
        /// </summary>
        public TextAnchor alignment
        {
            get {  return label.alignment; }
            set { label.alignment = value; }
        }

        /// <summary>
        /// Text.horizontalOverflow, 横向包围或溢出
        /// </summary>
        public HorizontalWrapMode horizontalOverflow
        {
            get { return label.horizontalOverflow; }
            set { label.horizontalOverflow = value; }
        }

        /// <summary>
        /// Text.verticalOverflow, 竖直截断或溢出
        /// </summary>
        public VerticalWrapMode verticalOverflow
        {
            get { return label.verticalOverflow; }
            set { label.verticalOverflow = value; }
        }

        /// <summary>
        /// Text.resizeTextForBestFit, 自适应
        /// </summary>
        public bool bestFit
        {
            get { return label.resizeTextForBestFit; }
            set { label.resizeTextForBestFit = value; }
        }

        /// <summary>
        /// Text.resizeTextMinSize, 自适应 最小字体值
        /// </summary>
        public int resizeTextMinSize
        {
            get { return label.resizeTextMinSize; }
            set { label.resizeTextMinSize = value; }
        }

        /// <summary>
        /// Text.resizeTextMaxSize, 自适应 最大字体值
        /// 1=无限大。
        /// </summary>
        public int resizeTextMaxSize
        {
            get { return label.resizeTextMaxSize; }
            set { label.resizeTextMaxSize = value; }
        }

        /// <summary>
        /// Text.preferredWidth, 优先宽度
        /// </summary>
        public float preferredWidth
        {
            get { return label.preferredWidth; }
        }

        /// <summary>
        /// Text.preferredHeight, 优先高度
        /// </summary>
        public float preferredHeight
        {
            get { return label.preferredHeight; }
        }

        #endregion

        #region 公有方法 设置文本

        /// <summary>
        /// 设置文本内容, 会把半角空格替换为不间断(不换行)空格
        /// </summary>
        /// <param name="txt"></param>
        public void SetText(string txt)
        {
            text = txt;
        }

        /// <summary>
        /// 设置文本内容, 不会把半角空格替换为不间断(不换行)空格
        /// </summary>
        /// <param name="txt"></param>
        public void SetOriginalText(string txt)
        {
            label.text = txt;
        }




        /// <summary>
        /// 填写格式化字符串内容, 会把半角空格替换为不间断(不换行)空格
        /// </summary>
        /// <param name="args"></param>
        public void SetTextFormatValues(params object[] args)
        {
            if(!string.IsNullOrEmpty(FormatStr))
            {
                text = string.Format(FormatStr, args);
            }
        }

        /// <summary>
        /// 填写格式化字符串内容, 会把半角空格替换为不间断(不换行)空格
        /// </summary>
        /// <param name="arg1"></param>
        public void SetTextFormatValues1(object arg1)
        {
            if (!string.IsNullOrEmpty(FormatStr))
            {
                text = string.Format(FormatStr, arg1.ToString());
            }
        }

        /// <summary>
        /// 填写格式化字符串内容, 会把半角空格替换为不间断(不换行)空格
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public void SetTextFormatValues2(object arg1, object arg2)
        {
            if (!string.IsNullOrEmpty(FormatStr))
            {
                text = string.Format(FormatStr, arg1.ToString(), arg2.ToString());
            }
        }

        /// <summary>
        /// 填写格式化字符串内容, 会把半角空格替换为不间断(不换行)空格
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        public void SetTextFormatValues3(object arg1, object arg2, object arg3)
        {
            if (!string.IsNullOrEmpty(FormatStr))
            {
                text = string.Format(FormatStr, arg1.ToString(), arg2.ToString(), arg3.ToString());
            }
        }




        /// <summary>
        /// 填写格式化字符串内容, 不会把半角空格替换为不间断(不换行)空格
        /// </summary>
        /// <param name="args"></param>
        public void SetOriginalTextFormatValues(params object[] args)
        {
            if (!string.IsNullOrEmpty(FormatStr))
            {
                label.text = string.Format(FormatStr, args);
            }
        }

        /// <summary>
        /// 填写格式化字符串内容, 不会把半角空格替换为不间断(不换行)空格
        /// </summary>
        /// <param name="arg1"></param>
        public void SetOriginalTextFormatValues1(object arg1)
        {
            if (!string.IsNullOrEmpty(FormatStr))
            {
                label.text = string.Format(FormatStr, arg1.ToString());
            }
        }

        /// <summary>
        /// 填写格式化字符串内容, 不会把半角空格替换为不间断(不换行)空格
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public void SetOriginalTextFormatValues2(object arg1, object arg2)
        {
            if (!string.IsNullOrEmpty(FormatStr))
            {
                label.text = string.Format(FormatStr, arg1.ToString(), arg2.ToString());
            }
        }

        /// <summary>
        /// 填写格式化字符串内容, 不会把半角空格替换为不间断(不换行)空格
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        public void SetOriginalTextFormatValues3(object arg1, object arg2, object arg3)
        {
            if (!string.IsNullOrEmpty(FormatStr))
            {
                label.text = string.Format(FormatStr, arg1.ToString(), arg2.ToString(), arg3.ToString());
            }
        }



        /// <summary>
        /// 填写字符串格式以及内容, 会把半角空格替换为不间断(不换行)空格
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="args"></param>
        public void SetTextWithFormat(string format, object[] args)
        {
            if (!string.IsNullOrEmpty(format))
            {
                text = string.Format(format, args);
            }
        }

        /// <summary>
        /// 填写字符串格式以及内容, 会把半角空格替换为不间断(不换行)空格
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="arg1"></param>
        public void SetTextWithFormat1(string format, object arg1)
        {
            if (!string.IsNullOrEmpty(format))
            {
                text = string.Format(format, arg1.ToString());
            }
        }

        /// <summary>
        /// 填写字符串格式以及内容, 会把半角空格替换为不间断(不换行)空格
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public void SetTextWithFormat2(string format, object arg1, object arg2)
        {
            if (!string.IsNullOrEmpty(format))
            {
                text = string.Format(format, arg1.ToString(), arg2.ToString());
            }
        }

        /// <summary>
        /// 填写字符串格式以及内容, 会把半角空格替换为不间断(不换行)空格
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        public void SetTextWithFormat3(string format, object arg1, object arg2, object arg3)
        {
            if (!string.IsNullOrEmpty(format))
            {
                text = string.Format(format, arg1.ToString(), arg2.ToString(), arg3.ToString());
            }
        }




        /// <summary>
        /// 填写字符串格式以及内容, 不会把半角空格替换为不间断(不换行)空格
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="args"></param>
        public void SetOriginalTextWithFormat(string format, object[] args)
        {
            if (!string.IsNullOrEmpty(format))
            {
                label.text = string.Format(format, args);
            }
        }

        /// <summary>
        /// 填写字符串格式以及内容, 不会把半角空格替换为不间断(不换行)空格
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="arg1"></param>
        public void SetOriginalTextWithFormat1(string format, object arg1)
        {
            if (!string.IsNullOrEmpty(format))
            {
                label.text = string.Format(format, arg1.ToString());
            }
        }

        /// <summary>
        /// 填写字符串格式以及内容, 不会把半角空格替换为不间断(不换行)空格
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public void SetOriginalTextWithFormat2(string format, object arg1, object arg2)
        {
            if (!string.IsNullOrEmpty(format))
            {
                label.text = string.Format(format, arg1.ToString(), arg2.ToString());
            }
        }

        /// <summary>
        /// 填写字符串格式以及内容, 不会把半角空格替换为不间断(不换行)空格
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// <param name="args"></param>
        public void SetOriginalTextWithFormat3(string format, object arg1, object arg2, object arg3)
        {
            if (!string.IsNullOrEmpty(format))
            {
                label.text = string.Format(format, arg1.ToString(), arg2.ToString(), arg3.ToString());
            }
        }

        #endregion

        #region 重写方法

        protected override void Reset()
        {
            base.Reset();
            _label = null;
        }

        #endregion

    }
}


