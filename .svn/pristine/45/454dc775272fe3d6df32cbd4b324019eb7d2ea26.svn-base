using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ASS_GP_USER_RESULSETINFOKTYPE_Handler : IHandler<MSG_GP_UER_GETBACKInfo>
{
    [Inject]
    public ILotteryModel model { get; set; }

    [Inject]
    public IUInfoModel uinfomodel { get; set; }

    [Inject]
    public ZHorderRtnSignal zhrtnSignal { get; set; }

    [Inject]
    public MsgSignal MsgSignal { get; set; }

    [Inject]
    public OrderSuccessSignal OrdSuccessSignal { get; set; }  //下订单或者追号成功

    [Inject]
    public CheDanSuccessSignal chedanSuccessSignal { get; set; }

    [Inject]
    public RefreshMoneySignal refreshMoneySignal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_UER_GETBACKInfo para)
    {
        if (para.byGetTypeBack == 10) //下单返回
        {
            if (para.byReturn > 100)
            {
                OrdSuccessSignal.Dispatch();
                uinfomodel.SetMoney(para.iOutMoney);
                refreshMoneySignal.Dispatch();
            }
			//应该把这个放这里才对
            MsgSignal.Dispatch(new MsgPara(para.GetDesc(), 2));
			Loading.GetInstance().HideLoading();
			Global.IsAppOrderRsp = true;
        } else if (para.byGetTypeBack == 5) //追号返回
        {
            if (para.byReturn >= 100) {
                zhrtnSignal.Dispatch(para.byReturn);
            } else {
                MsgSignal.Dispatch(new MsgPara(para.GetDesc(), 2));
            }
			Loading.GetInstance().HideLoading();
        }
        else if (para.byGetTypeBack == 6) //追号返回
        {
            MsgSignal.Dispatch(new MsgPara(para.GetDesc(), 2));
            OrdSuccessSignal.Dispatch();
			Loading.GetInstance().HideLoading();
        }
        else if (para.byGetTypeBack == 2) //赔率返点返回
        {
            NetworkManager.Instance.GetAward(model.lotteryCfg.lotteryId, model.currentSubMode.subModeId);
        }
        else if (para.byGetTypeBack == 8) //追号 取款
        {
            MsgSignal.Dispatch(new MsgPara(para.GetDesc(), 2));
            NetworkManager.Instance.RefreshMoney();
        }
        else if (para.byGetTypeBack == 0) //设置密保成功
        {
            MsgSignal.Dispatch(new MsgPara(para.GetDesc(), 2));
        }
        else if (para.byGetTypeBack == 7) //
        {
            MsgSignal.Dispatch(new MsgPara(para.GetDesc(), 2));
            if (para.byReturn == 0)//等于0的时候成功
            {
                NetworkManager.Instance.RefreshMoney();
                chedanSuccessSignal.Dispatch();
            }
        }
        else if (para.byGetTypeBack == 13|| para.byGetTypeBack == 14) //
        {
            MsgSignal.Dispatch(new MsgPara(para.GetDesc(), 2));
            if (para.byReturn == 0)//等于0的时候成功
            {
                NetworkManager.Instance.GetSevenDayInfo();
            }
        }
        else if (para.byGetTypeBack == 12) //
        {
            MsgSignal.Dispatch(new MsgPara(para.GetDesc(), 2));
            if (para.byReturn == 0)//等于0的时候成功
            {
                NetworkManager.Instance.GetAddMemberInfo();
            }
        }
        else
        {
            MsgSignal.Dispatch(new MsgPara(para.GetDesc(), 2));
        }
     
    }
}

