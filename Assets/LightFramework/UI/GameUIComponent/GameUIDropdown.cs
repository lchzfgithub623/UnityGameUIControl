using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace LightFramework.UI
{
    /// <summary>
    /// 封装的下拉列表控件
    /// </summary>
    [AddComponentMenu("UI/GameUI/GameUIDropdown", 35)]
    public class GameUIDropdown : GameUIBaseGrapha
    {
        #region 私有字段

        private Dropdown _dropdown;

        private UnityAction<int> valueChangedActions;
        private bool enableValueChangeNotify = true;
        #endregion

        #region 受保护属性

        /// <summary>
        /// 下拉列表组件, 限当前类和子类访问
        /// </summary>
        protected Dropdown dropdown
        {
            get
            {
                if (_dropdown == null)
                {
                    _dropdown = GetComponent<Dropdown>();
                    if (_dropdown == null)
                    {
                        Debug.LogError("the 'Dropdown' not found, name:" + (gameObject != null ? gameObject.name : ""));
                    }
                }

                return _dropdown;
            }
        }

        /// <summary>
        /// 图形组件, 限当前类和子类访问
        /// </summary>
        protected override Graphic graphic
        {
            get
            {
                return dropdown.targetGraphic;
            }
        }

        #endregion

        #region 公有属性

        /// <summary>
        /// 下拉列表组件, 外部访问
        /// </summary>
        public Dropdown Dropdown
        {
            get { return dropdown; }
        }

        /// <summary>
        /// dropdown.interactable
        /// </summary>
        public bool interactable
        {
            get { return dropdown.interactable; }
            set { dropdown.interactable = value; }
        }

        /// <summary>
        /// dropdown.captionText, 展示的 Text 组件
        /// </summary>
        public Text captionText
        {
            get { return dropdown.captionText; }
            set { dropdown.captionText = value; }
        }

        /// <summary>
        /// dropdown.captionImage, 展示的 Image 组件
        /// </summary>
        public Image captionImage
        {
            get { return dropdown.captionImage; }
            set { dropdown.captionImage = value; }
        }

        /// <summary>
        /// dropdown.itemText, 下拉列表项中的 Text 组件
        /// </summary>
        public Text itemText
        {
            get { return dropdown.itemText; }
            set { dropdown.itemText = value; }
        }

        /// <summary>
        /// dropdown.itemText, 下拉列表项中的 Image 组件
        /// </summary>
        public Image itemImage
        {
            get { return dropdown.itemImage; }
            set { dropdown.itemImage = value; }
        }

        /// <summary>
        /// dropdown.options, 数据项列表
        /// </summary>
        public List<Dropdown.OptionData> options
        {
            get { return dropdown.options; }
            set { dropdown.options = value; }
        }

        /// <summary>
        /// dropdown.value, 下拉列表当前选中的索引, 值从0开始, 默认0
        /// </summary>
        public int value
        {
            get { return dropdown.value; }
            set { dropdown.value = value; }
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

        #region 公有方法 显示或隐藏下拉框

        /// <summary>
        /// 设置 dropdown list 显示或隐藏
        /// </summary>
        /// <param name="bShow">true:显示; false:隐藏</param>
        public void SetDropdownListVisible(bool bShow)
        {
            if (bShow)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        /// <summary>
        /// dropdown.Show, 显示dropdown list
        /// </summary>
        /// <remarks>
        /// 方法和 Hide 对应
        /// </remarks>
        public void Show()
        {
            dropdown.Show();
        }

        /// <summary>
        /// dropdown.Hide, 隐藏dropdown list
        /// </summary>
        /// <remarks>
        /// 方法和 Show 对应
        /// </remarks>
        public void Hide()
        {
            dropdown.Hide();
        }

        #endregion

        #region 公有方法 移除选项

        /// <summary>
        /// 移除一个指定的数据项
        /// </summary>
        /// <param name="option">指定的数据项</param>
        public bool RemoveOption(Dropdown.OptionData option)
        {
            bool ret = dropdown.options.Remove(option);
            if (ret)
            {
                dropdown.RefreshShownValue();
            }
            return ret;
        }

        /// <summary>
        /// 移除一个指定的索引
        /// </summary>
        /// <param name="index">指定的索引</param>
        /// <returns>false:移除失败, 索引超出范围; true:移除成功 </returns>
        public bool RemoveOption(int index)
        {
            if (index >= 0 && index < dropdown.options.Count)
            {
                dropdown.options.RemoveAt(index);
                dropdown.RefreshShownValue();
                return true;
            }
            return false;
        }

        /// <summary>
        /// dropdown.ClearOptions, 清除所有数据项
        /// </summary>
        public void ClearOptions()
        {
            dropdown.ClearOptions();
        }

        #endregion

        #region 公有方法 添加一组数据项

        /// <summary>
        /// dropdown.AddOptions, 添加数据项列表
        /// </summary>
        /// <param name="options">数据项列表</param>
        public void AddOptions(List<Dropdown.OptionData> options)
        {
            dropdown.AddOptions(options);
        }

        /// <summary>
        /// dropdown.AddOptions, 添加精灵数据项列表
        /// </summary>
        /// <param name="options">精灵数据项列表</param>
        public void AddOptions(List<Sprite> options)
        {
            dropdown.AddOptions(options);
        }

        /// <summary>
        /// dropdown.AddOptions, 添加文本数据项列表
        /// </summary>
        /// <param name="options">文本数据项列表</param>
        public void AddOptions(List<string> options)
        {
            dropdown.AddOptions(options);
        }

        #endregion

        #region 公有方法 添加单个数据项

        /// <summary>
        /// 添加一条数据项
        /// </summary>
        /// <param name="option">一条数据项</param>
        public void AddOption(Dropdown.OptionData option)
        {
            dropdown.options.Add(option);
            dropdown.RefreshShownValue();
        }

        /// <summary>
        /// 添加一条精灵数据项
        /// </summary>
        /// <param name="option">一条精灵数据项</param>
        public void AddOption(Sprite option)
        {
            dropdown.options.Add(new Dropdown.OptionData(option));
            dropdown.RefreshShownValue();
        }

        /// <summary>
        /// 添加一条文本数据项
        /// </summary>
        /// <param name="option">一条文本数据项</param>
        public void AddOption(string option)
        {
            dropdown.options.Add(new Dropdown.OptionData(option));
            dropdown.RefreshShownValue();
        }

        /// <summary>
        /// 添加一条精灵和文本数据项
        /// </summary>
        /// <param name="optText">一个文本</param>
        /// <param name="optImg">一个精灵</param>
        public void AddOption(string optText, Sprite optImg)
        {
            dropdown.options.Add(new Dropdown.OptionData(optText, optImg));
            dropdown.RefreshShownValue();
        }

        #endregion

        #region 公有方法 事件监听

        /// <summary>
        /// 获取值变化事件的监听个数
        /// </summary>
        public int GetValueChangedListenerCount()
        {
            if(valueChangedActions != null)
            {
                return valueChangedActions.GetInvocationList().Length;
            }
            return 0;
        }

        /// <summary>
        /// 添加值变化事件监听
        /// </summary>
        /// <param name="action"></param>
        /// <remarks>
        /// 和方法 RemoveChangeListener 对应
        /// 同一个action委托不会重复添加
        /// </remarks>
        public void AddValueChangedListener(UnityAction<int> action)
        {
            if (valueChangedActions == null)
            {
                dropdown.onValueChanged.RemoveListener(OnBaseValueChanged);
                dropdown.onValueChanged.AddListener(OnBaseValueChanged);
            }

            valueChangedActions -= action;
            valueChangedActions += action;
        }

        /// <summary>
        /// 移除值变化事件监听
        /// </summary>
        /// <param name="action"></param>
        /// <remarks>
        /// 和方法 AddChangedListener 对应
        /// </remarks>
        public void RemoveValueChangedListener(UnityAction<int> action)
        {
            valueChangedActions -= action;
            if (valueChangedActions == null)
            {
                dropdown.onValueChanged.RemoveListener(OnBaseValueChanged);
            }
        }

        /// <summary>
        /// 移除所有值变化事件监听
        /// </summary>
        public void RemoveAllValueChangedListeners()
        {
            valueChangedActions = null;
            dropdown.onValueChanged.RemoveListener(OnBaseValueChanged);
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 设置滚动条数值
        /// </summary>
        /// <param name="input"> 当前所选内容的新索引。 </param>
        public void SetValue(int input)
        {
            dropdown.value = input;
        }

        /// <summary>
        /// 在下拉列表中设置当前选择的索引号，但不调用 ValueChanged 事件监听。
        /// </summary>
        /// <param name="input">当前所选内容的新索引。</param>
        public void SetValueWithoutNotify(int input)
        {
            bool temp = enableValueChangeNotify;
            enableValueChangeNotify = false;
            dropdown.value = input;
            enableValueChangeNotify = temp;
        }

        /// <summary>
        /// dropdown.RefreshShownValue, 刷新显示值
        /// </summary>
        public void RefreshShownValue()
        {
            dropdown.RefreshShownValue();
        }

        #endregion

        #region 私有方法 事件监听

        /// <summary>
        /// 封装的值变化事件监听
        /// </summary>
        /// <param name="arg"></param>
        private void OnBaseValueChanged(int arg)
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
            _dropdown = null;
        }
        #endregion
    }
}

