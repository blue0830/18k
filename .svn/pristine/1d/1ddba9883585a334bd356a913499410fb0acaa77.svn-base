using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//登录成功
public class MDM_GP_LOGON_Handler : IHandler<MSG_GP_R_LogonResult>
{
    [Inject]
    public IUInfoModel model { get; set; }

	[Inject]
	public IGameInfoModel gameModel { get; set; }

    [Inject]
    public LoginSignal loginSignal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_R_LogonResult para)
    {
		Loading.GetInstance().HideLoading ();
		TimeManager.Instance().UnRegister("checklogin");
		TimeManager.Instance().UnRegister("checkloginClose");
        if (head.bAssistantID == 5) //登录成功
        {
            model.SetUserinfo(para);
			Global.CurrentUserId = (uint)para.dwUserID;
			Global.CurrentUserPoint = para.GetPoint();
			Global.user = model.GetUserinfo();
            loginSignal.Dispatch(-1);
			Global.IsLoginApp = true;
            NetworkManager.Instance.LookupVersion();
			NetworkManager.Instance.GetGameTypes();//重新获取QP游戏信息
        }else
        {
            NetworkManager.Instance.LogOut();
            loginSignal.Dispatch((int)head.bHandleCode);
        }
    }
}

