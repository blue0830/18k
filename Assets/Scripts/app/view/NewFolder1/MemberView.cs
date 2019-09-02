/// An example view
/// ==========================
/// 

using System;
using System.Collections;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;


public class MemberView : View
{
    [Inject]
    public MsgSignal MsgSignal { get; set; }

    GameObject panel;
    MemberPanel panelScript;


    internal void init()
    {
        Transform parent = UIRootFinder.uiRootTran;
        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("MemberPanel");
        panel = Instantiate(asset) as GameObject;
        panel.transform.parent = parent;
        panel.transform.localScale = Vector3.one;

        panelScript = panel.GetComponent<MemberPanel>();

        panelScript.msgSignal.AddListener(OnsubPanelMsg);
        UIEventListener.Get(panelScript.returnbtn).onClick = OnReturnClick;
    }

    public void OnAddMemberInfo(MSG_GP_USER_GETPLAYMAXPOINTRESULT result)
    {
        panelScript.TianjiaInfoBack(result);
    }


    public void OnRecordBack(RecordBackObj obj)
    {
        panelScript.OnRecordBack(obj);
    }

    public void OnTouzhuxiangxi(TouzhuXiangxi obj)
    {
        panelScript.OpenXiangxi(obj);
    }

    protected override void OnDestroy()
    {
        panelScript.msgSignal.RemoveListener(OnsubPanelMsg);
        base.OnDestroy();

        Destroy(panel);
    }

    void OnsubPanelMsg(MsgPara para)
    {
        MsgSignal.Dispatch(para);
    }

    public void OnGetPeie(MSG_GP_USER_GETPLAYPE para)
    {
        panelScript.OnGetPeie(para);
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


