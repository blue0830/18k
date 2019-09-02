using UnityEngine;
using System.Collections;

public class TouZhuXiangQingPanel : UserSubPanelBase {
	//玩家账号
	public UILabel WanJiaZhangHaoLabel;
	//投注数量
	public UILabel TouZhuShuLiangLabel;
	//订单号
	public UILabel DingDanHaoLabel;
	//投注金额
	public UILabel TouZhuJinELabel;
	//投注倍数
	public UILabel TouZhuBeiShuLabel;
	//期号
	public UILabel QiHaoLabel;
	//投注总额
	public UILabel TouZhuZongELabel;
	//彩种
	public UILabel CaiZhongLabel;
	//中奖注数
	public UILabel ZhongJiangZhuShuLabel;
	//玩法
	public UILabel WanFaLabel;
	//单注奖金
	public UILabel DanZhuJiangJinLabel;
	//开奖号码
	public UILabel KaiJiangHaoMaLabel;
	//中奖金额
	public UILabel ZhongJiangJinELabel;
	//下单时间
	public UILabel XiaDanShiJianLabel;
	//返点
	public UILabel FanDianLabel;
	//返点金额
	public UILabel FanDianJinELabel;
	//状态
	public UILabel StatusLabel;
	//盈亏
	public UILabel YingKuiLabel;
	//投注内容
	public UILabel TouZhuNeiRongLabel;

    public UITextList TouZhuNeiRong;

	//返回按钮
	public GameObject ReturnBtn;

 

	void Start () {
		UIEventListener.Get(ReturnBtn).onClick = OnReturn;

        // for (int i = 0; i < 30; ++i)
        //{
        //    TouZhuNeiRong.Add(((i % 2 == 0) ? "[FFFFFF]" : "[AAAAAA]") +
        //        "This is an example paragraph for the text list, testing line " + i + "[-]");
        //}


       
	}

	// Update is called once per frame
	void Update () {

	}

	void OnReturn(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		gameObject.SetActive(false);
	}

	public void Show(TouzhuXiangxi dataobj)
	{
		gameObject.SetActive(true);

        WanJiaZhangHaoLabel.text = dataobj.UserName.ToString();

        TouZhuShuLiangLabel.text = dataobj.OrderCount.ToString();

        DingDanHaoLabel.text = dataobj.iNoteID.ToString();

        TouZhuJinELabel.text = dataobj.SingleMoney.ToString();

        TouZhuBeiShuLabel.text = dataobj.Multiple.ToString();

        QiHaoLabel.text = dataobj.ActivityName;

        TouZhuZongELabel.text = dataobj.Amount.ToString();

        CaiZhongLabel.text = dataobj.ClassName;

        ZhongJiangZhuShuLabel.text = dataobj.BingoCount.ToString();

        WanFaLabel.text = dataobj.OrderTypeName;

        DanZhuJiangJinLabel.text = dataobj.PeiLv.ToString();

        KaiJiangHaoMaLabel.text = dataobj.OpenNum;

        ZhongJiangJinELabel.text = dataobj.BingoMoney.ToString();

        XiaDanShiJianLabel.text = TimeHelper.yyyyMMddHHmmss(dataobj.AddTM);

        FanDianLabel.text = dataobj.Point.ToString();

        FanDianJinELabel.text = dataobj.PointMoney.ToString();

        //0表示未开奖，1表示中奖，2表示未中奖，3表示中奖停追
        string str = "";
        if (dataobj.IsBingo == 0)
        {
            str = "未开奖";
        }
        else if (dataobj.IsBingo == 1)
        {
            str = "中奖";
        }
        else if (dataobj.IsBingo == 2)
        {
            str = "未中奖";
        }
        else
        {
            str = "中奖停追";
        }

        StatusLabel.text = str;

        YingKuiLabel.text = dataobj.ResultMoney.ToString();

        //TouZhuNeiRongLabel.text = dataobj.OrderValue;

        orderValue = dataobj.OrderValue;
        StartCoroutine(fill());

        
	}

    string orderValue;
    IEnumerator fill()
    {

        yield return new WaitForEndOfFrame();
        TouZhuNeiRong.Clear();
        TouZhuNeiRong.Add(orderValue);
    }
}
