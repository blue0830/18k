using System;

//SQL异常
public class MDM_SQL_MAINERROR_Handler : IHandler<MSG_GP_S_SQL_Error>
{
	[Inject]
	public MsgSignal MsgSignal { get; set; }

	public void OnReceive(NetMessageHead head, MSG_GP_S_SQL_Error para)
	{
		if (para.byErrorType == 1) {
			MsgSignal.Dispatch (new MsgPara ("抱歉,您操作的太频繁了,请休息一下", 2));
		} else {
			MsgSignal.Dispatch(new MsgPara("系统异常请稍后再试",2));
		}
		Loading.GetInstance().HideLoading();
	}
}