/// Example mediator
/// =====================
/// Note how we no longer extend EventMediator, and inject Signals instead

using System;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;


//Not extending EventMediator anymore
public class RecordMediator : Mediator
{
	[Inject]
	public LogOutSignal logoutSignal { get; set; }
    [Inject]
    public RecordView view { get; set; }

    [Inject]
    public IConfigModel CfgModel { get; set; }

    [Inject]
    public MsgSignal MsgSignal { get; set; }

    [Inject]
    public LoRecordSignal lrSignal { get; set; }

    [Inject]
    public GoSelectViewSignal GoSelectSignal { get; set; }
    

    public override void OnRegister()
    {
        view.init();
        view.InitItems(CfgModel.GetLotteryCfg());
        view.buySignal.AddListener(OnBuySignal);
        lrSignal.AddListener(OnLoRecordSignal);
		logoutSignal.AddListener (OnLogOut);
    }

    public override void OnRemove()
    {
        view.buySignal.RemoveListener(OnBuySignal);
        lrSignal.RemoveListener(OnLoRecordSignal);
		logoutSignal.RemoveListener (OnLogOut);
    }

    void OnBuySignal(int para)
    {
        GoSelectSignal.Dispatch(para);
    }

    void OnLoRecordSignal(RecordObj obj)
    {
        view.UpdateContent(obj);
    }

	void OnLogOut(){
		view.close ();
	}
}

