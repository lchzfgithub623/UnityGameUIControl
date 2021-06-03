using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightFramework.UI;
using System;
using LightFramework;
using UnityEngine.UI;

public class SampleListView : UIBaseControl {
    private GameUIListView UIListViewUnReuse;
    private GameUIObject ListItemUnReuse1;
    private GameUIObject ListItemUnReuse2;

    private GameUIListView UIListViewReuse;
    private GameUIObject ListItemReuse;

    /// <summary>
    /// 0 非复用, 1:复用
    /// </summary>
    private int useType = 0;  //
    private GameUIListView curListView;

    private int addIndex = 0;
    private int itemInViewType = 0;
    private int moveIndex = 0;

    private GameUIImage imgPush;

    protected override void OnInit()
    {
        base.OnInit();
        SetCtrl("imgPush", ref imgPush);
        imgPush.SetActive(false);
        SetCtrl("UIListViewUnReuse", ref UIListViewUnReuse);
        
        SetCtrl("ListItemUnReuse1", ref ListItemUnReuse1);
        ListItemUnReuse1.SetActive(false);

        SetCtrl("ListItemUnReuse2", ref ListItemUnReuse2);
        ListItemUnReuse2.SetActive(false);

        SetCtrl("UIListViewReuse", ref UIListViewReuse);
        SetCtrl("ListItemReuse", ref ListItemReuse);
        UIListViewReuse.InitReuseItem<ReuseItem>(ListItemReuse);
        UIListViewReuse.onUpdateItem = delegate (GameUIListItem listItem) {
            var list = listItem as ReuseItem;
            list.SetInfo(list.Index);
        };
        var originSize = imgPush.GetRectTrans().sizeDelta;
        UIListViewReuse.onValueChanged = delegate (Vector2 percent)
        {
            // 往上拉添加一组新的列表项
            var contentSize = curListView.contentSize;
            var viewSize = curListView.viewSize;
            if (contentSize.y < viewSize.y)
            {
                imgPush.SetActive(false);
                return;
            }
            var contentPos = curListView.contentPos;

            var offPosY = viewSize.y - (contentSize.y - contentPos.y);
            if (offPosY < 30)
            {
                if(imgPush.IsActive() && !UIListViewReuse.Dragging)
                {
                    curListView.StopMovement();
                    for (int i = 0; i < 5; i++)
                    {
                        curListView.AddReuseItem();
                    }
                }
                imgPush.SetActive(false);
                return;
            }
            imgPush.SetActive(true);
            var scale = 1 + (offPosY - 20) / 35;
            imgPush.GetRectTrans().sizeDelta = new Vector2(originSize.x, originSize.y * scale);
        };

        

        ListViewTypeTabEvent();
        AddItem1Event();

        ToggleSetListOptionalEvent();
        InputListMaxSelectNumEvent();
        ToggleAllowSwitchOffEvent();

        UIDropdownListWeizhiEvent();
        InsertOneItemEvent();
        RemoveOneItemEvent();
        RemoveAllItemEvent();

        InputListMoveItemEvent();
        UIDropdownInViewTypeEvent();

        ToggleUseMaskEvent();
    }

    private void ListViewTypeTabEvent()
    {
        var toggle = GetCtrl<GameUIToggle>("ListViewTypeTab");
        var btn2 = GetCtrl<GameUIButton>("btnAddItemUn2");
        var btnInsert = GetCtrl<GameUIButton>("btnInsertOneItem");
        var btnRemove = GetCtrl<GameUIButton>("btnRemoveOneItem");
        var btnAddItemReuseRandom = GetCtrl<GameUIButton>("btnAddItemReuseRandom");
        var dropwz = GetCtrl<GameUIDropdown>("UIDropdownListWeizhi");
        toggle.isOn = (useType == 1);

        Action<bool> changeState = delegate (bool isRese)
        {
            UIListViewUnReuse.SetActive(!isRese);
            UIListViewReuse.SetActive(isRese);
            btn2.SetActive(!isRese);
            btnInsert.SetActive(!isRese);
            btnRemove.SetActive(!isRese);
            btnAddItemReuseRandom.SetActive(isRese);
            dropwz.SetActive(!isRese);
            curListView = isRese ? UIListViewReuse : UIListViewUnReuse;
            CheckIsOption();

            var input = GetCtrl<GameUIInputField>("InputListMaxSelectNum");
            var val = curListView.IsOptional ? curListView.SelectHander.MaxSelectNum : 0;
            input.text = val.ToString();

            var toggleS = GetCtrl<GameUIToggle>("ToggleAllowSwitchOff");
            var toggleVal = curListView.IsOptional ? curListView.SelectHander.AllowOffLast : false ;
            toggleS.isOn = toggleVal;
        };
        changeState(useType == 1);

        toggle.AddValueChangedListener(delegate (bool b)
        {
            useType = b ? 1 : 0;
            changeState(b);
        });
    }
    private void AddItem1Event()
    {
        var btn = GetCtrl<GameUIButton>("btnAddItemUn1");
        btn.AddClickListener(delegate () {
            if(useType == 0)
            {
                var item = curListView.AddUnReuseItem<UnReuseItemA>(ListItemUnReuse1);
                item.SetInfo(curListView.DataCount);
            }
            else
            {
                curListView.AddReuseItem();
            }
            
        });

        var btn2 = GetCtrl<GameUIButton>("btnAddItemUn2");
        btn2.AddClickListener(delegate () {
            if (useType == 0)
            {
                var item = curListView.AddUnReuseItem<UnReuseItemB>(ListItemUnReuse2);
                item.SetInfo(curListView.DataCount);
            }
        });

        // 添加一组随机数量的复用列表
        var btnAddItemReuseRandom = GetCtrl<GameUIButton>("btnAddItemReuseRandom");
        btnAddItemReuseRandom.AddClickListener(delegate () {
            if (useType == 1)
            {
                var count = UnityEngine.Random.Range(5, 20);
                var showStr = "添加一组随机数量:" + count + " 的列表项";
                Debug.Log(showStr);
                SampleTest.prompt.ShowText(showStr);
                curListView.SetReuseDataCount(count);
                curListView.RefreshAllReuseItems();
            }
        });
    }

    #region 可选的
    private void ToggleSetListOptionalEvent()
    {
        var toggle = GetCtrl<GameUIToggle>("ToggleCanSelect");
        toggle.isOn = curListView.IsOptional;
        toggle.AddValueChangedListener(delegate (bool isOn)
        {
            curListView.IsOptional = isOn;
            CheckSelectValue();
        });
    }

    private void CheckIsOption()
    {
        var toggle = GetCtrl<GameUIToggle>("ToggleCanSelect");
        toggle.isOn = curListView.IsOptional;
    }

    private void CheckSelectValue()
    {
        var input = GetCtrl<GameUIInputField>("InputListMaxSelectNum");
        var val = curListView.IsOptional ? curListView.SelectHander.MaxSelectNum : 0; ;
        input.text = val.ToString();

        var toggle = GetCtrl<GameUIToggle>("ToggleAllowSwitchOff");
        var b = curListView.IsOptional ? curListView.SelectHander.AllowOffLast : false;
        toggle.isOn = b;
    }

    private void InputListMaxSelectNumEvent()
    {
        var input = GetCtrl<GameUIInputField>("InputListMaxSelectNum");
        var val = curListView.IsOptional ? curListView.SelectHander.MaxSelectNum : 0; ;
        input.text = val.ToString();

        input.AddChangedListener(delegate (string s)
        {
            int num = 0;
            int.TryParse(s, out num);

            if (curListView.IsOptional)
            {
                curListView.SelectHander.MaxSelectNum = num;
            }
            CheckSelectValue();
        });
    }

    private void ToggleAllowSwitchOffEvent()
    {
        var toggle = GetCtrl<GameUIToggle>("ToggleAllowSwitchOff");
        var val = curListView.IsOptional ? curListView.SelectHander.AllowOffLast : false;
        toggle.isOn = val;
        toggle.AddValueChangedListener(delegate (bool isOn)
        {
            if (curListView.IsOptional)
            {
                curListView.SelectHander.AllowOffLast = isOn;
            }
            CheckSelectValue();
        });
    }
    #endregion


    private void UIDropdownListWeizhiEvent()
    {
        var drop = GetCtrl<GameUIDropdown>("UIDropdownListWeizhi");
        addIndex = drop.value;
        drop.AddValueChangedListener(delegate (int i) {

            addIndex = i;
        });
    }


    private void InsertOneItemEvent()
    {
        var btn = GetCtrl<GameUIButton>("btnInsertOneItem");

        btn.AddClickListener(delegate () {
            var insertIndex = 0;
            if (addIndex == 1)
            {
                insertIndex = 2;
            }
            else if (addIndex == 2)
            {
                insertIndex = curListView.DataCount;
            }
            if (useType == 1)
            {
                return;
            }

            var showStr = "在 " + insertIndex + " 位置插入一条列表项";
            Debug.Log(showStr);
            SampleTest.prompt.ShowText(showStr);
            var item = curListView.AddUnReuseItem<UnReuseItemA>(ListItemUnReuse1, insertIndex);
            item.SetInfo(curListView.DataCount, "特殊插入:" + insertIndex);
        });
    }

    private void RemoveOneItemEvent()
    {
        var btn = GetCtrl<GameUIButton>("btnRemoveOneItem");

        btn.AddClickListener(delegate () {
            var insertIndex = 0;
            if (addIndex == 1)
            {
                insertIndex = 1;
            }
            else if (addIndex == 2)
            {
                insertIndex = curListView.DataCount - 1;
            }
            if (useType == 1)
            {
                curListView.RemoveUnReuseItem(insertIndex);
                return;
            }
            
            var showStr = "在 " + insertIndex + " 位置删除该条列表项";
            Debug.Log(showStr);
            SampleTest.prompt.ShowText(showStr);
            curListView.RemoveUnReuseItem(insertIndex);
        });
    }

    private void RemoveAllItemEvent()
    {
        var btn = GetCtrl<GameUIButton>("btnRemoveAllItem");

        btn.AddClickListener(delegate () {
            curListView.RemoveAll();
        });
    }

    private void InputListMoveItemEvent()
    {
        var input = GetCtrl<GameUIInputField>("InputListMoveItem");
        input.text = moveIndex.ToString();
        input.AddEndEditListener(delegate (string s) {
            var n = 0;
            int.TryParse(s, out n);
            if(n > 0 && n >= curListView.DataCount)
            {
                moveIndex = curListView.DataCount - 1;
                input.text = moveIndex.ToString();
            }
            else
            {
                moveIndex = n;
            }

            moveIndexInView();
        });
    }

    private void moveIndexInView()
    {
        curListView.MoveItemInView(moveIndex, (GameUIListView.ListItemInViewAlign)itemInViewType);
    }
    private void UIDropdownInViewTypeEvent()
    {
        var drop = GetCtrl<GameUIDropdown>("UIDropdownInViewType");
        itemInViewType = drop.value;
        drop.AddValueChangedListener(delegate (int i) {
            itemInViewType = i;
            moveIndexInView();
        });

    }

    private void ToggleUseMaskEvent()
    {
        var toggle = GetCtrl<GameUIToggle>("ToggleUseMask");
        toggle.isOn = curListView.viewport.GetComponent<Mask>().enabled;
        toggle.Label.text = toggle.isOn ? "关闭Mask" : "开启Mask";

        toggle.AddValueChangedListener((b) => {
            curListView.viewport.GetComponent<Mask>().enabled = b;
            toggle.Label.text = toggle.isOn ? "关闭Mask" : "开启Mask";
        });
    }
}

public class ListItem : GameUIListItem
{
    protected GameUIText title;
    protected GameUIImage select;
    protected GameUIButton btn;

    protected int num;


    protected override void OnInit()
    {
        SetCtrl("title", ref title);
        SetCtrl("select", ref select);
        SetCtrl("btn", ref btn);
        btn.AddClickListener(onBtnClick);
        IsSelected = false;
    }

    public virtual void SetInfo(int num, string other = null)
    {
        this.num = num;
    }

    protected virtual void onBtnClick()
    {
        var ret = DoSelect(!IsSelected);
        if (ret == SelectableResultType.CannotUnselectLast)
        {
            var showStr = "无法取消最后选中项";
            Debug.Log(showStr);
            SampleTest.prompt.ShowText(showStr);
        }
        else if (ret == SelectableResultType.MaxSelectNumZero)
        {
            var showStr = "最大可选数量为0";
            Debug.Log(showStr);
            SampleTest.prompt.ShowText(showStr);
        }
        else if (ret == SelectableResultType.OverMaxSelectNum)
        {
            var showStr = "超过最大可选数量";
            Debug.Log(showStr);
            SampleTest.prompt.ShowText(showStr);
        }
    }

    protected override void OnSelectStateChanged(bool bSelected)
    {
        select.SetActive(bSelected);
        btn.SetButtonLab(bSelected ? "取消" : "选择");
    }
}
public class UnReuseItemA : ListItem
{

    private string other;

    public override int SelectableSign
    {
        get
        {
            return GetHashCode();
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
    }

    public override void SetInfo(int num, string other = null)
    {
        base.SetInfo(num);
        this.other = other;
        if (string.IsNullOrEmpty(other))
        {
            title.text = "AAAA:" + num + ",index:" + Index;
        }
        else
        {
            title.text = "AAAA:" + num + ",index:" + Index + ", " + this.other;
        }
        
    }

    protected override void OnIndexChanged(int index, int preIndex)
    {
        base.OnIndexChanged(index, preIndex);
        if (string.IsNullOrEmpty(other))
        {
            title.text = "AAAA:" + num + ",index:" + Index + "(之前:" + preIndex + ")";
        }
        else
        {
            title.text = "AAAA:" + num + ",index:" + Index + "(之前:" + preIndex + ")" + ", " + this.other;
        }
    }

    protected override bool OnBeforeToggleSelectState()
    {
        if (Index > 0 && Index % 3 == 0)
        {
            var showStr = "被选择前的检测:" + num + "能被3整除, 不能被选择";
            Debug.Log(showStr);
            SampleTest.prompt.ShowText(showStr);
            return false;
        }
        return true;
    }
}

public class UnReuseItemB : ListItem
{
    private string other;

    public override int SelectableSign
    {
        get
        {
            return GetHashCode();
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
    }

    public override void SetInfo(int num, string other = null)
    {
        base.SetInfo(num);
        this.other = other;
        if (string.IsNullOrEmpty(other))
        {
            title.text = "BBBB:" + num + ",index:" + Index;
        }
        else
        {
            title.text = "BBBB:" + num + ",index:" + Index + ", " + other;
        }
    }

    protected override void OnIndexChanged(int index, int preIndex)
    {
        base.OnIndexChanged(index, preIndex);
        if (string.IsNullOrEmpty(other))
        {
            title.text = "BB:" + num + ",index:" + Index + "(之前:" + preIndex + ")";
        }
        else
        {
            title.text = "BB:" + num + ",index:" + Index + "(之前:" + preIndex + ")" + ", " + this.other;
        }
    }
}

public class ReuseItem: ListItem
{
    protected override void OnInit()
    {
        base.OnInit();
    }

    public override void SetInfo(int num, string other = null)
    {
        base.SetInfo(num);
        if (string.IsNullOrEmpty(other))
        {
            title.text = "CCCC:" + num + ",index:" + Index;
        }
        else
        {
            title.text = "CCCC:" + num + ",index:" + Index + ", " + other;
        }
    }
}
