using UnityEngine;
using System.Collections;

public class YouXiRecordItem : MonoBehaviour {
	//用户名
	public UILabel YongHuMingLabel;
	//房间名
	public UILabel FangJianMingLabel;
	//开始金额
	public UILabel KaiShiJinELabel;
	//输赢
	public UILabel ShuYingLabel;
	//剩余金额
	public UILabel ShengYuJinELabel;
	//游戏时长
	public UILabel YouXiShiChangLabel;
	//结束时间
	public UILabel JieShuShiJianLabel;

	void Start () {


	}

	// Update is called once per frame
	void Update () {


	}

	public void FillData(RecordLookItemObj obj)
	{
		YongHuMingLabel.text =obj.data[0];
		FangJianMingLabel.text = obj.data[1];
		KaiShiJinELabel.text = obj.data[2];
		ShuYingLabel.text = obj.data[3];
		ShengYuJinELabel.text = obj.data[4];
		YouXiShiChangLabel.text = obj.data[5];
		JieShuShiJianLabel.text = obj.data[6];
	}
}
