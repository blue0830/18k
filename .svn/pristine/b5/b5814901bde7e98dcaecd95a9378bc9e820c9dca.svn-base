using System;
using UnityEngine;

//游戏开始回包
public class BaccaratTagBeginData_Handler : IHandler<BeginData>
{
	public void OnReceive(NetMessageHead head, BeginData para)
	{
		EventMgr.ins.DispEvent("180_134",new EventMgr.NetMsg(head,para));
	}
}