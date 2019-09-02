using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class MSG_GP_USER_GETPLAYMAXPOINTRESULT_Handler : IHandler<MSG_GP_USER_GETPLAYMAXPOINTRESULT>
{
    [Inject]
    public ILotteryModel model { get; set; }

    [Inject]
    public AddMemberInfoSignal lSignal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_USER_GETPLAYMAXPOINTRESULT para)
    {
        lSignal.Dispatch(para);
    }
}

