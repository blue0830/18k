using UnityEngine;
using System.Collections;

public class ShaiXuanZhuanZhangJiLu : UserSubPanelBase {

    //开始日期
	public UIInput StartDateInput;
	//开始日期
	public UIInput EndDateInput;


    public UIInput zhifuIdInput;

    public UIInput jieshouIdInput;


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
		StartDateInput.value =  TimeHelper.GetTimeStrFromUlong(ZhuanZhangJiLuPanel.startDate);
		EndDateInput.value = TimeHelper.GetTimeStrFromUlong(ZhuanZhangJiLuPanel.endDate);
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
        if (!string.IsNullOrEmpty(zhifuIdInput.value))
        {
            chName += string.Format("<&>UserID={0}", zhifuIdInput.value);
        }
        if (!string.IsNullOrEmpty(jieshouIdInput.value))
        {
            chName += string.Format("<&>DestUserID={0}", jieshouIdInput.value);
        }

        ZhuanZhangJiLuPanel.startDate = TimeHelper.GetTimeFromStr(StartDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(StartDateInput.value);
        ZhuanZhangJiLuPanel.endDate = TimeHelper.GetTimeFromStr(EndDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(EndDateInput.value);
        ZhuanZhangJiLuPanel.chName = chName;
		NetworkManager.Instance.LookupRecord(ZhuanZhangJiLuPanel.byRord, 2, ZhuanZhangJiLuPanel.byRord, 1, ZhuanZhangJiLuPanel.chName, ZhuanZhangJiLuPanel.startDate, ZhuanZhangJiLuPanel.endDate,ZhuanZhangJiLuPanel.lookuserId);

	}
}