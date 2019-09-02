using System;
using UnityEngine;

//游戏结算回包
public class BaccaratJieSuan_Handler : IHandler<JieSuan>
{
	public void OnReceive(NetMessageHead head, JieSuan para)
	{
		EventMgr.ins.DispEvent("180_136",new EventMgr.NetMsg(head,para));
	}
}