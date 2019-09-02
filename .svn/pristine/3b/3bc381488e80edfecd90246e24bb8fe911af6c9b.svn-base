using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class ASS_GP_USER_GETLASTQISHURESULT_Handler : IHandler<MSG_GP_USER_GETLASTQISHURESULT>
{
    [Inject]
    public ILotteryModel model { get; set; }

    [Inject]
    public ZhuiqiListSignal ZHSignal { get; set; }

    //获取可以追的期数
    public void OnReceive(NetMessageHead head, MSG_GP_USER_GETLASTQISHURESULT para)
    {

        Debug.Log("ASS_GP_USER_GETLASTQISHURESULT_Handler assID" + head.bAssistantID  );

        if(para!=null)
            ZHSignal.Dispatch(para.GetQISHUList());


    }
}

