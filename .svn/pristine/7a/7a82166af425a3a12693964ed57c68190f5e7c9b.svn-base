using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class MSG_GP_GongGao_Handler : IHandler<MSG_GP_GongGao>
{
    [Inject]
    public IGameInfoModel model { get; set; }

    [Inject]
    public GongGaoSignal GSignal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_GongGao para)
    {
		//处理换行的问题
		GSignal.Dispatch((int)para.byCueGg, para.GetchGg().Trim().Replace("\r\n", ""));
    }
}

