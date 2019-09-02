using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class MSG_GP_USER_HDZXXRCZSRULET_Handler : IHandler<MSG_GP_USER_HDZXXRCZSRULET>
{	
	[Inject]
	public ChongZhiSongSignal signal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_USER_HDZXXRCZSRULET para)
    {
		signal.Dispatch (para);
    }
}

