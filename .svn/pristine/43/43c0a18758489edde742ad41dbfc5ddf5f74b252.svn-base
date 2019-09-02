using System;
using UnityEngine;

//心跳包
public class MDM_CONNECT_Handler : IHandler<MDM_CONNECT_OBJ>
{
	public void OnReceive(NetMessageHead head, MDM_CONNECT_OBJ para)
	{
		Global.LastAppHeartBeatTime = TimeHelper.GetNowTime ();
		NetworkManager.Instance.Heartbeat();
	}
}