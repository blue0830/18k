using UnityEngine;
using System.Collections;

public class XiaJiTouZhuTongJiItem : MonoBehaviour {

    public UILabel[] Labels;

    public GameObject opBtn;

	void Start () {

        UIEventListener.Get(opBtn).onClick = Onmingxi;
    }

	// Update is called once per frame
	void Update () {


	}

	public void FillData(RecordLookItemObj obj)
	{
        for (int i = 0; i < Labels.Length; ++i)
        {
            Labels[i].text = obj.data[i];
        }
	}


    void Onmingxi(GameObject go)
    {
		XiajiTouZhuMingXiPanel.startDate = XiaJiTouZhuTongJiPanel.startDate;
		XiajiTouZhuMingXiPanel.endDate = XiaJiTouZhuTongJiPanel.endDate;
		int uid = int.Parse (Labels [0].text);
		if (uid == Global.CurrentUserId) {
			XiajiTouZhuMingXiPanel.lookuserId = 0;
		} else {
			XiajiTouZhuMingXiPanel.lookuserId = uid;
		}
		XiajiTouZhuMingXiPanel.chName = "";
		NetworkManager.Instance.LookupRecord(XiajiTouZhuMingXiPanel.byRord, 2, XiajiTouZhuMingXiPanel.byRord, 1, "", XiajiTouZhuMingXiPanel.startDate, XiajiTouZhuMingXiPanel.endDate, XiajiTouZhuMingXiPanel.lookuserId);
    }
}
