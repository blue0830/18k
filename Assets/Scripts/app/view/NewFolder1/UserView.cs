/// An example view
/// ==========================
/// 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

//用户中心View
public class UserView : View
{
    [Inject]
    public MsgSignal MsgSignal { get; set; }

	[Inject]
	public LogOutSignal LogoutSignal { get; set; }

    GameObject panel;
	//用户中心Panel
    UserPanel panelScript;


    internal void init()
    {
        Transform parent = UIRootFinder.uiRootTran;
        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("userPanel");
        panel = Instantiate(asset) as GameObject;
        panel.transform.parent = parent;
        panel.transform.localScale = Vector3.one;
        panelScript = panel.GetComponent<UserPanel>();
        UIEventListener.Get(panelScript.returnbtn).onClick = OnReturnClick;
        panelScript.msgSignal.AddListener(OnsubPanelMsg);
		panelScript.LogoutSignal.AddListener (OnLogOut);
    }

    public void FillContent(IUInfoModel uinfoModel)
    {
        panelScript.UnameLabel.text = uinfoModel.GetUserName();
        panelScript.IdLabel.text = uinfoModel.GetUserID().ToString();
	

        double d = uinfoModel.GetMoney() * 1.0 / 100;
        panelScript.MoneyLabel.text = String.Format("¥{0:0.00}", d);//彩票余额

        d = uinfoModel.GetGold() * 1.0 / 100;
        panelScript.GoldLabel.text = String.Format("¥{0:0.00}", d);//棋牌余额

		panelScript.uinfoModel = uinfoModel;
    }

	public void TransferRefresh(IUInfoModel umodel)
	{
		panelScript.transferPanel.refresh (umodel);
	}

    public void OnRecieveUserbaseinfo(MSG_GP_USER_GetUserInfoBack info)
    {
        panelScript.userbaseInfo = info;
    }


    protected override void OnDestroy()
    {
        panelScript.msgSignal.RemoveListener(OnsubPanelMsg);
		panelScript.LogoutSignal.RemoveListener(OnLogOut);

        base.OnDestroy();
        Destroy(panel);
    }

    void OnReturnClick(GameObject go)
    {
		AudioController.Instance.SoundPlay("active_item");
        close();
    }

    public void close()
    {
        Destroy(gameObject);
    }

    void OnsubPanelMsg(MsgPara para)
    {
        MsgSignal.Dispatch(para);
    }

    public void OnGerenZijin(MSG_GP_USER_GETBACKWDZHJBXX para)
    {
        panelScript.OnGeRenZiJinBack(para);
    }
		
	//团队资金 返回 回调
	public void OnTuanDuiZijin(MSG_GP_TEAM_GETBACKJBXX para)
	{
		panelScript.OnTuanDuiZijinBack(para);
	}

	//QQ&昵称,密码,取款密码 返回 回调
	public void OnQQMiMaQuKuanMiMa(MSG_GP_USER_ChangeUserPassWordResult para)
	{
		panelScript.OnQQMiMaQuKuanMiMaBack(para);
	}

    public void OnRecordBack(RecordBackObj obj)
    {
        panelScript.OnRecordBack(obj);
    }

    public void OnTouzhuxiangxi(TouzhuXiangxi obj)
    {
        panelScript.OpenXiangxi(obj);
    }

    public void OnAllBankInfo(List<GetBankInfo> bankInfo)
    {
        panelScript.OngetBankInfo(bankInfo);
    }

    public void reFreshTouchuRecord()
    {
        panelScript.reFreshTouchuRecord();
    }

    public void OnLogOut()
	{
		Global.LastAppHeartBeatTime = 0;//防止网络检查与此处冲突
		Global.IsLoginApp = false;
		LogoutSignal.Dispatch ();
		close();
    }
}