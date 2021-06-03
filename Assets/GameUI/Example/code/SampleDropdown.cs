using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightFramework.UI;
using UnityEngine.UI;
using UnityEngine.Events;

public class SampleDropdown : UIBaseControl {
    private GameUIDropdown UIDropdown;

    public List<Sprite> spriteList = new List<Sprite>();

    protected override void OnInit()
    {
        base.OnInit();
        SetCtrl("UIDropdown", ref UIDropdown);

        UnityAction<int> func = delegate (int index) {
            SampleTest.prompt.ShowText("UIDropdown.value = " + UIDropdown.value);
        };
        UIDropdown.AddValueChangedListener(func);
        // 下面三个 测试是否会重复添加同一个监听
        UIDropdown.AddValueChangedListener(func);
        UIDropdown.AddValueChangedListener(func);
        UIDropdown.AddValueChangedListener(func);

        DropValueClick();
        DropValueWithoutNotifyClick();
        DropAddOptionClick();
        DropRemoveOptionClick();
        DropRemoveAllOptionClick();
        DropRemoveOneOptionClick();
    }

    private List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();

    private void DropAddOptionClick()
    {
        var btn = GetCtrl<GameUIButton>("BtnDropAddOption");

        btn.AddClickListener(delegate () {
            Sprite sp = spriteList[list.Count % spriteList.Count];
            
            var option = new Dropdown.OptionData("option" + list.Count, sp);
            list.Add(option);
            UIDropdown.AddOption(option);
        });
    }

    private void DropRemoveOptionClick()
    {
        var btn = GetCtrl<GameUIButton>("BtnDropRemoveOption");

        btn.AddClickListener(delegate () {
            if (list.Count <= 0)
            {
                return;
            }
            list.RemoveAt(0);
            UIDropdown.RemoveOption(0);
            SampleTest.prompt.ShowText("UIDropdown.RemoveOption(0)");
        });
    }


    private void DropRemoveOneOptionClick()
    {
        var btn = GetCtrl<GameUIButton>("BtnDropRemoveOneOption");

        btn.AddClickListener(delegate () {
            if (list.Count <= 0)
            {
                return;
            }
            var option = list[0];
            list.RemoveAt(0);
            UIDropdown.RemoveOption(option);
            SampleTest.prompt.ShowText("UIDropdown.RemoveOption(option)");
        });
    }

    private void DropRemoveAllOptionClick()
    {
        var btn = GetCtrl<GameUIButton>("BtnDropRemoveAllOption");

        btn.AddClickListener(delegate () {
            list.Clear();
            UIDropdown.ClearOptions();
        });
    }


    private void DropValueClick()
    {
        var str = "随机选择索引:<color=green>{0}</color>";
        var btn = GetCtrl<GameUIButton>("BtnDropValue");
        var index = Random.Range(0, list.Count);
        btn.SetButtonLab(string.Format(str, index));
        btn.AddClickListener(delegate () {
            if (list.Count <= 0)
            {
                index = Random.Range(0, list.Count);
                return;
            }
            index = Random.Range(0, list.Count);
            UIDropdown.value = index;
            btn.SetButtonLab(string.Format(str, index));
        });
    }

    private void DropValueWithoutNotifyClick()
    {
        var str = "随机选择索引(不触发消息):<color=green>{0}</color>";
        var btn = GetCtrl<GameUIButton>("BtnDropValueWithoutNotify");
        var index = Random.Range(0, list.Count);
        btn.SetButtonLab(string.Format(str, index));
        btn.AddClickListener(delegate () {
            if (list.Count <= 0)
            {
                index = Random.Range(0, list.Count);
                return;
            }
            UIDropdown.SetValueWithoutNotify(index);
            index = Random.Range(0, list.Count);
            btn.SetButtonLab(string.Format(str, index));
        });
    }

}
