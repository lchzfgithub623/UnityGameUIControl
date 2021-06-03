using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightFramework.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LightFramework;

public class SampleButton : UIBaseControl {

    private GameUIButton UIButton;

    protected override void OnInit()
    {
        base.OnInit();
        SetCtrl("UIButton", ref UIButton);

        NormalClickEvent();
        NoClickEvent();
        CustomClickEvent();
        DoubleClickEvent();
        LongClickEvent();
        InOutEvent();
        CatchEvent();
    }

    private void onButtonNormalClick()
    {
        SampleTest.prompt.ShowText("normal click====");
    }


    private void NormalClickEvent()
    {
        var count = UIButton.GetClickListenerCount();
        var btn = GetCtrl<GameUIButton>("BtnNormalClick");
        var lab = count <= 0 ? "添加自带的点击事件" : "移除自带的点击事件";
        btn.SetButtonLab(lab);
        btn.AddClickListener(delegate () {
            SampleTest.prompt.ShowText("--------------------------------- " + lab);
            if (count > 0)
            {
                UIButton.RemoveClickListener(onButtonNormalClick);
            }
            else
            {
                UIButton.AddClickListener(onButtonNormalClick);
            }
            count = UIButton.GetClickListenerCount();
            lab = count <= 0 ? "添加自带的点击事件" : "移除自带的点击事件";
            btn.SetButtonLab(lab);
        });
    }

    private void NoClickEvent()
    {
        var b = UIButton.CanTouch;
        var btn = GetCtrl<GameUIButton>("BtnNoClick");
        var lab = b ? "设置按钮不可点击" : "设置按钮可点击";
        btn.SetButtonLab(lab);
        btn.AddClickListener(delegate () {
            SampleTest.prompt.ShowText("--------------------------------- " + lab);

            b = !b;
            UIButton.CanTouch = b;
            lab = b ? "设置按钮不可点击" : "设置按钮可点击";
            btn.SetButtonLab(lab);
        });
    }

    private void CustomClickEvent()
    {
        var count = UIButton.TouchMgr.ClickActionCount;
        var btn = GetCtrl<GameUIButton>("BtnCustomClick");
        var lab = count <= 0 ? "添加自定义的点击事件" : "移除自定义的点击事件";
        btn.SetButtonLab(lab);
        btn.AddClickListener(delegate ()
        {
            SampleTest.prompt.ShowText("--------------------------------- " + lab);
            if (count > 0)
            {
                UIButton.TouchMgr.RemoveClickListener(onCustomClick);
            }
            else
            {
                UIButton.TouchMgr.AddClickListener(onCustomClick);
            }
            count = UIButton.TouchMgr.ClickActionCount;
            lab = count <= 0 ? "添加自定义的点击事件" : "移除自定义的点击事件";
            btn.SetButtonLab(lab);
        });
    }
    private void onCustomClick(PointerEventData e)
    {
        SampleTest.prompt.ShowText("触发 自定义 的按钮点击");
    }

    private void DoubleClickEvent()
    {
        var count = UIButton.TouchMgr.DoubleClickActionCount;
        var btn = GetCtrl<GameUIButton>("BtnDoubleClick");
        var lab = count <= 0 ? "添加自定义的双击事件" : "移除自定义的双击事件";
        btn.SetButtonLab(lab);
        btn.AddClickListener(delegate ()
        {
            SampleTest.prompt.ShowText("--------------------------------- " + lab);
            if (count > 0)
            {
                UIButton.TouchMgr.RemoveDoubleClickListener(onDoubleCustomClick);
            }
            else
            {
                UIButton.TouchMgr.AddDoubleClickListener(onDoubleCustomClick);
            }
            count = UIButton.TouchMgr.DoubleClickActionCount;
            lab = count <= 0 ? "添加自定义的双击事件" : "移除自定义的双击事件";
            btn.SetButtonLab(lab);
        });
    }
    private void onDoubleCustomClick(PointerEventData e)
    {
        SampleTest.prompt.ShowText("触发 自定义 的按钮双击");
    }

    private void LongClickEvent()
    {
        var count = UIButton.TouchMgr.LongPressActionCount;
        var btn = GetCtrl<GameUIButton>("BtnLongClick");
        var lab = count <= 0 ? "添加自定义的长按事件" : "移除自定义的长按事件";
        btn.SetButtonLab(lab);
        btn.AddClickListener(delegate ()
        {
            SampleTest.prompt.ShowText("--------------------------------- " + lab);
            if (count > 0)
            {
                UIButton.TouchMgr.RemoveLongPressListener(onLongCustomClick);
                UIButton.TouchMgr.RemoveUpOrExitListener(onUpCustomClick);
            }
            else
            {
                UIButton.TouchMgr.AddLongPressListener(onLongCustomClick);
                UIButton.TouchMgr.AddUpOrExitListener(onUpCustomClick);
            }
            count = UIButton.TouchMgr.LongPressActionCount;
            lab = count <= 0 ? "添加自定义的长按事件" : "移除自定义的长按事件";
            btn.SetButtonLab(lab);
        });
    }
    private bool beginPress = false;
    private void onLongCustomClick(PointerEventData e)
    {
        beginPress = true;
        SampleTest.prompt.ShowText("触发 自定义 的按钮长按");
    }
    private void onUpCustomClick(bool bUp)
    {
        if(beginPress)
        {
            if (bUp)
            {
                SampleTest.prompt.ShowText("触发 自定义 的按钮长按后抬起" + bUp);
            }
            else
            {
                SampleTest.prompt.ShowText("触发 自定义 的按钮长按后 离开取消" + bUp);
            }
            beginPress = false;
        }
    }

    private void InOutEvent()
    {
        var count = UIButton.TouchMgr.InActionCount;
        var btn = GetCtrl<GameUIButton>("BtnInOut");
        var lab = count <= 0 ? "添加自定义的进入离开事件" : "移除自定义的进入离开事件";
        btn.SetButtonLab(lab);
        btn.AddClickListener(delegate ()
        {
            SampleTest.prompt.ShowText("--------------------------------- " + lab);
            if (count > 0)
            {
                UIButton.TouchMgr.RemoveInListener(onInCustomClick);
                UIButton.TouchMgr.RemoveUpOrExitListener(onExitCustomClick);
            }
            else
            {
                UIButton.TouchMgr.AddInListener(onInCustomClick);
                UIButton.TouchMgr.AddUpOrExitListener(onExitCustomClick);
            }
            count = UIButton.TouchMgr.InActionCount;
            lab = count <= 0 ? "添加自定义的进入离开事件" : "移除自定义的进入离开事件";
            btn.SetButtonLab(lab);
        });
    }
    private void onInCustomClick(PointerEventData e)
    {
        SampleTest.prompt.ShowText("触发 自定义 的按钮进入");
        UIButton.GetRectTrans().localScale = new Vector3(1.3f, 1.3f, 1.3f);
    }
    private void onExitCustomClick(bool bUp)
    {
        if(bUp)
        {
            return;
        }
        UIButton.GetRectTrans().localScale = new Vector3(1, 1, 1);
        SampleTest.prompt.ShowText("触发 自定义 的按钮离开" + bUp);
    }

    private void CatchEvent()
    {
        var btn = GetCtrl<GameUIButton>("BtnCatchClick");
        var lab = "添加截获点击事件";
        btn.SetButtonLab(lab);
        btn.AddClickListener(delegate () {
            SampleTest.prompt.ShowText("--------------------------------- " + lab + ",添加一次按钮点击事件的拦截逻辑");

            Button.ButtonClickedEvent preEvent = UIButton.onClick;
            Button.ButtonClickedEvent catchEvent = new Button.ButtonClickedEvent();
            catchEvent.AddListener(delegate () {
                SampleTest.prompt.ShowMessageBox("拦截按钮的正常点击事件成功, 处理自己的事情, 完毕后是否开触发按钮自带的事件?", (b) => {
                    if (b)
                    {
                        preEvent.Invoke();
                    }
                    UIButton.onClick = preEvent;
                });


            });
            UIButton.onClick = catchEvent;
        });
    }
}
