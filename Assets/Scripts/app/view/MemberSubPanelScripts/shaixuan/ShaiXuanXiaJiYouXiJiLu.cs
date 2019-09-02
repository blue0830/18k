using UnityEngine;
using System.Collections;

public class ShaiXuanXiaJiYouXiJiLu : UserSubPanelBase {

  
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
	}

	// Update is called once per frame
	void Update () {


	}

	public void Show()
	{
		StartDateInput.value =  TimeHelper.GetTimeStrFromUlong(XiaJiYouXiJiLuPanel.startDate);
		EndDateInput.value = TimeHelper.GetTimeStrFromUlong(XiaJiYouXiJiLuPanel.endDate);
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
//        if (!string.IsNullOrEmpty(MemberIdInput.value))
//        {
//            chName += string.Format("<&>UserID={0}", MemberIdInput.value);
//        }
//        if (!string.IsNullOrEmpty(MemberAccInput.value))
//        {
//            chName += string.Format("<&>UserName='{0}'", MemberAccInput.value);
//        }
		if (!string.IsNullOrEmpty(MemberIdInput.value))
        {
			XiaJiYouXiJiLuPanel.lookuserId = int.Parse(MemberIdInput.value);
        }
        XiaJiYouXiJiLuPanel.startDate = TimeHelper.GetTimeFromStr(StartDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(StartDateInput.value);
        XiaJiYouXiJiLuPanel.endDate = TimeHelper.GetTimeFromStr(EndDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(EndDateInput.value);
        XiaJiYouXiJiLuPanel.chName = chName;
		NetworkManager.Instance.LookupRecord(XiaJiYouXiJiLuPanel.byRord, 2, XiaJiYouXiJiLuPanel.byRord, 1, XiaJiYouXiJiLuPanel.chName, XiaJiYouXiJiLuPanel.startDate, XiaJiYouXiJiLuPanel.endDate,XiaJiYouXiJiLuPanel.lookuserId);

	}
}