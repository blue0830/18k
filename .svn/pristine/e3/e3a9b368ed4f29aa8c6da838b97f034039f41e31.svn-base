using UnityEngine;
using System.Collections.Generic;

public class ShaiXuanXiaJiYingKuiMingXi : UserSubPanelBase {

    //开始日期
	public UIInput StartDateInput;
	//开始日期
	public UIInput EndDateInput;

    public UIPopupList LeiXingList;

    public UIInput MemberIdInput;

  


    //按钮
    public GameObject SearchBtn;
	//返回按钮
	public GameObject ReturnBtn;

    string[] names = { "充值", "取款", "取款撤单", "转账(出)", "转账(入)", "管理员加减钱", "投注", "彩票返点", "中奖", "中奖停追", "撤单", "余额转换", "棋牌返点", "签到奖金", "广告费/领取红包", "充值大赠送奖金", "新人充值送红包", "代理分红", "收款账号变动", "转出棋牌" };
    int[] values = { 0, 1, 7, 2, 3, 6, 9, 10, 11, 12, 13, 31, 35, 17, 18, 38, 39, 36, 15, 102 };
    Dictionary<string, int> dic = new Dictionary<string, int>();
    string operaType = "";

    void Start () {
		UIEventListener.Get(ReturnBtn).onClick = OnReturn;
		UIEventListener.Get(SearchBtn).onClick = OnSearch;
	}

	// Update is called once per frame
	void Update () {


	}

	public void Show()
	{
		StartDateInput.value =  TimeHelper.GetTimeStrFromUlong(XiaJiYingKuiMingXIPanel.startDate);
		EndDateInput.value = TimeHelper.GetTimeStrFromUlong(XiaJiYingKuiMingXIPanel.endDate);

        LeiXingList.Clear();
        dic.Clear();
        LeiXingList.value = "全部";
        LeiXingList.AddItem("全部");
        for (var i = 0; i < 20; i++)
        {
            dic.Add(names[i], values[i]);
        }
        foreach (var name in names)
        {
            LeiXingList.AddItem(name);
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
        if ("全部" == LeiXingList.value)
        {
            operaType = "";
        }
        foreach (KeyValuePair<string, int> attr in dic)
        {
            if (attr.Key == LeiXingList.value)
            {
                operaType = attr.Value.ToString();
            }
        }
        string chName = "";
        if (!string.IsNullOrEmpty(operaType))
        {
            chName = string.Format("<&>OperaType={0}", operaType);
        }
//        if (!string.IsNullOrEmpty(MemberIdInput.value))
//        {
//            chName += string.Format("<&>UserID={0}", MemberIdInput.value);
//        }
		XiaJiYingKuiMingXIPanel.lookuserId = 0;
		if (!string.IsNullOrEmpty(MemberIdInput.value)){
			int uid = int.Parse (MemberIdInput.value);
			if (uid != Global.CurrentUserId) {
				XiaJiYingKuiMingXIPanel.lookuserId = uid;
			}
		}
        XiaJiYingKuiMingXIPanel.startDate = TimeHelper.GetTimeFromStr(StartDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(StartDateInput.value);
        XiaJiYingKuiMingXIPanel.endDate = TimeHelper.GetTimeFromStr(EndDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(EndDateInput.value);
        XiaJiYingKuiMingXIPanel.chName = chName;
		NetworkManager.Instance.LookupRecord(XiaJiYingKuiMingXIPanel.byRord, 2, XiaJiYingKuiMingXIPanel.byRord, 1, XiaJiYingKuiMingXIPanel.chName, XiaJiYingKuiMingXIPanel.startDate, XiaJiYingKuiMingXIPanel.endDate,XiaJiYingKuiMingXIPanel.lookuserId);

	}
}