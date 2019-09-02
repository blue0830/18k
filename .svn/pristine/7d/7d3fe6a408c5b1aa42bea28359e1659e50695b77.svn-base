using System;
using UnityEngine;
using System.Collections;


public class TransferPanel : UserSubPanelBase {
    //用来显示的label
    public UILabel CaiPiaoYuELabel;
    public UILabel YouXiYuELable;
    //下拉菜单
    public UIPopupList DuiHuanLeiXing;
    //输入框
    public UIInput JinEInput;
    public UIInput MiMaInput;
    //按钮
    public GameObject ReturnBtn;
	//提交
	public GameObject submitBtn;
   
	void Start () {
        UIEventListener.Get(ReturnBtn).onClick = OnReturn;
        UIEventListener.Get(submitBtn).onClick = OnSubmit;
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void show(IUInfoModel umodel,int type )
    {
        gameObject.SetActive(true);

		double d = umodel.GetMoney()*1.0/100;
		CaiPiaoYuELabel.text = String.Format("¥{0:0.00}", d);//彩票余额

		d = umodel.GetGold()*1.0/100;
		YouXiYuELable.text = String.Format("¥{0:0.00}", d);

        if (type == 1)
        {
            DuiHuanLeiXing.value = "兑换棋牌金额";
        }
        else
        {
			DuiHuanLeiXing.value = "兑换彩票金额";
        }
    }

	public void refresh(IUInfoModel umodel)
	{
		double d = umodel.GetMoney()*1.0/100;
		CaiPiaoYuELabel.text = String.Format("¥{0:0.00}", d);//彩票余额
		d = umodel.GetGold()*1.0/100;
		YouXiYuELable.text = String.Format("¥{0:0.00}", d);
	}

    void OnReturn(GameObject go)
    {
		AudioController.Instance.SoundPlay("active_item");
        gameObject.SetActive(false);
    }

    void OnSubmit(GameObject go)
    {
		AudioController.Instance.SoundPlay("active_item");
        if (string.IsNullOrEmpty(JinEInput.value))
        {
            msgSignal.Dispatch(new MsgPara("请输入金额",2));
            return;
        }
		if (string.IsNullOrEmpty(MiMaInput.value))
        {
            msgSignal.Dispatch(new MsgPara("请输入密码",2));
            return;
        }
        int type = 0;
		if (DuiHuanLeiXing.value == "兑换棋牌金额")
        {
            type = 1;
        }
        else
        {
            type = 2;
        }
		//兑换棋牌余额都是提交的数据都要乘以100。是以元为单位的。
		NetworkManager.Instance.TransferMoney(type,int.Parse(JinEInput.value)*100, MiMaInput.value);
		//都要输入框的数据清空，防止玩家重复点击。
		JinEInput.value = "";
		MiMaInput.value = "";
    }
}
