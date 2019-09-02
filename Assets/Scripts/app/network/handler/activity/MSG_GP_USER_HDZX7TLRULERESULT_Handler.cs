using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class MSG_GP_USER_HDZX7TLRULERESULT_Handler : IHandler<MSG_GP_USER_HDZX7TLRULERESULT>
{

    [Inject]
    public SevenDaySignal SSignal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_USER_HDZX7TLRULERESULT para)
    {
        SSignal.Dispatch(para);
    }
}

