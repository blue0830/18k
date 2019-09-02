using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class SScPlayCZorQKCue_Handler : IHandler<SScPlayCZorQKCue>
{
    [Inject]
    public IGameInfoModel model { get; set; }

    [Inject]
    public GongGaoSignal GSignal { get; set; }

    [Inject]
    public MsgSignal MsgSignal { get; set; }

    public void OnReceive(NetMessageHead head, SScPlayCZorQKCue para)
    {
        string timeStr = TimeHelper.yyyyMMddHHmmss (para.AddTime);
		MsgSignal.Dispatch(new MsgPara("尊敬的玩家 "+Global.CurrentUserName+ "\n 您于" + timeStr+ "\n" + para.GetChTipInfo(),2));
		NetworkManager.Instance.RefreshMoney ();
    }
}

