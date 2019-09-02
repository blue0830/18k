using UnityEngine;
using System.Collections;

public class QuKuanPWDPanel : UserSubPanelBase {
	//取款原始密码
	public UIInput QuKuanPWDInput;
	//取款新密码
	public UIInput QuKuanNewPWDInput;
	//取款新密码
	public UIInput QuKuanNewPWD2Input;
	//按钮
	public GameObject ReturnBtn;
	//按钮
	public GameObject SubmitBtn;

	void Start () {
		UIEventListener.Get(ReturnBtn).onClick = OnReturn;
		UIEventListener.Get(SubmitBtn).onClick = OnSubmit;
	}

	// Update is called once per frame
	void Update () {
	}

	public void Show()
	{
		gameObject.SetActive(true);
	}

	void OnReturn(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		gameObject.SetActive(false);
	}

	void OnSubmit(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		if (string.IsNullOrEmpty(QuKuanPWDInput.value))
		{
			msgSignal.Dispatch(new MsgPara("请输入原始取款密码",2));
			return;
		}
		if (string.IsNullOrEmpty(QuKuanNewPWDInput.value))
		{
			msgSignal.Dispatch(new MsgPara("请输入新取款密码",2));
			return;
		}
		if (string.IsNullOrEmpty(QuKuanNewPWD2Input.value))
		{
			msgSignal.Dispatch(new MsgPara("请再次输入新取款密码",2));
			return;
		}
		if (!QuKuanNewPWD2Input.value.Equals(QuKuanNewPWDInput.value))
		{
			msgSignal.Dispatch(new MsgPara("两次输入的取款密码不一致",2));
			return;
		}
		NetworkManager.Instance.ChangePasswd(2,QuKuanPWDInput.value,QuKuanNewPWDInput.value);
		QuKuanPWDInput.value = "";
		QuKuanNewPWDInput.value = "";
		QuKuanNewPWD2Input.value = "";
	}
}


