using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightFramework.UI;
public class SampleScrollbar : UIBaseControl {

    private GameUIScrollbar UIScrollbar;

    protected override void OnInit()
    {
        base.OnInit();
        SetCtrl("UIScrollbar", ref UIScrollbar);

        UIScrollbar.AddValueChangedListener(delegate (float f) {
            SampleTest.prompt.ShowText("Scrollbar.value = " + f);
        });

        InputScrollbarSizeEvent();
        InputScrollbarNumEvent();
        SliderSrollbarValueEvent();
    }

    private void InputScrollbarSizeEvent()
    {
        var input = GetCtrl<GameUIInputField>("InputScrollbarSize");
        input.text = UIScrollbar.size.ToString();
        input.AddChangedListener(delegate (string str) {
            float num = 0;
            float.TryParse(str, out num);
            UIScrollbar.size = num;
        });
    }

    private void InputScrollbarNumEvent()
    {
        var input = GetCtrl<GameUIInputField>("InputScrollbarNum");
        input.text = UIScrollbar.numberOfSteps.ToString();
        input.AddChangedListener(delegate (string str) {
            int num = 0;
            int.TryParse(str, out num);
            UIScrollbar.numberOfSteps = num;
        });
    }

    private void SliderSrollbarValueEvent()
    {
        var labStr = "value = <color=green>{0}</color>";
        var val = UIScrollbar.value;
        var slider = GetCtrl<GameUISlider>("SliderSrollbarValue");
        var txt = GetCtrl<GameUIText>("TxtScrollbarValue");
        slider.value = val;
        txt.FormatStr = labStr;
        txt.SetTextFormatValues(val);
        slider.AddValueChangedListener(delegate (float f)
        {
            val = f;
            UIScrollbar.value = val;
            txt.SetTextFormatValues(val);
        });
    }
}
