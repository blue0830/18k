using System;
using UnityEngine;

//游戏状态
public class BaccaratMSG_GM_S_GameInfo_Handler : IHandler<MSG_GM_S_GameInfo>
{
	public void OnReceive(NetMessageHead head, MSG_GM_S_GameInfo para)
	{
		EventMgr.ins.DispEvent("150_1",new EventMgr.NetMsg(head,para));
	}
}