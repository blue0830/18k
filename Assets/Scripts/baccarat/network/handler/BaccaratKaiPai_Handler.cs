using System;
using UnityEngine;

//游戏开牌回包
public class BaccaratKaiPai_Handler : IHandler<KaiPai>
{
	public void OnReceive(NetMessageHead head, KaiPai para)
	{
		EventMgr.ins.DispEvent("180_131",new EventMgr.NetMsg(head,para));
	}
}