using System;
using UnityEngine;

//游戏登录成功
public class BaccaratLoginSucc_Handler : IHandler<Empty>
{
	public void OnReceive(NetMessageHead head, Empty para)
	{
		EventMgr.ins.DispEvent("100_4",new EventMgr.NetMsg(head,para));
	}
}