using UnityEngine;
using System.Collections;

public class ShaiXuanQuKuanPanel : UserSubPanelBase {
	//开始日期
	public UIInput StartDateInput;
	//开始日期
	public UIInput EndDateInput;

	//按钮
	public GameObject SearchBtn;
	//返回按钮
	public GameObject ReturnBtn;

	void Start () {
		UIEventListener.Get(ReturnBtn).onClick = OnReturn;
		UIEventListener.Get(SearchBtn).onClick = OnSearch;
	}

	// Update is called once per frame
	void Update () {


	}

	void OnReturn(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		gameObject.SetActive(false);
	}

	void OnSearch(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		QuKuanRecordbkPanel.startDate = TimeHelper.GetTimeFromStr(StartDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(StartDateInput.value);
		QuKuanRecordbkPanel.endDate = TimeHelper.GetTimeFromStr(EndDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(EndDateInput.value);
		QuKuanRecordbkPanel.chName = "";
		NetworkManager.Instance.LookupRecord(QuKuanRecordbkPanel.byRord, 2, QuKuanRecordbkPanel.byRord, 1, QuKuanRecordbkPanel.chName, QuKuanRecordbkPanel.startDate, QuKuanRecordbkPanel.endDate);
	}

    public void Show()
    {
		StartDateInput.value =  TimeHelper.GetTimeStrFromUlong(QuKuanRecordbkPanel.startDate);
		EndDateInput.value = TimeHelper.GetTimeStrFromUlong(QuKuanRecordbkPanel.endDate); 
		gameObject.SetActive(true);
    }
}