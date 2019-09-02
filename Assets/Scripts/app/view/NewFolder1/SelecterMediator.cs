/// Example mediator
/// =====================
/// Note how we no longer extend EventMediator, and inject Signals instead

using System;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;


//Not extending EventMediator anymore
public class SelecterMediator : Mediator
{
	[Inject]
	public LogOutSignal logoutSignal { get; set; }

    [Inject]
    public SelecterView view { get; set; }

    [Inject]
    public IUInfoModel UInfoModel { get; set; }// 

    [Inject]
    public DeleteCfirmPSignal DpanelSignal { get; set; }

    [Inject]
    public MsgSignal MsgSignal { get; set; }

    [Inject]
    public ILotteryModel lmodel { get; set; }

    [Inject]
    public RecordBackSignal recordBackSignal { get; set; } //记录查询返回信号 

    [Inject]
    public LotterySignal lSignal { get; set; }

    [Inject]
    public GetPointSignal GetpSignal { get; set; }

    [Inject]
    public LoRecordSignal lrSignal { get; set; }

    [Inject]
    public RefreshMoneySignal RefreshMoneySignal { get; set; }//刷新金额 信号

    [Inject]
    public ShowCfirmPSignal ShowConfirmSignal { get; set; }

    public override void OnRegister()
    {
        view.init();
        ShowConfirmSignal.AddListener(OnShowSignal);
        lSignal.AddListener(OnRecieveQishu);
        GetpSignal.AddListener(OnRecieveAward);
        lrSignal.AddListener(OnLoRecordSignal);
        RefreshMoneySignal.AddListener(OnRefreshMoney);//刷新金额 监听器
		LoSocket.GetInstance().connectSignal.AddListener(OnConnect);
		logoutSignal.AddListener (OnLogOut);
        view.UpdateYuelabel(UInfoModel.GetMoney());
        recordBackSignal.AddListener(OnRecordBack);
    }

    public override void OnRemove()
    {
        lSignal.RemoveListener(OnRecieveQishu);
        GetpSignal.RemoveListener(OnRecieveAward);
        lrSignal.RemoveListener(OnLoRecordSignal);
        RefreshMoneySignal.RemoveListener(OnRefreshMoney);//移出 刷新金额 监听器
        ShowConfirmSignal.RemoveListener(OnShowSignal);
		logoutSignal.RemoveListener (OnLogOut);
        recordBackSignal.RemoveListener(OnRecordBack);
        TimeManager.Instance().UnRegister("SelecterMediatorTimer");
		LoSocket.GetInstance().connectSignal.RemoveListener(OnConnect);
        DpanelSignal.Dispatch();  //顺便删除confirm panel
        Debug.Log("SelecterMediator Mediator OnRemove");
    }

	void OnConnect()
	{
		view.RequestTime ();
	}

    void OnShowSignal(ConfirmPanelObj cobj)
    {
        view.ClearText();
    }

    void OnRefreshMoney()
    {
        view.UpdateYuelabel(UInfoModel.GetMoney());
    }


    void OnRecieveQishu()
    {
        string labelstr = "下单时间";
        int timer = lmodel.GetTimer(ref labelstr);

        Debug.LogWarning("OnRecieveQishu");

        if (labelstr == "下单时间")
        {
            view.isBlockOrder = false;
        }
        else {
            //view.isBlockOrder = true;
        }

        if (timer == 0)
        {

            view.SetQishuTime(null, string.Format("{0} [F4C303FF]{1}[-]", labelstr, TimeHelper.SecondToHour(0)));
        }
        else
        {
            TimeManager.Instance().UnRegister("SelecterMediatorTimer");
            TimeManager.Instance().Register("SelecterMediatorTimer", timer, 1000, 1000, (c, t) =>
            {
                if (view != null)
                {

                    view.SetQishuTime(lmodel.GetQishuStr(), string.Format("{0} [F4C303FF]{1}[-]", labelstr, TimeHelper.SecondToHour(timer - c + 1)));
                }
            });
        }

    }

    void OnRecordBack(RecordBackObj obj)
    {
        view.OnRecordBack(obj);

    }

    void OnRecieveAward()
    {
        view.SetAwardPt(lmodel.GetAwardinfo());
    }

    void OnLoRecordSignal(RecordObj obj)
    {
        view.updateRecord(obj);
    }

	void OnLogOut(){
		view.close ();
	}
}

