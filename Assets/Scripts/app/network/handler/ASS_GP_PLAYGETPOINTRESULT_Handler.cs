using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class ASS_GP_PLAYGETPOINTRESULT_Handler : IHandler<SScPlayGetPointResult>
{
    [Inject]
    public ILotteryModel model { get; set; }

    [Inject]
    public GetPointSignal GetpSignal { get; set; }

    public void OnReceive(NetMessageHead head, SScPlayGetPointResult para)
    {

        //Debug.Log("MDM_GP_SSC_RESULT_Handler iOrderTm: " + para.iOrderTm);
        //Debug.Log("MDM_GP_SSC_RESULT_Handler iWaitTm: " + para.iWaitTm);
  
        if (head.bAssistantID == 4) //获取返点
        {


            model.SetAwardinfo(para);

            GetpSignal.Dispatch();


        }

    }
}

