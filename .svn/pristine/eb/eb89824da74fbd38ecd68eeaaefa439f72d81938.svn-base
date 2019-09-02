using UnityEngine;
using System.Collections;

public class ShaiXuanYouXiRecordPanel : UserSubPanelBase {
	//开始日期
	public UIInput StartDateInput;
	//开始日期
	public UIInput EndDateInput;

	//按钮
	public GameObject SearchBtn;
	//返回按钮
	public GameObject ReturnBtn;

	void Start () {

        StartDateInput.value = TimeHelper.GetTimeStrFromUlong(YouXiRecordPanel.startDate);
        EndDateInput.value = TimeHelper.GetTimeStrFromUlong(YouXiRecordPanel.endDate);
        UIEventListener.Get(ReturnBtn).onClick = OnReturn;
        UIEventListener.Get(SearchBtn).onClick = OnSearch;
    }

	// Update is called once per frame
	void Update () {
		
	}

	public void Show()
	{
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
		YouXiRecordPanel.startDate = TimeHelper.GetTimeFromStr(StartDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(StartDateInput.value);
		YouXiRecordPanel.endDate = TimeHelper.GetTimeFromStr(EndDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(EndDateInput.value);
		YouXiRecordPanel.chName = "";
		NetworkManager.Instance.LookupRecord(YouXiRecordPanel.byRord, 2, YouXiRecordPanel.byRord, 1, YouXiRecordPanel.chName, YouXiRecordPanel.startDate, YouXiRecordPanel.endDate);
	}
}