using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShaiXuanTouZhuRecordPanel : UserSubPanelBase {
	//开始日期
	public UIInput StartDateInput;
	//开始日期
	public UIInput EndDateInput;
	//投注期号
	public UIInput TouZhuQiHaoInput;
	//投注类型
	public UIPopupList TouZhuLeiXingList;

	//按钮
	public GameObject ReturnBtn;
	//返回按钮
	public GameObject SearchBtn;

	string lotteryId="";

	public List<LotteryConfig> lcfgs;
	void Start () {
		UIEventListener.Get(ReturnBtn).onClick = OnReturn;
		UIEventListener.Get(SearchBtn).onClick = OnSearch;
		EventDelegate.Add (TouZhuLeiXingList.onChange, OnListChange);
	}

	// Update is called once per frame
	void Update () {


	}

	public void Show()
	{
		StartDateInput.value =  TimeHelper.GetTimeStrFromUlong(TouZhuRecordPanel.startDate);
        EndDateInput.value = TimeHelper.GetTimeStrFromUlong(TouZhuRecordPanel.endDate); 


        LotteryConfigLoader cfgLoader = ConfigManager.Instance.GetLotteryCfgLoader();
		lcfgs = cfgLoader.lotteryConfigs;

		TouZhuLeiXingList.Clear ();
		TouZhuLeiXingList.value = "全部";
		TouZhuLeiXingList.AddItem ("全部");
		for (int i = 0; i < lcfgs.Count; ++i)
        { 
			TouZhuLeiXingList.AddItem (lcfgs [i].name);
        }
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
		if (!string.IsNullOrEmpty (lotteryId)) {
			chName += string.Format ("<&>ClassID={0}", lotteryId);
		}
		if (!string.IsNullOrEmpty (TouZhuQiHaoInput.value)) {
			chName += string.Format ("<&>ActivityName='{0}'", TouZhuQiHaoInput.value);
		}
		TouZhuRecordPanel.startDate = TimeHelper.GetTimeFromStr(StartDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(StartDateInput.value);
		TouZhuRecordPanel.endDate = TimeHelper.GetTimeFromStr(EndDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(EndDateInput.value);
        TouZhuRecordPanel.chName = chName;
		NetworkManager.Instance.LookupRecord(TouZhuRecordPanel.byRord, 2, TouZhuRecordPanel.byRord, 1, TouZhuRecordPanel.chName, TouZhuRecordPanel.startDate, TouZhuRecordPanel.endDate);

	}

	void OnListChange()
	{
		if ("全部" == TouZhuLeiXingList.value) {
			lotteryId ="";
		}
		for (int i = 0; i < lcfgs.Count; ++i)
		{ 
			if (lcfgs [i].name == TouZhuLeiXingList.value) {
				lotteryId = lcfgs [i].lotteryId.ToString();
			}
		}
	}
}


