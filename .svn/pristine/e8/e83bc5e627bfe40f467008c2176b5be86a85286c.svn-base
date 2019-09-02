/// Example mediator
/// =====================
/// Note how we no longer extend EventMediator, and inject Signals instead

using System;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;


//Not extending EventMediator anymore
public class MessageMediator : Mediator
{
    [Inject]
    public MessageView view { get; set; }

    [Inject]
    public MsgSignal MsgSignal { get; set; }


    public override void OnRegister()
    {
        MsgSignal.AddListener(OnMsgshow);
        LoSocket.GetInstance().msgSigal.AddListener(OnSocketSignal);
    }

    public override void OnRemove()
    {
        MsgSignal.RemoveListener(OnMsgshow);
        LoSocket.GetInstance().msgSigal.RemoveListener(OnSocketSignal);
    }

    void OnMsgshow(MsgPara para)
    {
        if (string.IsNullOrEmpty(para.text))
        {
            return;
        }
        view.Show(para);
    }

    void OnSocketSignal(MsgPara para)
    {
        if (!view.isPause)
        {
            view.socketpara = para;
            view.socketmsg=true;
        }
         
    }





}


public class MsgPara
{
    Action confirmEvt;
    Action cancelEvt;

    public int type = 1;
    public  string text = "";

    int btnNum;

    public int GetBtnNum()
    {
        return btnNum;
    }

    //type 1:正常对话框 2:定时对话框
    public MsgPara(string txt,int tp=1, Action Confirm = null, Action cancel = null)
    {
        type = tp;
        text = txt;

        confirmEvt = Confirm;
        cancelEvt = cancel;
        if (confirmEvt != null && cancelEvt != null)
        {
            btnNum = 2;
        }
        else
        {
            btnNum = 1;
        }
    }

    public void DoConfirm()
    {
        if(confirmEvt!=null)
        confirmEvt();
    }

    public void DoCancel()
    {
        if (cancelEvt != null)
            cancelEvt();
    }

}

