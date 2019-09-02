using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MSG_GP_USER_GetCZbankInfo_Handler : IHandler<MSG_GP_USER_GetCZbankInfo>
{
    [Inject]
    public IGameInfoModel model { get; set; }

    [Inject]
    public GetAllbankinfoSignal GSignal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_USER_GetCZbankInfo para)
    {
        GetBankInfo obj = new GetBankInfo();
        obj.BankAccount = para.GetBankAccount();
        obj.BankName = para.GetBankName();
        obj.BankUrl = para.GetBankUrl();
        obj.TrueName = para.GetTrueName();
        obj.iShowBankType = para.iShowBankType;
        obj.Remark = para.GetRemark();
        model.AddGetBankInfo(obj);
        if (para.isEnd == 1)
        {
            GSignal.Dispatch(model.GetBankInfos());
            model.ClearBankInfo();
        }
    }
}

