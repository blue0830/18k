using UnityEngine;
using System;
using System.Collections;

public class TuiGuangPanel : MonoBehaviour {

    public UILabel TuiGuanText1;
	public UILabel TuiGuanText2;
	public UILabel TuiGuanText3;
	public UILabel TuiGuanText4;
	public UILabel TuiGuanText5;
    public GameObject returnBtn;
    void Start()
    {
        UIEventListener.Get(returnBtn).onClick = OnReturn;
    }

	public void OpenTuiGuangPanel(MSG_GP_USER_HDZXYJTGJLRUSULT para)
	{
		TuiGuanText1.text = para.iplaydrxx+"";
		TuiGuanText2.text = para.isjjl+"";
		TuiGuanText3.text = para.issjjl+"";
		TuiGuanText4.text = para.isssjjl+"";
		TuiGuanText5.text = "1.平台推广系统会自动即时发送奖金.\r\n2.同一个IP,平台账号,绑定的姓名及卡号,在每个活动日只可参与一次活动.\r\n3.请各位会员及代理谨记,GLG娱乐保留对此最终解释权.";
	}

	private string getStrMoney(int money){
		double d = money *1.0/100;
		return String.Format("¥{0:0.00}", d);//投注总额
	}

    // Update is called once per frame
    void Update()
    {

    }

    void OnReturn(GameObject go)
    {
		AudioController.Instance.SoundPlay("active_item");
        gameObject.SetActive(false);
    }
}
