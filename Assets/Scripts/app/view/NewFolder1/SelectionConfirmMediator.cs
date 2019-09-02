/// Example mediator
/// =====================
/// Note how we no longer extend EventMediator, and inject Signals instead

using System;
using UnityEngine;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;


//Not extending EventMediator anymore
public class SelectionConfirmMediator : Mediator
{
	[Inject]
	public LogOutSignal logoutSignal { get; set; }

    [Inject]
    public SelectionConfirmView view { get; set; }

    [Inject]
    public DeleteCfirmPSignal DpanelSignal { get; set; }

    [Inject]
    public ShowCfirmPSignal showSignal { get; set; }

    [Inject]
    public ZhuiqiListSignal ZHSignal { get; set; }

    [Inject]
    public ZHorderRtnSignal zhrtnSignal { get; set; }

    [Inject]
    public ILotteryModel lmodel { get; set; }

    [Inject]
	public OrderSuccessSignal OrdSuccessSignal { get; set; }  //下订单或者追号成功


    ZhuihaoPanel _zhPanelscript = null;

    public override void OnRegister()
    {
        view.init();
		logoutSignal.AddListener (OnLogOut);
        DpanelSignal.AddListener(OnDeleteSignal);
        showSignal.AddListener(OnShowSignal);
        ZHSignal.AddListener(OnGetZhuihaoList);
        zhrtnSignal.AddListener(OnZhuihaoRtn);
        OrdSuccessSignal.AddListener(OnOrderSuccess);
        view.orderSign.AddListener(OnTakeOrder);
        view.zhuihaoSignal.AddListener(OnZhuihao);
      
    }

    public override void OnRemove()
    {
        view.orderSign.RemoveListener(OnTakeOrder);
        view.zhuihaoSignal.RemoveListener(OnZhuihao);
		logoutSignal.RemoveListener (OnLogOut);
        DpanelSignal.RemoveListener(OnDeleteSignal);
        showSignal.RemoveListener(OnShowSignal);
        ZHSignal.RemoveListener(OnGetZhuihaoList);
        zhrtnSignal.RemoveListener(OnZhuihaoRtn);
        OrdSuccessSignal.RemoveListener(OnOrderSuccess);
    }

    void OnShowSignal(ConfirmPanelObj cobj)
    {
        view.ShowPanel(cobj);
    }

    void OnDeleteSignal()
    {
        view.close();
    }

    void OnTakeOrder(List<ConfirmPanelObj> ConfirmObjList)
    {
        string orderStr = "";
        double tamount = 0;
        for (int i = 0; i < ConfirmObjList.Count; ++i)
        {
            ConfirmPanelObj obj = ConfirmObjList[i];
            orderStr += obj.subCfg.subModeId + "#";
            orderStr += obj.contents + "#";

            if (obj.model == 1)
            {
                orderStr += "200#";
            }
            else if (obj.model == 2)
            {
                orderStr += "20#";
            }
            else if (obj.model == 3)
            {
                orderStr += "2#";
            }
            orderStr += obj.bs + "#";
            //orderStr += float.Parse(obj.amount) * 100 + "#";
			orderStr += MathUtil.calculate(obj.amount, 100.ToString(), '*') + "#";
            orderStr += obj.zs + "#";
            orderStr += obj.tzbs + "#";
            tamount = MathUtil.calculate(tamount.ToString(), obj.amount, '+');
        }
        tamount = MathUtil.calculate(tamount.ToString(), "100", '*');
        NetworkManager.Instance.TakeOrder(lmodel.lotteryCfg.lotteryId, lmodel.GetActivityId(), (int)tamount, ConfirmObjList.Count, orderStr);
    }

    void OnZhuihao(ZhuihaoOrderObj zobj)
    {
        LotteryConfig lcfg = zobj.cfirmObj.lCfg;
        NetworkManager.Instance.GetZhuihaoQshu(lcfg.lotteryId);
        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("ZhuihaoPanel");
        GameObject ItemObj = Instantiate(asset) as GameObject;
        ItemObj.transform.parent = UIRootFinder.uiRootTran;
        ItemObj.transform.localScale = Vector3.one;
        _zhPanelscript = ItemObj.GetComponent<ZhuihaoPanel>();
        _zhPanelscript.SetZhuihaoObj(zobj);
        _zhPanelscript.gameObject.SetActive(false);
    }

    void OnGetZhuihaoList(List<QihaoObj> qihaoList)
    {
        if (_zhPanelscript == null || _zhPanelscript.gameObject == null)
            return;
        _zhPanelscript.gameObject.SetActive(true);
        _zhPanelscript.OnGetZhuihaoList(qihaoList);
    }

    void OnZhuihaoRtn(int id)
    {
        if (_zhPanelscript == null || _zhPanelscript.gameObject == null)
            return;
        _zhPanelscript.OnZhuihaoRtn(id);
    }

    void OnOrderSuccess()
    {
        view.DeleteAll();
        if (_zhPanelscript==null||_zhPanelscript.gameObject == null)
            return;
        _zhPanelscript.Close();
        _zhPanelscript = null;
    }

	void OnLogOut(){
		view.close ();
	}
}