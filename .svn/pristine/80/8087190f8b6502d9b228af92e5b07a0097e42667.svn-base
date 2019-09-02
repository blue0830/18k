using UnityEngine;
using System;
using System.Collections;

public class TiXianPanel : UserSubPanelBase {
	//您的银行
	public UILabel BankNameLabel;
	//您的卡号
	public UILabel BankCodeLabel;
	//您的姓名
	public UILabel BankAccountLabel;
	//账户余额
	public UILabel YuELabel;

	//取款金额
	public UIInput JinEInput;
	//取款密码
	public UIInput QuKuanPWDInput;

	//按钮
	public GameObject ReturnBtn;
	//取款
	public GameObject SubmitBtn;

	void Start () {
		UIEventListener.Get(ReturnBtn).onClick = OnReturn;
		UIEventListener.Get(SubmitBtn).onClick = OnSubmit;
	}

	// Update is called once per frame
	void Update () {
	}

	void OnReturn(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		gameObject.SetActive(false);
	}

	public void Show(MSG_GP_USER_GetUserInfoBack userbaseInfo,IUInfoModel uinfoModel)
	{
		gameObject.SetActive(true);

		BankNameLabel.text = userbaseInfo.GetBackName();//您的银行

		BankCodeLabel.text = userbaseInfo.GetBackAccount();//您的卡号

		BankAccountLabel.text = userbaseInfo.GetTrueName();//您的姓名

		double d = uinfoModel.GetMoney()*1.0/100;
		YuELabel.text = String.Format ("¥{0:0.00}", d);//账户余额
	}

	void OnSubmit(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
        if (string.IsNullOrEmpty(BankCodeLabel.text))
        {
            msgSignal.Dispatch(new MsgPara("请先设置您的银行账户",2));
            return;
        }
        
		if (string.IsNullOrEmpty(JinEInput.value))
		{
			msgSignal.Dispatch(new MsgPara("请输入金额",2));
			return;
		}
		if (string.IsNullOrEmpty(QuKuanPWDInput.value))
		{
			msgSignal.Dispatch(new MsgPara("请输入密码",2));
			return;
		}
		//还有就是提现的时候，是元做为单位的，但是提现最少是元，所以提交的时候，那个数字要乘以100
		NetworkManager.Instance.Withdraw(QuKuanPWDInput.value,int.Parse(JinEInput.value)*100);
		//都要输入框的数据清空，防止玩家重复点击。
		QuKuanPWDInput.value = "";
		JinEInput.value = "";
	}
}
