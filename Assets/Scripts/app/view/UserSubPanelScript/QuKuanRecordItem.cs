using UnityEngine;
using System.Collections;

public class QuKuanRecordItem : MonoBehaviour {
	//流水号
	public UILabel LiuShuiHaoLabel;
	//提现时间
	public UILabel TiXianShiJianLabel;
	//收款开户行
	public UILabel ShouKuanKaiHuHangLabel;
	//提现金额
	public UILabel TiXianJinELabel;
	//提现人
	public UILabel TiXianRenLabel;
	//提现状态
	public UILabel TiXianStatusLabel;
	//提现说明
	public UILabel TiXianDescLabel;

	void Start () {


	}

	public void FillData(RecordLookItemObj obj)
	{
		LiuShuiHaoLabel.text =obj.data[0];
		TiXianShiJianLabel.text=obj.data[1];
		ShouKuanKaiHuHangLabel.text=obj.data[2];
		TiXianJinELabel.text=obj.data[3];
		TiXianRenLabel.text=obj.data[4];
		TiXianStatusLabel.text=obj.data[5];
		TiXianDescLabel.text=obj.data[6];
	}

	// Update is called once per frame
	void Update () {


	}
}
