using UnityEngine;
using System.Collections;

public class ZhuihaoItem : MonoBehaviour {

    public UIInput beishu;
    public UILabel qihao;
    public UILabel jine;
    public UIToggle selectToggle;

    public UISprite bgsprite;

    public string singleJine;

    public UpdateBotDelegate updateBot;
    public int id;
    // Use this for initialization
    void Start () {

        EventDelegate.Add(beishu.onChange, OnbeishuChange);
        EventDelegate.Add(selectToggle.onChange, OnselectChange);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnselectChange()
    {      
            updateBot();
    }

    public void OnbeishuChange()
    {   
        double intje = MathUtil.calculate(singleJine, beishu.value, '*');
        jine.text = intje.ToString();

        updateBot();
    }
}
