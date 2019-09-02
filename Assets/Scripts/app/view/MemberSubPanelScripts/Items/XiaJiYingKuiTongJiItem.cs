using UnityEngine;
using System.Collections;

public class XiaJiYingKuiTongJiItem : MonoBehaviour {

    public UILabel[] Labels;

    public GameObject chakanxiajiBtn;

    public GameObject mingxiBtn;

	void Start () {

        UIEventListener.Get(chakanxiajiBtn).onClick = Onkanxiaji;
        UIEventListener.Get(mingxiBtn).onClick = Onmingxi;
    }

	// Update is called once per frame
	void Update () {


	}

	public void FillData(RecordLookItemObj obj)
	{
		if (uint.Parse(obj.data[0]) == XiaJiYingKuiTongjiPanel.GetLastId())
        {
            chakanxiajiBtn.SetActive(false);
        } else
        {
            chakanxiajiBtn.SetActive(true);
        }

        for (int i = 0; i < Labels.Length; ++i)
        {
            Labels[i].text = obj.data[i];
        }
	}

    void Onkanxiaji(GameObject go)
    {
        XiaJiYingKuiTongjiPanel.AddNameId(Labels[1].text, uint.Parse(Labels[0].text));
        NetworkManager.Instance.LookupRecord(XiaJiYingKuiTongjiPanel.byRord, 2, XiaJiYingKuiTongjiPanel.byRord, 1, XiaJiYingKuiTongjiPanel.chName, XiaJiYingKuiTongjiPanel.startDate, XiaJiYingKuiTongjiPanel.endDate, (int)XiaJiYingKuiTongjiPanel.GetLastId(), XiaJiYingKuiTongjiPanel.chBySj);
    }

    void Onmingxi(GameObject go)
    {
		XiaJiYingKuiMingXIPanel.startDate = XiaJiYingKuiTongjiPanel.startDate;
		XiaJiYingKuiMingXIPanel.endDate = XiaJiYingKuiTongjiPanel.endDate;
		int uid = int.Parse (Labels [0].text);
		if (uid == Global.CurrentUserId) {
			XiaJiYingKuiMingXIPanel.lookuserId = 0;
		} else {
			XiaJiYingKuiMingXIPanel.lookuserId = uid;
		}
        XiaJiYingKuiMingXIPanel.chName = "";
        NetworkManager.Instance.LookupRecord(XiaJiYingKuiMingXIPanel.byRord, 2, XiaJiYingKuiMingXIPanel.byRord, 1, "", XiaJiYingKuiMingXIPanel.startDate, XiaJiYingKuiMingXIPanel.endDate,XiaJiYingKuiMingXIPanel.lookuserId);
    }
}
