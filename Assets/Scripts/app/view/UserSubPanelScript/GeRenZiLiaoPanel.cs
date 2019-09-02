using System;
using UnityEngine;
using System.Collections;
//个人资料页面
public class GeRenZiLiaoPanel : UserSubPanelBase {
	//用户账号
	public UILabel UserNameLabel;
	//用户ID
	public UILabel UIDLabel;
	//用户QQ
	public UILabel QQLabel;
	//上线QQ
	public UILabel UpperQQLabel;
	//彩票余额
	public UILabel CaiPiaoYuELabel;
	//棋牌余额
	public UILabel QiPaiYuELabel;
	//彩票返点
	public UILabel CaiPiaoFanDianLabel;
	//棋牌返点
	public UILabel QiPaiFanDianLabel;
	//上次登录ip
	public UILabel lastLoginIpLabel;
	//上次登录时间
	public UILabel lastLoginTimeLabel;
	//本次登录ip
	public UILabel currLoginIpLabel;
	//本次登录时间
	public UILabel currLoginTimeLabel;

	//返回按钮
	public GameObject ReturnBtn;

	void Start () {

		UIEventListener.Get(ReturnBtn).onClick = OnReturn;
	}

	// Update is called once per frame
	void Update () {


	}

	public void Show(IUInfoModel uinfoModel,MSG_GP_USER_GetUserInfoBack userbaseInfo)
	{
		gameObject.SetActive(true);

		UserNameLabel.text = uinfoModel.GetUserName();//用户账号

		UIDLabel.text = uinfoModel.GetUserID().ToString();//用户ID

		QQLabel.text = userbaseInfo.GetQQNum();//用户QQ

		UpperQQLabel.text = userbaseInfo.GetAgencyQQNum();//上线QQ

		double d = uinfoModel.GetMoney()*1.0/100;
		CaiPiaoYuELabel.text = String.Format("¥{0:0.00}", d);//彩票余额

		d = uinfoModel.GetGold()*1.0/100;
		QiPaiYuELabel.text = String.Format("¥{0:0.00}", d);//棋牌余额

		CaiPiaoFanDianLabel.text = String.Format("{0:0.00}%", uinfoModel.GetCpFd());//彩票返点

		d = uinfoModel.GetQpFd () * 3*1.0/100;
		QiPaiFanDianLabel.text =String.Format("{0:0.00}%", d);//棋牌返点

		lastLoginIpLabel.text =uinfoModel.GetLastLoginIp();//上次登录ips

		lastLoginTimeLabel.text =uinfoModel.GetLastLoginTime();//上次登录时间

		currLoginIpLabel.text =uinfoModel.GetCurrLoginIp();//本次登录ip

		currLoginTimeLabel.text =uinfoModel.GetCurrLoginTime();//本次登录时间
	}

	void OnReturn(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		gameObject.SetActive(false);
	}
}
