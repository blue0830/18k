using UnityEngine;
using System.Collections;

public class ChongZHiSongPanel : MonoBehaviour {

    public UILabel Timelabel;
    public UILabel label1;
	public UILabel label2;
	public GameObject awardBtn;
    // Use this for initialization

    public GameObject returnBtn;

	public void OpenChongZhiSongPanel(MSG_GP_USER_HDZXXRCZSRULET para)
	{
		Timelabel.text = string.Format("活动时间{0}至{1}", TimeHelper.GetTimeStrFromUlong(para.timesatarTM), TimeHelper.GetTimeStrFromUlong(para.timeendTM));
		label1.text = string.Format("新开户会员当天首次充值{0}以上,立即送{1}红包\r\n新开户会员当天首次充值{2}以上,立即送{3}红包", para.iczMoney1, para.ihbmoney1,para.iczMoney2, para.ihbmoney2);
		label2.text = "1.同一个IP，平台账号，绑定的姓名及卡号，在每个活动日只可参与一次活动。\r\n2.活动期间禁止一切刷佣金行为，对此平台将实时监控，玩法限制：5星大于80000注，4星大于8000注，3星大于800注，2星大于80注，定位胆大于8码的一律按作弊处理，严重者给予封号处理。";
		if (para.IsCanLq == 1) { //可以领奖
			awardBtn.SetActive (true);
		} else {
			awardBtn.SetActive (false);
		}
	}

    void Start () {
        UIEventListener.Get(returnBtn).onClick = OnReturn;
		UIEventListener.Get(awardBtn).onClick = OnAward;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnAward(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		NetworkManager.Instance.ChongZhiSongGetAward();
	}

    void OnReturn(GameObject go)
    {
		AudioController.Instance.SoundPlay("active_item");
        gameObject.SetActive(false);
    }
}
