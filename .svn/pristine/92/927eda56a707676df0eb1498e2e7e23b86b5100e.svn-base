using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class MSG_GP_USER_GETPLAYPE_Handler : IHandler<MSG_GP_USER_GETPLAYPE>
{
    [Inject]
    public MsgSignal MsgSignal { get; set; }

    [Inject]
    public GetPeiESignal Signal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_USER_GETPLAYPE para)
    {
        if (para.iReturn != 0)
        {
            MsgSignal.Dispatch(new MsgPara(para.GetChResult(), 2));
        }
        else
        {
            Signal.Dispatch(para);
        }
    }
}

