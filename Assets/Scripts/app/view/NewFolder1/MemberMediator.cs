/// Example mediator
/// =====================
/// Note how we no longer extend EventMediator, and inject Signals instead

using System;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;


//Not extending EventMediator anymore
public class MemberMediator : Mediator
{
	[Inject]
	public LogOutSignal logoutSignal { get; set; }
    [Inject]
    public MemberView view { get; set; }

    [Inject]
    public MsgSignal MsgSignal { get; set; }

    [Inject]
    public IGameInfoModel GameModel { get; set; } //到这个界面请求的用户基本信息

    [Inject]
    public AddMemberInfoSignal AddMemberInfoSignal { get; set; }
    [Inject]
    public RecordBackSignal recordBackSignal { get; set; } //记录查询返回信号 


    [Inject]
    public TouzhuXiangxiSignal touzhuxiangxiSignal { get; set; }

    [Inject]
    public GetPeiESignal GetPeiESignal { get; set; }

    public override void OnRegister()
    {
        ////Listen to the view for a Signal
        //view.clickSignal.AddListener(onViewClicked);

        view.init();

        AddMemberInfoSignal.AddListener(OnAddMemberInfo);
        touzhuxiangxiSignal.AddListener(OnTouzhuxiangxi);
        recordBackSignal.AddListener(OnRecordBack);
        GetPeiESignal.AddListener(OnGetPeie);
		logoutSignal.AddListener (OnLogOut);
    }

    public override void OnRemove()
    {
        AddMemberInfoSignal.RemoveListener(OnAddMemberInfo);
        touzhuxiangxiSignal.RemoveListener(OnTouzhuxiangxi);
        recordBackSignal.RemoveListener(OnRecordBack);
        GetPeiESignal.RemoveListener(OnGetPeie);
		logoutSignal.RemoveListener (OnLogOut);
        Debug.Log("LoginView Mediator OnRemove");
    }

    void OnAddMemberInfo(MSG_GP_USER_GETPLAYMAXPOINTRESULT result)
    {
        view.OnAddMemberInfo(result);
    }

    void OnTouzhuxiangxi()
    {
        view.OnTouzhuxiangxi(GameModel.xiangxiObj);

    }

    void OnRecordBack(RecordBackObj obj)
    {
        view.OnRecordBack(obj);
    }

    void OnGetPeie(MSG_GP_USER_GETPLAYPE para)
    {
        view.OnGetPeie(para);
    }

	void OnLogOut(){
		view.close ();
	}
}

