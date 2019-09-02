using UnityEngine;
using System.Collections;

//充值记录
public class ChongZhiRecordItem : MonoBehaviour {
	//流水号
	public UILabel LiuShuiHaoLabel;
	//充值金额
	public UILabel ChongZhiJinELabel;
	//充值时间
	public UILabel ChongZhiShiJianLabel;
	//充值状态
	public UILabel ChongZhiZhuangTaiLabel;
	//支付平台
	public UILabel ZhiFuPinTaiLabel;

	void Start () {


	}

	// Update is called once per frame
	void Update () {


	}

	public void FillData(RecordLookItemObj obj)
	{
		LiuShuiHaoLabel.text =obj.data[0];
		ChongZhiJinELabel.text = obj.data[1];
		ChongZhiShiJianLabel.text = obj.data[2];
		ChongZhiZhuangTaiLabel.text = obj.data[3];
		ZhiFuPinTaiLabel.text = obj.data[4];
	}
}
