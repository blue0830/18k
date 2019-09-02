using UnityEngine;
using System.Collections;

public class ShaiXuanHuiYuanZiLiao : UserSubPanelBase {

    public UIPopupList sortType;

    public UIPopupList sortMethod; //升or 降
    //开始日期
	public UIInput StartDateInput;
	//开始日期
	public UIInput EndDateInput;
	//流水号
  

    public UIInput MemberIdInput;

    public UIInput MemberAccInput;

    public UIInput MemberQQInput;

    public UIToggle YouEMember;
    public UIToggle onlineMember;


    //按钮
    public GameObject SearchBtn;
	//返回按钮
	public GameObject ReturnBtn;

	 

	void Start () {
		UIEventListener.Get(ReturnBtn).onClick = OnReturn;
		UIEventListener.Get(SearchBtn).onClick = OnSearch;

        sortMethod.Clear();
        sortMethod.AddItem("升序");
        sortMethod.AddItem("降序");
		sortMethod.value = "降序";

        sortType.Clear();
        sortType.AddItem("注册时间");
        sortType.AddItem("返点%");
        sortType.AddItem("余额");
        sortType.AddItem("最后登录时间");
        sortType.value = "注册时间";

    }

	// Update is called once per frame
	void Update () {


	}

	public void Show()
	{
		StartDateInput.value =  TimeHelper.GetTimeStrFromUlong(HuiYuanZiLiaoPanel.startDate);
		EndDateInput.value = TimeHelper.GetTimeStrFromUlong(HuiYuanZiLiaoPanel.endDate);
		gameObject.SetActive(true);
	}

	void OnReturn(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		gameObject.SetActive(false);
	}

	void OnSearch(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
        //条件
		string chName = "";
        if (!string.IsNullOrEmpty(MemberIdInput.value))
        {
            chName += string.Format("<&>UserID={0}", MemberIdInput.value);
        }
        if (!string.IsNullOrEmpty(MemberAccInput.value))
        {
			chName += string.Format("<&>UserName='{0}'", MemberAccInput.value);
        }
        if (!string.IsNullOrEmpty(MemberQQInput.value))
        {
            chName += string.Format("<&>QQNum={0}", MemberQQInput.value);
        }
        if (onlineMember.value)
        {
            chName += "<&>OnlineFlag>0";
        }
        if (YouEMember.value)
        {
            chName += "<&>bankmoney>0";
        }
  
        string sortype = "";
        if (sortType.value == "注册时间")
        {
            sortype = "registerTm#{0}";
        }
        else if (sortType.value == "返点%")
        {
            sortype = "bankmoney#{0},registerTm#<";
        }
        else if (sortType.value == "余额")
        {
            sortype = "point#{0},registerTm#<";
        }
        else if (sortType.value == "最后登录时间")
        {
            sortype = "LastLoginTM#{0}";
        }

        string chbysj = "";
        if (sortMethod.value == "升序")
        {
            chbysj = string.Format(sortype, ">");
        }
        else
        {
            chbysj = string.Format(sortype, "<");
        }

        HuiYuanZiLiaoPanel.startDate = TimeHelper.GetTimeFromStr(StartDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(StartDateInput.value);
        HuiYuanZiLiaoPanel.endDate = TimeHelper.GetTimeFromStr(EndDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(EndDateInput.value);
        HuiYuanZiLiaoPanel.chName = chName;
        HuiYuanZiLiaoPanel.chBySj = chbysj;
		NetworkManager.Instance.LookupRecord(HuiYuanZiLiaoPanel.byRord, 2, HuiYuanZiLiaoPanel.byRord, 1, HuiYuanZiLiaoPanel.chName, HuiYuanZiLiaoPanel.startDate, HuiYuanZiLiaoPanel.endDate, (int)HuiYuanZiLiaoPanel.GetLastId(), HuiYuanZiLiaoPanel.chBySj);

	}
}