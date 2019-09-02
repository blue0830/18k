using System;
using UnityEngine;

//游戏心跳
public class Baccarat_MDM_CONNECT_Handler : IHandler<Empty>
{
	public void OnReceive(NetMessageHead head, Empty para)
	{
		Global.LastGameHeartBeatTime = TimeHelper.GetNowTime ();
		GameNetworkManager.Instance.Heartbeat();
	}
}