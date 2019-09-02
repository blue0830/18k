using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class MSG_GP_USER_HDZXYJTGJLRUSULT_Handler : IHandler<MSG_GP_USER_HDZXYJTGJLRUSULT>
{
	
	[Inject]
	public TuiGuangSignal SSignal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_USER_HDZXYJTGJLRUSULT para)
    {
		SSignal.Dispatch (para);
    }
}

