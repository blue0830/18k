using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//充值页面
public class ChongZhiPanel : UserSubPanelBase {
	//支付方式
	public UIPopupList ZhiFuFangShiList;
	//收款人
	public UILabel ShouKuanRenLabel;
	//收款账户
	public UILabel ShouKuanZhangHuLabel;
	//开户银行
	public UILabel KaiHuYinHangLabel;
	//付款ID
	public UILabel UIDLabel;

	//打开充值网页
	public GameObject OpenUrl;
	//返回按钮
	public GameObject ReturnBtn;

    //要隐藏
    public GameObject[] hide;

    public GameObject[] allObj;

    public UIGrid grid;
    public UIScrollView scrollview;
	IUInfoModel uinfoModel;

	List<GetBankInfo> _Myinfos = new List<GetBankInfo>();

    GetBankInfo selectedInfo;
	void Start () {

        UIEventListener.Get(ReturnBtn).onClick = OnReturn;
        UIEventListener.Get(OpenUrl).onClick = OnOPen;

        EventDelegate.Add(ZhiFuFangShiList.onChange , OnChange);
       
	}

	// Update is called once per frame
	void Update () {


	}

	public void show(List<GetBankInfo> infos,IUInfoModel uinfoModel)
    {
		gameObject.SetActive(true);
		this.uinfoModel = uinfoModel;
		_Myinfos.Clear ();
        ZhiFuFangShiList.Clear();
        ZhiFuFangShiList.value = "";

		for (int i = 0; i < infos.Count; ++i)
        {
            infos[i].BankName = (i + 1).ToString() + "." + infos[i].BankName;

            _Myinfos.Add (infos[i]);

			ZhiFuFangShiList.AddItem(infos[i].BankName);
            if (i==0&&string.IsNullOrEmpty(ZhiFuFangShiList.value))
            {
				ZhiFuFangShiList.value = infos[i].BankName;
				selectedInfo = infos[i];
				FillContent();
            }
        }
    }

    void OnChange()
    {
        for (int i = 0; i < _Myinfos.Count; ++i)
        {
            if (ZhiFuFangShiList.value == _Myinfos[i].BankName)
            {
                selectedInfo = _Myinfos[i];
                break;
            }
        }
        FillContent();
    }

	void FillContent()
    {
        ShouKuanRenLabel.text = selectedInfo.TrueName;
        ShouKuanZhangHuLabel.text = selectedInfo.BankAccount;
        //KaiHuYinHangLabel.text = selectedInfo.BankName;
		KaiHuYinHangLabel.text = selectedInfo.BankName.Substring(selectedInfo.BankName.IndexOf(".")+1);
		UIDLabel.text = uinfoModel.GetUserID().ToString();//用户ID
        if (selectedInfo.iShowBankType >= 19 && selectedInfo.iShowBankType <=25)
        {
            //隐藏下面
            for (int i = 0; i < hide.Length; ++i)
            {
                hide[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < hide.Length; ++i)
            {
                hide[i].SetActive(true);
            }
        }

        StartCoroutine(process());
    }

    IEnumerator process()
    {

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < allObj.Length; ++i)
        {
            allObj[i].SetActive(false);
            allObj[i].SetActive(true);
        }
        //if (selectedInfo.iShowBankType == 19 || selectedInfo.iShowBankType == 20)
		if(selectedInfo.iShowBankType >= 19 && selectedInfo.iShowBankType <=25)
		{
            //隐藏下面
            for (int i = 0; i < hide.Length; ++i)
            {
                hide[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < hide.Length; ++i)
            {
                hide[i].SetActive(true);
            }
        }
        grid.repositionNow = true;
        scrollview.ResetPosition();
    }

    void OnReturn(GameObject go)
    {
		AudioController.Instance.SoundPlay("active_item");
        gameObject.SetActive(false);
    }

    void OnOPen(GameObject go)
    {
		AudioController.Instance.SoundPlay("active_item");
        string url = selectedInfo.BankUrl;
        //if (selectedInfo.iShowBankType == 19 || selectedInfo.iShowBankType == 20)
		if(selectedInfo.iShowBankType >= 19 && selectedInfo.iShowBankType <=25)
        {
			url += Global.CurrentUserName;
        }
        Application.OpenURL(url);
    }
}
