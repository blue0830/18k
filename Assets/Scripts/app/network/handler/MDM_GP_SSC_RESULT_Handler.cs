using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class MDM_GP_SSC_RESULT_Handler : IHandler<MSG_GP_SSC_GETCUE>
{
    [Inject]
    public ILotteryModel model { get; set; }

    [Inject]
    public LotterySignal lSignal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_SSC_GETCUE para)
    {
        if (model.IscurLottery((int)head.bAssistantID))
        {
            model.SetQishuTime(para);
            lSignal.Dispatch();
        }
    }
}

