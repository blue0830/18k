using System;
using UnityEngine;
using System.Collections;

public class SevenDayPanel : MonoBehaviour {
	public UILabel continuousCheckInLabel;

	public UILabel currentConsumeLabel;
	public UILabel descLabel;

    public UILabel[] smLabels;
	public UILabel[] sm2Labels;

    public GameObject[] signBtns;

    public GameObject awardBtn;

    public GameObject returnBtn;

    public void Open7dayPanel(MSG_GP_USER_HDZX7TLRULERESULT para)
    {
        for (int i = 0; i < smLabels.Length; ++i)
        {
			smLabels[i].text = getStrMoney(para.iXifei[i]);
			sm2Labels[i].text = getStrMoney(para.iSong[i]);
        }
		currentConsumeLabel.text = getStrMoney(para.iCurDayXF)+"元";
		continuousCheckInLabel.text = para.iSingedCount+"天";

        for (int i = 0; i < signBtns.Length; ++i)
        {
            if (i + 1 == para.iCueNumBtn) //当前签到按钮
            {
                signBtns[i].SetActive(true);
				UIEventListener.Get(signBtns[i]).onClick = OnSign;
            }
            else
            {
                signBtns[i].SetActive(false);
            }
        }

        if (para.BtnLingjiangVisible == 1) //可以领奖
        {
            awardBtn.SetActive(true);
        }
        else
        {
            awardBtn.SetActive(false);
        }
		//descLabel.text = "1.每天有效投注金额达到2000元即可参加签到活动，必须连续签到7天视满勤签到。如：期间中断，前期签到视为无效，则从下次签到重新计算周期\r\n2.每日签到最高封顶奖金100元视为满额签到，周期均满勤满额签到额外奖励100元。如，每日消费≥20000元*连续7天*奖金0.5%=20000*7*0.5%=700+100\r\n3.温馨提醒：本活动7天满勤签到最低奖金70元。满勤满额签到最高奖金为800元。注意：每天签到1次，请即时关注投注流水，记得下线前参加签到。\r\n4.活动期间禁止一切刷佣金行为，对此平台将实时监控，玩法限制：5星大于80000注，4星大于8000注，3星大于800注，2星大于80注，定位胆大于8码的一律按作弊处理，严重者给予封号处理。";
		descLabel.text = "1.每天有效投注金额达到2000元即可参加签到活动，必须连续签到7天视满勤签到。\r\n2.活动期间禁止一切刷佣金行为，对此平台将实时监控。";
    }

    // Use this for initialization
    void Start () {
//        for (int i = 0; i < signBtns.Length; ++i)
//        {
//            UIEventListener.Get(signBtns[i]).onClick = OnSign;
//        }
        UIEventListener.Get(awardBtn).onClick = OnAward;
        UIEventListener.Get(returnBtn).onClick = OnReturn;
        
    }

	private string getStrMoney(int money){
		double d = money *1.0/100;
		return String.Format("{0:0.00}", d);//投注总额
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnSign(GameObject go)
    {
        int i = int.Parse(go.name);
        NetworkManager.Instance.SevenDaySign(i);
    }

    void OnAward(GameObject go)
    {
		AudioController.Instance.SoundPlay("active_item");
        NetworkManager.Instance.SevenDayGetAward();
    }

    void OnReturn(GameObject go)
    {
		AudioController.Instance.SoundPlay("active_item");
        gameObject.SetActive(false);
    }
}
