using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.signal.impl;

//用户中心Panel
public class UserPanel : MonoBehaviour
{
	public Signal<MsgPara> msgSignal = new Signal<MsgPara> ();

	public Signal LogoutSignal = new Signal ();

	//需要保存的数据
	public IUInfoModel uinfoModel;
	public MSG_GP_USER_GetUserInfoBack userbaseInfo;

	//Label
	public UILabel UnameLabel;
	public UILabel IdLabel;
	public UILabel MoneyLabel;
	public UILabel GoldLabel;

	//列表按钮
	public GameObject shuaxinBtn;
	public GameObject musicBtn;
	public GameObject ToLotteryBtn;
	public GameObject ToGameBtn;
	public GameObject TiXianBtn;
	public GameObject TopupBtn;
	public GameObject returnbtn;
	public GameObject GeRenZiLiaoBtn;
	public GameObject GeRenZiJinBtn;
	public GameObject TuanDuiShuJuBtn;
	public GameObject TiKuanZhangHuBtn;
	public GameObject ZhangHaoXinXiBtn;
	public GameObject ZhangHaoBaohuBtn;
	public GameObject DengluMiMaBtn;
	public GameObject QuKuanMiMaBtn;
	public GameObject TouZhuJiLuBtn;
	public GameObject QuKuanJiLuBtn;
	public GameObject ChongZhiJiLuBtn;
	public GameObject YingKuiJiLuBtn;
	public GameObject YouxiJiluBtn;
	public GameObject LogOutBtn;
    public GameObject kefuBtn;

	//Panels
	public TransferPanel transferPanel;//金额转换 Panel
	public ChongZhiPanel chongzhiPanel;//充值 Panel
	public TiXianPanel tixianPanel;//提现 Panel

    Transform panelRoot;
    //下面的panel 设置为启动时才加载
	private GeRenZiLiaoPanel gerenziliaoPanel;//个人资料 Panel
	private GeRenZiJinPanel gerenzijinPanel;//个人资金 Panel
	private TuanDuiZiJinPanel tuanDuiZiJinPanel;//团队资金 Panel
	private TiKuanZhangHuPanel tiKuanZhangHuPanel;//提款账户 Panel
	private ZhangHaoXinXiPanel zhangHaoXinXiPanel;//账号信息 Panel
	private ZhangHaoBaoHuPanel zhangHaoBaoHuPanel;//账号保护 Panel
	private DengLuPWDPanel dengLuPWDPanel;//登录密码 Panel
	private QuKuanPWDPanel quKuanPWDPanel;//取款密码 Panel
	private TouZhuRecordPanel touzhuRecordpanel;//投注记录 Panel
	private QuKuanRecordbkPanel qukuanRecordPanel;//取款记录 Panel
	private ChongZhiRecordPanel chongzhiRecordpanel;//充值记录 Panel
	private YingKuiRecordPanel yingkuiRecordPanel;//盈亏记录 Panel
	private YouXiRecordPanel youxiRecordPanel;//游戏记录 Panel
    private TouZhuXiangQingPanel touizhuxiangqingPanel;//投注详情 Panel

	// Use this for initialization
	void Start ()
	{
        panelRoot = transferPanel.transform.parent;

        AddSignal ();
        
        UIEventListener.Get(shuaxinBtn).onClick = OnShuaxinClick;//刷新

        UIEventListener.Get (ToLotteryBtn).onClick = OnTransferClick;//转彩票
		UIEventListener.Get (ToGameBtn).onClick = OnTransferClick;//转棋牌
		UIEventListener.Get (TopupBtn).onClick = OnTopupClick;//充值
		UIEventListener.Get (TiXianBtn).onClick = OnTiXianClick;//提现
		
		UIEventListener.Get (GeRenZiLiaoBtn).onClick = OnRenZiLiaoClick;//个人资料
		UIEventListener.Get (GeRenZiJinBtn).onClick = OnRenZiJinClick;//个人资金
		UIEventListener.Get (TuanDuiShuJuBtn).onClick = OnTuanDuiShuJuClick;//团队资金

		UIEventListener.Get (TiKuanZhangHuBtn).onClick = OnTiKuanZhangHuClick;//提款账户
		UIEventListener.Get (ZhangHaoXinXiBtn).onClick = OnZhangHaoXinXiClick;//账号信息
		UIEventListener.Get (ZhangHaoBaohuBtn).onClick = OnZhangHaoBaoHuClick;//账号保护
		UIEventListener.Get (DengluMiMaBtn).onClick = OndengLuPWDClick;//登录密码
		UIEventListener.Get (QuKuanMiMaBtn).onClick = OnquKuanPWDClick;//取款密码

		UIEventListener.Get (TouZhuJiLuBtn).onClick = OnTouZhuJiLuClick;//投注记录
		UIEventListener.Get (QuKuanJiLuBtn).onClick = QuKuanJiLuClick;//取款记录
		UIEventListener.Get (ChongZhiJiLuBtn).onClick = OnChongZhiJiLuClick;//充值记录
		UIEventListener.Get (YingKuiJiLuBtn).onClick = OnYingKuiJiLuClick;//盈亏记录
		UIEventListener.Get (YouxiJiluBtn).onClick = OnYouXiJiluClick;//游戏记录

		UIEventListener.Get (LogOutBtn).onClick = OnLogOut;//退出登录

        UIEventListener.Get(kefuBtn).onClick = OnKefuClick;

		UIEventListener.Get (musicBtn).onClick = OnMusicClick;
	}

	void OnMusicClick(GameObject go)
	{
		if (AudioController.Instance.playSound) {
			go.GetComponent<UISprite>().spriteName="souxx2";
			AudioController.Instance.playSound = false;
		} else {
			go.GetComponent<UISprite>().spriteName="souxx";
			AudioController.Instance.playSound = true;
			AudioController.Instance.SoundPlay("active_item");
		}
	}

	// Update is called once per frame
	void Update ()
	{
	}

	void OnDestroy ()
	{
		RemoveSignal ();
	}

    void OnKefuClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        Application.OpenURL(Constant.CUSTOMER_SERVICE_URL);
    }

    void OnShuaxinClick(GameObject go)
    {
		AudioController.Instance.SoundPlay("active_item");
        NetworkManager.Instance.RefreshMoney();
    }
		
    void AddSignal ()
	{
		transferPanel.msgSignal.AddListener (OnMsg);
		chongzhiPanel.msgSignal.AddListener (OnMsg);
		tixianPanel.msgSignal.AddListener (OnMsg);
	}

	void RemoveSignal ()
	{
		transferPanel.msgSignal.RemoveListener (OnMsg);
		chongzhiPanel.msgSignal.RemoveListener (OnMsg);
		tixianPanel.msgSignal.RemoveListener (OnMsg);

        if(gerenziliaoPanel!=null)
            gerenziliaoPanel.msgSignal.RemoveListener (OnMsg);//个人资料
		if(gerenzijinPanel!=null)
			gerenzijinPanel.msgSignal.RemoveListener (OnMsg);//个人资金
		if(tuanDuiZiJinPanel!=null)
			tuanDuiZiJinPanel.msgSignal.RemoveListener (OnMsg);//团队资金
		if(tiKuanZhangHuPanel!=null)
			tiKuanZhangHuPanel.msgSignal.RemoveListener (OnMsg);//提款
		if(zhangHaoXinXiPanel!=null)
			zhangHaoXinXiPanel.msgSignal.RemoveListener (OnMsg);//账号
		if(zhangHaoBaoHuPanel!=null)
			zhangHaoBaoHuPanel.msgSignal.RemoveListener (OnMsg);//账号
		if(dengLuPWDPanel!=null)
			dengLuPWDPanel.msgSignal.RemoveListener (OnMsg);//登录密码
		if(quKuanPWDPanel!=null)
			quKuanPWDPanel.msgSignal.RemoveListener (OnMsg);//取款密码
		if(touzhuRecordpanel!=null)
			touzhuRecordpanel.msgSignal.RemoveListener (OnMsg);
		if(qukuanRecordPanel!=null)
			qukuanRecordPanel.msgSignal.RemoveListener(OnMsg);
		if(chongzhiRecordpanel!=null)
			chongzhiRecordpanel.msgSignal.RemoveListener (OnMsg);
		if(yingkuiRecordPanel!=null)
			yingkuiRecordPanel.msgSignal.RemoveListener (OnMsg);
		if(youxiRecordPanel!=null)
			youxiRecordPanel.msgSignal.RemoveListener (OnMsg);
	}

	void OnMsg (MsgPara p)
	{
		msgSignal.Dispatch (p);
	}

	void OnTransferClick (GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		transferPanel.show (uinfoModel, go.name == "tolottery"?2:1);
	}

	void OnTopupClick (GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		NetworkManager.Instance.GetTopupInfo ();
	}

	public void OngetBankInfo (List<GetBankInfo> bankInfo)
	{
		chongzhiPanel.show (bankInfo,uinfoModel);
	}

	//提现 请求
	void OnTiXianClick (GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		tixianPanel.Show (userbaseInfo, uinfoModel);
	}

    //个人资料 请求
    void OnRenZiLiaoClick(GameObject go)
    {
		AudioController.Instance.SoundPlay("active_item");
        if (gerenziliaoPanel == null)
        {
			gerenziliaoPanel = LoadPanel("GeRenZiLIaoPanel").GetComponent<GeRenZiLiaoPanel>();
            gerenziliaoPanel.msgSignal.AddListener(OnMsg);
        }
		gerenziliaoPanel.Show (uinfoModel, userbaseInfo);
	}

	//个人资金 请求
	void OnRenZiJinClick (GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		GeRenZiJinPanel.startDate = TimeHelper.GetNowTime ();
		GeRenZiJinPanel.endDate = TimeHelper.GetNowTime ();
		NetworkManager.Instance.GetMyMoney (GeRenZiJinPanel.startDate, GeRenZiJinPanel.endDate);
	}

	//个人资金 返回
	public void OnGeRenZiJinBack (MSG_GP_USER_GETBACKWDZHJBXX para)
	{
		if (gerenzijinPanel == null)
		{
			gerenzijinPanel = LoadPanel("GeRenZiJinPanel").GetComponent<GeRenZiJinPanel>();
			gerenzijinPanel.msgSignal.AddListener(OnMsg);
		}
		gerenzijinPanel.Show (para);
	}

	//团队资金 请求
	void OnTuanDuiShuJuClick (GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		TuanDuiZiJinPanel.startDate = TimeHelper.GetNowTime ();
		TuanDuiZiJinPanel.endDate = TimeHelper.GetNowTime ();
		NetworkManager.Instance.GetTeamData (TuanDuiZiJinPanel.startDate, TuanDuiZiJinPanel.endDate);
	}

	//团队资金 返回
	public void OnTuanDuiZijinBack (MSG_GP_TEAM_GETBACKJBXX para)
	{
		if (tuanDuiZiJinPanel == null)
		{
			tuanDuiZiJinPanel = LoadPanel("TuanDuiZiJinPanel").GetComponent<TuanDuiZiJinPanel>();
			tuanDuiZiJinPanel.msgSignal.AddListener(OnMsg);
		}
		tuanDuiZiJinPanel.Show (para);
	}

	//提款账户 请求
	void OnTiKuanZhangHuClick (GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		if (tiKuanZhangHuPanel == null)
		{
			tiKuanZhangHuPanel = LoadPanel("TiKuanZhanghuPanel").GetComponent<TiKuanZhangHuPanel>();
			tiKuanZhangHuPanel.msgSignal.AddListener(OnMsg);
		}
		tiKuanZhangHuPanel.Show (uinfoModel, userbaseInfo);
	}

	//账户信息 请求
	void OnZhangHaoXinXiClick (GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		if (zhangHaoXinXiPanel == null)
		{
			zhangHaoXinXiPanel = LoadPanel("ZhangHaoXinXIPanel").GetComponent<ZhangHaoXinXiPanel>();
			zhangHaoXinXiPanel.msgSignal.AddListener(OnMsg);
		}
		zhangHaoXinXiPanel.Show (uinfoModel, userbaseInfo);
	}

	//账号保护 请求
	void OnZhangHaoBaoHuClick (GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		if (zhangHaoBaoHuPanel == null)
		{
			zhangHaoBaoHuPanel = LoadPanel("ZhanghaoBaoHuPanel").GetComponent<ZhangHaoBaoHuPanel>();
			zhangHaoBaoHuPanel.msgSignal.AddListener(OnMsg);
		}
		zhangHaoBaoHuPanel.Show (uinfoModel);
	}

	//登录密码 请求
	void OndengLuPWDClick (GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		if (dengLuPWDPanel == null)
		{
			dengLuPWDPanel = LoadPanel("DengLuMiMaPanel").GetComponent<DengLuPWDPanel>();
			dengLuPWDPanel.msgSignal.AddListener(OnMsg);
		}
		dengLuPWDPanel.Show ();
	}

	//取款密码 请求
	void OnquKuanPWDClick (GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		if (quKuanPWDPanel == null)
		{
			quKuanPWDPanel = LoadPanel("QuKuanMiMaPanel").GetComponent<QuKuanPWDPanel>();
			quKuanPWDPanel.msgSignal.AddListener(OnMsg);
		}
		quKuanPWDPanel.Show ();
	}

    public void OnQQMiMaQuKuanMiMaBack(MSG_GP_USER_ChangeUserPassWordResult para)
    {
		if (!string.IsNullOrEmpty (para.GetChData ())) {
			msgSignal.Dispatch (new MsgPara (para.GetChData (),2));
		}
		if ((int)para.byCangeType == 1) {
            if (para.GetBIsSuc())
            {
                //刷新登录密码
                string passMd5 = Util.GetMd5Hash(DengLuPWDPanel.currentPass);
                if (PlayerPrefs.GetInt("savePassword", 0) != 0)
                {
                    PlayerPrefs.SetString("Password", passMd5);
                }
            }
        }
	}

	void OnTouZhuJiLuClick (GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		//静态变量
		TouZhuRecordPanel.startDate = TimeHelper.GetNowTime ();
		TouZhuRecordPanel.endDate = TimeHelper.GetNowTime ();
		TouZhuRecordPanel.chName = @"";
		RequestRecord (TouZhuRecordPanel.byRord);
	}

	void QuKuanJiLuClick(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		QuKuanRecordbkPanel.startDate = TimeHelper.GetNowTime ();
		QuKuanRecordbkPanel.endDate = TimeHelper.GetNowTime ();
		QuKuanRecordbkPanel.chName = "";
		RequestRecord (QuKuanRecordbkPanel.byRord);
	}

	void OnChongZhiJiLuClick (GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		ChongZhiRecordPanel.startDate = TimeHelper.GetNowTime ();
		ChongZhiRecordPanel.endDate = TimeHelper.GetNowTime ();
		ChongZhiRecordPanel.chName = @"";
		RequestRecord (ChongZhiRecordPanel.byRord);
	}

	void OnYingKuiJiLuClick (GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		//静态变量
		YingKuiRecordPanel.startDate = TimeHelper.GetNowTime ();
		YingKuiRecordPanel.endDate = TimeHelper.GetNowTime ();
		YingKuiRecordPanel.chName = @"";
		RequestRecord (YingKuiRecordPanel.byRord);
	}

	void OnYouXiJiluClick (GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		YouXiRecordPanel.startDate = TimeHelper.GetNowTime ();
		YouXiRecordPanel.endDate = TimeHelper.GetNowTime ();
		YouXiRecordPanel.chName = "";
		RequestRecord (YouXiRecordPanel.byRord);
	}

	void RequestRecord (byte id, string ch = @"")
	{
		//子标识暂用1
		NetworkManager.Instance.LookupRecord (id, 1, id, 1, ch, TimeHelper.GetNowTime (), TimeHelper.GetNowTime ());
	}

	public void OnRecordBack (RecordBackObj obj)
	{
		byte mainId = obj.byMainType;
		switch (mainId) {
		case TouZhuRecordPanel.byRord:
			if (touzhuRecordpanel == null)
			{
				touzhuRecordpanel = LoadPanel("TouZhuRecordPanel").GetComponent<TouZhuRecordPanel>();
				touzhuRecordpanel.msgSignal.AddListener(OnMsg);
			}
			touzhuRecordpanel.show (obj);
			break;
		case QuKuanRecordbkPanel.byRord:
			if (qukuanRecordPanel == null)
			{
				qukuanRecordPanel = LoadPanel("QuKuanRecordPanel").GetComponent<QuKuanRecordbkPanel>();
				qukuanRecordPanel.msgSignal.AddListener(OnMsg);
			}
			qukuanRecordPanel.show (obj);
			break;
		case ChongZhiRecordPanel.byRord:
			if (chongzhiRecordpanel == null)
			{
				chongzhiRecordpanel = LoadPanel("ChongZhiRecordPanel").GetComponent<ChongZhiRecordPanel>();
				chongzhiRecordpanel.msgSignal.AddListener(OnMsg);
			}
			chongzhiRecordpanel.show (obj);
			break;
		case YingKuiRecordPanel.byRord:
			if (yingkuiRecordPanel == null)
			{
				yingkuiRecordPanel = LoadPanel("YingKuiRecordPanel").GetComponent<YingKuiRecordPanel>();
				yingkuiRecordPanel.msgSignal.AddListener(OnMsg);
			}
			yingkuiRecordPanel.show (obj);
			break;
		case YouXiRecordPanel.byRord:
			if (youxiRecordPanel == null)
			{
				youxiRecordPanel = LoadPanel("YouXiRecordPanel").GetComponent<YouXiRecordPanel>();
				youxiRecordPanel.msgSignal.AddListener(OnMsg);
			}
			youxiRecordPanel.show (obj);
			break;
		}
	}

	public void OpenXiangxi (TouzhuXiangxi obj)
	{
		if (touizhuxiangqingPanel == null)
		{
			touizhuxiangqingPanel = LoadPanel("TouZhuXiangqingPanel").GetComponent<TouZhuXiangQingPanel>();
			touizhuxiangqingPanel.msgSignal.AddListener(OnMsg);
		}
		touizhuxiangqingPanel.Show (obj);
	}

    public void reFreshTouchuRecord()
    {
        RequestRecord(TouZhuRecordPanel.byRord);
    }

    void OnLogOut (GameObject go)
	{
		LogoutSignal.Dispatch ();
	}


    GameObject LoadPanel(string name)
    {
        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("subPanel/"+name);
        GameObject panel = Instantiate(asset) as GameObject;
        panel.transform.parent = panelRoot;
        panel.transform.localScale = Vector3.one;

        return panel;
    }
}
