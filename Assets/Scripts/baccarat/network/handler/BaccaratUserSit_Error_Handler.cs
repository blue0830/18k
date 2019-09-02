using System;
using UnityEngine;

//游戏登录回包
public class BaccaratUserSit_Error_Handler : IHandler<Empty>
{
	public void OnReceive(NetMessageHead head, Empty para)
	{
		EventMgr.ins.DispEvent("102_8",new EventMgr.NetMsg(head,para));
	}
}