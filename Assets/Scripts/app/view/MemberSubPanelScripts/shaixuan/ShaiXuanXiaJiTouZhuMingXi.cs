using UnityEngine;
using System.Collections.Generic;

public class ShaiXuanXiaJiTouZhuMingXi : UserSubPanelBase {

    public UIPopupList TouZhuLeiXingList;

    //开始日期
	public UIInput StartDateInput;
	//开始日期
	public UIInput EndDateInput;

    public UIInput MemberIdInput;
    //按钮
    public GameObject SearchBtn;
	//返回按钮
	public GameObject ReturnBtn;

    public List<LotteryConfig> lcfgs;

    void Start () {
		UIEventListener.Get(ReturnBtn).onClick = OnReturn;
		UIEventListener.Get(SearchBtn).onClick = OnSearch;
	}

	// Update is called once per frame
	void Update () {
	}

	public void Show()
	{
		StartDateInput.value =  TimeHelper.GetTimeStrFromUlong(XiajiTouZhuMingXiPanel.startDate);
		EndDateInput.value = TimeHelper.GetTimeStrFromUlong(XiajiTouZhuMingXiPanel.endDate);
        LotteryConfigLoader cfgLoader = ConfigManager.Instance.GetLotteryCfgLoader();
        lcfgs = cfgLoader.lotteryConfigs;
        TouZhuLeiXingList.Clear();
        TouZhuLeiXingList.value = "全部";
        TouZhuLeiXingList.AddItem("全部");
        for (int i = 0; i < lcfgs.Count; ++i)
        {
            TouZhuLeiXingList.AddItem(lcfgs[i].name);
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
        string lotteryId = "";
        if ("全部" == TouZhuLeiXingList.value)
        {
            lotteryId = "";
        }
        for (int i = 0; i < lcfgs.Count; ++i)
        {
            if (lcfgs[i].name == TouZhuLeiXingList.value)
            {
                lotteryId = lcfgs[i].lotteryId.ToString();
            }
        }
        string chName = "";
        if (!string.IsNullOrEmpty(lotteryId))
        {
            chName += string.Format("<&>ClassID={0}", lotteryId);
        }
		XiajiTouZhuMingXiPanel.lookuserId = 0;
		if (!string.IsNullOrEmpty(MemberIdInput.value)){
			int uid = int.Parse (MemberIdInput.value);
			if (uid != Global.CurrentUserId) {
				XiajiTouZhuMingXiPanel.lookuserId = uid;
			}
		}
        XiajiTouZhuMingXiPanel.startDate = TimeHelper.GetTimeFromStr(StartDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(StartDateInput.value);
        XiajiTouZhuMingXiPanel.endDate = TimeHelper.GetTimeFromStr(EndDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(EndDateInput.value);
        XiajiTouZhuMingXiPanel.chName = chName;
		NetworkManager.Instance.LookupRecord(XiajiTouZhuMingXiPanel.byRord, 2, XiajiTouZhuMingXiPanel.byRord, 1, XiajiTouZhuMingXiPanel.chName, XiajiTouZhuMingXiPanel.startDate, XiajiTouZhuMingXiPanel.endDate,XiajiTouZhuMingXiPanel.lookuserId);

	}
}