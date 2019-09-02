/// Example mediator
/// =====================
/// Note how we no longer extend EventMediator, and inject Signals instead

using System;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;


//Not extending EventMediator anymore
public class ActivityMediator : Mediator
{
	[Inject]
	public LogOutSignal logoutSignal { get; set; }

    [Inject]
    public ActivityView view { get; set; }

    [Inject]
    public MsgSignal MsgSignal { get; set; }

	[Inject]
	public ChongZhiSongSignal CZSSignal { get; set; }

    [Inject]
    public SevenDaySignal SSignal { get; set; }

	[Inject]
	public TuiGuangSignal TGSignal { get; set; }


    public override void OnRegister()
    {
        view.init();
		logoutSignal.AddListener (OnLogOut);
        SSignal.AddListener(OnSevenDaySignal);
		CZSSignal.AddListener(OnChongZhiSongSignal);
		TGSignal.AddListener(OnTuiGuangSignal);
    }

    public override void OnRemove()
    {
        SSignal.RemoveListener(OnSevenDaySignal);
		CZSSignal.RemoveListener(OnChongZhiSongSignal);
		TGSignal.RemoveListener(OnTuiGuangSignal);
		logoutSignal.RemoveListener (OnLogOut);
        Debug.Log("LoginView Mediator OnRemove");
    }

    void OnSevenDaySignal(MSG_GP_USER_HDZX7TLRULERESULT para)
    {
        view.OpenSevenDay(para);
    }

	void OnChongZhiSongSignal(MSG_GP_USER_HDZXXRCZSRULET para)
	{
		view.OpenChongZhiSongPanel(para);
	}

	void OnTuiGuangSignal(MSG_GP_USER_HDZXYJTGJLRUSULT para)
	{
		view.OpenTuiGuangPanel(para);
	}

	void OnLogOut(){
		view.close ();
	}
}