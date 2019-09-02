using UnityEngine;
using System.Collections;

public class ShaiXuanChongZhiRecordPanel : UserSubPanelBase {
	//开始日期
	public UIInput StartDateInput;
	//开始日期
	public UIInput EndDateInput;
	//流水号
    public UIInput LiuShuiHaoLabel;
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
		StartDateInput.value =  TimeHelper.GetTimeStrFromUlong(ChongZhiRecordPanel.startDate);
		EndDateInput.value = TimeHelper.GetTimeStrFromUlong(ChongZhiRecordPanel.endDate);
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
		if (!string.IsNullOrEmpty (lineNumber)) {
			chName = string.Format ("<&>ClassID={0}", lineNumber);
		}
		ChongZhiRecordPanel.startDate = TimeHelper.GetTimeFromStr(StartDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(StartDateInput.value);
		ChongZhiRecordPanel.endDate = TimeHelper.GetTimeFromStr(EndDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(EndDateInput.value);
		ChongZhiRecordPanel.chName = chName;
		NetworkManager.Instance.LookupRecord(ChongZhiRecordPanel.byRord, 2, ChongZhiRecordPanel.byRord, 1, ChongZhiRecordPanel.chName, ChongZhiRecordPanel.startDate, ChongZhiRecordPanel.endDate);

	}
}