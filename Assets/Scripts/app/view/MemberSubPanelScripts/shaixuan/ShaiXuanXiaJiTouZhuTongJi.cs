using UnityEngine;
using System.Collections;

public class ShaiXuanXiaJiTouZhuTongJi : UserSubPanelBase {

 
    //开始日期
	public UIInput StartDateInput;
	//开始日期
	public UIInput EndDateInput;


    public UIInput MemberIdInput;

    public UIInput MemberAccInput;



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
		StartDateInput.value =  TimeHelper.GetTimeStrFromUlong(XiaJiTouZhuTongJiPanel.startDate);
		EndDateInput.value = TimeHelper.GetTimeStrFromUlong(XiaJiTouZhuTongJiPanel.endDate);
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

        XiaJiTouZhuTongJiPanel.startDate = TimeHelper.GetTimeFromStr(StartDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(StartDateInput.value);
        XiaJiTouZhuTongJiPanel.endDate = TimeHelper.GetTimeFromStr(EndDateInput.value)==0?TimeHelper.GetNowTime():TimeHelper.GetTimeFromStr(EndDateInput.value);
        XiaJiTouZhuTongJiPanel.chName = chName;
		NetworkManager.Instance.LookupRecord(XiaJiTouZhuTongJiPanel.byRord, 2, XiaJiTouZhuTongJiPanel.byRord, 1, XiaJiTouZhuTongJiPanel.chName, XiaJiTouZhuTongJiPanel.startDate, XiaJiTouZhuTongJiPanel.endDate, XiaJiTouZhuTongJiPanel.lookuserId);

	}
}