using UnityEngine;
using System.Collections;

public class ShaiXuanGeRenZiJinPanel : UserSubPanelBase {
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

	void OnReturn(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		gameObject.SetActive(false);
	}

	void OnSearch(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");

        GeRenZiJinPanel.startDate = TimeHelper.GetTimeFromStr(StartDateInput.value) == 0 ? TimeHelper.GetNowTime() : TimeHelper.GetTimeFromStr(StartDateInput.value);
        GeRenZiJinPanel.endDate = TimeHelper.GetTimeFromStr(EndDateInput.value) == 0 ? TimeHelper.GetNowTime() : TimeHelper.GetTimeFromStr(EndDateInput.value);

        ulong startDate = GeRenZiJinPanel.startDate;
        ulong endDate = GeRenZiJinPanel.endDate;

        NetworkManager.Instance.GetMyMoney(startDate, endDate);
	}

	// Update is called once per frame
	void Update () {


	}

	public void Show()
	{
        StartDateInput.value = TimeHelper.GetTimeStrFromUlong(GeRenZiJinPanel.startDate);
        EndDateInput.value = TimeHelper.GetTimeStrFromUlong(GeRenZiJinPanel.endDate); 
		gameObject.SetActive(true);
	}
}