using UnityEngine;
using UnityEngine.UI;

namespace LightFramework.UI
{
    /// <summary>
    /// Graphic扩展
    /// </summary>
    public static partial class GraphicExtension
    {
        /// <summary>
        /// 以十六进制字符串的形式返回颜色，格式为“RRGGBBAA”, 带透明度
        /// </summary>
        /// <param name="graphic">对象的 GameObject </param>
        /// <returns></returns>
        public static string GetColorHexStrRGBA(this Graphic graphic)
        {
            Color c = graphic.color;
            return ColorUtility.ToHtmlStringRGBA(c);
        }

        /// <summary>
        /// 以十六进制字符串的形式返回颜色，格式为“#RRGGBBAA”, 带透明度
        /// </summary>
        /// <param name="graphic">对象的 GameObject </param>
        /// <returns></returns>
        public static string GetColorHexStrRGBAWithSharp(this Graphic graphic)
        {
            Color c = graphic.color;
            return "#" + ColorUtility.ToHtmlStringRGBA(c);
        }

        /// <summary>
        /// 以十六进制字符串的形式返回颜色，格式为“RRGGBB”, 不带透明度
        /// </summary>
        /// <param name="graphic">对象的 GameObject </param>
        /// <returns></returns>
        public static string GetColorHexStrRGB(this Graphic graphic)
        {
            Color c = graphic.color;
            return ColorUtility.ToHtmlStringRGB(c);
        }

        /// <summary>
        /// 以十六进制字符串的形式返回颜色，格式为“#RRGGBB”, 不带透明度
        /// </summary>
        /// <param name="graphic">对象的 GameObject </param>
        /// <returns></returns>
        public static string GetColorHexStrRGBWithSharp(this Graphic graphic)
        {
            Color c = graphic.color;
            return "#" + ColorUtility.ToHtmlStringRGB(c);
        }

        /// <summary>
        /// 设置十六进制颜色值, 有没有'#'前缀都可以, 例:#RRGGBBAA RRGGBBAA  #RRGGBB RRGGBB
        /// 未指定透明度时alpha默认为FF
        /// </summary>
        /// <param name="graphic">对象的 GameObject </param>
        /// <param name="colorHexStr"></param>
        public static void SetColorHex(this Graphic graphic, string colorHexStr)
        {
            if(graphic == null)
            {
                return;
            }
            Color c;
            if (!colorHexStr.StartsWith("#"))
            {
                colorHexStr = "#" + colorHexStr;
            }
            bool ret = ColorUtility.TryParseHtmlString(colorHexStr, out c);
            if (ret)
            {
                graphic.color = c;
            }
        }

        /// <summary>
        /// 设置十六进制颜色值, 要有'#'前缀都, 例:#RRGGBBAA  #RRGGBB 
        /// 未指定透明度时alpha默认为FF
        /// </summary>
        /// <param name="graphic">对象的 GameObject </param>
        /// <param name="colorHexStr">带有#前缀颜十六进制色值</param>
        /// <remarks>
        /// colorHexStr 也支持传入以下词组
        /// red, cyan, blue, darkblue, lightblue, purple, yellow, lime, fuchsia, white, silver, grey, black, orange, brown, maroon, green, olive, navy, teal, aqua, magenta
        /// </remarks>
        public static void SetColorHexWithSharp(this Graphic graphic, string colorHexStr)
        {
            if (graphic == null)
            {
                return;
            }
            Color c;
            bool ret = ColorUtility.TryParseHtmlString(colorHexStr, out c);
            if (ret)
            {
                graphic.color = c;
            }
        }

        /// <summary>
        /// 获取透明度 [0-1]
        /// </summary>
        /// <param name="graphic">对象的 GameObject </param>
        /// <returns></returns>
        public static float GetAlpha(this Graphic graphic)
        {
            return graphic.color.a;
        }

        /// <summary>
        /// 获取透明度 [0-255]  颜色模式的透明度
        /// </summary>
        /// <param name="graphic">对象的 GameObject </param>
        /// <returns></returns>
        public static int GetAlphaByColorMode(this Graphic graphic)
        {
            return (int)(graphic.color.a * 255);
        }

        /// <summary>
        /// 设置透明度
        /// </summary>
        /// <param name="graphic">对象的 GameObject </param>
        /// <param name="alpha">[0-1]</param>
        public static void SetAlpha(this Graphic graphic, float alpha)
        {
            if(graphic == null)
            {
                return;
            }
            Color c = graphic.color;
            c.a = alpha;
            graphic.color = c;
        }

        /// <summary>
        /// 设置透明度 颜色模式的透明度
        /// </summary>
        /// <param name="graphic">对象的 GameObject </param>
        /// <param name="colorA">[0-255]</param>
        public static void SetAlphaByColorMode(this Graphic graphic, int colorA)
        {
            if (graphic == null)
            {
                return;
            }
            Color c = graphic.color;
            c.a = (float)colorA / 255;
            graphic.color = c;
        }
    }
}

