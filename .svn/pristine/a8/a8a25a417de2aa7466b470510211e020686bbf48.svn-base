using System;
using UnityEngine;
using System.Collections.Generic;

//掉线游戏玩家信息
public class NetCutUserInfoStruct_Handler : IHandler<UserInfoStruct[]>
{
	[Inject]
	public IBaccaratInfoModel model { get; set; }

	public void OnReceive(NetMessageHead head, UserInfoStruct[] para)
	{
		if (para != null) {
			foreach(UserInfoStruct user in para){
				model.AddOffLineUserInfo(user);
			}
		}
		//处理掉线房间玩家列表数据
		if(head.bHandleCode==12){//发送完毕
			EventMgr.ins.DispEvent("101_2",new EventMgr.NetMsg(head,model.OffLineUserInfos()));
			model.ClearOffLineUserInfos();
		}
	}
}