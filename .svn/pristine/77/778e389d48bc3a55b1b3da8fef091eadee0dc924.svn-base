using System;
using UnityEngine;

//游戏下注回包
public class BaccaratXiaZhu_Handler : IHandler<XiaZhu>
{
	public void OnReceive(NetMessageHead head, XiaZhu para)
	{
		EventMgr.ins.DispEvent("180_130",new EventMgr.NetMsg(head,para));
	}
}