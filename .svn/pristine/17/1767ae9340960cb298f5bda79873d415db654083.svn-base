using System;
using UnityEngine;
using System.Collections.Generic;

//进入游戏玩家信息
public class EnterGameRoom_Handler : IHandler<UserInfoStruct>
{
	public void OnReceive(NetMessageHead head, UserInfoStruct para)
	{
		EventMgr.ins.DispEvent("102_5",new EventMgr.NetMsg(head,para));
	}
}