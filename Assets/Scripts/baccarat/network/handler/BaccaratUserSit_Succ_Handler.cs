using System;
using UnityEngine;

//游戏坐下成功
public class BaccaratUserSit_Succ_Handler : IHandler<MSG_GR_R_UserSit>
{
	public void OnReceive(NetMessageHead head, MSG_GR_R_UserSit para)
	{
		if (head.bHandleCode == 50) {//坐下成功
			EventMgr.ins.DispEvent("102_2",new EventMgr.NetMsg(head,para));
		}
	}
}