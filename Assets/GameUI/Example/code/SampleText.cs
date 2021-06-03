using LightFramework;
using LightFramework.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleText : UIBaseControl
{
    private GameUIText UIText;

    const string BreakStr = "111111111111111111111111111111111111111 bbbbbbbb";
    const string InitStr = "选择对应的操作_<color=red>选择对应的操作</color>_选择对应的操作_选择对应的操作_选择对应的操作_选择对应的操作_选择对应的操作";

    int oldFontSize = 20;
    protected override void OnInit()
    {
        base.OnInit();
        SetCtrl("UIText", ref UIText);
        oldFontSize = UIText.fontSize;


        GetCtrl<GameUIButton>("BtnBreakLine").AddClickListener(delegate () {
            GameUIText.NeedNoBreakSpace = false;
            UIText.text = BreakStr;
        });
        GetCtrl<GameUIButton>("BtnNoBreakLine").AddClickListener(delegate () {
            GameUIText.NeedNoBreakSpace = true;
            UIText.text = BreakStr;
        });

        GetCtrl<GameUIButton>("BtnReset").AddClickListener(ResetOriginalText);

        TextFontStyleEvent();
        RichTextEvent();
        TextAlignmentEvent();
        FontSizeEvent();
        LinSpaceEvent();
        HorizontalOverflowEvent();
        VerticalOverflowEvent();
        BestFitEvent();

        ResetOriginalText();
    }

    private void ResetOriginalText()
    {
        UIText.text = InitStr;
        UIText.fontSize = oldFontSize;
        UIText.lineSpacing = 1;
    }

    private void TextFontStyleEvent()
    {
        var labStr = ".fontStyle = <color=green>FontStyle.{0}</color>";
        var fontStyle = UIText.fontStyle;
        var btn = GetCtrl<GameUIButton>("BtnFontStyle");
        var fontStyleList = new List<FontStyle>() { FontStyle.Normal, FontStyle.Bold, FontStyle.Italic, FontStyle.BoldAndItalic };
        btn.SetButtonLab(string.Format(labStr, fontStyle));
        btn.AddClickListener(delegate () {
            fontStyle = UIText.fontStyle;
            var fIndex = fontStyleList.FindIndex(t => t == fontStyle);
            var nextIndex = fIndex + 1;
            if (nextIndex >= fontStyleList.Count)
            {
                nextIndex = 0;
            }
            fontStyle = fontStyleList[nextIndex];
            UIText.fontStyle = fontStyle;

            btn.SetButtonLab(string.Format(labStr, fontStyle));
        });
    }
    private void RichTextEvent()
    {
        var labStr = ".supportRichText = <color={1}>{0}</color>";
        var supportRichText = UIText.supportRichText;
        var toggle = GetCtrl<GameUIToggle>("ToggleRichText");
        var txt = GetCtrl<GameUIText>("LabelRichText");
        toggle.isOn = supportRichText;
        txt.FormatStr = labStr;
        txt.SetTextFormatValues(supportRichText, supportRichText ? "green" : "red");
        toggle.AddValueChangedListener(delegate (bool v) {
            supportRichText = v;
            UIText.supportRichText = supportRichText;
            txt.SetTextFormatValues(supportRichText, supportRichText ? "green" : "red");
        });
    }

    private void TextAlignmentEvent()
    {
        var labStr = ".alignment = <color=green>TextAnchor.{0}</color>";
        var alignment = UIText.alignment;
        var btn = GetCtrl<GameUIButton>("BtnAlignment");
        var alignmentList = new List<TextAnchor>() {
            TextAnchor.LowerCenter, TextAnchor.LowerLeft, TextAnchor.LowerRight,
            TextAnchor.MiddleCenter, TextAnchor.MiddleLeft, TextAnchor.MiddleRight,
            TextAnchor.UpperCenter, TextAnchor.UpperLeft, TextAnchor.UpperRight
        };
        btn.SetButtonLab(string.Format(labStr, alignment));
        btn.AddClickListener(delegate () {
            alignment = UIText.alignment;
            var fIndex = alignmentList.FindIndex(t => t == alignment);
            var nextIndex = fIndex + 1;
            if (nextIndex >= alignmentList.Count)
            {
                nextIndex = 0;
            }
            alignment = alignmentList[nextIndex];
            UIText.alignment = alignment;

            btn.SetButtonLab(string.Format(labStr, alignment));
        });
    }

    private void FontSizeEvent()
    {
        var labStr = ".fontSize = <color=green>{0}</color>";
        var fontSize = UIText.fontSize;
        var slider = GetCtrl<GameUISlider>("SliderFontSize");
        var txt = GetCtrl<GameUIText>("TxtFontSize");
        slider.maxValue = fontSize;
        slider.value = fontSize;
        txt.FormatStr = labStr;
        txt.SetTextFormatValues(fontSize);
        slider.AddValueChangedListener(delegate (float v) {
            fontSize = Mathf.FloorToInt(v);
            UIText.fontSize = fontSize;
            txt.SetTextFormatValues(fontSize);
        });
    }
    private void LinSpaceEvent()
    {
        var labStr = ".lineSpacing = <color=green>{0}</color>";
        var lineSpacing = UIText.lineSpacing;
        var slider = GetCtrl<GameUISlider>("SliderLineSpace");
        var txt = GetCtrl<GameUIText>("TxtLineSpace");
        slider.maxValue = 5f;
        slider.value = lineSpacing;
        txt.FormatStr = labStr;
        txt.SetTextFormatValues(lineSpacing);
        slider.AddValueChangedListener(delegate (float v) {
            UIText.lineSpacing = v;
            txt.SetTextFormatValues(v);
        });
    }

    private void HorizontalOverflowEvent()
    {
        var labStr = ".horizontalOverflow = <color=green>HorizontalWrapMode.{0}</color>";
        var horizontalOverflow = UIText.horizontalOverflow;
        var btn = GetCtrl<GameUIButton>("BtnHorizontalOverflow");
        var horizontalOverflowList = new List<HorizontalWrapMode>() {
            HorizontalWrapMode.Wrap, HorizontalWrapMode.Overflow
        };
        btn.SetButtonLab(string.Format(labStr, horizontalOverflow));
        btn.AddClickListener(delegate () {
            horizontalOverflow = UIText.horizontalOverflow;
            var fIndex = horizontalOverflowList.FindIndex(t => t == horizontalOverflow);
            var nextIndex = fIndex + 1;
            if (nextIndex >= horizontalOverflowList.Count)
            {
                nextIndex = 0;
            }
            horizontalOverflow = horizontalOverflowList[nextIndex];
            UIText.horizontalOverflow = horizontalOverflow;

            btn.SetButtonLab(string.Format(labStr, horizontalOverflow));
        });
    }

    private void VerticalOverflowEvent()
    {
        var labStr = ".verticalOverflow = <color=green>TextAnchor.{0}</color>";
        var verticalOverflow = UIText.verticalOverflow;
        var btn = GetCtrl<GameUIButton>("BtnVerticalOverflow");
        var verticalOverflowList = new List<VerticalWrapMode>() {
            VerticalWrapMode.Truncate, VerticalWrapMode.Overflow
        };
        btn.SetButtonLab(string.Format(labStr, verticalOverflow));
        btn.AddClickListener(delegate () {
            verticalOverflow = UIText.verticalOverflow;
            var fIndex = verticalOverflowList.FindIndex(t => t == verticalOverflow);
            var nextIndex = fIndex + 1;
            if (nextIndex >= verticalOverflowList.Count)
            {
                nextIndex = 0;
            }
            verticalOverflow = verticalOverflowList[nextIndex];
            UIText.verticalOverflow = verticalOverflow;
            UIText.text = InitStr + InitStr + InitStr + InitStr + InitStr + InitStr;
            btn.SetButtonLab(string.Format(labStr, verticalOverflow));
        });
    }

    private void BestFitEvent()
    {
        var labStr = ".bestFit = <color={1}>{0}</color>";
        var bestFit = UIText.bestFit;
        var btn = GetCtrl<GameUIButton>("BtnBestFit");
        var inputMin = GetCtrl<GameUIInputField>("InputMinSize");
        var inputMax = GetCtrl<GameUIInputField>("InputMaxSize");

        Action changeMinMaxFunc = delegate () {
            if (bestFit)
            {
                inputMin.text = UIText.resizeTextMinSize.ToString();
                inputMax.text = UIText.resizeTextMaxSize.ToString();
            }
        };
        Action changeLabFunc = delegate () {
            inputMin.SetActive(bestFit);
            inputMax.SetActive(bestFit);
            btn.SetButtonLab(string.Format(labStr, bestFit, bestFit ? "green" : "red"));

            changeMinMaxFunc();
        };
        changeLabFunc();
        btn.AddClickListener(delegate () {
            bestFit = !bestFit;
            UIText.bestFit = bestFit;
            changeLabFunc();
        });

        inputMin.AddChangedListener(delegate (string str) {
            int num = 0;
            int.TryParse(str, out num);
            UIText.resizeTextMinSize = num;
            changeMinMaxFunc();
        });
        inputMax.AddChangedListener(delegate (string str) {
            int num = 0;
            int.TryParse(str, out num);
            UIText.resizeTextMaxSize = num;
            changeMinMaxFunc();
        });
    }

    
    

    
}
