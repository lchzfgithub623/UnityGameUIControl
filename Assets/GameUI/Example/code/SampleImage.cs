using LightFramework;
using LightFramework.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SampleImage : UIBaseControl
{
    private GameUIImage UIImageSimple;
    private GameUIImage UIImageSliced;
    private GameUIImage UIImageTiled;
    private GameUIImage UIImageFiled;

    protected override void OnInit()
    {
        base.OnInit();
        SetCtrl("UIImageSimple", ref UIImageSimple);
        SetCtrl("UIImageSliced", ref UIImageSliced);
        SetCtrl("UIImageTiled", ref UIImageTiled);
        SetCtrl("UIImageFiled", ref UIImageFiled);


        

        ImageClickEvent();
        ImageNativeSizeEvent();
        SliderImageAlphaHitEvent();
        ImagePreserveEvent();
        SliderImagePreserveEvent();

        ImageFillCenterEvent();
        TiledFillCenterEvent();

        SliderFillAmountEvent();
        FillMethodEvent();
        ClockwiseEvent();
    }

    private void ImageNativeSizeEvent()
    {
        var oldSize = UIImageSimple.GetRectTrans().rect.size;
        var btn = GetCtrl<GameUIButton>("BtnImageNativeSize");
        btn.AddClickListener(delegate () {
            var isNativeSize = UIImageSimple.IsNativeSize;
            if(!isNativeSize)
            {
                btn.SetButtonLab("设置自定义尺寸");
                UIImageSimple.SetNativeSize();
            }
            else
            {
                btn.SetButtonLab("设置原生尺寸");
                UIImageSimple.SetSizeDelta(oldSize);
            }
            var newSize = UIImageSimple.GetRectTrans().rect.size;
            SampleTest.prompt.ShowText("把当前尺寸 : " + oldSize + " 设置为" + (!isNativeSize ? "\"原生尺寸\"" : "\"自定义尺寸\"") + " : " + newSize);
        });
    }

    private void ImageClickEvent()
    {
        UIImageSimple.TouchMgr.AddClickListener(delegate (PointerEventData e) {
            SampleTest.prompt.ShowText("image 被点击");
        });
        var labStr = ".CanTouch = <color={1}>{0}</color>";
        var val = UIImageSimple.CanTouch;
        var btn = GetCtrl<GameUIButton>("BtnImageClick");
        var slider = GetCtrl<GameUISlider>("SliderImageAlphaHit");
        slider.SetActive(val);
        btn.SetButtonLab(string.Format(labStr, val, val ? "green" : "red"));
        btn.AddClickListener(delegate() {
            val = !val;
            SampleTest.prompt.ShowText("设置 image 点击事件:" + val);
            UIImageSimple.CanTouch = val;
            slider.SetActive(val);
            btn.SetButtonLab(string.Format(labStr, val, val ? "green" : "red"));
        });
    }

    private void SliderImageAlphaHitEvent()
    {
        var labStr = ".alphaHitTestMinimumThreshold = <color=green>{0}</color>";
        var alphaHitTestMinimumThreshold = UIImageSimple.alphaHitTestMinimumThreshold;
        var slider = GetCtrl<GameUISlider>("SliderImageAlphaHit");
        var txt = GetCtrl<GameUIText>("TxtImageAlphaHit");
        slider.value = alphaHitTestMinimumThreshold;
        txt.FormatStr = labStr;
        txt.SetTextFormatValues(alphaHitTestMinimumThreshold);
        slider.AddValueChangedListener(delegate (float v)
        {
            alphaHitTestMinimumThreshold = Mathf.Round(v * 100f) / 100;
            SampleTest.prompt.ShowText("image 透明度大于等于: " + alphaHitTestMinimumThreshold + " 才能触发点击事件");
            UIImageSimple.alphaHitTestMinimumThreshold = alphaHitTestMinimumThreshold;
            txt.SetTextFormatValues(alphaHitTestMinimumThreshold);
        });
    }


    private void ImagePreserveEvent()
    {
        var labStr = ".preserveAspect = <color={1}>{0}</color>";
        
        var img = GetCtrl<GameUIImage>("UIImageSimpleBae");
        var btn = GetCtrl<GameUIButton>("BtnImagePreserve");
        var val = img.preserveAspect;
        btn.SetButtonLab(string.Format(labStr, val, val ? "green" : "red"));
        btn.AddClickListener(delegate () {
            val = !val;
            img.preserveAspect = val;
            SampleTest.prompt.ShowText("设置 image" + (val ? "改变图像尺寸时根据原有的宽高比 等比改变" : "改变尺寸 时不考虑等比改变"));
            btn.SetButtonLab(string.Format(labStr, val, val ? "green" : "red"));
        });
    }

    private void SliderImagePreserveEvent()
    {
        var labStr = "修改 width = <color=green>{0}</color> \n对应 height = <color=green>{1}</color>";
        var img = GetCtrl<GameUIImage>("UIImageSimpleBae");
        var slider = GetCtrl<GameUISlider>("SliderImagePreserve");
        var txt = GetCtrl<GameUIText>("TxtImagePreserve");
        var rect = img.GetRectTrans().rect;
        slider.maxValue = rect.width;
        slider.value = rect.width;
        txt.FormatStr = labStr;
        txt.SetTextFormatValues(rect.width, rect.height);
        slider.AddValueChangedListener(delegate (float v)
        {
            var size = new Vector2(v, rect.height);
            img.GetRectTrans().sizeDelta = size;
            txt.SetTextFormatValues(img.GetRectTrans().sizeDelta.x, img.GetRectTrans().sizeDelta.y);
        });
    }


    private void ImageFillCenterEvent()
    {
        var labStr = ".fillCenter = <color={1}>{0}</color>";
        var val = UIImageSliced.fillCenter;
        var btn = GetCtrl<GameUIButton>("BtnImageFillCenter");
        btn.SetButtonLab(string.Format(labStr, val, val ? "green" : "red"));
        btn.AddClickListener(delegate () {
            val = !val;
            UIImageSliced.fillCenter = val;
            btn.SetButtonLab(string.Format(labStr, val, val ? "green" : "red"));
        });
    }
    private void TiledFillCenterEvent()
    {
        var labStr = ".fillCenter = <color={1}>{0}</color>";
        var val = UIImageTiled.fillCenter;
        var btn = GetCtrl<GameUIButton>("BtnTiledFillCenter");
        btn.SetButtonLab(string.Format(labStr, val, val ? "green" : "red"));
        btn.AddClickListener(delegate () {
            val = !val;
            UIImageTiled.fillCenter = val;
            btn.SetButtonLab(string.Format(labStr, val, val ? "green" : "red"));
        });
    }

    private void FillMethodEvent()
    {
        var labStr = ".fillMethod = <color=green>Image.FillMethod.{0}</color>";
        var fillMethod = UIImageFiled.fillMethod;
        var btn = GetCtrl<GameUIButton>("BtnFillMethod");
        var btnClockwise = GetCtrl<GameUIButton>("BtnClockwise");
        var verticalOverflowList = new List<Image.FillMethod>() {
            Image.FillMethod.Horizontal, Image.FillMethod.Vertical,
            Image.FillMethod.Radial90, Image.FillMethod.Radial180, Image.FillMethod.Radial360
        };
        btn.SetButtonLab(string.Format(labStr, fillMethod));
        btnClockwise.SetActive(fillMethod == Image.FillMethod.Radial90 || fillMethod == Image.FillMethod.Radial180 || fillMethod == Image.FillMethod.Radial360);
        FillOriginEvent();
        btn.AddClickListener(delegate () {
            fillMethod = UIImageFiled.fillMethod;
            var fIndex = verticalOverflowList.FindIndex(t => t == fillMethod);
            var nextIndex = fIndex + 1;
            if (nextIndex >= verticalOverflowList.Count)
            {
                nextIndex = 0;
            }
            fillMethod = verticalOverflowList[nextIndex];
            UIImageFiled.fillMethod = fillMethod;
            btn.SetButtonLab(string.Format(labStr, fillMethod));
            btnClockwise.SetActive(fillMethod == Image.FillMethod.Radial90 || fillMethod == Image.FillMethod.Radial180 || fillMethod == Image.FillMethod.Radial360);
            FillOriginEvent();
        });
    }

    private void SliderFillAmountEvent()
    {
        var labStr = ".fillAmount = <color=green>{0}</color>";
        var fillAmount = UIImageFiled.fillAmount;
        var slider = GetCtrl<GameUISlider>("SliderFillAmount");
        var txt = GetCtrl<GameUIText>("TxtFillAmount");
        slider.value = fillAmount;
        txt.FormatStr = labStr;
        txt.SetTextFormatValues(fillAmount);
        slider.AddValueChangedListener(delegate (float v)
        {
            fillAmount = Mathf.Round(v * 100f) / 100;
            UIImageFiled.fillAmount = fillAmount;
            txt.SetTextFormatValues(fillAmount);
        });
    }

    private void FillOriginEvent()
    {
        var originType = "";
        var originName = "";
        var labStr = ".fillOrigin = <color=green>{1}.{0}</color>";
        var fillOrigin = UIImageFiled.fillOrigin;
        var fillMethod = UIImageFiled.fillMethod;
        var btn = GetCtrl<GameUIButton>("BtnFillOrigin");

        Action<int> getEnumName = delegate (int v) {
            if (fillMethod == Image.FillMethod.Horizontal)
            {
                originName = ((Image.OriginHorizontal)v).ToString();
            }
            else if (fillMethod == Image.FillMethod.Vertical)
            {
                originName = ((Image.OriginVertical)v).ToString();
            }
            else if (fillMethod == Image.FillMethod.Radial90)
            {
                originName = ((Image.Origin90)v).ToString();
            }
            else if (fillMethod == Image.FillMethod.Radial180)
            {
                originName = ((Image.Origin180)v).ToString();
            }
            else if (fillMethod == Image.FillMethod.Radial360)
            {
                originName = ((Image.Origin360)v).ToString();
            }
        };
        var fillOriginList = new List<int>() {};
        if(fillMethod == Image.FillMethod.Horizontal)
        {
            originType = "(int)Image.OriginHorizontal";
            fillOriginList.Add((int)Image.OriginHorizontal.Left);
            fillOriginList.Add((int)Image.OriginHorizontal.Right);
        }
        else if(fillMethod == Image.FillMethod.Vertical)
        {
            originType = "(int)Image.OriginVertical";
            fillOriginList.Add((int)Image.OriginVertical.Bottom);
            fillOriginList.Add((int)Image.OriginVertical.Top);
        }
        else if (fillMethod == Image.FillMethod.Radial90)
        {
            originType = "(int)Image.Origin90";
            fillOriginList.Add((int)Image.Origin90.BottomLeft);
            fillOriginList.Add((int)Image.Origin90.BottomRight);
            fillOriginList.Add((int)Image.Origin90.TopLeft);
            fillOriginList.Add((int)Image.Origin90.TopRight);
        }
        else if (fillMethod == Image.FillMethod.Radial180)
        {
            originType = "(int)Image.Origin180";
            fillOriginList.Add((int)Image.Origin180.Bottom);
            fillOriginList.Add((int)Image.Origin180.Left);
            fillOriginList.Add((int)Image.Origin180.Right);
            fillOriginList.Add((int)Image.Origin180.Top);
        }
        else if (fillMethod == Image.FillMethod.Radial360)
        {
            originType = "(int)Image.Origin360";
            fillOriginList.Add((int)Image.Origin360.Bottom);
            fillOriginList.Add((int)Image.Origin360.Left);
            fillOriginList.Add((int)Image.Origin360.Right);
            fillOriginList.Add((int)Image.Origin360.Top);
        }
        getEnumName(fillOrigin);
        btn.SetButtonLab(string.Format(labStr, originName, originType));
        btn.RemoveAllClickListeners();
        btn.AddClickListener(delegate () {
            fillOrigin = UIImageFiled.fillOrigin;
            var fIndex = fillOriginList.FindIndex(t => t == fillOrigin);
            if(fIndex == -1)
            {
                fIndex = 0;
            }
            var nextIndex = fIndex + 1;
            if (nextIndex >= fillOriginList.Count)
            {
                nextIndex = 0;
            }
            fillOrigin = fillOriginList[nextIndex];
            UIImageFiled.fillOrigin = fillOrigin;

            getEnumName(fillOrigin);
            btn.SetButtonLab(string.Format(labStr, originName, originType));
        });
    }

    private void ClockwiseEvent()
    {
        var labStr = ".fillClockwise = <color={1}>{0}</color>";
        var val = UIImageFiled.fillClockwise;
        var btn = GetCtrl<GameUIButton>("BtnClockwise");
        btn.SetButtonLab(string.Format(labStr, val, val ? "green" : "red"));
        btn.AddClickListener(delegate () {
            val = !val;
            UIImageFiled.fillClockwise = val;
            btn.SetButtonLab(string.Format(labStr, val, val ? "green" : "red"));
        });
    }
}
