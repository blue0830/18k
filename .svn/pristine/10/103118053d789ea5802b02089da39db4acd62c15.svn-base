using System;
using UnityEngine;
using System.Collections;
//个人资金页面
public class GeRenZiJinPanel : UserSubPanelBase {

    //筛选保存
    public static ulong startDate;
    public static ulong endDate;

	//投注总额
	public UILabel TouZhuTotalLabel;
	//中奖总额
	public UILabel WinTotalLabel;
	//返点总额
	public UILabel BackTotalLabel;
	//彩票实际盈亏
	public UILabel CaiPiaoShiJiYingKuiLabel;
	//充值总额
	public UILabel ChongZhiTotalLabel;
	//提现总额
	public UILabel TiXianTotalLabel;
	//广告收入
	public UILabel GuangGaoShouRuLabel;
	//棋牌实际盈亏	
	public UILabel QiPaiShiJiYingKuiLabel;
	//盈亏汇总
	public UILabel YingKuiTotalLabel;

	//返回按钮
	public GameObject ReturnBtn;
    public GameObject shaixuanBtn;


	public ShaiXuanGeRenZiJinPanel shaixuanpanel;

	void Start () {
		UIEventListener.Get(ReturnBtn).onClick = OnReturn;

        UIEventListener.Get(shaixuanBtn).onClick = OnShaixuan;
	}

	public void Show(MSG_GP_USER_GETBACKWDZHJBXX para)
	{
        shaixuanpanel.gameObject.SetActive(false);

		gameObject.SetActive(true);

		double d = para.iAmount *1.0/100;
		TouZhuTotalLabel.text =String.Format("¥{0:0.00}", d);//投注总额

		d = para.iBingo *1.0/100;
		WinTotalLabel.text =String.Format("¥{0:0.00}", d);//中奖总额

		d = para.iPoint *1.0/100;
		BackTotalLabel.text =String.Format("¥{0:0.00}", d);//返点总额

		d = (para.iBingo+para.ixjPoint-para.iAmount) *1.0/100;
		CaiPiaoShiJiYingKuiLabel.text =String.Format("¥{0:0.00}", d);//彩票实际盈亏

		d = para.iRecharge *1.0/100;
		ChongZhiTotalLabel.text =String.Format("¥{0:0.00}", d);//充值总额

		d = para.iExchange *1.0/100;
		TiXianTotalLabel.text =String.Format("¥{0:0.00}", d);//提现总额

		d = para.iPlayAd *1.0/100;
		GuangGaoShouRuLabel.text =String.Format("¥{0:0.00}", d);//广告收入

		d = para.iPlayQpYk *1.0/100;
		QiPaiShiJiYingKuiLabel.text =String.Format("¥{0:0.00}", d);//棋牌实际盈亏	

		d = (para.iPlayQpYk+para.iBingo+para.ixjPoint-para.iAmount) *1.0/100;
		YingKuiTotalLabel.text =String.Format("¥{0:0.00}", d);//盈亏汇总
    }


	// Update is called once per frame
	void Update () {


	}

	void OnReturn(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		gameObject.SetActive(false);
	}

    void OnShaixuan(GameObject go)
    {
		AudioController.Instance.SoundPlay("active_item");
        shaixuanpanel.Show();
    }
}
