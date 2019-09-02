using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class ASS_GP_BJPK10LASTOPENNUMRESULT_Handler : IHandler<MSG_GP_PK10_GETCUE>
{
    [Inject]
    public ILotteryModel model { get; set; }

    [Inject]
    public LotterySignal lSignal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_PK10_GETCUE para)
    {
        if (model.IscurLottery((int)head.bAssistantID))
        {
            model.SetQishuTime(para);
            lSignal.Dispatch();
        }
    }
}

