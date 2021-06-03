using UnityEngine;
using UnityEngine.UI;

namespace LightFramework.UI
{
    /// <summary>
    /// Image扩展
    /// </summary>
    public static partial class ImageExtension 
    {
        /// <summary>
        /// image 的灰色材质
        /// </summary>
        public static Material GrayMaterial
        {
            set;get;
        }

        /// <summary>
        /// iamge 置灰
        /// </summary>
        /// <param name="img">对象的Image </param>
        /// <param name="bGray">true:置灰, false:不置灰</param>
        public static void SetGray(this Image img, bool bGray = true)
        {
            if(img == null)
            {
                return;
            }
            if(bGray)
            {
                img.material = GrayMaterial;
            }
            else
            {
                img.material = null;
            }
        }
    }
}

