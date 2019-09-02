using System;
using UnityEngine;

//游戏场景
public class BaccaratTagChongLian_Handler : IHandler<ChongLian>
{
	public void OnReceive(NetMessageHead head, ChongLian para)
	{
		EventMgr.ins.DispEvent("150_2",new EventMgr.NetMsg(head,para));
	}
}