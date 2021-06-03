using UnityEngine;
using UnityEngine.UI;

namespace LightFramework.UI
{
    /// <summary>
    /// 封装的图片控件
    /// </summary>
    [AddComponentMenu("UI/GameUI/GameUIImage", 11)]
    public class GameUIImage : GameUIBaseGrapha
    {
        #region 私有字段

        private Image _image;

        #endregion

        #region 受保护属性

        /// <summary>
        /// 图片组件, 限当前类和子类访问
        /// </summary>
        protected Image image
        {
            get
            {
                if(_image == null)
                {
                    _image = GetComponent<Image>();
                    if(_image == null)
                    {
                        Debug.LogError("the 'Image' not found, name:" + (gameObject != null ? gameObject.name : ""));
                    }
                    MaxFillSize = _image.rectTransform.sizeDelta;
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
        /// image.overrideSprite, 用于渲染的覆盖精灵
        /// </summary>
        /// <remarks>
        /// 给 overrideSprite 赋值, 会保留 sprite 不变
        /// overrideSprite 再次变为null时就会显示原来的 sprite
        /// </remarks>
        public Sprite overrideSprite
        {
            get { return image.overrideSprite; }
            set { image.overrideSprite = value; }
        }

        /// <summary>
        /// image.sprite, 用于渲染此图像的精灵
        /// </summary>
        public Sprite sprite
        {
            get { return image.sprite; }
            set { image.sprite = value; }
        }

        /// <summary>
        /// image.type, 图片类型
        /// </summary>
        public Image.Type imageType
        {
            get { return image.type; }
            set { image.type = value; }
        }

        /// <summary>
        /// image.fillMethod, 填充方式
        /// </summary>
        public Image.FillMethod fillMethod
        {
            get { return image.fillMethod; }
            set { image.fillMethod = value; }
        }

        /// <summary>
        /// image.fillCenter, Whether or not to render the center of a Tiled or Sliced image
        /// 是否渲染中间区域
        /// </summary>
        /// <remarks>
        /// imageType为 Sliced 或 Tiled 时, 当sprite设置了九宫格, fillCenter 为false则不会渲染九宫格中间的部分
        /// </remarks>
        public bool fillCenter
        {
            get { return image.fillCenter; }
            set { image.fillCenter = value; }
        }

        /// <summary>
        /// image.fillOrigin, Controls the origin point of the Fill process. Value means different things with each fill method.
        /// 参考 Image枚举 OriginHorizontal  OriginVertical Origin90 Origin180 Origin360
        /// </summary>
        /// <remarks>
        /// imageType 为 Filled 有效
        /// </remarks>
        public int fillOrigin
        {
            get { return image.fillOrigin; }
            set { image.fillOrigin = value; }
        }

        /// <summary>
        /// image.fillAmount, 填充值[0-1]
        /// </summary>
        /// <remarks>
        /// imageType 为 Filled 有效
        /// </remarks>
        public float fillAmount
        {
            get { return image.fillAmount; }
            set { image.fillAmount = value; }
        }

        /// <summary>
        /// image.fillClockwise, 时钟方向 顺时针  
        /// </summary>
        /// <remarks>
        /// imageType 为 Filled,
        /// 且 fillMethod 为 Radial90  Radial180 Radial360 有效
        /// </remarks>
        public bool fillClockwise
        {
            get { return image.fillClockwise; }
            set { image.fillClockwise = value; }
        }

        /// <summary>
        /// image.preserveAspect, 是否保留sprite原有的纵横比(宽高比) 
        /// true:sprite将按照原宽高比调整尺寸
        /// </summary>
        /// <remarks>
        /// imageType 为 Simple 或 Filled 有效
        /// </remarks>
        public bool preserveAspect
        {
            get { return image.preserveAspect; }
            set { image.preserveAspect = value;}
        }

        /// <summary>
        /// The alpha threshold specifies the minimum alpha a pixel must have for the event
        /// to considered a "hit" on the Image.
        /// </summary>
        /// <remarks>
        /// 需要开启image 的 Read/WriteEnabled 
        /// </remarks>
        public float alphaHitTestMinimumThreshold
        {
            get { return image.alphaHitTestMinimumThreshold; }
            set { image.alphaHitTestMinimumThreshold = value; }
        }

        /// <summary>
        /// image.hasBorder, image是否有边框
        /// </summary>
        public bool hasBorder
        {
            get { return image.hasBorder; }
        }

        /// <summary>
        /// image.pixelsPerUnit 单位像素
        /// </summary>
        public float pixelsPerUnit
        {
            get { return image.pixelsPerUnit; }
        }

        /// <summary>
        /// 是否原始尺寸, 因为是float对比 不是最精确的, 误差在0.00001f 之内 即为原始尺寸
        /// </summary>
        public bool IsNativeSize
        {
            get
            {
                float w = overrideSprite.rect.width / pixelsPerUnit;
                float h = overrideSprite.rect.height / pixelsPerUnit;
                RectTransform rectTransform = image.rectTransform;
                float nowW = rectTransform.rect.width;
                float nowH = rectTransform.rect.height;

                return (Mathf.Abs(nowW - w) < 0.00001f) && (Mathf.Abs(nowH - h) < 0.00001f);
            }
        }

        /// <summary>
        /// 图片的最大填充调整尺寸
        /// </summary>
        public Vector2 MaxFillSize
        {
            get;set;
        }

        /// <summary>
        /// 自定义填充类型, 0:横向,1:竖向
        /// </summary>
        public int CustomFillType
        {
            get;set;
        }

        /// <summary>
        /// 填充值 [0-1]
        /// </summary>
        public float FillAmount
        {
            get
            {
                if(Image.Type.Filled == image.type)
                {
                    return image.fillAmount;
                }
                Vector2 delta = image.rectTransform.sizeDelta;
                if (0 == CustomFillType)
                {
                    return MaxFillSize.x == 0 ? 0: (delta.x / MaxFillSize.x);
                }
                return MaxFillSize.y == 0 ? 0 : (delta.y / MaxFillSize.y);
            }
            set
            {
                value = Mathf.Clamp01(value);
                if(Image.Type.Filled == image.type)
                {
                    image.fillAmount = value;
                }
                else
                {
                    Vector2 delta = image.rectTransform.sizeDelta;
                    if (0 == CustomFillType)
                    {
                        delta.x = MaxFillSize.x * value;
                    }
                    else
                    {
                        delta.y = MaxFillSize.y * value;
                    }
                    image.rectTransform.sizeDelta = delta;
                }
            }
        }

        public override bool CanTouch
        {
            get
            {
                return raycastTarget;
            }

            set
            {
                raycastTarget = value;
                SetTouchMgrEnabled(value);
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

        #region 公有方法

        /// <summary>
        /// 设置原始尺寸
        /// </summary>
        public void SetNativeSize()
        {
            image.SetNativeSize();
        }

        /// <summary>
        /// 图片置灰
        /// </summary>
        /// <param name="bGray">是否置灰</param>
        public void SetGray(bool bGray)
        {
            image.SetGray(bGray);
        }
        #endregion
    }
}


