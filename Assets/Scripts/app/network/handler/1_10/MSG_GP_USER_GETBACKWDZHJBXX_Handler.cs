using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class MSG_GP_USER_GETBACKWDZHJBXX_Handler : IHandler<MSG_GP_USER_GETBACKWDZHJBXX>
{
    [Inject]
    public IGameInfoModel model { get; set; }

    [Inject]
    public GeRenzijiSignal Signal { get; set; }

    //个人资金
    public void OnReceive(NetMessageHead head, MSG_GP_USER_GETBACKWDZHJBXX para)
    {

        Signal.Dispatch(para);

    }
}

