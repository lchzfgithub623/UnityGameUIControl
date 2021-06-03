using UnityEngine;
using UnityEngine.Events;
using LightFramework.UI;
using LightFramework;
using System;

public class MesagePanel : UIBaseControl
{
    GameUIText TxtMsgContent;
    GameUIButton BtnMsgTrue;
    GameUIButton BtnMsgFalse;

    protected override void OnInit()
    {
        base.OnInit();

        SetCtrl("TxtMsgContent", ref TxtMsgContent);
        SetCtrl("BtnMsgTrue", ref BtnMsgTrue);
        BtnMsgTrue.AddClickListener(OnBtnMsgTrueClick);
        SetCtrl("BtnMsgFalse", ref BtnMsgFalse);
        BtnMsgFalse.AddClickListener(OnBtnMsgFalseClick);
    }

    #region 对外方法
    UnityAction<bool> clickAction;
    public void Show(string content, UnityAction<bool> clickAction)
    {
        this.clickAction = clickAction;
        this.SetActive(true);

        TxtMsgContent.text = content;

    }

    public void Hide()
    {
        this.SetActive(false);
    }


    #endregion


    private void OnBtnMsgTrueClick()
    {
        if(clickAction != null)
        {
            clickAction.Invoke(true);
        }
        Hide();
    }

    private void OnBtnMsgFalseClick()
    {
        if (clickAction != null)
        {
            clickAction.Invoke(false);
        }
        Hide();
    }


}
