using UnityEngine;
using System.Collections;
//账号信息页面
public class ZhangHaoXinXiPanel : UserSubPanelBase {
	//账号
	public UILabel TouZhuTotalLabel;
	//原QQ号码
	public UILabel YuanQQLabel;
	//新QQ号码
	public UIInput NewQQInput;
	//昵称
	public UIInput NiChengInput;
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

	void OnSubmit(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		if (string.IsNullOrEmpty(NewQQInput.value))
		{
			msgSignal.Dispatch(new MsgPara("请输入新QQ号码",2)); 
			return;
		}
		if (string.IsNullOrEmpty(NiChengInput.value))
		{
			msgSignal.Dispatch(new MsgPara("请输入新昵称",2)); 
			return;
		}
		NetworkManager.Instance.ChangePasswd(3,NewQQInput.value,NiChengInput.value);
	}

	public void Show(IUInfoModel uinfoModel,MSG_GP_USER_GetUserInfoBack userbaseInfo)
	{
		gameObject.SetActive(true);
		TouZhuTotalLabel.text = uinfoModel.GetUserName();//账号
		YuanQQLabel.text = userbaseInfo.GetQQNum();//原QQ号码
		NewQQInput.value = userbaseInfo.GetQQNum();//新QQ号码
		NiChengInput.value = uinfoModel.GetNickName();//昵称
	}
}
