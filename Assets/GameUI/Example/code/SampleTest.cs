using LightFramework;
using LightFramework.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SampleTest : UIBaseControl {
    protected GameUIPanel MainPanel;
    protected GameUIListView MenuList;
    protected GameUIObject MenuItem;

    protected GameUIButton BtnBack;
    protected GameUIButton BtnExit;
    protected GameUIButton BtnClearLog;
    
    protected GameUIListView LogList;
    protected GameUIText LogItem;

    MesagePanel MsgPanel;
    /// <summary>
    /// key:名字, value:脚本组件
    /// </summary>
    private Dictionary<string, GameUIPanel> subPanels = new Dictionary<string, GameUIPanel>();

    /// <summary>
    /// key:名字, value:脚本的类名
    /// </summary>
    private List<KeyValuePair<string, string>> menuNameList = new List<KeyValuePair<string, string>>() {
        new KeyValuePair<string, string>("GameUIText", "SampleText"),
        new KeyValuePair<string, string>("GameUIImage", "SampleImage"),
        new KeyValuePair<string, string>("GameUIButton", "SampleButton"),
        new KeyValuePair<string, string>("GameUIToggle", "SampleToggle"),
        new KeyValuePair<string, string>("GameUISlider", "SampleSlider"),
        new KeyValuePair<string, string>("GameUIScrollbar", "SampleScrollbar"),
        new KeyValuePair<string, string>("GameUIDropdown", "SampleDropdown"),
        new KeyValuePair<string, string>("GameUIInputField", "SampleInputField"),
        new KeyValuePair<string, string>("GameUIListView", "SampleListView"),

    };

    #region 重写
    protected override void OnInit()
    {
        base.OnInit();

        SetCtrl("MenuList", ref MenuList);
        SetCtrl("MenuItem", ref MenuItem);
        MenuList.InitReuseItem<SampleMenuItem>(MenuItem);
        MenuList.onUpdateItem = OnUpdateItem;

        SetCtrl("BtnBack", ref BtnBack);
        BtnBack.AddClickListener(OnBtnBackClick);

        SetCtrl("BtnExit", ref BtnExit);
        BtnExit.AddClickListener(delegate () {
            Application.Quit();
        });

        SetCtrl("BtnClearLog", ref BtnClearLog);
        BtnClearLog.AddClickListener(ClearAllLog);

        SetCtrl("MainPanel", ref MainPanel);

        for (int i = 0; i < menuNameList.Count; i++)
        {
            var panelName = menuNameList[i].Key;
            var className = menuNameList[i].Value;
            var panel = GetCtrl<GameUIPanel>(panelName);
            if (panel != null)
            {
                panel.GetOrAddComponent(Type.GetType(className));
                subPanels[panelName] = panel;
            }
        }

        SetCtrl("LogList", ref LogList);
        SetCtrl("LogItem", ref LogItem);
        LogList.InitReuseItem<LogListItem>(LogItem);
        LogList.onUpdateItem = OnUpdateLogItem;

        prompt = this;

        var msg = GetCtrl("MesagePanel");
        MsgPanel = msg.GetOrAddComponent<MesagePanel>();
        MsgPanel.Hide();
    }

    protected override void OnShow()
    {
        base.OnShow();
        InitMainList();
        // 默认显示
        ShowPanel(null);
    }

    #endregion

    #region 列表
    private void InitMainList()
    {
        MenuList.SetReuseDataCount(menuNameList.Count);
        MenuList.RefreshAllReuseItems();
    }

    private void OnUpdateItem(GameUIListItem listItem)
    {
        var menuItem = listItem as SampleMenuItem;
        var index = menuItem.Index;
        if (index < menuNameList.Count)
        {
            menuItem.SetButtonLab(menuNameList[index].Key);
        }
        menuItem.ShowPanelFunc = ShowPanel;
    }

    #endregion

    #region 显示面板

    /// <summary>
    /// 显示面板
    /// name为null, 显示主界面
    /// </summary>
    /// <param name="name"></param>
    private void ShowPanel(string name)
    {
        ClearAllLog();
        if (!string.IsNullOrEmpty(name))
        {
            MainPanel.SetActive(false);
            BtnBack.SetActive(true);
            BtnExit.SetActive(false);
        }
        else
        {
            MainPanel.SetActive(true);
            BtnBack.SetActive(false);
            BtnExit.SetActive(true);
        }
        foreach (var item in subPanels)
        {
            bool showSub = item.Key == name;
            item.Value.SetActive(showSub);
        }
    }


    private void OnBtnBackClick()
    {
        ShowPanel(null);
    }

    #endregion


    public static SampleTest prompt;

    #region 日志
    private List<string> logDataList = new List<string>();

    private void OnUpdateLogItem(GameUIListItem listItem)
    {
        var item = listItem as LogListItem;
        var index = item.Index;
        Debug.Log("刷新日志:" + index + ", logDataList:" + logDataList.Count + ", value:" + logDataList[index]);
        if (index < logDataList.Count)
        {
            item.SetData(logDataList[index]);
        }
    }

    private void ClearAllLog()
    {
        LogList.RemoveAll();
        logDataList.Clear();
    }
    public void ShowText(string txt)
    {
        var str = "[" + DateTime.Now.ToString("hh:mm:ss") + "] " + txt;

        logDataList.Add(str);
        LogList.AddReuseItem();
        LogList.MoveItemInView(logDataList.Count - 1, GameUIListView.ListItemInViewAlign.RightOrBottom);
    }
    #endregion

    #region 对话框

    /// <summary>
    /// 显示对话框
    /// </summary>
    /// <param name="content"></param>
    /// <param name="result"></param>
    public void ShowMessageBox(string content, UnityAction<bool> result)
    {
        MsgPanel.Show(content, result);
    }

    #endregion

}

public class SampleMenuItem : GameUIListItem
{
    protected GameUIButton ItemBtn;
    public delegate void ShowPanel(string name);
    public ShowPanel ShowPanelFunc;

    private string labName;
    protected override void OnInit()
    {
        base.OnInit();
        SetCtrl("ItemBtn", ref ItemBtn);
        ItemBtn.AddClickListener(onItemBtnClick);
    }

    public void SetButtonLab(string name)
    {
        labName = name;
        ItemBtn.SetButtonLab(name);
    }

    private void onItemBtnClick()
    {
        if(ShowPanelFunc != null)
        {
            ShowPanelFunc(labName);
        }
    }
}

public class LogListItem:GameUIListItem
{
    public GameUIText LogItem;

    protected override void OnInit()
    {
        base.OnInit();

        SetCtrl("LogItem", ref LogItem);
    }

    public void SetData(string obj)
    {
        LogItem.text = obj;
    }
}


