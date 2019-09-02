using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class MSG_GP_USER_PLAYJINERZH_Handler : IHandler<MSG_GP_USER_PLAYJINERZH>
{
    [Inject]
    public IUInfoModel model { get; set; }

    [Inject]
    public RefreshMoneySignal Signal { get; set; }

	[Inject]
	public TransferSignal TransferSignal { get; set; }

    [Inject]
    public MsgSignal MsgSignal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_USER_PLAYJINERZH para)
    {
		if (para.iReturn==0) {//成功
			model.SetGold(para.iqpmoney);
			model.SetMoney(para.icpmoney);
			Signal.Dispatch();
			TransferSignal.Dispatch();
		}
		MsgSignal.Dispatch(new MsgPara(para.GetChResult(),2));
    }
}

