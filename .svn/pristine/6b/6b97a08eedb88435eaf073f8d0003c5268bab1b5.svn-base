/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

namespace baijiale
{
	public class UI_main : GComponent
	{
		public GButton m_sqsz;
		public GButton m_lishi;
		public GButton m_gold1;
		public GButton m_gold2;
		public GButton m_gold3;
		public GButton m_gold4;
		public GButton m_gold6;
		public GButton m_gold5;
		public GTextField m_timeText;
		public UI_Head m_head;
		public GList m_zhuangList;
		public GList m_xianfenList;
		public GButton m_xia5;
		public GButton m_xia0;
		public GButton m_xia2;
		public GButton m_xia6;
		public GButton m_xia4;
		public GButton m_xia3;
		public GButton m_xia7;
		public GButton m_xia1;
		public GButton m_sqxz;

		public GTextField[] m_desktopAll = new GTextField[8];
		public GTextField[] m_desktopMe = new GTextField[8];
		public GComponent m_goldBox;

		public GTextField m_notice;
		public GTextField m_timeDesc;
		public GTextField m_zMessage;
		public GImage m_waitZhuang;

		public int szLimit = 0;//上庄的金币限制
		UI_ludan ludanUI;
		PokeBar bar = new PokeBar ();

		public const string URL = "ui://nyvoaldgsazt39";

		public static UI_main CreateInstance()
		{
			return (UI_main)UIPackage.CreateObject("baijiale","main");
		}

		public UI_main()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_sqsz = (GButton)this.GetChild("sqsz");
			m_lishi = (GButton)this.GetChild("lishi");
			m_gold1 = (GButton)this.GetChild("gold1");
			m_gold2 = (GButton)this.GetChild("gold2");
			m_gold3 = (GButton)this.GetChild("gold3");
			m_gold4 = (GButton)this.GetChild("gold4");
			m_gold6 = (GButton)this.GetChild("gold6");
			m_gold5 = (GButton)this.GetChild("gold5");
			m_timeText = (GTextField)this.GetChild("timeText");
			m_head = (UI_Head)this.GetChild("head");
			m_zhuangList = (GList)this.GetChild("zhuangList");
			m_xianfenList = (GList)this.GetChild("xianfenList");
			m_sqxz = (GButton)this.GetChild("sqxz");
			m_xia5 = (GButton)this.GetChild("xia5");
			m_xia3 = (GButton)this.GetChild("xia3");
			m_xia4 = (GButton)this.GetChild("xia4");
			m_xia6 = (GButton)this.GetChild("xia6");
			m_xia7 = (GButton)this.GetChild("xia7");
			m_xia2 = (GButton)this.GetChild("xia2");
			m_xia0 = (GButton)this.GetChild("xia0");
			m_xia1 = (GButton)this.GetChild("xia1");

			for (int i = 0; i < 8; i++) {
				m_desktopAll[i] = (GTextField)this.GetChild("desktopAll"+i);
				m_desktopMe[i] = (GTextField)this.GetChild("desktopMe"+i);
			}
			ClearDesktopMoney ();

			m_goldBox = (GComponent)this.GetChild ("goldbox");
			m_goldBox.touchable = false;
			m_timeDesc = (GTextField)this.GetChild ("timeDesc");
			m_notice = (GTextField)this.GetChild ("notice");
			m_zMessage = (GTextField)this.GetChild ("zMessage");
			m_waitZhuang = (GImage)this.GetChild ("waitZhuang");

			m_zhuangList.itemRenderer = RenderZhuangList;
			//m_zhuangList.RemoveChildrenToPool ();
			//m_zhuangList.AddItemFromPool ();
			m_zhuangList.SetVirtual();
			m_zhuangList.numItems = 0;

			m_xianfenList.itemRenderer = RenderXianfenList;
			//m_xianfenList.RemoveChildrenToPool ();
			//m_xianfenList.AddItemFromPool ();
			m_xianfenList.SetVirtual();
			m_xianfenList.numItems = 0;

			SetShangZhuangBtn (true);
			InitZhuangBtn ();
			InitLiShi ();

			InitGoldBtn ();
			InitXiaBtn ();
		}

		void InitLiShi (){
			ludanUI = (UI_ludan)UIPackage.CreateObject ("baijiale", "ludan");

			this.GetChild ("lishi").asButton.onClick.Add (() => {
				GRoot.inst.AddChild (ludanUI);
				ludanUI.SetSize(GRoot.inst.width,GRoot.inst.height);
			});
		}

		void InitZhuangBtn(){
			m_sqxz.onClick.Add (()=>{
				GameNetworkManager.Instance.shangZhuang(false);//申请上庄或者下庄
			});

			m_sqsz.onClick.Add (()=>{
				//判断上庄限制
				if(Global.user.dwMoney >= szLimit){
					GameNetworkManager.Instance.shangZhuang(true);//申请上庄或者下庄
				}else{
					double d = szLimit * 1.0 / 100;
					string msg = System.String.Format("上庄失败，上庄至少需要 {0:0.00} 元。", d);
					UI_InfoMsg.Show (msg);
				}
			});
		}
			
		void RenderZhuangList(int index,GObject obj){
			UI_zhuangjiaList item = (UI_zhuangjiaList)obj;

			if (m_zhuangData!=null && index < m_zhuangData.Count) {
				UserInfoStruct tmp = m_zhuangData [index];
				double d = tmp.dwMoney * 1.0 / 100;
				string s = System.String.Format("{0:0.00}", d);
				item.OnData (tmp.GetNickName(), s);
			}

		}

		void RenderXianfenList(int index,GObject obj){
			UI_xiafenList item = (UI_xiafenList)obj;

			if (m_xiafenData == null) {
				return;
			}
			double d;
			string s = "0.00",a = "";
			switch (index) {
				case 0:
					d = m_xiafenData [0] * 1.0 / 100;
					s = System.String.Format ("{0:0.00}", d);
					a = "庄可下分";
					break;
				case 1:
					d = m_xiafenData [3] * 1.0 / 100;
					s = System.String.Format("{0:0.00}", d);
					a ="闲可下分";
					break;
				case 2:
					d = m_xiafenData [6] * 1.0 / 100;
					s = System.String.Format("{0:0.00}", d);
					a ="平可下分";
					break;
			}
			item.OnData (a, s);
		}


		List<UserInfoStruct> m_zhuangData;
		int[] m_xiafenData;
		GButton[] golds = new GButton[6];
		GButton[] xiaBtns = new GButton[8];
		List<GLoader> goldLoaders = new List<GLoader> ();
		/// <summary>
		/// 下注金币类型
		/// </summary>
		public int moneType = -1;

		//初始化下分按钮
		void InitXiaBtn(){
			for (int i = 0; i <= 7; i++) {
				GButton tmp = this.GetChild ("xia"+i).asButton;
				xiaBtns [i] = tmp;

				tmp.onClick.Add (OnXia);
			}
		}

		void OnXia(EventContext context){
			if (moneType == -1) {
				return;
			}
			string senderName = (context.sender as GButton).name;
			for (int i = 0; i <= 7; i++) {
				string btnName = xiaBtns [i].name;
				if (senderName == btnName) {
					GameNetworkManager.Instance.bet (i,moneType+1);
				}
			}
		}

		//初始化金币按钮
		void InitGoldBtn(){
			for (int i = 0; i <= 5; i++) {
				GButton tmp = this.GetChild ("gold"+(i+1)).asButton;
				golds [i] = tmp;

				tmp.onClick.Add (OnGold);
				golds [i].selected = (i == moneType);
			}



		}

		void OnGold(EventContext context){
			for(int j=0;j<=5;j++){
				if(context.sender == golds[j]){
					if (moneType == j) {
						golds [j].selected = false;
						moneType = -1;
					} else {
						golds[j].selected = true;
						moneType = j;
					}
				}else{
					golds[j].selected = false;
				}
			}
		}

		public IEnumerator CountDownTime(string title,float starTime,float timeLength,Action<int> act = null){
			int recv = (int)timeLength;
			while (recv>0) {
				float count = Time.time - starTime;
				recv = (int)(timeLength - count);

				if (act != null) {
					act.Invoke (recv);
				}
				this.SetTimeText (title,recv);
				yield return new WaitForSeconds (1);
			}
		}

		//设置庄家列表
		public void SetZhuangData(List<UserInfoStruct> zhuangData){
			m_zhuangData = zhuangData;
			m_zhuangList.numItems = zhuangData.Count;
		}


		//增加一条下分记录
		public void SetXiafenItem(int[] xiafenData){
			m_xiafenData = xiafenData;
			m_xianfenList.numItems = 3;
		}

		//设置上庄下庄
		public void SetShangZhuangBtn(bool isShang){
			m_sqsz.visible = isShang;
			m_sqxz.visible = !isShang;
			m_waitZhuang.visible = true;
		}

		/// <summary>
		/// 设置筹码是否能使用
		/// </summary>
		/// <param name="money">当前金币数</param>
		public void InitMoneyDisable(int money){
			int tmp = money / 100;
			m_gold1.enabled = tmp >= 1;
			m_gold2.enabled = tmp >= 10;
			m_gold3.enabled = tmp >= 100;
			m_gold4.enabled = tmp >= 1000;
			m_gold5.enabled = tmp >= 5000;
			m_gold6.enabled = tmp >= 10000;

			m_gold1.grayed = !m_gold1.enabled;
			m_gold2.grayed = !m_gold2.enabled;
			m_gold3.grayed = !m_gold3.enabled;
			m_gold4.grayed = !m_gold4.enabled;
			m_gold5.grayed = !m_gold5.enabled;
			m_gold6.grayed = !m_gold6.enabled;

			if (money == 0) {
				m_gold1.selected = false;
				m_gold2.selected = false;
				m_gold3.selected = false;
				m_gold4.selected = false;
				m_gold5.selected = false;
				m_gold6.selected = false;
			}
		}

		public void ClearDesktopMoney(){
			for (int i = 0; i < 8; i++) {
				m_desktopAll [i].text = "";
				m_desktopMe [i].text = "";
			}
		}

		/// <summary>
		/// 庄家信息
		/// </summary>
		/// <param name="zName">庄家姓名</param>
		/// <param name="zScore">庄家成绩</param>
		/// <param name="zTotalScore">庄家总分</param>
		/// <param name="roundCount">坐庄局数</param>
		public void ShowZhuangInfo(string zName,int zScore,int zTotalScore,int roundCount){
			m_zMessage.text = "" +
				"庄家名称: " + zName +
				"\n庄家成绩: " + System.String.Format("{0:0.00}", zScore*1.0/100) +
				"\n庄家总分: " + System.String.Format("{0:0.00}", MathUtil.calculate(zTotalScore.ToString(), 100.ToString(), '/')) +
				"\n游戏局数: " + roundCount;
		}

		/// <summary>
		/// 下注区域金额
		/// </summary>
		/// <param name="moneys">位置0代表0下注区域，元素是金额</param>
		public void ShowDesktopMoney(int [] moneys){
			for (int i = 0; i < 8; i++) {
				if(moneys [i]>0)
					m_desktopAll [i].text = (moneys [i]/100f).ToString ("0.00");
			}
		}

		/// <summary>
		/// 玩家本人下注区域金额
		/// </summary>
		/// <param name="moneys">位置0代表0下注区域，元素是金额</param>
		public void ShowMeDesktopMoney(int [] moneys){
			for (int i = 0; i < 8; i++) {
				if(moneys [i]>0)
					m_desktopMe [i].text = (moneys [i]/100f).ToString ("0.00");
			}
		}

		/// <summary>
		/// 高亮展示赢的下注区域
		/// </summary>
		/// <param name="wins">游戏的赢钱区域 0庄，1庄天王，2庄对子，3闲，4先天王，5闲对子，6和，7同点和</param>
		public void ShowDesktopWins(int [] wins){
			//只要内容值大于0就高亮显示
			//高亮闪烁5秒
			CoroutineTool.inst.StartCoroutine(ShowDesktopWinsCor(wins));
		}

		System.Collections.IEnumerator ShowDesktopWinsCor(int[] wins){
			for (int i = 0; i < wins.Length; i++) {
				if(wins[i]>0){
					GButton item = xiaBtns [i];
					item.touchable = false;
					item.GetController ("button").selectedPage = "down";
				}
			}
			for (int j = 0; j < 30; j++) {
				yield return new WaitForSeconds (0.2f);
				for (int i = 0; i < wins.Length; i++) {
					//GButton item = xiaBtns [wins [i]];
					if(wins[i]>0){
						GButton item = xiaBtns [i];
						item.visible = !item.visible;
					}
				}
			}
			yield return new WaitForSeconds (1);
			for (int i = 0; i < xiaBtns.Length; i++) {
				GButton item = xiaBtns [i];
				item.GetController ("button").selectedPage = "up";
				item.visible = true;
				item.touchable = true;
			}
		}

		/// <summary>
		/// 下注动画
		/// </summary>
		/// <param name="xiaType">下注类型</param>
		/// <param name="monType">筹码类型</param>
		/// <param name="xSeed">x坐标种子</param>
		/// <param name="ySeed">y坐标种子</param>
		public void ShowDesktopClip(int xiaType,int monType,int xSeed,int ySeed){
			GButton btn = xiaBtns [xiaType];
			float w = btn.size.x;
			float h = btn.size.y;
			float x = btn.position.x;
			float y = btn.position.y;
			//float px = btn.pivot.x;
			//float py = btn.pivot.y;

			float right = x + w - 60;
			float left = x + 20;

			if (xiaType == 1 || xiaType == 2) {
				right -= 20;
			}
			if (xiaType == 4 || xiaType == 5) {
				left += 20;
			}
			UnityEngine.Random.InitState (xSeed);
			float rx = UnityEngine.Random.Range (left, right);
			UnityEngine.Random.InitState (ySeed);
			float ry = UnityEngine.Random.Range (y+20, y + h-60);
			ShowDesktopClip (monType, new Vector2 (rx, ry));
		}

		/// <summary>
		/// 下注动画
		/// </summary>
		/// <param name="type">筹码类型</param></param>
		/// <param name="pos">筹码放置坐标</param>
		public void ShowDesktopClip(int type,Vector2 pos){
			GLoader loader = new GLoader ();
			switch (type) {
			case 1:
				loader.url = "ui://nyvoaldgsaztd";
				break;
			case 2:
				loader.url = "ui://nyvoaldgsaztc";
				break;
			case 3:
				loader.url = "ui://nyvoaldgsaztb";
				break;
			case 4:
				loader.url = "ui://nyvoaldgsazta";
				break;
			case 5:
				loader.url = "ui://nyvoaldgsazt9";
				break;
			case 6:
				loader.url = "ui://nyvoaldgsazt8";
				break;
			}

			m_goldBox.AddChild (loader);
			loader.SetScale (0.4f, 0.4f);
			loader.position = golds [type-1].position;

			DOTween.To (() => {
				return loader.position;
			}, (x) => {
				loader.position = x;
			}, new Vector3(pos.x,pos.y,0), 0.5f);

			goldLoaders.Add (loader);
		}

		/// <summary>
		/// 清理牌面筹码
		/// </summary>
		public void ClearDesktopClip(){
			for (int i = 0; i < goldLoaders.Count; i++) {
				goldLoaders [i].Dispose ();
			}
			goldLoaders.Clear ();
		}

		/// <summary>
		/// 设置路单列表
		/// </summary>
		/// <param name="ludanData">0:庄,1:闲,2:平</param>
		public void InitLudan(List<BaccaraHistory> ludanData){
			List<int> tmpData = new List<int> ();
			for (int i = 0; i < ludanData.Count; i++) {
				tmpData.Add (ludanData [i].winner - 1);
			}
			ludanUI.SetLudanData (tmpData);
		}

		/// <summary>
		/// 设置倒计时显示
		/// </summary>
		/// <param name="hour">Hour.</param>
		/// <param name="second">Second.</param>
		public void SetTimeText(string timeDesc,int second){
			m_timeDesc.text = timeDesc;
			if (second < 10) {
				m_timeText.text = "00:0" + second;
			} else {
				m_timeText.text = "00:" + second;
			}
		}

		/// <summary>
		/// 显示扑克牌板
		/// </summary>
		public void ShowPai(List<Card> zCards,int z2point,int z3point,int zToD,List<Card> xCards,int x2point,int x3point,int xToD){
			bar.Init ();
			bar.Open ();
			bar.fucFanPai(zCards,z2point,z3point,zToD,xCards,x2point,x3point,xToD);
		}

		/// <summary>
		/// 显示结算面板
		/// </summary>
		/// <param name="bfMoney">Bf money.</param>
		/// <param name="fhMoney">Fh money.</param>
		/// <param name="zjMoney">Zj money.</param>
		public void ShowSettlement(int bfMoney,int fhMoney,int zjMoney){
			UI_Settlement settlementUI = (UI_Settlement)UIPackage.CreateObjectFromURL (UI_Settlement.URL);
			GRoot.inst.AddChild (settlementUI);
			settlementUI.SetSize(GRoot.inst.width,GRoot.inst.height);

			string bfMoneyStr, fhMoneyStr, zjMoneyStr;

			double d = bfMoney * 1.0 / 100;
			bfMoneyStr = System.String.Format("{0:0.00}", d);

			d = fhMoney * 1.0 / 100;
			fhMoneyStr = System.String.Format("{0:0.00}", d);

			d = zjMoney * 1.0 / 100;
			zjMoneyStr = System.String.Format("{0:0.00}", d);

			settlementUI.OnData (bfMoneyStr, fhMoneyStr, zjMoneyStr);
		}

		/// <summary>
		/// 设置公告
		/// </summary>
		/// <param name="notice">Notice.</param>
		public void SetNotice(string notice){
			this.m_notice.text = notice;
		}
	}
}