using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightFramework.UI;
using UnityEngine.UI;
using LightFramework;

public class SampleToggle : UIBaseControl {

    public List<GameUIToggle> toggleList = new List<GameUIToggle>();
    private ToggleGroup toggleGroup;
    protected override void OnInit()
    {
        base.OnInit();
        toggleGroup = GetComponent<ToggleGroup>();
        for (int i = 0; i < 10; i++)
        {
            var index = i;
            var toggle = GetCtrl<GameUIToggle>("UIToggle" + (i + 1));
            if(toggle != null)
            {
                toggle.group = toggleGroup;
                toggleList.Add(toggle);
                toggle.AddValueChangedListener(delegate (bool isOn) {
                    onCheck(index, toggle);
                });
            }
        }
        toggleGroup.SetAllTogglesOff();

        BtnSwitchOffClickEvent();
    }
    private void onCheck(int index, GameUIToggle toggle)
    {
        var str = "->Toggle" + (index + 1) + (toggle.isOn ? "选中:" : "未选中");
        Debug.Log(str + ", index:" + index);
        SampleTest.prompt.ShowText(str);

        if (index == 0)
        {
            toggle.Label.text = toggle.isOn ? "Toggle1(被选择)" : "Toggle1";
        }
        else if (index == 2)
        {
            var normalRect = toggle.NormalStateNode.GetRectTrans();
            var perPos = normalRect.anchoredPosition;
            normalRect.anchoredPosition = new Vector2(toggle.isOn ? 100 : 0, perPos.y);
            
        }
    }


    private void BtnSwitchOffClickEvent()
    {
        var labStr = "ToggleGroup.allowSwitchOff = <color={1}>{0}</color>";
        var val = toggleGroup.allowSwitchOff;
        var btn = GetCtrl<GameUIButton>("BtnSwitchOff");
        btn.SetButtonLab(string.Format(labStr, val, val ? "green" : "red"));
        btn.AddClickListener(delegate () {
            val = !val;
            toggleGroup.allowSwitchOff = val;
            btn.SetButtonLab(string.Format(labStr, val, val ? "green" : "red"));
        });
    }
}
