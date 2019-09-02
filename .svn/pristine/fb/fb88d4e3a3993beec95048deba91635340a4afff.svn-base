using System;
using UnityEngine;

//游戏连接成功
public class BaccaratConnSucc_Handler : IHandler<GameConnSuccess>
{
	public void OnReceive(NetMessageHead head, GameConnSuccess para)
	{
		EventMgr.ins.DispEvent("1_3",new EventMgr.NetMsg(head,para));
	}
}