﻿using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl;

public class SelectionPanel : MonoBehaviour {
    public Signal<MsgPara> msgSignal = new Signal<MsgPara>();

    public GameObject ReturnBtn;
    public GameObject ModeBtn;

    public UILabel lotteryNameLabel;
    public UILabel lotteryModeLabel;
    public UISprite ModeTriangle;

    public UILabel qishuLabel;
    public UILabel timeLabel;

    public UILabel awardLabel;
    public UILabel proportionLabel; //返点
    public UIInput mutipleLabel; //倍数


    public UIToggle YuanToggle;
    public UIToggle JiaoToggle;
    public UIToggle FenToggle;

    public UISlider slider;
    public GameObject arrowLeftBtn;
    public GameObject arrowRightBtn;

    public GameObject ClearBtn;
    public GameObject ConfirmBtn;
    public UILabel ConfirmLabel;
    public UILabel YuELabel;
    //说明组件
    public GameObject helpBtn;
    public GameObject expBtn;
    public GameObject awardBtn;
    public UIGrid helpGrid;
    public Helpcontent helpcontent;

    //最近记录
    public UILabel recordQishu;
    public UILabel recordresult;

    //check boxes
    public GameObject CheckBoxRoot;
    public UIToggle[] CheckBoxes;

    public string jineString = "";
    public int zsSave= 0;

    public Scrollview_V scrollview_v;
    public ModePanel modePanel;
    public GameObject textroot;
    public UIInput textInput;
    public GameObject MoreRecordBtn;
    public GameObject functionBtn;
    public GameObject botupContent;
    public GameObject closebotupBtn;
    public GameObject goConfirmBtn;
    public GameObject beishuAdd;
    public GameObject beishuSub;
	public GameObject backObj;

    public GameObject btnTouZhuRecord;

    Transform panelRoot;
    private TouZhuRecordPanel touzhuRecordpanel;//投注记录 Panel


    // Use this for initialization
    void Start () {
        UIEventListener.Get(functionBtn).onClick= functionClick;
        UIEventListener.Get(closebotupBtn).onClick = closebotupClick;
		UIEventListener.Get(backObj).onClick = closebotupClick;
        UIEventListener.Get(btnTouZhuRecord).onClick = OnTouZhuRecordClicked;

       // panelRoot = transferPanel.transform.parent;
    }
	
	// Update is called once per frame
	void Update () {
	}

    void functionClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        botupContent.SetActive(true);
        functionBtn.SetActive(false);
    }

    void closebotupClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        botupContent.SetActive(false);
        functionBtn.SetActive(true);
    }

    void OnTouZhuRecordClicked(GameObject sender)
    {
        AudioController.Instance.SoundPlay("active_item");
        //静态变量
        TouZhuRecordPanel.startDate = TimeHelper.GetNowTime();
        TouZhuRecordPanel.endDate = TimeHelper.GetNowTime();
        TouZhuRecordPanel.chName = @"";
        RequestRecord(TouZhuRecordPanel.byRord);
    }

    void RequestRecord(byte id, string ch = @"")
    {
        //子标识暂用1
        NetworkManager.Instance.LookupRecord(id, 1, id, 1, ch, TimeHelper.GetNowTime(), TimeHelper.GetNowTime());
    }

    public void OnRecordBack(RecordBackObj obj)
    {
        byte mainId = obj.byMainType;
        switch (mainId)
        {
            case TouZhuRecordPanel.byRord:
                if (touzhuRecordpanel == null)
                {
                    touzhuRecordpanel = LoadPanel("TouZhuRecordPanel").GetComponent<TouZhuRecordPanel>();
                    touzhuRecordpanel.msgSignal.AddListener(OnMsg);
                    touzhuRecordpanel.GetComponent<UIPanel>().depth = 13;
                }
                touzhuRecordpanel.show(obj);
                break;
        }
    }

    GameObject LoadPanel(string name)
    {
        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("subPanel/" + name);
        GameObject panel = Instantiate(asset) as GameObject;
        panel.transform.parent = UIRootFinder.uiRootTran;//panelRoot;
        panel.transform.localScale = Vector3.one;

        return panel;
    }

    void OnMsg(MsgPara p)
    {
        msgSignal.Dispatch(p);
    }

    private void OnDestroy()
    {
        if (touzhuRecordpanel != null)
            touzhuRecordpanel.msgSignal.RemoveListener(OnMsg);
    }
}
