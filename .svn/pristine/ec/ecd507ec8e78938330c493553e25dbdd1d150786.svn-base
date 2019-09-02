/// An example view
/// ==========================
/// 

using System;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;


public class MessageView : View
{

    public Signal clickSignal = new Signal();

    GameObject panel;
    MessagePanel panelScript;

    MsgPara msgPara;

    public bool socketmsg=false;
    public MsgPara socketpara = null;
    public bool isPause = false;

    List<GameObject> showPanelList = new List<GameObject>();  //定时的panel

    void init()
    {
        if (panel != null)
        {
            return;
        }

        Transform parent = UIRootFinder.uiRootTran;
        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("MessagePanel");
        panel = Instantiate(asset) as GameObject;
        panel.transform.parent = parent;
        panel.transform.localScale = Vector3.one;

        panelScript = panel.GetComponent<MessagePanel>();

        UIEventListener.Get(panelScript.btn).onClick = OnConfirm;
        UIEventListener.Get(panelScript.confirmBtn).onClick = OnConfirm;
        UIEventListener.Get(panelScript.cancelBtn).onClick = OnCancel;
        UIEventListener.Get(panelScript.btnClose).onClick = OnCancel;
    }

    void Update()
    {
        if (socketmsg)
        {
            socketmsg = false;
            if (socketpara != null)
                Show(socketpara);
        }
    }

    internal void Show(MsgPara para)
    {
        msgPara = para;

        if (msgPara.type == 1)
        {
            init();
            panelScript.message.text = para.text;
            if (msgPara.GetBtnNum() == 2)
            {
                panelScript.onebtnroot.SetActive(false);
                panelScript.twobtnroot.SetActive(true);
            }
            else
            {
                panelScript.onebtnroot.SetActive(true);
                panelScript.twobtnroot.SetActive(false);
            }
        }
        else
        {        
            //先把不存在的移除掉 
            for (int i = showPanelList.Count-1; i >= 0; --i)
            {
                if (showPanelList[i] == null)
                {
                    showPanelList.RemoveAt(i);
                }
            }
            int depth = 1001+ showPanelList.Count;

            GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("onlyshow");
            GameObject showPanel = Instantiate(asset) as GameObject;
            showPanel.transform.parent = UIRootFinder.uiRootTran;
            showPanel.transform.localScale = Vector3.one;
            showPanel.GetComponent<UIPanel>().depth = depth;

            MessageShow script = showPanel.GetComponent<MessageShow>();
            script.message.text = msgPara.text;

            showPanelList.Add(showPanel);

        }
    }




    void OnCancel(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        if (msgPara.GetBtnNum() == 1)
        {
            msgPara.DoConfirm();
        }
        else
        {
            msgPara.DoCancel();
        }
        HideMsg();
    }

    void OnConfirm(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");

        msgPara.DoConfirm();
        HideMsg();
    }

    public void HideMsg()
    {
        Destroy(panel);
        msgPara = null;
    }

 
    void OnApplicationPause(bool Pause)
    {
        isPause = Pause;
        if (Pause)
        {
			Global.LastAppHeartBeatTime = 0;//防止网络检查与此处冲突
            LoSocket.GetInstance().Pause();
        }
        else
        {
            LoSocket.GetInstance().Resume();
        }
    }
}


