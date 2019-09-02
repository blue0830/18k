/// Example mediator
/// =====================
/// Note how we no longer extend EventMediator, and inject Signals instead

using System.Collections.Generic;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;


//Not extending EventMediator anymore
public class MainMediator : Mediator
{
    [Inject]
    public MainView view { get; set; }

    [Inject]
    public LoginSignal loginSignal { get; set; }

    [Inject]
    public MsgSignal MsgSignal { get; set; }

    [Inject]
    public IConfigModel CfgModel { get; set; }

    [Inject]
    public GoSelectViewSignal GoSelectSignal { get; set; }


    [Inject]
    public GongGaoSignal GSignal { get; set; }


	[Inject]
	public LogOutSignal LogoutSignal { get; set; }

	[Inject]
	public QpGameInfoSignal qpGameInfoSignal { get; set; }

	[Inject]
	public QpRoomInfoSignal qpRoomInfoSignal { get; set; }

    public override void OnRegister()
    {
        view.init();
        view.InitIcons(CfgModel.GetLotteryCfg());
        loginSignal.AddListener(OnLoginSignal);
        GoSelectSignal.AddListener(OnBuySignal);
        GSignal.AddListener(OnGSignal);
		LogoutSignal.AddListener (OnLogout);
		qpGameInfoSignal.AddListener (OnGameReturn);
		qpRoomInfoSignal.AddListener (OnRoomReturn);
    }

    public override void OnRemove()
    {
        loginSignal.RemoveListener(OnLoginSignal);
        GoSelectSignal.RemoveListener(OnBuySignal);
        GSignal.RemoveListener(OnGSignal);
		LogoutSignal.RemoveListener (OnLogout);
		qpGameInfoSignal.RemoveListener (OnGameReturn);
		qpRoomInfoSignal.RemoveListener (OnRoomReturn);
    }

	void OnGameReturn(List<ComNameInfo> comNameInfo)
	{
		view.UpdateGameList (comNameInfo);
	}

	void OnRoomReturn(List<ComRoomInfo> comRoomInfo)
	{
		view.OnRoomInfo(comRoomInfo);
	}

    void OnBuySignal(int para)
    {
        view.OpenSelectView(para);
    }

    void OnGSignal(int seq ,string content)
    {
        view.ProcessGG(seq, content);
    }


    void OnLoginSignal(int para)
    {
        if (para == -1)
        {
            NetworkManager.Instance.GetGongGao();
        }
    }


	void OnLogout()
	{
		
	}
}

