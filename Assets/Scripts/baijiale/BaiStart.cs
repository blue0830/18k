using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using baijiale;


public class BaiStart : MonoBehaviour {

	static UI_main mainUI;

	UI_Head head;

	HashSet<UserInfoStruct> onLineUsers = new HashSet<UserInfoStruct>();//在线玩家

	HashSet<UserInfoStruct> offLineUsers = new HashSet<UserInfoStruct>();//离线玩家

	HashSet<UserInfoStruct> szUsers = new HashSet<UserInfoStruct>();

	UserInfoStruct myInfo;//玩家本人

	byte bGameStation;//游戏状态

	int currZhuangStation = 255;//当前庄家位置

	string currZName = "";//当前庄家名称

	int currZScore = 0;//当前庄家成绩

	int currZTScore = 0;//当前庄家总分

	int currGameCount = 0;//当前游戏局数

	int xzCDTime = 0;//下注时间

	int kpCDTime = 0;//开牌时间

	int kxCDTime = 0;//空闲时间

	IEnumerator recvTimeDown;

	int [] myBets = new int [8];

	bool isMyBet = false;

	bool waitOut = false;

	int myScore = 0;

	int betTotalMoney = 0;

	void Start () {
		Debug.LogWarning ("BaiStart start()");
		GameObject cv = GameObject.Find ("ContextView");
		if (cv != null) {
			cv.transform.Find ("UI Root").gameObject.SetActive (false);
		}

		SetAllPackageItemExtension ();

		GRoot.inst.SetContentScaleFactor (1136, 640);
		UIPackage.AddPackage ("baijiale/baijiale");
		mainUI = UIPackage.CreateObject ("baijiale", "main") as UI_main;
		GRoot.inst.AddChild (mainUI);

		head = mainUI.GetChild ("head") as UI_Head;
		GButton quit = head.GetChild ("quitBtn").asButton;
		quit.onClick.Add (OnLeave);

		Screen.orientation = ScreenOrientation.Landscape;


		//add by zjl
		mainUI.SetNotice("游戏服务器连接中....");
		TimeManager.Instance ().UnRegister ("loadGame");
		Loading.GetInstance ().HideLoading();

		//添加事件监听器
		EventMgr.ins.AddEventListener ("1_3", OnConnSucc);//游戏服务器连接成功
		//登录
		EventMgr.ins.AddEventListener ("100_4", OnLoginSucc);//登录游戏服务器成功
		EventMgr.ins.AddEventListener ("100_3", OnLoginError);//登录游戏服务器失败
		//玩家
		EventMgr.ins.AddEventListener("101_1",OnOnLineUserInfos);//推送在线玩家信息
		EventMgr.ins.AddEventListener ("101_2", OnOffLineUserInfos);//推送掉线玩家信息
		//桌子位置
		EventMgr.ins.AddEventListener("102_1",OnUpSucc);//玩家起身成功信息
		EventMgr.ins.AddEventListener("102_2",OnSitSucc);//玩家坐下成功信息
		EventMgr.ins.AddEventListener("102_8",OnSitError);//玩家坐下失败信息
		//进入&离开
		EventMgr.ins.AddEventListener("102_5",OnUserEnterSucc);//玩家进入游戏信息
		EventMgr.ins.AddEventListener("102_6",OnUserLeftSucc);//玩家离开游戏信息
		//金钱
		EventMgr.ins.AddEventListener("103_6",OnUserMoneyUpdate);//玩家金币更新
		//游戏
		EventMgr.ins.AddEventListener("150_1",OnGameStatus);//游戏状态
		EventMgr.ins.AddEventListener("150_2",OnGameScenes);//游戏场景

		EventMgr.ins.AddEventListener("180_130",OnXiaZhu);//游戏下注
		EventMgr.ins.AddEventListener("180_133",OnShangZhuang);//游戏上庄

		EventMgr.ins.AddEventListener("180_134",OnGameBegin);//游戏开始
		EventMgr.ins.AddEventListener("180_131",OnKaiPai);//游戏开牌
		EventMgr.ins.AddEventListener("180_136",OnGameJieSuan);//游戏结算

		EventMgr.ins.AddEventListener("1000_02",OnNetError);//断线重连

		GameSocket.GetInstance().Connect(Global.CurrentSelGameRoom.GetSzServiceIP(),(int)Global.CurrentSelGameRoom.uServicePort);
	}

	void Awake(){
		Application.targetFrameRate = 30;//每秒30帧
		Screen.sleepTimeout = SleepTimeout.NeverSleep;//屏幕常亮
	}
		
	void OnNetError(string str,System.Object obj){
		UI_Alert.ShowMsg("无法连接，请检查您的网络",()=>{
			Leave();
		},()=>{
			Leave();
		});
	}

	void PlayGameSound(int type,string sound){
		if (type == 1) {//游戏背景
			if(Global.GameBackgroundMusic)
				AudioController.Instance.SoundGamePlay("baccarat/"+sound);
		}
		if (type == 2) {//游戏音效
			if(Global.GameEffectMusic)
				AudioController.Instance.SoundGamePlay("baccarat/"+sound);
		}
	}

	void ShowSzList(int [] array){
		szUsers.Clear();
		for (int i = 0; i < array.Length; i++) {
			int station = array[i];
			foreach(UserInfoStruct user in onLineUsers){
				if (station == (int)user.bDeskStation) {
					szUsers.Add(user);
				}
			}
		}
		RefreshSzList();
	}

	void RefreshSzList(){
		mainUI.SetZhuangData (new List<UserInfoStruct>(szUsers));
	}

	void Show30History(int [] array){
		List<BaccaraHistory> histories = new List<BaccaraHistory>();
		bool flag = true;
		for (int i = 0; i < 30; ++i)
		{
			BaccaraHistory history = new BaccaraHistory();
			for (int j = i*4,end = i*4+4,index =0; j<array.Length&&j < end; ++j,++index)
			{
				if (index == 0) {
					history.zPoint = array [j];
				}
				if (index == 1) {
					history.xPoint = array [j];
				}
				if (index == 2) {
					if(flag&&array[j]==0){
						flag = false;
					}
					history.winner = array[j];//1庄2闲3和
				}
			}
			histories.Add (history);
		}
		if(flag)
			mainUI.InitLudan (histories);
	}


	void Leave(){
		GameObject cv = GameObject.Find ("ContextView");
		if (cv != null) {
			cv.transform.Find ("UI Root").gameObject.SetActive (true);
		}
		Global.IsLoginGame = false;
		AudioController.Instance.SoundAllStop();
		if (recvTimeDown != null) {
			CoroutineTool.inst.StopCoroutine (recvTimeDown);
		}
			
		onLineUsers.Clear();
		offLineUsers.Clear();
		szUsers.Clear();
		currZhuangStation = 255;//当前庄家位置
		currZName = "";//当前庄家名称
 		currZScore = 0;//当前庄家成绩
		currZTScore = 0;//当前庄家总分
		currGameCount = 0;//当前游戏局数
		xzCDTime = 0;//下注时间
		kpCDTime = 0;//开牌时间
		kxCDTime = 0;//空闲时间
		recvTimeDown = null;
		myBets = new int [8];
		isMyBet = false;
		waitOut = false;
		myScore = 0;
		betTotalMoney = 0;

		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		mainUI.Dispose();
		GRoot.inst.RemoveChildren();
		Screen.orientation = ScreenOrientation.Portrait;

		GameSocket.GetInstance().ManualShutDown ();
		NetworkManager.Instance.RefreshMoney ();//刷新金额
	}

	void OnLeave(){
		if (Global.IsLoginGame) {
			if (bGameStation != 20 && bGameStation != 22) {//非游戏状态
				waitOut = true;
				GameNetworkManager.Instance.getUp ();
			} else {
				if (!isMyBet && (currZhuangStation != (int)myInfo.bDeskStation)) {//我自己是否下注了&&并且不是庄家
					waitOut = true;
					GameNetworkManager.Instance.getQuit ();
				} else {
					UI_Alert.ShowMsg ("您现在退出将会被系统托管您的游戏，是否真的要退出?", () => {
						Leave ();
					});
				}
			}
		} else {//游戏还在登录中
			Leave ();
		}
	}

	void OnGameBegin(string str,System.Object obj){
		EventMgr.NetMsg msg = (EventMgr.NetMsg)obj;
		BeginData info = (BeginData)msg.para;
		bGameStation = 20;
		isMyBet = false;
		PlayGameSound(2,"start");
		mainUI.ClearDesktopMoney();
		mainUI.ClearDesktopClip();
		mainUI.SetNotice("游戏开始了,请您下分...");
		mainUI.szLimit = info.m_iShangZhuangLimit;
		currZhuangStation = info.m_iNowNtStation;
		currZScore = info.m_iNTdata [2];
		currZTScore = info.m_iNTdata [1];
		currGameCount = info.m_iNTdata[3]+1;
		foreach(UserInfoStruct user in onLineUsers){
			if (info.m_iNTdata[0] == (int)user.bDeskStation) {
				currZName = user.GetNickName ();
			}
		}
		mainUI.ShowZhuangInfo(currZName,currZScore,currZTScore,currGameCount);
		//处理游戏开始UI逻辑
		ShowSzList(info.m_iNTlist);
		Show30History(info.m_iResultInfo);
		mainUI.SetXiafenItem(info.m_iMaxZhu);
		if (currZhuangStation == (int)myInfo.bDeskStation) {
			mainUI.InitMoneyDisable (0);//自己是庄家，禁用
		} else {
			mainUI.InitMoneyDisable (betTotalMoney);
		}
		xzCDTime = info.m_iXiaZhuTime;
		kpCDTime = info.m_iKaiPaiTime;
		kxCDTime = info.m_iFreeTime;
		ShowCountDownTime ("下注时间",Time.time,xzCDTime,CalcXZTime);
	}

	void CalcXZTime(int leftTime){
		if (leftTime < 6&&leftTime > 0) {
			PlayGameSound(2,"warning");
		}
		if (leftTime == 17) {
			PlayGameSound(2,"game_start_bg");
		}
	}

	void CalcKPTime(int leftTime){
		if (leftTime == 1) {
			int [] array = new int[8];
			mainUI.SetXiafenItem(array);
		}
		if (leftTime == 0) {
			mainUI.SetNotice("空闲时间...");
			bGameStation = 23;
			myBets = new int[8];
			mainUI.ClearDesktopMoney();
			mainUI.ClearDesktopClip();
			mainUI.InitMoneyDisable(0);
			ShowCountDownTime ("空闲时间",Time.time,kxCDTime);
			PlayGameSound(2,"game_end");
		}
	}

	void ShowCountDownTime(string title,float starTime,float timeLength,Action<int> act = null){
		if (recvTimeDown != null) {
			CoroutineTool.inst.StopCoroutine (recvTimeDown);
		}
		recvTimeDown = mainUI.CountDownTime (title,starTime, timeLength, act);
		CoroutineTool.inst.StartCoroutine(recvTimeDown);
	}

	void OnKaiPai(string str,System.Object obj){
		EventMgr.NetMsg msg = (EventMgr.NetMsg)obj;
		KaiPai info = (KaiPai)msg.para;
		bGameStation = 22;
		mainUI.InitMoneyDisable(0);
		mainUI.SetNotice("开牌时间...");
		ShowCountDownTime ("开牌时间",Time.time,kpCDTime,CalcKPTime);
		Show30History(info.m_iResultInfo);
		ShowSzList(info.zhuangstation);
		int zToD, xToD;
		if (info.m_iZPaiXing [3] > 0 && info.m_iZPaiXing [2] > 0) {
			zToD = 3;//天王对子
		} else if (info.m_iZPaiXing [3] > 0) {
			zToD = 2;//对子
		} else if (info.m_iZPaiXing [2] > 0) {
			zToD = 1;//天王
		} else {
			zToD = -1;
		}
		if (info.m_iXPaiXing [3] > 0 && info.m_iXPaiXing [2] > 0) {
			xToD = 3;//天王对子
		} else if (info.m_iXPaiXing [3] > 0) {
			xToD = 2;//对子
		} else if (info.m_iXPaiXing [2] > 0) {
			xToD = 1;//天王
		} else {
			xToD = -1;
		}
		//m_iZPaiXing 庄家牌型,元素0前两张牌的值，元素1总牌值，元素2天王，元素3对子，元素4和
		//		for(int i =0;i<info.m_iZPaiXing.Length;i++){
		//			if (i == 0) {
		//				Debug.LogWarning ("庄的前两张牌的值:"+info.m_iZPaiXing[i]);
		//			}
		//			if (i == 1) {
		//				Debug.LogWarning ("庄的总牌值:"+info.m_iZPaiXing[i]);
		//			}
		//			if (i == 2) {
		//				Debug.LogWarning ("庄的天王值:"+info.m_iZPaiXing[i]);
		//			}
		//			if (i == 3) {
		//				Debug.LogWarning ("庄的对子值:"+info.m_iZPaiXing[i]);
		//			}
		//			//这块显示逻辑我和你电话沟通说清楚
		//		}
		//		//m_iXPaiXing 闲家牌型,元素0前两张牌的值，元素1总牌值，元素2天王，元素3对子，元素4和
		//		for(int i =0;i<info.m_iXPaiXing.Length;i++){
		//			if (i == 0) {
		//				Debug.LogWarning ("闲的前两张牌的值:"+info.m_iXPaiXing[i]);
		//			}
		//			if (i == 1) {
		//				Debug.LogWarning ("闲的总牌值:"+info.m_iXPaiXing[i]);
		//			}
		//			if (i == 2) {
		//				Debug.LogWarning ("闲的天王值:"+info.m_iXPaiXing[i]);
		//			}
		//			if (i == 3) {
		//				Debug.LogWarning ("闲的对子值:"+info.m_iXPaiXing[i]);
		//			}
		//		}
		mainUI.ShowPai(info.getCards().zCards,info.m_iZPaiXing[0],info.m_iZPaiXing[1],zToD,info.getCards().xCards,info.m_iXPaiXing[0],info.m_iXPaiXing[1],xToD);
	}

	void OnGameJieSuan(string str,System.Object obj){
		EventMgr.NetMsg msg = (EventMgr.NetMsg)obj;
		JieSuan info = (JieSuan)msg.para;
		bGameStation = 22;
		PlayGameSound(2,"calc");
		mainUI.InitMoneyDisable(0);
		ShowSzList(info.zhuangstation);
		Show30History(info.m_iResultInfo);
		if ((int)myInfo.bDeskStation == currZhuangStation) {//自己就是庄家
			mainUI.ShowSettlement(info.m_iUserFen,info.m_iMeFanFen,0);
		} else {
			mainUI.ShowSettlement(info.m_iUserFen,info.m_iMeFanFen,info.m_iZhuangFen);
		}
//		for (int i = 0; i < info.m_iWinQuYu.Length; i++) {
//			Debug.LogWarning ("中奖区域 i:"+i+",money:"+info.m_iWinQuYu[i]);
//		}
		mainUI.ShowDesktopWins(info.m_iWinQuYu);

		Debug.LogWarning ("庄家的得分:"+info.m_iZhuangFen);
		Debug.LogWarning ("闲家的得分:"+info.m_iXianFen);
		Debug.LogWarning ("当前玩家的得分:"+info.m_iUserFen);
		Debug.LogWarning ("当前庄家赢的金币:"+info.m_iNtWin);

		currZScore += info.m_iZhuangFen;
		currZTScore += info.m_iZhuangFen;
		mainUI.ShowZhuangInfo(currZName,currZScore,currZTScore,currGameCount);
	}

	void OnShangZhuang(string str,System.Object obj){
		EventMgr.NetMsg msg = (EventMgr.NetMsg)obj;
		ShangZhuang info = (ShangZhuang)msg.para;
		ShowSzList(info.zhuangstation);
		if (info.success == 1) {//上庄成功
			if ((int)myInfo.bDeskStation == info.station) {
				if (info.shang == 1) {
					//按钮UI需要变为“申请下庄”
					mainUI.SetShangZhuangBtn (false);
				} else {
					//按钮UI需要变为“申请上庄”
					mainUI.SetShangZhuangBtn (true);
				}
			}
		} else {
			UI_InfoMsg.Show("上庄失败");
		}
	}

	void OnGameScenes(string str,System.Object obj){
		EventMgr.NetMsg msg = (EventMgr.NetMsg)obj;
		ChongLian info = (ChongLian)msg.para;
		mainUI.ClearDesktopMoney();
		mainUI.ClearDesktopClip();
		mainUI.InitMoneyDisable(0);
		mainUI.szLimit = info.m_iShangZhuangLimit;
		currZhuangStation = info.m_iNowNtStation;
		currZScore = info.m_iNTdata [2];
		currZTScore = info.m_iNTdata [1];
		currGameCount = info.m_iNTdata[3]+1;
		foreach(UserInfoStruct user in onLineUsers){
			if (info.m_iNTdata[0] == (int)user.bDeskStation) {
				currZName = user.GetNickName ();
			}
		}

		Show30History(info.m_iResultInfo);
		mainUI.SetXiafenItem(info.m_iMaxZhu);
		ShowSzList(info.zhuangstation);
		foreach (int station in info.zhuangstation) {
			if ((int)myInfo.bDeskStation == station) {
				mainUI.SetShangZhuangBtn (false);
			}
		}
		mainUI.ShowZhuangInfo("",0,0,0);
		if(bGameStation==0||bGameStation==1){//游戏等待状态
			mainUI.SetNotice("当前无庄,申请即可上庄");
			mainUI.SetXiafenItem(new int[8]);
		}
		if(bGameStation==20){//游戏下注状态
			mainUI.SetNotice("游戏开始了,请您下分...");
			currZScore = info.m_iNTdata [2];
			currZTScore = info.m_iNTdata [1];
			currGameCount = info.m_iNTdata[3]+1;
			foreach(UserInfoStruct user in onLineUsers){
				if (info.m_iNTdata[0] == (int)user.bDeskStation) {
					currZName = user.GetNickName ();
				}
			}
			mainUI.ShowZhuangInfo(currZName,currZScore,currZTScore,currGameCount);
			mainUI.ShowDesktopMoney(info.m_iQuYuZhu);
			mainUI.ShowMeDesktopMoney(info.m_iMeZhu);

			betTotalMoney -= info.m_iMeZhu[8]; 
			if (currZhuangStation == (int)myInfo.bDeskStation) {
				mainUI.InitMoneyDisable (0);//自己是庄家，禁用
			} else {
				mainUI.InitMoneyDisable (betTotalMoney);
			}

			xzCDTime = info.m_iSYTime-4;//开始倒计时（必须要减去4倒计时）
			kpCDTime = 15;
			kxCDTime = 10;
			ShowCountDownTime ("下注时间",Time.time,xzCDTime,CalcXZTime);
			for(int i = 0;i<8;i++){//本把每个区域下的注额
				//0庄下注区域 1庄天王下注区域 2庄对子下注区域 3 闲下注区域 4闲天王下注区域 5闲对子下注区域 6和下注区域 7同点和下注区域
				if (info.m_iQuYuZhu [i] > 0) {
					//1:1元，2:10元，3:100元，4:100元，5:5000元，6:1w元
					int left = info.m_iQuYuZhu [i];
					int count = left / 1000000;
					left = count > 0 ? (left % 1000000) : left;
					ShowBetInfo (i, count, 6);

					count = left / 500000;
					left = count > 0 ? (left % 500000) : left;
					ShowBetInfo (i, count, 5);

					count = left / 100000;
					left = count > 0 ? (left % 100000) : left;
					ShowBetInfo (i, count, 4);

					count = left / 10000;
					left = count > 0 ? (left % 10000) : left;
					ShowBetInfo (i, count, 3);

					count = left / 1000;
					left = count > 0 ? (left % 1000) : left;
					ShowBetInfo (i, count, 2);

					count = left / 100;
					left = count > 0 ? (left % 100) : left;
					ShowBetInfo (i, count, 1);
				}
			}
		}
		if(bGameStation==22){//游戏开牌&结算状态
			mainUI.SetNotice("游戏开始开牌...");
			currZScore = info.m_iNTdata [2];
			currZTScore = info.m_iNTdata [1];
			currGameCount = info.m_iNTdata[3]+1;
			foreach(UserInfoStruct user in onLineUsers){
				if (info.m_iNTdata[0] == (int)user.bDeskStation) {
					currZName = user.GetNickName ();
				}
			}
			mainUI.ShowZhuangInfo(currZName,currZScore,currZTScore,currGameCount);
			xzCDTime = 0;
			kpCDTime = info.m_iSYTime;
			kxCDTime = 10;
			mainUI.ShowMeDesktopMoney(info.m_iMeZhu);
			mainUI.ShowDesktopMoney(info.m_iQuYuZhu);

			ShowCountDownTime ("开牌时间",Time.time,kpCDTime,CalcKPTime);
			int zToD, xToD;
			if (info.m_iZPaiXing [3] > 0 && info.m_iZPaiXing [2] > 0) {
				zToD = 3;//天王对子
			} else if (info.m_iZPaiXing [3] > 0) {
				zToD = 2;//对子
			} else if (info.m_iZPaiXing [2] > 0) {
				zToD = 1;//天王
			} else {
				zToD = -1;
			}
			if (info.m_iXPaiXing [3] > 0 && info.m_iXPaiXing [2] > 0) {
				xToD = 3;//天王对子
			} else if (info.m_iXPaiXing [3] > 0) {
				xToD = 2;//对子
			} else if (info.m_iXPaiXing [2] > 0) {
				xToD = 1;//天王
			} else {
				xToD = -1;
			}
			mainUI.ShowPai(info.getCards().zCards,info.m_iZPaiXing[0],info.m_iZPaiXing[1],zToD,info.getCards().xCards,info.m_iXPaiXing[0],info.m_iXPaiXing[1],xToD);
			for(int i = 0;i<8;i++){//本把每个区域下的注额
				//0庄下注区域 1庄天王下注区域 2庄对子下注区域 3 闲下注区域 4闲天王下注区域 5闲对子下注区域 6和下注区域 7同点和下注区域
				if (info.m_iQuYuZhu [i] > 0) {
					//1:1元，2:10元，3:100元，4:100元，5:5000元，6:1w元
					int left = info.m_iQuYuZhu [i];
					int count = left / 1000000;
					left = count > 0 ? (left % 1000000) : left;
					ShowBetInfo (i, count, 6);

					count = left / 500000;
					left = count > 0 ? (left % 500000) : left;
					ShowBetInfo (i, count, 5);

					count = left / 100000;
					left = count > 0 ? (left % 100000) : left;
					ShowBetInfo (i, count, 4);

					count = left / 10000;
					left = count > 0 ? (left % 10000) : left;
					ShowBetInfo (i, count, 3);

					count = left / 1000;
					left = count > 0 ? (left % 1000) : left;
					ShowBetInfo (i, count, 2);

					count = left / 100;
					left = count > 0 ? (left % 100) : left;
					ShowBetInfo (i, count, 1);
				}
			}
			if ((int)myInfo.bDeskStation == currZhuangStation) {//自己就是庄家
				mainUI.ShowSettlement(info.m_iUserFen,0,0);
			} else {
				mainUI.ShowSettlement(info.m_iUserFen,0,info.m_iZhuangFen);
			}
		}
		if(bGameStation==23){//游戏空闲状态
			mainUI.SetNotice("空闲时间...");
			xzCDTime = 0;
			kpCDTime = 0;
			kxCDTime = info.m_iSYTime;
			ShowCountDownTime ("空闲时间",Time.time,kxCDTime);
		}
	}

	void OnGameStatus(string str,System.Object obj){
		EventMgr.NetMsg msg = (EventMgr.NetMsg)obj;
		myScore = 0;// 分数清零
		bGameStation = ((MSG_GM_S_GameInfo)msg.para).bGameStation;
	}

	void ShowBetInfo(int type,int count,int moneytype){
		if(count>0){
			for(int i = 0;i<count;i++){
				System.Random ran=new System.Random();
				int xSeed=ran.Next(10,100);
				int ySeed=ran.Next(10,100);
				mainUI.ShowDesktopClip(type,moneytype,xSeed,ySeed);
			}
		}
	}

	void OnUserMoneyUpdate(string str,System.Object obj){
		EventMgr.NetMsg msg = (EventMgr.NetMsg)obj;
		MSG_GR_R_UserPoint point = (MSG_GR_R_UserPoint)msg.para;
		if (Global.CurrentUserId == point.dwUserID) {//自己
			myInfo.dwPoint += point.dwPoint;
			myInfo.dwMoney += point.dwMoney;
			betTotalMoney = myInfo.dwMoney;
			myScore += point.dwMoney;
			myInfo.uLostCount += point.bLostCount;
			myInfo.uWinCount += point.bWinCount;
			myInfo.uMidCount += point.bMidCount;
			myInfo.uCutCount += point.bCutCount;
			double d = myInfo.dwMoney * 1.0 / 100;
			string s = System.String.Format("{0:0.00}", d);
			double c = myScore * 1.0 / 100;
			string e = System.String.Format("{0:0.00}", c);
			head.initGoldData (s, e);
		}else{
			foreach(UserInfoStruct user in onLineUsers){
				if (point.dwUserID == user.dwUserID) {//更新玩家信息的数据
					user.dwPoint += point.dwPoint;
					user.dwMoney += point.dwMoney;
					user.uLostCount += point.bLostCount;
					user.uWinCount += point.bWinCount;
					user.uMidCount += point.bMidCount;
					user.uCutCount += point.bCutCount;
				}
			}
		}
		RefreshSzList();
	}

	void OnUserEnterSucc(string str,System.Object obj){
		EventMgr.NetMsg msg = (EventMgr.NetMsg)obj;
		UserInfoStruct user = (UserInfoStruct)msg.para;
		if(onLineUsers.Contains(user))
			onLineUsers.Add(user);//添加用户信息
	}

	void OnUserLeftSucc(string str,System.Object obj){
		EventMgr.NetMsg msg = (EventMgr.NetMsg)obj;
		MSG_GR_R_UserLeft leftInfo = (MSG_GR_R_UserLeft)msg.para;
		UserInfoStruct leftUser = null;
		foreach (UserInfoStruct user in onLineUsers) {
			if (leftInfo.dwUserID == user.dwUserID)
				leftUser = user;
		}
		if(leftUser!=null)
			onLineUsers.Remove(leftUser);//移除用户信息
	}

	void OnUpSucc(string str,System.Object obj){
		EventMgr.NetMsg msg = (EventMgr.NetMsg)obj;
		MSG_GR_R_UserSit sitInfo = (MSG_GR_R_UserSit)msg.para;
		if (Global.CurrentUserId == sitInfo.dwUserID) {//如果是自己起身，就需要离开游戏
			Leave();
		}else{
			foreach(UserInfoStruct user in onLineUsers){
				if (sitInfo.dwUserID == user.dwUserID) {//更新玩家信息的数据
					user.bDeskStation = 255;
				}
			}
		}
	}

	void OnSitError(string str,System.Object obj){
		EventMgr.NetMsg netMsg = (EventMgr.NetMsg)obj;
		NetMessageHead head = netMsg.head;
		string msg = "抱歉,服务器网络繁忙,请重新进入游戏房间后选择游戏桌位置!";
		if (head.bHandleCode == 51) {
			msg = "坐下此位置失败，游戏已经开始了!";
		}
		if (head.bHandleCode == 52) {
			msg = "坐下此位置失败，下次动作快一点喔!";
		}
		if (head.bHandleCode == 53) {
			msg = "游戏桌密码错误，请在游戏设置中重新设置您的携带密码!";
		}
		if (head.bHandleCode == 54) {
			msg = "同桌玩家不允许有相同IP地址的玩家一起进行游戏!";
		}
		if (head.bHandleCode == 55) {
			msg = "同桌的玩家认为您的逃跑率太高,不愿意和您游戏!";
		}
		if (head.bHandleCode == 56) {
			msg = "同桌的玩家认为您的积分太低,不愿意和您游戏!";
		}
		if (head.bHandleCode == 57) {
			msg = "同桌的玩家认为您的积分太高,不愿意和您游戏!";
		}
		if (head.bHandleCode == 58) {
			msg = "此桌有您不欢迎的玩家!";
		}
		if (head.bHandleCode == 59) {
			msg = "此游戏桌需要至少%ld 的游戏积分,您的积分不够,不能游戏!";
		}
		if (head.bHandleCode == 60) {
			if (waitOut) {
				UI_Alert.ShowMsg ("您现在退出将会被系统托管您的游戏，是否真的要退出?", () => {
					Leave ();
				});
			} else {
				mainUI.SetNotice("您正在游戏中...");
			}
			return;
		}
		if (head.bHandleCode == 61) {
			msg = "您不能加入此游戏桌游戏!";
		}
		if (head.bHandleCode == 62) {
			msg = "您的比赛已经结束了,不能继续参加比赛!";
		}
		UI_Alert.ShowMsg (msg,()=> { Leave(); },()=> { Leave(); });
	}

	void OnSitSucc(string str,System.Object obj){
		EventMgr.NetMsg msg = (EventMgr.NetMsg)obj;
		MSG_GR_R_UserSit sitInfo = (MSG_GR_R_UserSit)msg.para;
		int uid = (int)sitInfo.dwUserID;
		if (uid == myInfo.dwUserID) {//更新玩家本人信息的数据
			myInfo.bDeskStation = sitInfo.bDeskStation;
			myInfo.bUserState = sitInfo.bUserState;
			GameNetworkManager.Instance.getGameStatus();//获取游戏信息
		} else {
			foreach(UserInfoStruct user in onLineUsers){
				if (uid == user.dwUserID) {//更新玩家信息的数据
					user.bDeskStation = sitInfo.bDeskStation;
					user.bUserState = sitInfo.bUserState;
				}
			}
		}
	}

	void OnOffLineUserInfos(string str,System.Object obj){
		EventMgr.NetMsg msg = (EventMgr.NetMsg)obj;
		List<UserInfoStruct> users = (List<UserInfoStruct>)msg.para;
		foreach(UserInfoStruct user in users){
			offLineUsers.Add(user);//更新掉线玩家信息列表
		}
	}

	void OnOnLineUserInfos(string str,System.Object obj){
		//这是首次进入游戏把当前房间的用户信息都发给客户端
		//保存房间玩家列表数据
		EventMgr.NetMsg msg = (EventMgr.NetMsg)obj;
		List<UserInfoStruct> users = (List<UserInfoStruct>)msg.para;
		foreach(UserInfoStruct user in users){
			onLineUsers.Add(user);//更新当前在线玩家信息列表
			if((uint)user.dwUserID==Global.CurrentUserId){//保存玩家自己信息的数据
				myInfo = user;
				betTotalMoney = myInfo.dwMoney;
				double d = myInfo.dwMoney * 1.0 / 100;
				string s = System.String.Format("{0:0.00}", d);
				double c = myScore * 1.0 / 100;
				string e = System.String.Format("{0:0.00}", c);
				head.initGoldData (s, e);
				head.initPlayData (myInfo.dwUserID.ToString(), myInfo.GetNickName ());
			}
		}
		byte bDeskStation = 0 ;//坐下的位置
		for (byte i = 0; i < 180; i++) {//0～179个位置，按照顺序查找可以坐下的位置
			bool exist = false;
			foreach(UserInfoStruct user in users){
				if (user.bDeskStation != 255&&i == user.bDeskStation) {
					exist = true;
					break;
				}
			}
			if (!exist) {
				bDeskStation = i;
				break;
			}
		}
		GameNetworkManager.Instance.Sit(bDeskStation);//找到位置坐下
	}

	void OnLoginSucc(string str,System.Object obj){
		Global.IsLoginGame = true;
		mainUI.SetNotice("游戏房间坐下中....");
	}

	void OnLoginError(string str,System.Object obj){
		EventMgr.NetMsg netMsg = (EventMgr.NetMsg)obj;
		NetMessageHead head = netMsg.head;
		string msg = "";
		if (head.bHandleCode == 0) {
			msg = "您的账号存在异常，请稍后再次登录，如仍然无效，请联系客服";
		}
		if (head.bHandleCode == 3) {
			msg = "用户不存在或者密码错误";
		}
		if (head.bHandleCode == 2) {
			//msg = "您是否在大厅游戏过程中更改了登录密码,请关闭大厅重新登录再进入游戏,如有疑问,请联系客服";
			msg = "用户不存在或者密码错误";
		}
		if (head.bHandleCode == 4) {
			msg = "您强退或频繁进出游戏房间，帐号被托管。您可以稍后返回游戏或再进入其他游戏房间!如仍然无效，请联系本站客服!";
		}
		if (head.bHandleCode == 5) {
			msg = "登录IP禁止";
		}
		if (head.bHandleCode == 6) {
			msg = "不是指定地址";
		}
		if (head.bHandleCode == 7) {
			msg = "会员游戏房间";
		}
		if (head.bHandleCode == 9) {
			msg = "此账号正在使用中";
		}
		if (head.bHandleCode == 13) {
			msg = "暂停登录服务器";
		}
		if (head.bHandleCode == 160) {
			msg = "比赛游戏房间";
		}
		if (head.bHandleCode == 161) {
			msg = "时间到期";
		}
		UI_Alert.ShowMsg (msg,()=> { Leave(); },()=> { Leave(); });
	}

	void OnConnSucc(string str,System.Object obj){
		//UI_InfoMsg.Show("用户密码:" + Global.CurrentUserPwd);
		GameNetworkManager.Instance.Login();//登录游戏服务器
		mainUI.SetNotice("游戏服务器登录中....");
	}

	void OnXiaZhu(string str,System.Object obj){
		EventMgr.NetMsg msg = (EventMgr.NetMsg)obj;
		XiaZhu info = (XiaZhu)msg.para;
		mainUI.SetXiafenItem(info.m_iMaxZhu);
		if ((int)myInfo.bDeskStation == info.station) {
			betTotalMoney -= info.money;
			mainUI.InitMoneyDisable(betTotalMoney);
			myBets [info.type] += info.money;
			mainUI.ShowMeDesktopMoney(myBets);
			isMyBet = true;//自己下注标识
		}
		if (info.moneytype > 3&&info.moneytype < 6) {
			PlayGameSound(2,"bet_1k_5k");
		}else if(info.moneytype == 6) {
			PlayGameSound(2,"bet_1w");
		} else {
			PlayGameSound(2,"bet");
		}
		mainUI.ShowDesktopMoney(info.m_iQuYuZhu);
		mainUI.ShowDesktopClip(info.type,info.moneytype,info.getXSeed(),info.getYSeed());
	}

	public static void SetAllPackageItemExtension()
	{
		UIObjectFactory.SetPackageItemExtension(UI_zhuangjiaList.URL, typeof(UI_zhuangjiaList));
		UIObjectFactory.SetPackageItemExtension(UI_xiafenList.URL, typeof(UI_xiafenList));
		UIObjectFactory.SetPackageItemExtension(UI_main.URL, typeof(UI_main));
		UIObjectFactory.SetPackageItemExtension(UI_Head.URL, typeof(UI_Head));
		UIObjectFactory.SetPackageItemExtension(UI_ludan.URL, typeof(UI_ludan));
		UIObjectFactory.SetPackageItemExtension(UI_LudanItem.URL, typeof(UI_LudanItem));
		UIObjectFactory.SetPackageItemExtension(UI_Settlement.URL, typeof(UI_Settlement));
		UIObjectFactory.SetPackageItemExtension(UI_Alert.URL, typeof(UI_Alert));
		UIObjectFactory.SetPackageItemExtension(UI_SetUI.URL, typeof(UI_SetUI));
		UIObjectFactory.SetPackageItemExtension(UI_InfoMsg.URL, typeof(UI_InfoMsg));
	}

	void OnDestroy(){
		//添加事件监听器
		EventMgr.ins.RemoveEventListner ("1_3", OnConnSucc);//游戏服务器连接成功
		//登录
		EventMgr.ins.RemoveEventListner ("100_4", OnLoginSucc);//登录游戏服务器成功
		EventMgr.ins.RemoveEventListner ("100_3", OnLoginError);//登录游戏服务器失败
		//玩家
		EventMgr.ins.RemoveEventListner("101_1",OnOnLineUserInfos);//推送在线玩家信息
		EventMgr.ins.RemoveEventListner ("101_2", OnOffLineUserInfos);//推送掉线玩家信息
		//桌子位置
		EventMgr.ins.RemoveEventListner("102_1",OnUpSucc);//玩家起身成功信息
		EventMgr.ins.RemoveEventListner("102_2",OnSitSucc);//玩家坐下成功信息
		EventMgr.ins.RemoveEventListner("102_8",OnSitError);//玩家坐下失败信息
		//进入&离开
		EventMgr.ins.RemoveEventListner("102_5",OnUserEnterSucc);//玩家进入游戏信息
		EventMgr.ins.RemoveEventListner("102_6",OnUserLeftSucc);//玩家离开游戏信息
		//金钱
		EventMgr.ins.RemoveEventListner("103_6",OnUserMoneyUpdate);//玩家金币更新
		//游戏
		EventMgr.ins.RemoveEventListner("150_1",OnGameStatus);//游戏状态
		EventMgr.ins.RemoveEventListner("150_2",OnGameScenes);//游戏场景

		EventMgr.ins.RemoveEventListner("180_130",OnXiaZhu);//游戏下注
		EventMgr.ins.RemoveEventListner("180_133",OnShangZhuang);//游戏上庄

		EventMgr.ins.RemoveEventListner("180_134",OnGameBegin);//游戏开始
		EventMgr.ins.RemoveEventListner("180_131",OnKaiPai);//游戏开牌
		EventMgr.ins.RemoveEventListner("180_136",OnGameJieSuan);//游戏结算
	}

	void Update () {
		if ((Input.GetKeyDown(KeyCode.Escape))){
			OnLeave();
		}
		if (Global.GameTryReConnTimes > GameSocket.MAXRETRYTTIMES) //重试
		{
			Global.GameTryReConnTimes = 0;
			UI_Alert.ShowMsg("连接游戏服务器超时，是否重试?",()=>{
				GameSocket.GetInstance ().ManualShutDown();
				GameSocket.GetInstance ().Connect();
			},()=>{
				Leave();
			});
		}
		EventMgr.ins.DispEvent(EventMgr.EnterFrame,null);
		if (Global.LastGameHeartBeatTime!=0&&Global.IsLoginGame) {
			ulong nowTime = TimeHelper.GetNowTime ();
			ulong left = nowTime - Global.LastGameHeartBeatTime;
			if (left > 15) {//回包超时跳出提示，重新登录
				if(Global.CurrentGameId==10301800){//重连
					GameSocket.GetInstance ().ManualShutDown();
					GameSocket.GetInstance().Connect(Global.CurrentSelGameRoom.GetSzServiceIP(),(int)Global.CurrentSelGameRoom.uServicePort);
					Global.LastGameHeartBeatTime = 0;
				}
			}
		}
	}

	void OnApplicationPause(bool Pause)
	{
		if (Pause){
			Global.LastGameHeartBeatTime = 0;//防止网络检查与此处冲突
			GameSocket.GetInstance().Pause();
		}else{
			GameSocket.GetInstance().Resume();
		}
	}
}
