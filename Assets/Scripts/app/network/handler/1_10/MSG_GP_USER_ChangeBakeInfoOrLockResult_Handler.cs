using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class MSG_GP_USER_ChangeBakeInfoOrLockResult_Handler : IHandler<MSG_GP_USER_ChangeBakeInfoOrLockResult>
{
    [Inject]
    public IGameInfoModel model { get; set; }

    [Inject]
    public GongGaoSignal GSignal { get; set; }

	[Inject]
	public MsgSignal MsgSignal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_USER_ChangeBakeInfoOrLockResult para)
    {
		if (!string.IsNullOrEmpty (para.GetChData())) {
			MsgSignal.Dispatch(new MsgPara(para.GetChData(),2));
		}
		if (para.bIsSuc==1) {
			//刷新用户信息
			NetworkManager.Instance.GetUserData ();
		}
    }
}

