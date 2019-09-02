using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MSG_GP_USER_SHUAXINRESULT_Handler : IHandler<MSG_GP_USER_SHUAXINRESULT>
{
    [Inject]
    public IUInfoModel umodel { get; set; }

    [Inject]
    public RefreshMoneySignal rSignal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_USER_SHUAXINRESULT para)
    {
        umodel.SetGold(para.iWalletMoney);
        umodel.SetMoney(para.iMoney);
		Global.user = umodel.GetUserinfo();
        rSignal.Dispatch();
    }
}

