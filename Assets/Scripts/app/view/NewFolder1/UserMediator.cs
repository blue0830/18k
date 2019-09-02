/// Example mediator
/// =====================
/// Note how we no longer extend EventMediator, and inject Signals instead

using System;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using System.Collections.Generic;

//Not extending EventMediator anymore
public class UserMediator : Mediator
{
	[Inject]
	public LogOutSignal logoutSignal { get; set; }
    [Inject]
    public UserView view { get; set; }

    [Inject]
    public LoginSignal loginSignal { get; set; }

    [Inject]
    public MsgSignal MsgSignal { get; set; }

    [Inject]
	public IUInfoModel UInfoModel { get; set; }//登录 信号

    [Inject]
    public IGameInfoModel GameModel { get; set; } //到这个界面请求的用户基本信息

    [Inject]
	public GetUserinfoSignal udinfoSignal { get; set; }  //用户中心 信号

    [Inject]
    public GeRenzijiSignal GerenzijinSignal { get; set; }//个人资金 信号

	[Inject]
	public TuanduizijiSignal  TuanduizijiSignal{ get; set; }//团队资金 信号

    [Inject]
	public RefreshMoneySignal RefreshMoneySignal { get; set; }//刷新金额 信号

	[Inject]
	public TransferSignal TransferSignal { get; set; }//刷新转金额 信号

	[Inject]
	public QQMiMaQuKuanMiMaSignal qqMiMaQuKuanMiMaSignal { get; set; }//修改QQ&昵称/密码/取款密码 信号


    [Inject]
    public RecordBackSignal recordBackSignal { get; set; } //记录查询返回信号 

    [Inject]
    public TouzhuXiangxiSignal touzhuxiangxiSignal { get; set; }


    [Inject]
    public GetAllbankinfoSignal AllbankInfoSignal { get; set; }


    [Inject]
    public CheDanSuccessSignal chedanSuccessSignal { get; set; } //撤单成功

    public override void OnRegister()
    {
        view.init();
        
        ////Listen to the view for a Signal
        //view.clickSignal.AddListener(onViewClicked);

		//添加 监听器
		TransferSignal.AddListener(OnTransferMoney);//转金额 监听器
		RefreshMoneySignal.AddListener(OnRefreshMoney);//刷新金额 监听器
		udinfoSignal.AddListener(OnuserBaseinfo);//用户中心 监听器
        GerenzijinSignal.AddListener(OnRecGerenZijin);//个人资金 监听器
		TuanduizijiSignal.AddListener(OnRecTuanDuiZijin);//团队资金 监听器
		qqMiMaQuKuanMiMaSignal.AddListener(OnRecQQMiMaQuKuanMiMa);//团队资金 监听器

        chedanSuccessSignal.AddListener(OnChedansuccess);

        recordBackSignal.AddListener(OnRecordBack);
        touzhuxiangxiSignal.AddListener(OnTouzhuxiangxi);
        AllbankInfoSignal.AddListener(OnAllBankInfo);
		logoutSignal.AddListener (OnLogOut);
        view.FillContent(UInfoModel);

		//进入用户中心界面必须先请求一次用户中心接口，并保存返回信息到全局
        NetworkManager.Instance.GetUserData();
    }

    public override void OnRemove()
    {
		//移出 监听器
		TransferSignal.RemoveListener(OnTransferMoney);//转金额 监听器
		RefreshMoneySignal.RemoveListener(OnRefreshMoney);//移出 刷新金额 监听器
		udinfoSignal.RemoveListener(OnuserBaseinfo);//移出 用户中心 监听器
		GerenzijinSignal.RemoveListener(OnRecGerenZijin);//移出 个人资金 监听器
		TuanduizijiSignal.RemoveListener(OnRecTuanDuiZijin);//移出 团队资金 监听器
        chedanSuccessSignal.RemoveListener(OnChedansuccess);
        recordBackSignal.RemoveListener(OnRecordBack);
        touzhuxiangxiSignal.RemoveListener(OnTouzhuxiangxi);
        AllbankInfoSignal.RemoveListener(OnAllBankInfo);
		qqMiMaQuKuanMiMaSignal.RemoveListener(OnRecQQMiMaQuKuanMiMa);//团队资金 监听器
		logoutSignal.RemoveListener (OnLogOut);
        Debug.Log("LoginView Mediator OnRemove");
    }


    void OnRefreshMoney()
    {
        view.FillContent(UInfoModel);
    }

	void OnTransferMoney()
	{
		view.TransferRefresh(UInfoModel);
	}

    void OnuserBaseinfo()
    {
        view.OnRecieveUserbaseinfo(GameModel.userinfo);
    }

    void OnRecGerenZijin(MSG_GP_USER_GETBACKWDZHJBXX para)
    {
        view.OnGerenZijin(para);
    }

	void OnRecTuanDuiZijin(MSG_GP_TEAM_GETBACKJBXX para)
	{
		view.OnTuanDuiZijin(para);
	}

	void OnRecQQMiMaQuKuanMiMa(MSG_GP_USER_ChangeUserPassWordResult para)
	{
		view.OnQQMiMaQuKuanMiMa(para);
	}

    void OnRecordBack(RecordBackObj obj)
    {
        view.OnRecordBack(obj);

    }

    void OnTouzhuxiangxi()
    {
        view.OnTouzhuxiangxi(GameModel.xiangxiObj);

    }

    void OnAllBankInfo(List<GetBankInfo> baninfos)
    {
        view.OnAllBankInfo(baninfos);
    }

    void OnChedansuccess()
    {
        view.reFreshTouchuRecord();
    }

	void OnLogOut(){
		view.close ();
	}
}

