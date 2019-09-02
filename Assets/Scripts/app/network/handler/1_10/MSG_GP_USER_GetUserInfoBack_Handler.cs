using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class MSG_GP_USER_GetUserInfoBack_Handler : IHandler<MSG_GP_USER_GetUserInfoBack>
{
    [Inject]
    public IGameInfoModel model { get; set; }

    [Inject]
    public GetUserinfoSignal GSignal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_USER_GetUserInfoBack para)
    {
        model.userinfo = para;
        GSignal.Dispatch();
    }
}

