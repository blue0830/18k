using System;
using UnityEngine;

//游戏经验值
public class BaccaraASS_SHOW_JS_Handler : IHandler<MSG_GR_R_UserPoint>
{
	public void OnReceive(NetMessageHead head, MSG_GR_R_UserPoint para)
	{
		if (head.bHandleCode == 0) {
			EventMgr.ins.DispEvent("103_6",new EventMgr.NetMsg(head,para));
		}
	}
}