using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightFramework.UI;
using UnityEngine.UI;
using System;

public class SampleSlider : UIBaseControl {

    private GameUISlider UISlider;

    protected override void OnInit()
    {
        base.OnInit();
        SetCtrl("UISlider", ref UISlider);

        UISlider.AddValueChangedListener(delegate (float f) {
            SampleTest.prompt.ShowText("slider.value = " + f);
        });

        DirectionEvent();
        SetDirectionEvent();
        InputSliderMinMaxEvent();
        WholeNumbersEvent();
    }

    private void DirectionEvent()
    {
        var labStr = ".direction = <color=green>Slider.Direction.{0}</color>";
        var direction = UISlider.direction;
        var btn = GetCtrl<GameUIButton>("BtnDirection");
        var directionList = new List<Slider.Direction>() {
           Slider.Direction.LeftToRight, Slider.Direction.RightToLeft, Slider.Direction.BottomToTop, Slider.Direction.TopToBottom
        };
        btn.SetButtonLab(string.Format(labStr, direction));
        btn.AddClickListener(delegate () {
            direction = UISlider.direction;
            var fIndex = directionList.FindIndex(t => t == direction);
            var nextIndex = fIndex + 1;
            if (nextIndex >= directionList.Count)
            {
                nextIndex = 0;
            }
            direction = directionList[nextIndex];
            UISlider.direction = direction;
            btn.SetButtonLab(string.Format(labStr, direction));
        });
    }

    private void SetDirectionEvent()
    {
        var labStr = "BtnSetDirection(<color=green>Slider.Direction.{0}</color>, true)";
        var direction = UISlider.direction;
        var btn = GetCtrl<GameUIButton>("BtnSetDirection");
        var directionList = new List<Slider.Direction>() {
           Slider.Direction.LeftToRight, Slider.Direction.RightToLeft, Slider.Direction.BottomToTop, Slider.Direction.TopToBottom
        };
        btn.SetButtonLab(string.Format(labStr, direction));
        btn.AddClickListener(delegate () {
            direction = UISlider.direction;
            var fIndex = directionList.FindIndex(t => t == direction);
            var nextIndex = fIndex + 1;
            if (nextIndex >= directionList.Count)
            {
                nextIndex = 0;
            }
            direction = directionList[nextIndex];
            UISlider.SetDirection(direction, true);
            btn.SetButtonLab(string.Format(labStr, direction));
        });
    }

    private void InputSliderMinMaxEvent()
    {
        var inputMin = GetCtrl<GameUIInputField>("InputSliderMinValue");
        var inputMax = GetCtrl<GameUIInputField>("InputSliderMaxValue");
        inputMin.text = UISlider.minValue.ToString();
        inputMax.text = UISlider.maxValue.ToString();

        inputMin.AddChangedListener(delegate (string str) {
            float num = 0;
            float.TryParse(str, out num);
            UISlider.minValue = num;
        });
        inputMax.AddChangedListener(delegate (string str) {
            float num = 0;
            float.TryParse(str, out num);
            UISlider.maxValue = num;
        });
    }

    private void WholeNumbersEvent()
    {
        var str = ".wholeNumbers = <color=green>{0}</color>";
        var b = UISlider.wholeNumbers;
        var btn = GetCtrl<GameUIButton>("BtnWholeNumbers");
        var promptLab = b ? "设置数值变化为浮点数变化" : "设置数值变化为整数变化";
        var lab = string.Format(str, b);
        btn.SetButtonLab(lab);
        btn.AddClickListener(delegate () {
            SampleTest.prompt.ShowText("--------------------------------- " + promptLab);

            b = !b;
            UISlider.wholeNumbers = b;
            promptLab = b ? "设置数值变化为浮点数变化" : "设置数值变化为整数变化";
            lab = string.Format(str, b);
            btn.SetButtonLab(lab);
        });
    }
}
