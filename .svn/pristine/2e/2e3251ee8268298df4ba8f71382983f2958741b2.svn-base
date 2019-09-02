using UnityEngine;
using System.Collections;

public class TiKuanZhangHuPanel : UserSubPanelBase {

	//提款账号
	public UILabel TiKuanZhangHaoLabel;
	//账号ID
	public UILabel UIDLabel;

	//开户名
	public UIInput KaiHuMingLabel;
	//开户行
	public UIPopupList KaiHuHangList;
	//开户地址
	public UIInput KaiHuDiZhiLabel;
	//银行卡号
	public UIInput BankCodeLabel;
	//取款密码
	public UIInput QuKuanPWDLabel;

	//按钮
	public GameObject ReturnBtn;
	//提交
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

	public void Show(IUInfoModel uinfoModel,MSG_GP_USER_GetUserInfoBack userbaseInfo)
	{
		gameObject.SetActive(true);

		TiKuanZhangHaoLabel.text = uinfoModel.GetUserName();//提款账号

		UIDLabel.text = uinfoModel.GetUserID().ToString();//账号ID

		KaiHuHangList.value = "";
		KaiHuHangList.Clear();
		foreach (var backName in userbaseInfo.GetChQkBackInfos()) {
			if (!string.IsNullOrEmpty (backName.Trim())) {
				if (string.IsNullOrEmpty(KaiHuHangList.value))
				{
					KaiHuHangList.value = backName;
				}
				KaiHuHangList.AddItem(backName);
			}
		}

		if (userbaseInfo.isLockBank == 1) {//锁定处理
			KaiHuMingLabel.enabled = false;
			KaiHuMingLabel.value = userbaseInfo.GetTrueName ();//开户名

			KaiHuDiZhiLabel.enabled = false;
			KaiHuDiZhiLabel.value = userbaseInfo.GetBackAddress (); //开户地址

			KaiHuHangList.enabled = false;
			KaiHuHangList.value = userbaseInfo.GetBackName ();//开户行

			BankCodeLabel.enabled = false;
			BankCodeLabel.value = userbaseInfo.GetBackAccount ();//银行卡号

			QuKuanPWDLabel.enabled = false;
			QuKuanPWDLabel.value = "********";//取款密码
		}
	}

	void OnSubmit(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		if (string.IsNullOrEmpty(KaiHuMingLabel.value))
		{
			msgSignal.Dispatch(new MsgPara("请输入开户名",2));
			return;
		}
		if (string.IsNullOrEmpty(KaiHuHangList.value))
		{
			msgSignal.Dispatch(new MsgPara("请选择开户行",2));
			return;
		}
		if (string.IsNullOrEmpty(KaiHuDiZhiLabel.value))
		{
			msgSignal.Dispatch(new MsgPara("请输入开户地址",2));
			return;
		}
		if (string.IsNullOrEmpty(BankCodeLabel.value))
		{
			msgSignal.Dispatch(new MsgPara("请输入银行卡号",2));
			return;
		}
		if (string.IsNullOrEmpty (QuKuanPWDLabel.value)) {
			msgSignal.Dispatch (new MsgPara ("请输入密码",2));
			return;
		}
		if (QuKuanPWDLabel.enabled) {//锁定状态不能提交
			NetworkManager.Instance.SetBankAccount (QuKuanPWDLabel.value,KaiHuHangList.value,KaiHuMingLabel.value,BankCodeLabel.value,KaiHuDiZhiLabel.value);
		}
	}
}
