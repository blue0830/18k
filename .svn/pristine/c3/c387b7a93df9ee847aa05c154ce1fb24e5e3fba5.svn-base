using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class MSG_GP_USER_HDZXXRCZZSLQHBRESULT_Handler : IHandler<MSG_GP_USER_HDZXXRCZZSLQHBRESULT>
{
	[Inject]
	public MsgSignal MsgSignal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_USER_HDZXXRCZZSLQHBRESULT para)
    {
        if(para.iReturn>1)
        {
            NetworkManager.Instance.RefreshMoney();
        }
		MsgSignal.Dispatch(new MsgPara(para.GetChResult(), 2));
    }
}

