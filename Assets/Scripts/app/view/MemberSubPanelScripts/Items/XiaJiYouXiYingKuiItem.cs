using UnityEngine;
using System.Collections;

public class XiaJiYouXiYingKuiItem : MonoBehaviour {

    public UILabel[] Labels;

    public GameObject opBtn;
    public GameObject mingxiBtn;     

	void Start () {

        UIEventListener.Get(opBtn).onClick = Onkanxiaji;
        UIEventListener.Get(mingxiBtn).onClick = Onmingxi;
    }

	// Update is called once per frame
	void Update () {


	}

	public void FillData(RecordLookItemObj obj)
	{

        if (uint.Parse(obj.data[0]) == XiaJiYouXiYingKuiPanel.GetLastId())
        {
            opBtn.SetActive(false);
        }
        else
        {
            opBtn.SetActive(true);
        }

        for (int i = 0; i < Labels.Length; ++i)
        {
            Labels[i].text = obj.data[i];
        }
	}
    void Onkanxiaji(GameObject go)
    {
        XiaJiYouXiYingKuiPanel.AddNameId(Labels[1].text, uint.Parse(Labels[0].text));
        NetworkManager.Instance.LookupRecord(XiaJiYouXiYingKuiPanel.byRord, 2, XiaJiYouXiYingKuiPanel.byRord, 1, XiaJiYouXiYingKuiPanel.chName, XiaJiYouXiYingKuiPanel.startDate, XiaJiYouXiYingKuiPanel.endDate, (int)XiaJiYouXiYingKuiPanel.GetLastId(), XiaJiYouXiYingKuiPanel.chBySj);
    }

    void Onmingxi(GameObject go)
    {
        XiaJiYouXiJiLuPanel.startDate = XiaJiYouXiYingKuiPanel.startDate;
        XiaJiYouXiJiLuPanel.endDate = XiaJiYouXiYingKuiPanel.endDate;
        int uid = int.Parse(Labels[0].text);
		if (uid == Global.CurrentUserId)
        {
            XiaJiYouXiJiLuPanel.lookuserId = 0;
        }
        else
        {
            XiaJiYouXiJiLuPanel.lookuserId = uid;
        }
        XiaJiYouXiJiLuPanel.chName = "";
        NetworkManager.Instance.LookupRecord(XiaJiYouXiJiLuPanel.byRord, 2, XiaJiYouXiJiLuPanel.byRord, 1, "", XiaJiYouXiJiLuPanel.startDate, XiaJiYouXiJiLuPanel.endDate, XiaJiYouXiJiLuPanel.lookuserId);
    }

}
