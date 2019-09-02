using UnityEngine;
using System.Collections;

public class ZhangHaoBaoHuPanel : UserSubPanelBase {

	//账号
	public UILabel ZhangHaoLabel;
	//账号ID
	public UILabel UIDLabel;

	//账保问题
	public UIInput QuestionInput;
	//账保答案
	public UIInput AnswerInput;
	//账保答案
	public UIInput QuKuanMiMaInput;

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
		if (string.IsNullOrEmpty(QuestionInput.value))
		{
			msgSignal.Dispatch(new MsgPara("请输入账保问题",2));
			return;
		}
		if (string.IsNullOrEmpty(AnswerInput.value))
		{
			msgSignal.Dispatch(new MsgPara("请输入账保答案",2));
			return;
		}
		if (string.IsNullOrEmpty(QuKuanMiMaInput.value))
		{
			msgSignal.Dispatch(new MsgPara("请输入取款密码",2));
			return;
		}
		string[] questions = new string[3];
		string[] answers = new string[3];
		questions [0] = QuestionInput.value;
		questions [1] = "";
		questions [2] = "";
		answers [0] = AnswerInput.value;
		answers [1] = "";
		answers [2] = "";
		NetworkManager.Instance.SetAccountProtect (QuKuanMiMaInput.value,questions,answers);
	}

	public void Show(IUInfoModel uinfoModel)
	{
		gameObject.SetActive(true);

		ZhangHaoLabel.text = uinfoModel.GetUserName();//账号

		UIDLabel.text = uinfoModel.GetUserID().ToString();//ID
	}
}
