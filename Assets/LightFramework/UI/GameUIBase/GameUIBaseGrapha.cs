using UnityEngine;
using UnityEngine.UI;

namespace LightFramework.UI
{
    /// <summary>
    /// 封装grapha图形为带grapha组件的基类
    /// </summary>
    public class GameUIBaseGrapha : GameUIBaseComponent
    {
        #region 受保护属性

        /// <summary>
        /// 图形组件, 需要子类重写
        /// </summary>
        protected virtual Graphic graphic
        {
            get;set;
        }

        #endregion

        #region 公有属性

        /// <summary>
        /// graphic.color, 设置颜色值
        /// </summary>
        public Color color
        {
            get { return graphic.color; }
            set { graphic.color = value; }
        }

        /// <summary>
        /// 十六进制颜色值, 带透明度, 有没有'#'前缀都可以, 例:#RRGGBBAA RRGGBBAA
        /// </summary>
        public string colorHexStrRGBA
        {
            get
            {
                return graphic.GetColorHexStrRGBA();
            }
            set
            {
                graphic.SetColorHex(value);
            }
        }

        /// <summary>
        /// 十六进制颜色值, 不带透明度, 有没有'#'前缀都可以, 例:#RRGGBB RRGGBB
        /// </summary>
        public string colorHexStrRGB
        {
            get
            {
                return graphic.GetColorHexStrRGB();
            }
            set
            {
                graphic.SetColorHex(value);
            }
        }

        /// <summary>
        /// 透明度 [0-1]
        /// </summary>
        public float alpha
        {
            get { return graphic.GetAlpha(); }
            set
            {
                graphic.SetAlpha(value);
            }
        }

        /// <summary>
        /// 颜色模式的透明度 [0-255]
        /// </summary>
        public int alphaColorMode
        {
            get { return graphic.GetAlphaByColorMode(); }
            set
            {
                graphic.SetAlphaByColorMode(value);
            }
        }

        /// <summary>
        /// 是否接收射线
        /// </summary>
        public bool raycastTarget
        {
            get { return graphic.raycastTarget; }
            set { graphic.raycastTarget = value; }
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 设置颜色值
        /// </summary>
        /// <param name="color">颜色值</param>
        public void SetColor(Color color)
        {
            graphic.color = color;
        }

        /// <summary>
        /// 设置十六进制颜色值, 带透明度, 有没有'#'前缀都可以, 例:#RRGGBBAA RRGGBBAA
        /// 未指定透明度时alpha默认为FF
        /// </summary>
        /// <param name="colorHexStrRGBA">十六进制颜色值, 带透明度</param>
        public void SetColorHexStrRGBA(string colorHexStrRGBA)
        {
            graphic.SetColorHex(colorHexStrRGBA);
        }

        /// <summary>
        /// 设置十六进制颜色值, 不带透明度, 有没有'#'前缀都可以, 例:#RRGGBB RRGGBB
        /// </summary>
        /// <param name="colorHexStrRGB">十六进制颜色值, 不带透明度</param>
        public void SetColorHexStrRGB(string colorHexStrRGB)
        {
            graphic.SetColorHex(colorHexStrRGB);
        }

        /// <summary>
        /// 设置透明度 [0-1]
        /// </summary>
        /// <param name="alpha">透明度值[0-1]</param>
        public void SetAlpha(float alpha)
        {
            graphic.SetAlpha(alpha);
        }

        /// <summary>
        /// 设置颜色模式的透明度 [0-255]
        /// </summary>
        /// <param name="colorA">颜色透明度[0-255]</param>
        public void SetAlphaByColorMode(int colorA)
        {
            graphic.SetAlphaByColorMode(colorA);
        }

        #endregion
    }
}

