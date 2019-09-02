using UnityEngine;
using System.Collections;
//添加下级
public class TianJiaXiaJiPanel : UserSubPanelBase {
	 
	//按钮
	public GameObject ReturnBtn;
    public GameObject confirmBtn;

    public UIPopupList TypePopupList;
    public UIInput AccountInput;

    public UIInput passwordInput;
    public UIInput confirmInput;

    public UIInput tikuanpswdInput;
    public UIInput tikuanConfirmInput;

    public UIInput xiajifandian;
    public GameObject xiajifandianadd;
    public GameObject xiajifandiansub;

    public UILabel shengyupeieLabel;

    public UIPopupList urlList;
    public GameObject copyBtn;

    public UIInput  wangyefandian;
    public GameObject wangyefandianadd;
    public GameObject wangyefandiansub;

    public UILabel webPointLabel;

    public GameObject changeBtn;

    private MSG_GP_USER_GETPLAYMAXPOINTRESULT savePara;

    void Start () {
		UIEventListener.Get(ReturnBtn).onClick = OnReturn;
        UIEventListener.Get(confirmBtn).onClick = OnAddClick;

        UIEventListener.Get(copyBtn).onClick = OnCopyClick;
        UIEventListener.Get(changeBtn).onClick = OnChangeClick;

        UIEventListener.Get(xiajifandianadd).onClick = OnChangeXiaJiFanDian;
        UIEventListener.Get(xiajifandiansub).onClick = OnChangeXiaJiFanDian;

        UIEventListener.Get(wangyefandianadd).onClick = OnChangeWangYeFanDian;
        UIEventListener.Get(wangyefandiansub).onClick = OnChangeWangYeFanDian;

        EventDelegate.Add(xiajifandian.onSubmit, XiaJiFandianChange);
        EventDelegate.Add(wangyefandian.onSubmit, WangYeFandianChange);
    }

	// Update is called once per frame
	void Update () {
	}

    public void Show(MSG_GP_USER_GETPLAYMAXPOINTRESULT para)
    {
        gameObject.SetActive(true);

        savePara = para;

        AccountInput.value = "";
        passwordInput.value = "";
        confirmInput.value = "";
        tikuanpswdInput.value = "";
        tikuanConfirmInput.value = "";

        xiajifandian.value = "0.0";
        wangyefandian.value = "0.0";

        webPointLabel.text = string.Format("当前网页注册返点：{0}", para.GetdWebPoint());

        urlList.Clear();
        for (int i = 0; i < para.GetUrlList().Count; i ++)
        {
			urlList.AddItem(para.GetUrlList()[i]+"/reg/?user="+Global.CurrentUserId);
        }
        TypePopupList.value = "代理";

        shengyupeieLabel.text = para.GetDescribe();
    }

	void OnReturn(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		gameObject.SetActive(false);
	}


    void OnAddClick(GameObject go)
    {

        if (string.IsNullOrEmpty(AccountInput.value))
        {
            msgSignal.Dispatch(new MsgPara("请输入用户名", 2));
            return;
        }
		if (AccountInput.value.Length<6||AccountInput.value.Length>20)
		{
			msgSignal.Dispatch(new MsgPara("请输入6~20个长度的用户名", 2));
			return;
		}
        if (string.IsNullOrEmpty(passwordInput.value))
        {
            passwordInput.value = "888888";
            confirmInput.value = "888888";

        }
        if (string.IsNullOrEmpty(tikuanpswdInput.value))
        {
            tikuanpswdInput.value = "999999";
            tikuanConfirmInput.value = "999999";
        }


        if (passwordInput.value != confirmInput.value)
        {
            msgSignal.Dispatch(new MsgPara("登录密码不一致", 2));
            return;
        }

        if (tikuanpswdInput.value != tikuanConfirmInput.value)
        {
            msgSignal.Dispatch(new MsgPara("提款密码不一致", 2));
            return;
        }

        int type = 0;
        if (TypePopupList.value.Equals("代理"))
        {
            type = 1;
        }

        NetworkManager.Instance.AddMember(type, AccountInput.value, passwordInput.value, tikuanpswdInput.value, xiajifandian.value);

    }

    void OnCopyClick(GameObject go)
    {
        UniClipboard.SetText(urlList.value)  ;
		msgSignal.Dispatch (new MsgPara ("复制链接成功",2));
    }

    void OnChangeClick(GameObject go)
    {
        NetworkManager.Instance.SetWebFandian(wangyefandian.value);
    }


    void OnChangeXiaJiFanDian(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
		double beishu = float.Parse(xiajifandian.value);
        if (go.name == "add")
        {
            if (beishu <= savePara.GetdMaxPoint())
            {
                beishu += 0.1;
                xiajifandian.value = string.Format("{0:0.0}", beishu);
            }
        }
        else
        {
 
            if (beishu >0)
            {
                beishu-=0.1;
                xiajifandian.value = string.Format("{0:0.0}", beishu);
            }
        }
    }


    void OnChangeWangYeFanDian(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
		double beishu = float.Parse(wangyefandian.value);
        if (go.name == "add")
        {
            if (beishu <= savePara.GetdMaxPoint())
            {
                beishu += 0.1;
                wangyefandian.value = string.Format("{0:0.0}", beishu);
            }

        }
        else
        {
            if (beishu > 0)
            {
                beishu-=0.1;
                wangyefandian.value = string.Format("{0:0.0}", beishu);
            }
        }
    }

    void XiaJiFandianChange()
    {

        if (string.IsNullOrEmpty(xiajifandian.value))
        {
            xiajifandian.value = "0.0";
        }
        float a = 0;
        if (float.TryParse(xiajifandian.value, out a))
        {
            if (a <= 0)
            {
                xiajifandian.value = "0.0";
            }
            else if (a > savePara.GetdMaxPoint())
            {
                xiajifandian.value = string.Format("{0:0.0}", savePara.GetdMaxPoint());
            }
        }
        else
        {
            xiajifandian.value = "0.0";
        }

    }

    void WangYeFandianChange()
    {

        if (string.IsNullOrEmpty(wangyefandian.value))
        {
            wangyefandian.value = "0.0";
        }
        float a = 0;
        if (float.TryParse(wangyefandian.value, out a))
        {
            if (a <= 0)
            {
                wangyefandian.value = "0.0";
            } else if (a> savePara.GetdMaxPoint())
            {
                wangyefandian.value = string.Format("{0:0.0}", savePara.GetdMaxPoint());
            }
        }
        else
        {
            wangyefandian.value = "0.0";
        }

    }
}
