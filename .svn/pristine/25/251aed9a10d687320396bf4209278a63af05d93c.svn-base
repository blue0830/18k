using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class SScGetPlayYingKuiRecord_Handler : IHandler<SScGetPlayYingKuiRecord>
{
    [Inject]
    public IGameInfoModel model { get; set; }

    [Inject]
    public GongGaoSignal GSignal { get; set; }


    [Inject]
    public MsgSignal MsgSignal { get; set; }

    public void OnReceive(NetMessageHead head, SScGetPlayYingKuiRecord para)
    {
		double d = para.iResultMoney *1.0/100;
		string money = String.Format("¥{0:0.00}", d);
		MsgSignal.Dispatch(new MsgPara("尊敬的玩家 "+Global.CurrentUserName+ "\n" + para.GetClassName()+" 第"+para.GetActivityName()+ "期已经开奖\n" + "您共有"+para.iCountNote+ "条投注记录\n" + "本期盈亏为"+money,2));
		NetworkManager.Instance.RefreshMoney ();
    }
}

