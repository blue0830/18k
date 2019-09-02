using System;
using UnityEngine;

//游戏登录失败回包
public class BaccaratLoginError_Handler : IHandler<Empty>
{
	[Inject]
	public MsgSignal MsgSignal { get; set; }

	public void OnReceive(NetMessageHead head, Empty para)
	{
		EventMgr.ins.DispEvent("100_3",new EventMgr.NetMsg(head,para));
	}
}