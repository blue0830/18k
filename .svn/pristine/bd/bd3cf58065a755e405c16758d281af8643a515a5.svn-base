using UnityEngine;
using System.Collections;

public class FanDianPanel : MonoBehaviour {
    public UILabel username;
    public UILabel fandian;

    public UIInput xiajifandian;
    public GameObject xiajifandianadd;
    public GameObject xiajifandiansub;

    public GameObject btn;

    public GameObject returnBtn;

    string name;

    public void show(uint id,string uname, string fdian)
    {
        name = uname;
        gameObject.SetActive(true);

        username.text = uname;
        fandian.text = fdian;
        xiajifandian.value = "0.0";
    }

	// Use this for initialization
	void Start () {
        UIEventListener.Get(btn).onClick = OnChange;
        UIEventListener.Get(returnBtn).onClick = OnReturn;
        

        UIEventListener.Get(xiajifandianadd).onClick = OnChangeXiaJiFanDian;
        UIEventListener.Get(xiajifandiansub).onClick = OnChangeXiaJiFanDian;

        EventDelegate.Add(xiajifandian.onSubmit, XiaJiFandianChange);
    }

    void OnChange(GameObject go)
    {
        NetworkManager.Instance.ChangeFanDian(name, xiajifandian.value);


        NetworkManager.Instance.LookupRecord(HuiYuanZiLiaoPanel.byRord, 1, HuiYuanZiLiaoPanel.byRord, 1, HuiYuanZiLiaoPanel.chName, HuiYuanZiLiaoPanel.startDate, HuiYuanZiLiaoPanel.endDate);
    }

    void OnReturn(GameObject go)
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
	
	}


    void OnChangeXiaJiFanDian(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        float beishu = float.Parse(xiajifandian.value);
        if (go.name == "add")
        {
			if (beishu <= Global.CurrentUserPoint-0.1)
            {
                beishu += 0.1f;
                xiajifandian.value = string.Format("{0:0.0}", beishu);
            }
        }
        else
        {

            if (beishu > 0)
            {
                beishu -= 0.1f;
                xiajifandian.value = string.Format("{0:0.0}", beishu);
            }
        }
    }

    void XiaJiFandianChange()
    {

        if (string.IsNullOrEmpty(xiajifandian.value))
        {
            xiajifandian.value = "0.0";
        }
        float a = 0;
        if (float.TryParse(xiajifandian.value, out a))
        {
            if (a <= 0)
            {
                xiajifandian.value = "0.0";
            }
			else if (a > Global.CurrentUserPoint - 0.1)
            {
				xiajifandian.value = string.Format("{0:0.0}", Global.CurrentUserPoint - 0.1<0 ? 0 : Global.CurrentUserPoint - 0.1);
            }
        }
        else
        {
            xiajifandian.value = "0.0";
        }

    }
}
