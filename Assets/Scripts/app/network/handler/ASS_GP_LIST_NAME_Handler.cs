using System;
using UnityEngine;

//游戏名称
public class ASS_GP_LIST_NAME_Handler : IHandler<ComNameInfo>
{
	[Inject]
	public IGameInfoModel model { get; set; }

	[Inject]
	public QpGameInfoSignal signal { get; set; }

	public void OnReceive(NetMessageHead head, ComNameInfo para)
	{
		model.AddQpGameInfo(para);
		if (head.bHandleCode == 10) {//数据接收完毕
			signal.Dispatch(model.QpGameInfos());
			model.ClearQpGameInfos();
		}
	}
}