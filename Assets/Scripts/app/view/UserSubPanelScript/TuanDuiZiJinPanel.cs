using System;
using UnityEngine;
using System.Collections;
//团队资金页面
public class TuanDuiZiJinPanel : UserSubPanelBase {

    //筛选保存
    public static ulong startDate;
    public static ulong endDate;

	//团队余额
	public UILabel TeamYuELabel;
	//团队充值
	public UILabel TeamChongZhiLabel;
	//团队取款
	public UILabel TeamQuKuanLabel;
	//团队人数
	public UILabel TeamCountLabel;
	//今日新注册
	public UILabel TodayNewRegLabel;
	//今日新充值
	public UILabel TodayChongZhinLabel;
	//当前在线会员
	public UILabel CurrOnLineLabel;


	//彩票投注总额
	public UILabel CaiPiaoTouZhuLabel;
	//彩票中奖总额
	public UILabel CaiPiaoWinLabel;
	//团队彩票返点
	public UILabel TeamCaiPiaoFanDianLabel;
	//我的彩票返点
	public UILabel MyCaiPiaoFanDianLabel;
	//彩票撤单总额
	public UILabel CaiPiaoCheDanLabel;
	//团队彩票盈亏
	public UILabel TeamCaiPiaoYingKuiLabel;


	//棋牌洗码量-赢
	public UILabel QiPaiXimaLiangWinLabel;
	//团队棋牌盈亏
	public UILabel TeamQiPaiYingKuiLabel;
	//团队棋牌返点
	public UILabel TeamQiPaiFanDianLabel;
	//我的棋牌返点
	public UILabel MyQiPaiFanDianLabel;


	//团队佣金总额
	public UILabel TuanDuiYongJinZongELabel;
	//我的综合盈亏
	public UILabel TuanDuiZongHeYingKuiLabel;

	//返回按钮
	public GameObject ReturnBtn;
    public GameObject Shaixuan;

	public ShaiXuanTuanDuiZiJinPanel shaixuanPanel;

	void Start () {
		UIEventListener.Get(ReturnBtn).onClick = OnReturn;
        UIEventListener.Get(Shaixuan).onClick = OnShaixuan;
	}



	// Update is called once per frame
	void Update () {


	}

	public void Show(MSG_GP_TEAM_GETBACKJBXX para)
	{
		shaixuanPanel.gameObject.SetActive (false);
		gameObject.SetActive(true);


		double d = (para.iteamTdYeZ + para.iteamTdqpye) *1.0/100;
		TeamYuELabel.text =String.Format("¥{0:0.00}", d);//团队余额

		d = para.itempTdCz *1.0/100;
		TeamChongZhiLabel.text = String.Format ("¥{0:0.00}", d);//团队充值

		d = para.iteamTdqk *1.0/100;
		TeamQuKuanLabel.text = String.Format ("¥{0:0.00}", d);//团队取款

		TeamCountLabel.text = para.iteamTdrs.ToString();//团队人数

		TodayNewRegLabel.text = para.iteamJrXzc.ToString();//今日新注册

		TodayChongZhinLabel.text = para.iteamJrXcz.ToString();//今日新充值

		CurrOnLineLabel.text = para.itempZxhy.ToString();//当前在线会员

		d = para.iteamCptzze *1.0/100;
		CaiPiaoTouZhuLabel.text = String.Format ("¥{0:0.00}", d);//彩票投注总额

		d = para.iteamCpzjze *1.0/100;
		CaiPiaoWinLabel.text = String.Format ("¥{0:0.00}", d);//彩票中奖总额

		d = para.iteamtdcpfd *1.0/100;
		TeamCaiPiaoFanDianLabel.text = String.Format ("¥{0:0.00}", d);//团队彩票返点

		d = para.iwdcpfd *1.0/100;
		MyCaiPiaoFanDianLabel.text = String.Format ("¥{0:0.00}", d);//我的彩票返点

		d = para.iwdcpcdze *1.0/100;
		CaiPiaoCheDanLabel.text = String.Format ("¥{0:0.00}", d);//彩票撤单总额

		d = para.iteamtdcpyk *1.0/100;
		TeamCaiPiaoYingKuiLabel.text = String.Format ("¥{0:0.00}", d);//团队彩票盈亏

		d = para.iteamqpxml *1.0/100;
		QiPaiXimaLiangWinLabel.text = String.Format ("¥{0:0.00}", d);//棋牌洗码量-赢

		d = para.iteamtdqpyk *1.0/100;
		TeamQiPaiYingKuiLabel.text = String.Format ("¥{0:0.00}", d);//团队棋牌盈亏

		d = (para.iteamtdbrqpfd+para.iteamtddzqpfd)*1.0/100;
		TeamQiPaiFanDianLabel.text = String.Format ("¥{0:0.00}", d);//团队棋牌返点

		d = para.iwdqpfd *1.0/100;
		MyQiPaiFanDianLabel.text = String.Format ("¥{0:0.00}", d);//我的棋牌返点

		d = para.iteamtdyjze *1.0/100;
		TuanDuiYongJinZongELabel.text = String.Format ("¥{0:0.00}", d); //团队佣金总额

		d = (para.iteamtdcpyk+para.iteamtdqpyk) *1.0/100;
		TuanDuiZongHeYingKuiLabel.text = String.Format ("¥{0:0.00}", d);//我的综合盈亏
	}

	void OnReturn(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		gameObject.SetActive(false);
	}

    void OnShaixuan(GameObject go)
    {
		AudioController.Instance.SoundPlay("active_item");
        shaixuanPanel.Show();
    }
}