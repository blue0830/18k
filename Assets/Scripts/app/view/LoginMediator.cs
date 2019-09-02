/// Example mediator
/// =====================
/// Note how we no longer extend EventMediator, and inject Signals instead

using System;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;


//Not extending EventMediator anymore
public class LoginMediator : Mediator
{
    [Inject]
    public LoginView view { get; set; }

    [Inject]
    public LoginSignal loginSignal { get; set; }

    [Inject]
    public MsgSignal MsgSignal { get; set; }

	[Inject]
	public LogOutSignal LogoutSignal { get; set; }


    public override void OnRegister()
    {
        loginSignal.AddListener(OnLoginSignal);
		LogoutSignal.AddListener (OnLogOut);
        view.init();
    }

    public override void OnRemove()
    {
        loginSignal.RemoveListener(OnLoginSignal);
		LogoutSignal.RemoveListener (OnLogOut);
    }

    void OnLoginSignal(int para)
    {
        if (para == -1)
        {
            view.close();
        }
        else if (para == 2)
        {
			MsgSignal.Dispatch(new MsgPara("用户不存在",2));
        }
        else if (para == 3)
        {
			MsgSignal.Dispatch(new MsgPara("用户不存在或者密码错误",2));
        }
        else if (para == 4)
        {
			MsgSignal.Dispatch(new MsgPara("此账号被禁止登录",2));
        }
        else if (para == 5)
        {
			MsgSignal.Dispatch(new MsgPara("您所在IP地址被禁止登录",2));
        }
        else if (para == 11)
        {
			Global.LastAppHeartBeatTime = 0;//防止网络检查与此处冲突
			MsgSignal.Dispatch(new MsgPara("此账号已经在别处登录",2));
            LogoutSignal.Dispatch();
        }
        else if (para == 20)
        {
			MsgSignal.Dispatch(new MsgPara("帐号已经在其他机器上锁机",2));
        }
        else
        {
			MsgSignal.Dispatch(new MsgPara("数据异常",2));
        }
    }

	void OnLogOut()
	{
		NetworkManager.Instance.LogOut ();
		view.Show ();
	}
}

