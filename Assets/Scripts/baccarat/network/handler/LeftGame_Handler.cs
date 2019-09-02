using System;
using UnityEngine;
using System.Collections.Generic;

//离开游戏玩家信息
public class LeftGame_Handler : IHandler<MSG_GR_R_UserLeft>
{
	public void OnReceive(NetMessageHead head, MSG_GR_R_UserLeft para)
	{
		EventMgr.ins.DispEvent("102_6",new EventMgr.NetMsg(head,para));
	}
}