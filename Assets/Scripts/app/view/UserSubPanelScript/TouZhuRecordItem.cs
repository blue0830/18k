using UnityEngine;
using System.Collections;

public class TouZhuRecordItem : MonoBehaviour {
	//序列号
	public UILabel XuLieHaoLabel;
	//投注彩种
	public UILabel TouZhuCaiZhongLabel;
	//投注模式
	public UILabel TouZhuMoShiLabel;
	//投注期号
	public UILabel TouZhuQiHaoLabel;
	//投注金额
	public UILabel TouZhuJinELabel;
	//盈亏记录
	public UILabel YingKuiJiLuLabel;
	//投注日期
	public UILabel TouZhuRiQiLabel;
	//状态
	public UILabel StautsLabel;

	//详细
	public GameObject DetailBtn;
	//撤单
	public GameObject CheDanBtn;

    int noteId;
	void Start () {
        UIEventListener.Get(DetailBtn).onClick = OnDetail;
        UIEventListener.Get(CheDanBtn).onClick = OnCheDan;

	}

	// Update is called once per frame
	void Update () {


	}

    public void FillData(RecordLookItemObj obj)
    {
        noteId = int.Parse(obj.data[0]);
        XuLieHaoLabel.text =obj.data[0];

        TouZhuCaiZhongLabel.text = obj.data[1];

        TouZhuMoShiLabel.text = obj.data[2];

        TouZhuQiHaoLabel.text = obj.data[3];

        TouZhuJinELabel.text = obj.data[4];

        YingKuiJiLuLabel.text = obj.data[5];

        TouZhuRiQiLabel.text = obj.data[6];

        StautsLabel.text = obj.data[7];

       
        for (int i = 0; i < obj.data.Count; ++i)
        {
            if (obj.data[i] == "双击明细")
            {
                CheDanBtn.SetActive(false);
                CheDanBtn.transform.parent.GetComponent<UIGrid>().Reposition();
            }
        }
    }

    void OnDetail(GameObject go)
    {
		AudioController.Instance.SoundPlay("active_item");
        NetworkManager.Instance.TouchuMingxi(noteId);
    }
    void OnCheDan(GameObject go)
    {
		AudioController.Instance.SoundPlay("active_item");
        NetworkManager.Instance.TouchuCheDan(noteId);
    }
}


