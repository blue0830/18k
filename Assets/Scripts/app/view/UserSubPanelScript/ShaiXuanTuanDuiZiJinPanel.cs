using UnityEngine;
using System.Collections;

public class ShaiXuanTuanDuiZiJinPanel : UserSubPanelBase {
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

	public void Show()
	{
        StartDateInput.value = TimeHelper.GetTimeStrFromUlong(TuanDuiZiJinPanel.startDate);
        EndDateInput.value = TimeHelper.GetTimeStrFromUlong(TuanDuiZiJinPanel.endDate); 

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
        TuanDuiZiJinPanel.startDate= TimeHelper.GetTimeFromStr(StartDateInput.value) == 0 ? TimeHelper.GetNowTime() : TimeHelper.GetTimeFromStr(StartDateInput.value);
        TuanDuiZiJinPanel.endDate= TimeHelper.GetTimeFromStr(EndDateInput.value) == 0 ? TimeHelper.GetNowTime() : TimeHelper.GetTimeFromStr(EndDateInput.value);

        ulong startDate = TuanDuiZiJinPanel.startDate;
        ulong endDate = TuanDuiZiJinPanel.endDate;

        NetworkManager.Instance.GetTeamData(startDate, endDate);
	}
}