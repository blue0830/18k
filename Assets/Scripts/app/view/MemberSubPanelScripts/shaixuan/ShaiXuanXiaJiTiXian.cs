using UnityEngine;
using System.Collections;

public class ShaiXuanXiaJiTiXian : UserSubPanelBase {

    //开始日期
	public UIInput StartDateInput;
	//开始日期
	public UIInput EndDateInput;
	//流水号

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
		StartDateInput.value =  TimeHelper.GetTimeStrFromUlong(XiaJiTiXianPanel.startDate);
		EndDateInput.value = TimeHelper.GetTimeStrFromUlong(XiaJiTiXianPanel.endDate);
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
        if (!string.IsNullOrEmpty(MemberIdInput.value))
        {
            chName += string.Format("<&>UserID={0}", MemberIdInput.value);
        }
        if (!string.IsNullOrEmpty(MemberAccInput.value))
        {
            chName += string.Format("<&>UserName='{0}'", MemberAccInput.value);
        }

        XiaJiTiXianPanel.startDate = TimeHelper.GetTimeFromStr(StartDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(StartDateInput.value);
        XiaJiTiXianPanel.endDate = TimeHelper.GetTimeFromStr(EndDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(EndDateInput.value);
        XiaJiTiXianPanel.chName = chName;
		NetworkManager.Instance.LookupRecord(XiaJiTiXianPanel.byRord, 2, XiaJiTiXianPanel.byRord, 1, XiaJiTiXianPanel.chName, XiaJiTiXianPanel.startDate, XiaJiTiXianPanel.endDate, XiaJiTiXianPanel.lookuserId);

	}
}