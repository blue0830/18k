/// An example view
/// ==========================
/// 

using System;
using System.Collections;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;


public class RecordView : View
{
    [Inject]
    public MsgSignal MsgSignal { get; set; }

    GameObject panel;
    RecordPanel panelScript;

    public Signal<int> buySignal = new Signal<int>();

    internal void init()
    {
        Transform parent = UIRootFinder.uiRootTran;
        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("RecordPanel");
        panel = Instantiate(asset) as GameObject;
        panel.transform.parent = parent;
        panel.transform.localScale = Vector3.one;

        panelScript = panel.GetComponent<RecordPanel>();


        UIEventListener.Get(panelScript.returnbtn).onClick = OnReturnClick;
    }


    public void InitItems(LotteryConfigLoader cfgloader)
    {
        panelScript.CreateItems(cfgloader.lotteryConfigs);
    }

    public void UpdateContent(RecordObj obj)
    {
        panelScript.UpdateInfo(obj, GoBuyLottery);
    }


    void GoBuyLottery(int id)
    {
        close();
        buySignal.Dispatch(id);
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();

        Destroy(panel);
    }

    void OnReturnClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        close();
    }

    public void close()
    {
        Destroy(gameObject);
    }
}


