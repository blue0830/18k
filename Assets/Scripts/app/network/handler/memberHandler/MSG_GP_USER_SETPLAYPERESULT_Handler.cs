using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class MSG_GP_USER_SETPLAYPERESULT_Handler : IHandler<MSG_GP_USER_SETPLAYPERESULT>
{
    [Inject]
    public MsgSignal MsgSignal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_USER_SETPLAYPERESULT para)
    {
        MsgSignal.Dispatch(new MsgPara(para.GetchResult(), 2));

        NetworkManager.Instance.FenPeiPeiE(0);
    }
}

