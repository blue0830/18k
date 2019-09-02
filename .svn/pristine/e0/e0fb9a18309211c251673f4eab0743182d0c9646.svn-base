using System;
using UnityEngine;
using System.Collections.Generic;

//游戏玩家信息
public class BaccaratUserInfoStruct_Handler : IHandler<UserInfoStruct[]>
{
	[Inject]
	public IBaccaratInfoModel model { get; set; }

	public void OnReceive(NetMessageHead head, UserInfoStruct[] para)
	{
		if (para != null) {
			foreach(UserInfoStruct user in para){
				model.AddOnLineUserInfo(user);
			}
		}
		if(head.bHandleCode==12){//发送完毕
			EventMgr.ins.DispEvent("101_1",new EventMgr.NetMsg(head,model.OnLineUserInfos()));
			model.ClearOnLineUserInfos ();
		}
	}
}