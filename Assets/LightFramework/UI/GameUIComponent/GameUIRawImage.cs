using UnityEngine;
using UnityEngine.UI;

namespace LightFramework.UI
{
    /// <summary>
    /// 封装的 RawImagge 控件
    /// </summary>
    [AddComponentMenu("UI/GameUI/GameUIRawImage", 12)]
    public class GameUIRawImage : GameUIBaseGrapha
    {
        #region 私有字段

        private RawImage _rawImage;

        #endregion

        #region 受保护字段

        /// <summary>
        /// RawImage组件, 限当前类和子类访问
        /// </summary>
        protected RawImage rawImage
        {
            get
            {
                if(_rawImage == null)
                {
                    _rawImage = GetComponent<RawImage>();
                    if (_rawImage == null)
                    {
                        Debug.LogError("the 'RawImage' not found, name:" + (gameObject != null ? gameObject.name : ""));
                    }
                }
                return _rawImage;
            }
        }

        /// <summary>
        /// 图形组件, 限当前类和子类访问
        /// </summary>
        protected override Graphic graphic
        {
            get
            {
                return rawImage;
            }
        }

        #endregion

        #region 公有属性

        /// <summary>
        /// RawImage组件, 外部访问
        /// </summary>
        public RawImage RawImage
        {
            get { return rawImage; }
        }

        /// <summary>
        /// rawImage.texture, 设置或获取texture
        /// </summary>
        public Texture texture
        {
            get { return rawImage.texture; }
            set { rawImage.texture = value; }
        }

        /// <summary>
        /// The RawImage texture coordinates.
        /// </summary>
        public Rect uvRect
        {
            get { return rawImage.uvRect; }
            set { rawImage.uvRect = value; }
        }

        #endregion

        #region 重写方法

        protected override void Reset()
        {
            base.Reset();
            _rawImage = null;
        }

        #endregion
    }
}

