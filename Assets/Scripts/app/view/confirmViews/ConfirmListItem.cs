using UnityEngine;
using System.Collections;

public class ConfirmListItem : MonoBehaviour {

    public UILabel LotteryTxtLabel;
    public UILabel SubModeLabel;
    public UILabel zbyLabel;

    public GameObject DeleteBtn;

    public ConfirmPanelObj obj;

    public void FillContent(ConfirmPanelObj cobj)
    {
        obj = cobj;
        string mode = "-";
        if (cobj.model == 1)
        {
            mode += "元模式";
        }
        else if (cobj.model == 2)
        {
            mode += "角模式";
        }
        else
        {
            mode += "分模式";
        }
        LotteryTxtLabel.text = cobj.showContent+ mode;

        SubModeLabel.text = string.Format("[{0}]", cobj.subCfg.name);

        zbyLabel.text = string.Format("{0}注 {1}倍 {2}元", cobj.zs, cobj.bs , cobj.amount);

    }
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
