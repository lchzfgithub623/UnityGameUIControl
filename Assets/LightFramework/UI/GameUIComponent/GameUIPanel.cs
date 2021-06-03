using UnityEngine;
using UnityEngine.UI;

namespace LightFramework.UI
{
    /// <summary>
    /// 封装的Panel控件
    /// </summary>
    [AddComponentMenu("UI/GameUI/GameUIPanel", 29)]
    public class GameUIPanel : GameUIBaseGrapha
    {
        #region 私有字段

        private Image _image;

        [SerializeField]
        private bool hasImage;

        [SerializeField]
        private Color bgColor = new Color(0, 0, 0, 0.2f);

        [SerializeField]
        private bool isTransparent;

        #endregion

        #region 受保护属性

        /// <summary>
        /// 图片组件, 限当前类和子类访问
        /// </summary>
        protected Image image
        {
            get
            {
                if (_image == null)
                {
                    _image = GetComponent<Image>();
                }
                return _image;
            }
        }

        /// <summary>
        /// 图形组件, 限当前类和子类访问
        /// </summary>
        protected override Graphic graphic
        {
            get
            {
                return image;
            }
        }

        #endregion

        #region 公有属性

        /// <summary>
        /// 图片组件, 外部访问
        /// </summary>
        public Image Image
        {
            get { return image; }
        }

        /// <summary>
        /// 面板是否有背景图
        /// </summary>
        public bool HasBgImage
        {
            get
            {
                hasImage = image != null && image.enabled;
                return hasImage;
            }
            set
            {
                if(image != null)
                {
                    hasImage = value;
                    image.enabled = hasImage;
                }
                else
                {
                    hasImage = false;
                }
                if(!hasImage)
                {
                    isTransparent = true;
                }
            }
        }

        /// <summary>
        /// 背景颜色值
        /// </summary>
        public Color BgColor
        {
            get { return bgColor; }
            set { bgColor = value; }
        }

        /// <summary>
        /// 面板背景是否完全透明
        /// </summary>
        public bool IsTransparent
        {
            get
            {
                isTransparent = image == null || !image.isActiveAndEnabled || image.color.a == 0;
                return isTransparent;
            }
            set
            {
                if (isTransparent == value)
                {
                    return;
                }
                if (image == null)
                {
                    isTransparent = true;
                    return;
                }

                isTransparent = value;
                if (isTransparent)
                {
                    image.color = Color.clear;
                }
                else
                {
                    image.color = bgColor;
                }
            }
        }

        /// <summary>
        /// 是否可以触摸
        /// </summary>
        public override bool CanTouch
        {
            get
            {
                return graphic != null && raycastTarget;
            }

            set
            {
                if(graphic != null)
                {
                    raycastTarget = value;
                    SetTouchMgrEnabled(value);
                }
            }
        }
        #endregion

        #region 重写方法

        protected override void Reset()
        {
            base.Reset();
            _image = null;
        }
        #endregion
    }
}

