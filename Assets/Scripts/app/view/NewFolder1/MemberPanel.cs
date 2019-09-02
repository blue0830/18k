using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.signal.impl;

public class MemberPanel : MonoBehaviour
{

    public Signal<MsgPara> msgSignal = new Signal<MsgPara>();

    public GameObject returnbtn;

    public Transform subRoot;

    //buttons
    public GameObject huiyuanziliaoBtn;
    public GameObject tianjiaxiajiBtn;
    public GameObject zhuceshezhiBtn;

    public GameObject xiajitongzhutongjiBtn;
    public GameObject xiajitouzhumingxiBtn;
    public GameObject xiajiyingkuitongjiBtn;
    public GameObject xiajiyingkuimingxiBtn;

    public GameObject xiajichongzhiBtn;
    public GameObject xiajitixianBtn;
    public GameObject zhuanzhangxiajiBtn;
    public GameObject zhuanzhangjiluBtn;
    public GameObject xiajiyouxiyingkuiBtn;
    public GameObject xiajiyouxijiluBtn;


    //panels could be null
    HuiYuanZiLiaoPanel huiyuanziliaoPanel;
    TianJiaXiaJiPanel tanjiaxiajiPanel;
    ZhuCeSheZhiPanel zhuceshezhiPanel;

    XiaJiTouZhuTongJiPanel xiajitouzhutongjiPanel;
    XiajiTouZhuMingXiPanel xiajitouzhumingxiPanel;
    XiaJiYingKuiTongjiPanel xiajiyingkuitongjiPanel;
    XiaJiYingKuiMingXIPanel xiajiyingkuimingxiPanel;

    XiaJiChongZhiPanel xiajichongzhiPanel;
    XiaJiTiXianPanel xiajitixianPanel;
    ZhuanZhangXiaJiPanel zhuanzhangxiajiPanel;
    ZhuanZhangJiLuPanel zhuanzhangjilupanel;
    XiaJiYouXiYingKuiPanel xiajiyouxiyingkuiPanel;
    XiaJiYouXiJiLuPanel xiajiyouxijiluPanel;

    private TouZhuXiangQingPanel touizhuxiangqingPanel;//投注详情 Panel

    // Use this for initialization
    void Start()
    {
        UIEventListener.Get(huiyuanziliaoBtn).onClick = huiyuanziliaoClick;
        UIEventListener.Get(tianjiaxiajiBtn).onClick = tianjiaxiajiClick;
        UIEventListener.Get(zhuceshezhiBtn).onClick = zhuceshezhiClick;


        UIEventListener.Get(xiajitongzhutongjiBtn).onClick = xiajitongzhutongjiClick;
        UIEventListener.Get(xiajitouzhumingxiBtn).onClick = xiajitouzhumingxiClick;
        UIEventListener.Get(xiajiyingkuitongjiBtn).onClick = xiajiyingkuitongjiClick;
        UIEventListener.Get(xiajiyingkuimingxiBtn).onClick = xiajiyingkuimingxiClick;

        UIEventListener.Get(xiajichongzhiBtn).onClick = xiajichongzhiClick;
        UIEventListener.Get(xiajitixianBtn).onClick = xiajitixianClick;
        UIEventListener.Get(zhuanzhangxiajiBtn).onClick = zhuanzhangxiajiClick;
        UIEventListener.Get(zhuanzhangjiluBtn).onClick = zhuanzhangjiluClick;
        UIEventListener.Get(xiajiyouxiyingkuiBtn).onClick = xiajiyouxiyingkuiClick;
        UIEventListener.Get(xiajiyouxijiluBtn).onClick = xiajiyouxijiluClick;

    }
    // Update is called once xiajiyouxijiluBtn;per frame
    void Update()
    {


    }

    void OnDestroy()
    {
        RemoveSignal();
    }

    void RemoveSignal()
    {

        if (huiyuanziliaoPanel != null)
            huiyuanziliaoPanel.msgSignal.RemoveListener(OnMsg);//个人资料
     
    }

    //int openedPanel
    void tianjiaxiajiClick(GameObject go) {
        AudioController.Instance.SoundPlay("active_item");
        NetworkManager.Instance.GetAddMemberInfo();
    }

    void zhuceshezhiClick(GameObject go) {

    }

    public void TianjiaInfoBack(MSG_GP_USER_GETPLAYMAXPOINTRESULT para)
    {
        if (tanjiaxiajiPanel == null)
        {
            tanjiaxiajiPanel = LoadPanel("TianJiaXiaJiPanel").GetComponent<TianJiaXiaJiPanel>();
            tanjiaxiajiPanel.msgSignal.AddListener(OnMsg);
        }
        tanjiaxiajiPanel.Show(para);
    }

    void zhuanzhangxiajiClick(GameObject go) {
		AudioController.Instance.SoundPlay("active_item");
        if (zhuanzhangxiajiPanel==null)
        {
            zhuanzhangxiajiPanel = LoadPanel("ZhuanZhangXiaJiPanel").GetComponent<ZhuanZhangXiaJiPanel>();
            zhuanzhangxiajiPanel.msgSignal.AddListener(OnMsg);
        }
        zhuanzhangxiajiPanel.Show();
    }

    void huiyuanziliaoClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        HuiYuanZiLiaoPanel.startDate = TimeHelper.GetNowTime();
        HuiYuanZiLiaoPanel.endDate = TimeHelper.GetNowTime();
		HuiYuanZiLiaoPanel.chBySj = "registerTm#<";
		HuiYuanZiLiaoPanel.chName = "";
        HuiYuanZiLiaoPanel.lookedName.Clear();
        HuiYuanZiLiaoPanel.lookedID.Clear();
        RequestRecord(HuiYuanZiLiaoPanel.byRord,HuiYuanZiLiaoPanel.chName,HuiYuanZiLiaoPanel.chBySj,0);
    }

    void xiajitongzhutongjiClick(GameObject go) {
        AudioController.Instance.SoundPlay("active_item");
        XiaJiTouZhuTongJiPanel.startDate = TimeHelper.GetNowTime();
        XiaJiTouZhuTongJiPanel.endDate = TimeHelper.GetNowTime();
        XiaJiTouZhuTongJiPanel.chName = "";
        RequestRecord(XiaJiTouZhuTongJiPanel.byRord);
    }
    void xiajitouzhumingxiClick(GameObject go) {
        AudioController.Instance.SoundPlay("active_item");
        XiajiTouZhuMingXiPanel.startDate = TimeHelper.GetNowTime();
        XiajiTouZhuMingXiPanel.endDate = TimeHelper.GetNowTime();
        XiajiTouZhuMingXiPanel.chName = "";
        RequestRecord(XiajiTouZhuMingXiPanel.byRord);
    }
    void xiajiyingkuitongjiClick(GameObject go) {
        AudioController.Instance.SoundPlay("active_item");
        XiaJiYingKuiTongjiPanel.startDate = TimeHelper.GetNowTime();
        XiaJiYingKuiTongjiPanel.endDate = TimeHelper.GetNowTime();
		XiaJiYingKuiTongjiPanel.chBySj = "PayMoney#<";
		XiaJiYingKuiTongjiPanel.chName = "";
        XiaJiYingKuiTongjiPanel.lookedName.Clear();
        XiaJiYingKuiTongjiPanel.lookedID.Clear();
        RequestRecord(XiaJiYingKuiTongjiPanel.byRord,XiaJiYingKuiTongjiPanel.chName,XiaJiYingKuiTongjiPanel.chBySj,0);
    }
    void xiajiyingkuimingxiClick(GameObject go) {
        AudioController.Instance.SoundPlay("active_item");
        XiaJiYingKuiMingXIPanel.startDate = TimeHelper.GetNowTime();
        XiaJiYingKuiMingXIPanel.endDate = TimeHelper.GetNowTime();
        XiaJiYingKuiMingXIPanel.chName = "";
        RequestRecord(XiaJiYingKuiMingXIPanel.byRord);
    }

    void xiajichongzhiClick(GameObject go) {
        AudioController.Instance.SoundPlay("active_item");
        XiaJiChongZhiPanel.startDate = TimeHelper.GetNowTime();
        XiaJiChongZhiPanel.endDate = TimeHelper.GetNowTime();
        XiaJiChongZhiPanel.chName = "";
        RequestRecord(XiaJiChongZhiPanel.byRord);
    }


    void xiajitixianClick(GameObject go) {
        AudioController.Instance.SoundPlay("active_item");
        XiaJiTiXianPanel.startDate = TimeHelper.GetNowTime();
        XiaJiTiXianPanel.endDate = TimeHelper.GetNowTime();
        XiaJiTiXianPanel.chName = "";
        RequestRecord(XiaJiTiXianPanel.byRord);
    }


    void zhuanzhangjiluClick(GameObject go) {
        AudioController.Instance.SoundPlay("active_item");
        ZhuanZhangJiLuPanel.startDate = TimeHelper.GetNowTime();
        ZhuanZhangJiLuPanel.endDate = TimeHelper.GetNowTime();
        ZhuanZhangJiLuPanel.chName = "";
        RequestRecord(ZhuanZhangJiLuPanel.byRord);
    }


    void xiajiyouxiyingkuiClick(GameObject go) {
        AudioController.Instance.SoundPlay("active_item");
        XiaJiYouXiYingKuiPanel.startDate = TimeHelper.GetNowTime();
        XiaJiYouXiYingKuiPanel.endDate = TimeHelper.GetNowTime();
		XiaJiYouXiYingKuiPanel.chBySj = "qpPoint#<";
		XiaJiYouXiYingKuiPanel.chName = "";
        XiaJiYouXiYingKuiPanel.lookedName.Clear();
        XiaJiYouXiYingKuiPanel.lookedID.Clear();
        RequestRecord(XiaJiYouXiYingKuiPanel.byRord,XiaJiYouXiYingKuiPanel.chName,XiaJiYouXiYingKuiPanel.chBySj,0);
    }


    void xiajiyouxijiluClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        XiaJiYouXiJiLuPanel.startDate = TimeHelper.GetNowTime();
        XiaJiYouXiJiLuPanel.endDate = TimeHelper.GetNowTime();
        XiaJiYouXiJiLuPanel.chName = "";
        RequestRecord(XiaJiYouXiJiLuPanel.byRord);
    }
		
	void RequestRecord(byte id, string ch = @"",string chBySj = @"", int uid=-1)
    {
        //子标识暂用1
		NetworkManager.Instance.LookupRecord(id, 1, id, 1, ch, TimeHelper.GetNowTime(), TimeHelper.GetNowTime(), uid, chBySj);
	}

    public void OnRecordBack(RecordBackObj obj)
    {
        byte mainId = obj.byMainType;
        switch (mainId)
        {
            case HuiYuanZiLiaoPanel.byRord:
                if (huiyuanziliaoPanel == null)
                {
                    huiyuanziliaoPanel = LoadPanel("HuiYuanZiLiaoPanel").GetComponent<HuiYuanZiLiaoPanel>();
                    huiyuanziliaoPanel.msgSignal.AddListener(OnMsg);
                }
                huiyuanziliaoPanel.show(obj);
                break;
            case XiajiTouZhuMingXiPanel.byRord:
                if (xiajitouzhumingxiPanel == null)
                {
                    xiajitouzhumingxiPanel = LoadPanel("XiaJiTouZhuMingXiPanel").GetComponent<XiajiTouZhuMingXiPanel>();
                    xiajitouzhumingxiPanel.msgSignal.AddListener(OnMsg);
                }
                xiajitouzhumingxiPanel.show(obj);
                break;
            case XiaJiTouZhuTongJiPanel.byRord:
                if (xiajitouzhutongjiPanel == null)
                {
                    xiajitouzhutongjiPanel = LoadPanel("XiaJiTouZhuTongJiPanel").GetComponent<XiaJiTouZhuTongJiPanel>();
                    xiajitouzhutongjiPanel.msgSignal.AddListener(OnMsg);
                }
                xiajitouzhutongjiPanel.show(obj);
                break;
            case XiaJiYingKuiMingXIPanel.byRord:
                if (xiajiyingkuimingxiPanel == null)
                {
                    xiajiyingkuimingxiPanel = LoadPanel("XiaJiYingKuiMingXIPanel").GetComponent<XiaJiYingKuiMingXIPanel>();
                    xiajiyingkuimingxiPanel.msgSignal.AddListener(OnMsg);
                }
                xiajiyingkuimingxiPanel.show(obj);
                break;
            case XiaJiYingKuiTongjiPanel.byRord:
                if (xiajiyingkuitongjiPanel == null)
                {
                    xiajiyingkuitongjiPanel = LoadPanel("XiaJiYingKuiTongjiPanel").GetComponent<XiaJiYingKuiTongjiPanel>();
                    xiajiyingkuitongjiPanel.msgSignal.AddListener(OnMsg);
                }
                xiajiyingkuitongjiPanel.show(obj);
                break;
            case XiaJiChongZhiPanel.byRord:
                if (xiajichongzhiPanel == null)
                {
                    xiajichongzhiPanel = LoadPanel("XiaJiChongZhiPanel").GetComponent<XiaJiChongZhiPanel>();
                    xiajichongzhiPanel.msgSignal.AddListener(OnMsg);
                }
                xiajichongzhiPanel.show(obj);
                break;
            case XiaJiTiXianPanel.byRord:
                if (xiajitixianPanel == null)
                {
                    xiajitixianPanel = LoadPanel("XiaJiTiXianPanel").GetComponent<XiaJiTiXianPanel>();
                    xiajitixianPanel.msgSignal.AddListener(OnMsg);
                }
                xiajitixianPanel.show(obj);
                break;
           
            case ZhuanZhangJiLuPanel.byRord:
                if (zhuanzhangjilupanel == null)
                {
                    zhuanzhangjilupanel = LoadPanel("ZhuanZhangJiLuPanel").GetComponent<ZhuanZhangJiLuPanel>();
                    zhuanzhangjilupanel.msgSignal.AddListener(OnMsg);
                }
                zhuanzhangjilupanel.show(obj);
                break;
            case XiaJiYouXiYingKuiPanel.byRord:
                if (xiajiyouxiyingkuiPanel == null)
                {
                    xiajiyouxiyingkuiPanel = LoadPanel("XiaJiYouXiYingKuiPanel").GetComponent<XiaJiYouXiYingKuiPanel>();
                    xiajiyouxiyingkuiPanel.msgSignal.AddListener(OnMsg);
                }
                xiajiyouxiyingkuiPanel.show(obj);
                break;
            case XiaJiYouXiJiLuPanel.byRord:
                if (xiajiyouxijiluPanel == null)
                {
                    xiajiyouxijiluPanel = LoadPanel("XiaJiYouXiJiLuPanel").GetComponent<XiaJiYouXiJiLuPanel>();
                    xiajiyouxijiluPanel.msgSignal.AddListener(OnMsg);
                }
                xiajiyouxijiluPanel.show(obj);

                break;
        }
    }

    public void OpenXiangxi(TouzhuXiangxi obj)
    {
        if (touizhuxiangqingPanel == null)
        {
            touizhuxiangqingPanel = LoadPanel("TouZhuXiangqingPanel").GetComponent<TouZhuXiangQingPanel>();
            touizhuxiangqingPanel.msgSignal.AddListener(OnMsg);
        }
        touizhuxiangqingPanel.Show(obj);
    }

    public void OnGetPeie(MSG_GP_USER_GETPLAYPE para)
    {
        huiyuanziliaoPanel.ShowPeiE(para);
    }

    void OnMsg(MsgPara p)
    {
        msgSignal.Dispatch(p);
    }

    GameObject LoadPanel(string name)
    {
        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("subPanelMember/" + name);
        GameObject panel = Instantiate(asset) as GameObject;
        panel.transform.parent = subRoot;
        panel.transform.localScale = Vector3.one;

        return panel;
    }
}
