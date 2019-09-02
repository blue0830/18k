/// An example view
/// ==========================
/// 

using System;
using System.Collections;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using System.Threading;


public class LoginView : View
{
    [Inject]
    public MsgSignal MsgSignal { get; set; }

    GameObject panel;

    LoginPanel panelScript;

    internal void init()
    {
        Transform parent = UIRootFinder.uiRootTran;
        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("LoginPanel");
        panel = Instantiate(asset) as GameObject;
        panel.transform.parent = parent;
        panel.transform.localScale = Vector3.one;

        panelScript = panel.GetComponent<LoginPanel>();

        UIEventListener.Get(panelScript.loginbtn).onClick = OnLoginClick;
        UIEventListener.Get(panelScript.registerbtn).onClick = OnRegClick;
        UIEventListener.Get(panelScript.forgetbtn).onClick = OnforgetClick;
        UIEventListener.Get(panelScript.kefubtn).onClick = OnkefuClick;
    }
		
    void OnLoginClick(GameObject go)
    {
		AudioController.Instance.SoundPlay("active_item");

		string username = panelScript.username.value;
		string password = panelScript.password.value;
		if (string.IsNullOrEmpty(username.Trim() ))
		{

			MsgSignal.Dispatch(new MsgPara("用户名不能为空",2));

		}
		else if (string.IsNullOrEmpty(password.Trim()))
		{
			MsgSignal.Dispatch(new MsgPara("密码不能为空",2));
		}
		else
		{

			string passMd5 = Util.GetMd5Hash(password);

			string temp = PlayerPrefs.GetString("Password", "");
			if (!panelScript.IsInputPswd&&!string.IsNullOrEmpty(temp))
			{
				passMd5 = temp;
			}

			PlayerPrefs.SetString("UserName", username);


			if (panelScript.savePassword.value)
			{
				PlayerPrefs.SetString("Password", passMd5);
				PlayerPrefs.SetInt("savePassword", 1);
			}
			else
			{
				PlayerPrefs.SetInt("savePassword", 0);
			}

			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				MsgSignal.Dispatch(new MsgPara("无法连接，请检查您的网络", 2));
				return;
			}
			Global.IsLoginApp = false;
			LoSocket.GetInstance().ManualConnect();
			NetworkManager.Instance.Login(username, passMd5);
			Loading.GetInstance ().ShowLoading ("服务器正在验证账号密码中......");
			Global.AppLoginViewTryReConnTimes = 0;//重新设定
			TimeManager.Instance().Register("checklogin", 8, 4000, 4000, (c, t) =>
			{
				if(!Global.IsLoginApp){
					if(Global.AppLoginViewTryReConnTimes>0){
						PlayerPrefs.DeleteKey("ipListIndex");
					}
					LoSocket.GetInstance().ManualShutDown();//先关闭
					LoSocket.GetInstance().ManualConnect();//再连接
					Global.AppLoginViewTryReConnTimes++;
				}
			});
			TimeManager.Instance().Register("checkloginClose", 1, 1000, 37000, (c, t) =>
			{
				if(!Global.IsLoginApp){
					MsgSignal.Dispatch(new MsgPara("暂时无法登录服务器,请稍后再试!", 2));
					Loading.GetInstance ().HideLoading();
				}
			});
		}
    }

    void OnRegClick(GameObject go)
    {
    }

    void OnforgetClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        MsgSignal.Dispatch(new MsgPara("请联系客服,通过设定资料找回密码",2));
    }

    void OnkefuClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
		Application.OpenURL(Constant.CUSTOMER_SERVICE_URL);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Destroy(panel);
    }

    public void close()
    {
        TimeManager.Instance().UnRegister("checklogin");
		TimeManager.Instance().UnRegister("checkloginClose");
        panelScript.gameObject.SetActive(false);
    }

	public void Show()
	{
		panelScript.gameObject.SetActive (true);
		panelScript.Reset ();
	}
}