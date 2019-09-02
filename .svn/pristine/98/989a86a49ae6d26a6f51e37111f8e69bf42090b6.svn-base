/// An example view
/// ==========================
/// 

using System;
using System.Collections;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;


public class ActivityView : View
{
    [Inject]
    public MsgSignal MsgSignal { get; set; }

    GameObject panel;
    ActivityPanel panelScript;


    internal void init()
    {
        Transform parent = UIRootFinder.uiRootTran;
        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("activityPanel");
        panel = Instantiate(asset) as GameObject;
        panel.transform.parent = parent;
        panel.transform.localScale = Vector3.one;

        panelScript = panel.GetComponent<ActivityPanel>();

        UIEventListener.Get(panelScript.returnbtn).onClick = OnReturnClick;
    }


 

    protected override void OnDestroy()
    {
        base.OnDestroy();

        Destroy(panel);
    }

    void OnReturnClick(GameObject go)
    {
        close();
    }

    public void close()
    {
        Destroy(gameObject);
    }

    public void OpenSevenDay(MSG_GP_USER_HDZX7TLRULERESULT para)
    {
		AudioController.Instance.SoundPlay("active_item");
        panelScript.Open7dayPanel(para);
    }

	public void OpenChongZhiSongPanel(MSG_GP_USER_HDZXXRCZSRULET para)
	{
		AudioController.Instance.SoundPlay("active_item");
		panelScript.OpenChongZhiSongPanel(para);
	}

	public void OpenTuiGuangPanel(MSG_GP_USER_HDZXYJTGJLRUSULT para)
	{
		AudioController.Instance.SoundPlay("active_item");
		panelScript.OpenPermanentPromotionPanel(para);
	}
}


