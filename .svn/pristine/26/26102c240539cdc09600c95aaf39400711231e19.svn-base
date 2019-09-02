using System;
using UnityEngine;
using System.Collections.Generic;

//游戏玩家起身信息
public class UserUp_Succ_Handler : IHandler<MSG_GR_R_UserSit>
{
	public void OnReceive(NetMessageHead head, MSG_GR_R_UserSit para)
	{
		if(head.bHandleCode==50)
			EventMgr.ins.DispEvent("102_1",new EventMgr.NetMsg(head,para));
	}
}