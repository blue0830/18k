using System;
using UnityEngine;

//游戏上庄成功回包
public class BaccaratShangZhuang_Handler : IHandler<ShangZhuang>
{
	public void OnReceive(NetMessageHead head, ShangZhuang para)
	{
		EventMgr.ins.DispEvent("180_133",new EventMgr.NetMsg(head,para));
	}
}