using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class MSG_GP_TEAM_GETBACKJBXX_Handler : IHandler<MSG_GP_TEAM_GETBACKJBXX>
{
    [Inject]
    public IGameInfoModel model { get; set; }

    [Inject]
    public TuanduizijiSignal Signal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_TEAM_GETBACKJBXX para)
    {


        Signal.Dispatch(para);

    }
}

