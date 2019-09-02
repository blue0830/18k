using UnityEngine;
using System.Collections;

public class HuiYuanZiLiaoItem : MonoBehaviour {

    public UILabel[] Labels;

    public GameObject xiugaifandianBtn;
    public GameObject chakanxiajiBtn;
    public GameObject fenpeipeieBtn;

    ShowFandian showFandian;


    void Start () {
        UIEventListener.Get(xiugaifandianBtn).onClick = Ongaifandian;
        UIEventListener.Get(chakanxiajiBtn).onClick = Onkanxiaji;
        UIEventListener.Get(fenpeipeieBtn).onClick = Ongaipeie;
    }

	// Update is called once per frame
	void Update () {
	}

	public void FillData(RecordLookItemObj obj, ShowFandian f)
	{
        showFandian = f;
        for (int i = 0; i < Labels.Length; ++i)
        {
            Labels[i].text = obj.data[i];
        }
	}

    void Onkanxiaji(GameObject go)
    {
        HuiYuanZiLiaoPanel.AddNameId(Labels[1].text, uint.Parse(Labels[0].text));
        NetworkManager.Instance.LookupRecord(HuiYuanZiLiaoPanel.byRord, 2, HuiYuanZiLiaoPanel.byRord, 1, HuiYuanZiLiaoPanel.chName, HuiYuanZiLiaoPanel.startDate, HuiYuanZiLiaoPanel.endDate, (int)HuiYuanZiLiaoPanel.GetLastId(), HuiYuanZiLiaoPanel.chBySj);
    }

    void Ongaifandian(GameObject go)
    {
        showFandian(uint.Parse(Labels[0].text), Labels[1].text, Labels[4].text);
    }

    void Ongaipeie(GameObject go)
    {
        NetworkManager.Instance.FenPeiPeiE(uint.Parse(Labels[0].text));
    }
}
