using UnityEngine;
using System.Collections;

public class DengLuPWDPanel : UserSubPanelBase {
	//原始密码
	public UIInput PWDInput;
	//新密码
	public UIInput NewPWDInput;
	//新密码
	public UIInput NewPWD2Input;

	//按钮
	public GameObject ReturnBtn;
	//按钮
	public GameObject SubmitBtn;

    public static string currentPass;

	void Start () {
		UIEventListener.Get(ReturnBtn).onClick = OnReturn;
		UIEventListener.Get(SubmitBtn).onClick = OnSubmit;
	}

	// Update is called once per frame
	void Update () {
	}

	public void Result(MSG_GP_USER_ChangeUserPassWordResult para)
	{
		
	}

	public void Show()
	{
		AudioController.Instance.SoundPlay("active_item");
		gameObject.SetActive(true);
	}

	void OnReturn(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		gameObject.SetActive(false);
	}

	void OnSubmit(GameObject go)
	{
		if (string.IsNullOrEmpty(PWDInput.value))
		{
			msgSignal.Dispatch(new MsgPara("请输入原始密码",2));
			return;
		}
		if (string.IsNullOrEmpty(NewPWDInput.value))
		{
			msgSignal.Dispatch(new MsgPara("请输入新密码",2));
			return;
		}
		if (string.IsNullOrEmpty(NewPWD2Input.value))
		{
			msgSignal.Dispatch(new MsgPara("请再次输入新密码",2));
			return;
		}
		if (!NewPWD2Input.value.Equals(NewPWDInput.value))
		{
			msgSignal.Dispatch(new MsgPara("两次输入的密码不一致",2));
			return;
		}
		NetworkManager.Instance.ChangePasswd(1,PWDInput.value,NewPWDInput.value);
        currentPass = NewPWDInput.value;

        PWDInput.value = "";
		NewPWDInput.value = "";
		NewPWD2Input.value = "";
	}
}


