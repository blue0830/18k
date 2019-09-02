/// An example view
/// ==========================
/// 

using System;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

using System.Collections;

public class MainView : View
{
    [Inject]
    public MsgSignal MsgSignal { get; set; }

	[Inject]
	public LogOutSignal loginSignal { get; set; }

    GameObject panel;
    MainPanel panelScript;

    Transform contextTrans;


    List<int> gonggaoSeq = new List<int>();
    Dictionary<int, string> gonggaoDic = new Dictionary<int, string>();
    int seqIndex = 0; //当前正在播第几条公告
    bool startplay = false;

    private LotteryConfigLoader lotteryCfgLoader = null;

	GameListPanel script;

    int  toggleState;

	private HashSet<ComNameInfo> comNameInfoList=new HashSet<ComNameInfo>(); //保存游戏列表

    internal void init()
    {
        Transform parent = UIRootFinder.uiRootTran;
        contextTrans = parent.parent;

        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("MainPanel");
        panel = Instantiate(asset) as GameObject;
        panel.transform.parent = parent;
        panel.transform.localScale = Vector3.one;

        panelScript = panel.GetComponent<MainPanel>();

        for (int i = 0; i < panelScript.subBtns.Length; ++i)
        {

            UIEventListener.Get(panelScript.subBtns[i]).onClick = OnBotBtn;
        }

        for (int i = 0; i < panelScript.mainToggle.Length; ++i)
        {

            EventDelegate.Add(panelScript.mainToggle[i].onChange, OnToggleChange);

            UIEventListener.Get(panelScript.mainToggle[i].gameObject).onClick = OnToggleClick;
        }
		script = panelScript.gamelistPanel.GetComponent<GameListPanel> ();
		script.msgSignal.AddListener(OnRoomMsg);
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape)))
        {
            MsgSignal.Dispatch(new MsgPara("确定要退出吗？",1,()=> { Application.Quit(); },()=> { }));
        }
		if (Global.LastAppHeartBeatTime!=0&&Global.IsLoginApp) {
			ulong nowTime = TimeHelper.GetNowTime ();
			ulong left = nowTime - Global.LastAppHeartBeatTime;
			if (left > 15) {//回包超时跳出提示，重新登录
				Global.LastAppHeartBeatTime = 0;
				MsgSignal.Dispatch(new MsgPara("网络异常请重新登录",2));
				loginSignal.Dispatch ();
			}
		}
		if (Global.IsLoginApp) {//保存最近连接服务器的index
			int ipListIndex = PlayerPrefs.GetInt ("ipListIndex",-1);
			if (ipListIndex!=Global.AppConnIpsIndex) {
				PlayerPrefs.SetInt("ipListIndex", Global.AppConnIpsIndex);//保存最近连接服务器的index
			}
		}
    }

    public void InitIcons(LotteryConfigLoader cfgLoader )
    {
        if (cfgLoader == null)
            return;
        lotteryCfgLoader = cfgLoader;
       
        List<LotteryConfig> temp = new List<LotteryConfig>();
      
		if (panelScript.mainToggle [0].value) {
			toggleState = 1;
			for (int i = 0; i < cfgLoader.lotteryConfigs.Count; ++i) {
				if (cfgLoader.lotteryConfigs [i].lotteryType == 1 || cfgLoader.lotteryConfigs [i].lotteryType == 4) {
					temp.Add (cfgLoader.lotteryConfigs [i]);
				}
			}
		} else if (panelScript.mainToggle [1].value) {
			toggleState = 2;
			for (int i = 0; i < cfgLoader.lotteryConfigs.Count; ++i) {
				if (cfgLoader.lotteryConfigs [i].lotteryType == 2) {
					temp.Add (cfgLoader.lotteryConfigs [i]);
				}
			}
		} else if (panelScript.mainToggle [2].value) {
			toggleState = 3;
			for (int i = 0; i < cfgLoader.lotteryConfigs.Count; ++i) {
				if (cfgLoader.lotteryConfigs [i].lotteryType == 3) {
					temp.Add (cfgLoader.lotteryConfigs [i]);
				}
			}
		} else {
			toggleState = 4;
			if (comNameInfoList != null) {
				foreach (ComNameInfo gameInfo in comNameInfoList) {
					LotteryConfig fakeCfg = new LotteryConfig ();
					fakeCfg.lotteryId =(int) gameInfo.uNameID;
					fakeCfg.iconName = "icon_"+gameInfo.uNameID.ToString();
					temp.Add (fakeCfg);
				}
			}
		}

        panelScript.CreateIcons(temp);

        for (int i = 0; i < panelScript.Icons.Count; ++i)
        {
            UIEventListener.Get(panelScript.Icons[i]).onClick = OnClickIcon;
        }
    }

    void OnClickIcon(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        int lotteryId = int.Parse(go.name);
		if (lotteryId == 10301800) {//百家乐
			string version = null;
			string updateUrl = null;
			foreach (ComNameInfo gameInfo in comNameInfoList) {
				if (lotteryId == (int)gameInfo.uNameID) {
					version = gameInfo.GetVersion ();
					updateUrl = gameInfo.GetChdownLoadUrl ();
				}
			}
			if (!Constant.BJL_VERSION.Equals(version)) {
				MsgSignal.Dispatch (new MsgPara ("您的游戏版本过低，\n点击确认下载最新版本", 1, () => {
					Application.OpenURL (updateUrl);
				}, () => {
				}));
			} else {
				Global.CurrentGameId = 10301800;
				NetworkManager.Instance.GetGameRooms (1, 10301800);
				panelScript.gamelistPanel.SetActive(true);//创建房间列表
			}
		} else {//彩票
			OpenSelectView(lotteryId);
		}
    }

	void OnRoomMsg(MsgPara para){
		MsgSignal.Dispatch(para);
	}
		
	public void OnRoomInfo(List<ComRoomInfo> ComRoomInfos )
	{
		script.CreateIcons(ComRoomInfos);
	}
		
    void OnToggleClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
    }

    void OnToggleChange()
    {
		bool anyToggle= false;
        int temp=1;
		if (panelScript.mainToggle [0].value) {
			temp = 1;
			anyToggle =  true;
		} else if (panelScript.mainToggle [1].value) {
			temp = 2;
			anyToggle =  true;
		} else if (panelScript.mainToggle [2].value) {
			temp = 3;
			anyToggle =  true;
		}else if (panelScript.mainToggle [3].value) {
			temp = 4;
			anyToggle =  true;
		}
		if (comNameInfoList.Count==0) {
			panelScript.mainToggle [3].enabled = false;
		}

			
		if(temp!=toggleState&&anyToggle)
            InitIcons(lotteryCfgLoader);
    }

    public void OpenSelectView(int lotteryId)
    {
        GameObject Go = new GameObject();
        Go.name = "SelecterView";
        SelecterView sv = Go.AddComponent<SelecterView>();
        Go.transform.parent = contextTrans;
        sv.SetLotteryCfg(lotteryCfgLoader.GetLotteryConfig(lotteryId));

        // 顺便连confirm panel也创建好
        GameObject Gotemp = new GameObject();
        Gotemp.name = "SelectionConfirmView";
        Gotemp.AddComponent<SelectionConfirmView>();
        Gotemp.transform.parent = contextTrans;
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();

        Destroy(panel);
    }

    //never close main
    public void close()
    {
        Destroy(gameObject);
    }

    void OnBotBtn(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        string name = go.name;
        switch (name)
        {
			case "youxidating":
                break;
            case "huiyuanguanli":
                OpenHuiyuan();
                break;
            case "touzhujilu":
                OpenTouzhu();
                break;
            case "huodongzhongxin":
                OpenHuodong();
                break;
            case "yonghuzhongxin":
                OpenUser();
                break;
        }

    }

	public void UpdateGameList(List<ComNameInfo> cniList)
	{
		comNameInfoList.Clear();
		foreach (ComNameInfo cni in cniList) {
			comNameInfoList.Add (cni);
		}
		panelScript.mainToggle [3].enabled = true;
	}

    void OpenMain()
    {
       
    }

    void OpenHuiyuan()
    {
        GameObject Go = new GameObject();
        Go.name = "MemberView";
        Go.AddComponent<MemberView>();
        Go.transform.parent = contextTrans;
    }

    void OpenTouzhu()
    {
        GameObject Go = new GameObject();
        Go.name = "RecordView";
        Go.AddComponent<RecordView>();
        Go.transform.parent = contextTrans;
    }
    void OpenHuodong()
    {
        GameObject Go = new GameObject();
        Go.name = "ActivityView";
        Go.AddComponent<ActivityView>();
        Go.transform.parent = contextTrans;
    }
    void OpenUser()
    {
        GameObject Go = new GameObject();
        Go.name = "UserView";
        Go.AddComponent<UserView>();
        Go.transform.parent = contextTrans;
    }


    public void ProcessGG(int seq, string cont)
    {
        if (!gonggaoSeq.Contains(seq))
        {
            gonggaoSeq.Add(seq);
            gonggaoSeq.Sort();
        }

        if (!gonggaoDic.ContainsKey(seq))
            gonggaoDic.Add(seq, cont);
        else
            gonggaoDic[seq] = cont;

        if (!startplay)
            PlayGG();
    }

    void PlayGG()
    {
        if (seqIndex >= gonggaoSeq.Count)
        {
            startplay = false;
            return;
        }


        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("GonggaoLabel");
        GameObject ItemObj = Instantiate(asset) as GameObject;
        ItemObj.transform.parent = panelScript.GongGaoRoot;
        ItemObj.transform.localPosition = new Vector3(0, 2000, 0);
        ItemObj.transform.localScale = Vector3.one;
        ItemObj.name = seqIndex.ToString();

        GongGaoPanel gg = ItemObj.GetComponent<GongGaoPanel>();
       


        gg.SetData(gonggaoDic[gonggaoSeq[seqIndex]], OnGGPlayFinish);


        if (seqIndex == gonggaoSeq.Count - 1)
        {
            seqIndex = 0;
        }
        else
        {
            seqIndex++;
        }

        startplay = true;
    }

    void OnGGPlayFinish()
    {
        PlayGG();
    }
}


