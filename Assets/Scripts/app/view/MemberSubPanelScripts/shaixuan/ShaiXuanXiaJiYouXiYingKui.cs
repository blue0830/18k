using UnityEngine;
using System.Collections;

public class ShaiXuanXiaJiYouXiYingKui : UserSubPanelBase {

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
        sortType.AddItem("团队棋牌盈亏");
		sortType.AddItem("团队棋牌返点");
		sortType.value = "团队棋牌返点";

    }

    // Update is called once per frame
    void Update () {


	}

	public void Show()
	{
		StartDateInput.value =  TimeHelper.GetTimeStrFromUlong(XiaJiYouXiYingKuiPanel.startDate);
		EndDateInput.value = TimeHelper.GetTimeStrFromUlong(XiaJiYouXiYingKuiPanel.endDate);
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
		if (sortType.value == "团队棋牌盈亏")
        {
            sortype = "playResult#{0}";
        }
		else if (sortType.value == "团队棋牌返点")
        {
            sortype = "qpPoint#{0}";
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
        XiaJiYouXiYingKuiPanel.startDate = TimeHelper.GetTimeFromStr(StartDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(StartDateInput.value);
        XiaJiYouXiYingKuiPanel.endDate = TimeHelper.GetTimeFromStr(EndDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(EndDateInput.value);
        XiaJiYouXiYingKuiPanel.chName = chName;
        XiaJiYouXiYingKuiPanel.chBySj = chbysj;
		NetworkManager.Instance.LookupRecord(XiaJiYouXiYingKuiPanel.byRord, 2, XiaJiYouXiYingKuiPanel.byRord, 1, XiaJiYouXiYingKuiPanel.chName, XiaJiYouXiYingKuiPanel.startDate, XiaJiYouXiYingKuiPanel.endDate, (int)XiaJiYouXiYingKuiPanel.GetLastId(), XiaJiYouXiYingKuiPanel.chBySj);
	}
}