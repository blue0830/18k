using UnityEngine;
using System.Collections;

public class ShaiXuanXiaJiYingKuiTongJi : UserSubPanelBase {

    public UIPopupList sortType;

    public UIPopupList sortMethod; //升or 降
    //开始日期
	public UIInput StartDateInput;
	//开始日期
	public UIInput EndDateInput;


    public UIInput MemberIdInput;

    public UIInput MemberAccInput;



    //按钮
    public GameObject SearchBtn;
	//返回按钮
	public GameObject ReturnBtn;

	string lineNumber="";

	void Start () {
		UIEventListener.Get(ReturnBtn).onClick = OnReturn;
		UIEventListener.Get(SearchBtn).onClick = OnSearch;

        sortMethod.Clear();
        sortMethod.AddItem("升序");
        sortMethod.AddItem("降序");
		sortMethod.value = "降序";

        sortType.Clear();
        sortType.AddItem("团队充值");
		sortType.AddItem("团队取款");
		sortType.AddItem("团队投注");
		sortType.AddItem("团队中奖");
		sortType.AddItem("团队返点");
		sortType.AddItem("团队盈亏");
		sortType.AddItem("团队广告");
        sortType.value = "团队充值";
    }

	// Update is called once per frame
	void Update () {


	}

	public void Show()
	{
		StartDateInput.value =  TimeHelper.GetTimeStrFromUlong(XiaJiYingKuiTongjiPanel.startDate);
		EndDateInput.value = TimeHelper.GetTimeStrFromUlong(XiaJiYingKuiTongjiPanel.endDate);
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
        string chName = "";
        if (!string.IsNullOrEmpty(MemberIdInput.value))
        {
            chName += string.Format("<&>UserID={0}", MemberIdInput.value);
        }
        if (!string.IsNullOrEmpty(MemberAccInput.value))
        {
            chName += string.Format("<&>UserName='{0}'", MemberAccInput.value);
        }

        string sortype = "";
        if (sortType.value == "团队充值")
        {
            sortype = "PayMoney#{0}";
        }
		else if (sortType.value == "团队取款")
        {
            sortype = "OutMoney#{0}";
        }
		else if (sortType.value == "团队投注")
        {
            sortype = "OrderAmount#{0}";
        }
		else if (sortType.value == "团队中奖")
        {
            sortype = "BingoMoney#{0}";
        }
		else if (sortType.value == "团队返点")
        {
            sortype = "OrderPoint#{0}";
        }
		else if (sortType.value == "团队盈亏")
        {
            sortype = "OrderResult#{0}";
        }
		else if (sortType.value == "团队广告")
        {
            sortype = "PlayAd#{0}";
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

        XiaJiYingKuiTongjiPanel.startDate = TimeHelper.GetTimeFromStr(StartDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(StartDateInput.value);
        XiaJiYingKuiTongjiPanel.endDate = TimeHelper.GetTimeFromStr(EndDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(EndDateInput.value);
        XiaJiYingKuiTongjiPanel.chName = chName;
        XiaJiYingKuiTongjiPanel.chBySj = chbysj;

        NetworkManager.Instance.LookupRecord(XiaJiYingKuiTongjiPanel.byRord, 2, XiaJiYingKuiTongjiPanel.byRord, 1, XiaJiYingKuiTongjiPanel.chName, XiaJiYingKuiTongjiPanel.startDate, XiaJiYingKuiTongjiPanel.endDate, (int)XiaJiYingKuiTongjiPanel.GetLastId(), XiaJiYingKuiTongjiPanel.chBySj);

	}
}