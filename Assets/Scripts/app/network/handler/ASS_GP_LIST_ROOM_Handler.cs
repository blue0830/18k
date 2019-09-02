using System;
using UnityEngine;

//游戏名称
public class ASS_GP_LIST_ROOM_Handler : IHandler<ComRoomInfo>
{
	[Inject]
	public IGameInfoModel model { get; set; }

	[Inject]
	public QpRoomInfoSignal signal { get; set; }

	public void OnReceive(NetMessageHead head, ComRoomInfo para)
	{
		model.AddQpRoomInfo(para);
		if (head.bHandleCode == 10) {//数据接收完毕
			signal.Dispatch(model.QpRoomInfos());
			model.ClearQpRoomInfos();
		}
	}
}