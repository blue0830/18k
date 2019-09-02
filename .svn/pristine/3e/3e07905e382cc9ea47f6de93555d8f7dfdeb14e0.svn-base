using UnityEngine;
using System.Collections.Generic;

public class RecordItem : MonoBehaviour
{
    public UISprite lotteryIcon;
    public UILabel lotteryNameLabel;

    public UILabel titleNO;
    public UILabel lotteryResult;

    public UILabel[] Nos;
    public UILabel[] Results;

    public GameObject GoBuyBtn;
    public GameObject Morebtn;

    RecordObj recordObj;
    GoBuyDelegate goBuy;

    public Transform root;

    public int lotteryId = -1;

    public void FillContent(RecordObj obj, GoBuyDelegate onGoBuy)
    {
        if (lotteryId ==-1||lotteryId == obj.lotteryId)
        {
            lotteryId = obj.lotteryId;
        }
        else
        {
            return;
        }

        lotteryNameLabel.text = ConfigManager.Instance.GetLotteryCfgLoader().GetLotteryConfig(obj.lotteryId).name;


        recordObj = obj;
        goBuy = onGoBuy;
        List<RecordItemObj> list = obj.recordItems;
        if (list == null || list.Count == 0)
        {
            return;
        }
        titleNO.text = list[0].titleStr;
        lotteryResult.text = getResultStr_big(obj.lotteryType, list[0]);


        int count = list.Count-1 > 3 ? 3 : list.Count-1;

        for (int i = 0; i < count; i++)
        {
            Nos[i].text = list[i + 1].titleStr;
            Results[i].text = getResultStr(obj.lotteryType, list[i + 1]);
        }
    }


    string getResultStr(int type , RecordItemObj itemobj)
    {
        string str = "";
        if (type == 1 || type == 3)
        {
            str += itemobj.iNUM1;
            str += "  ";
            str += itemobj.iNUM2;
            str += "  ";
            str += itemobj.iNUM3;
            str += "  ";
            str += itemobj.iNUM4;
            str += "  ";
            str += itemobj.iNUM5;
        }
        else if (type == 2) //3d
        {
            str += itemobj.iNUM1;
            str += "  ";
            str += itemobj.iNUM2;
            str += "  ";
            str += itemobj.iNUM3;
        }
        else if (type == 4) //pk10
        {
            str += itemobj.iNUM1;
            str += "  ";
            str += itemobj.iNUM2;
            str += "  ";
            str += itemobj.iNUM3;
            str += "  ";
            str += itemobj.iNUM4;
            str += "  ";
            str += itemobj.iNUM5;
            str += "  ";
            str += itemobj.iNUM6;
            str += "  ";
            str += itemobj.iNUM7;
            str += "  ";
            str += itemobj.iNUM8;
            str += "  ";
            str += itemobj.iNUM9;
            str += "  ";
            str += itemobj.iNUM10;
        }

        return str;
    }

    string getResultStr_big(int type, RecordItemObj itemobj)
    {
        string str = "";
        if (type == 1 || type == 3)
        {
            str += numStr(itemobj.iNUM1);
            str += numStr(itemobj.iNUM2);
            str += numStr(itemobj.iNUM3);
            str += numStr(itemobj.iNUM4);
            str += numStr(itemobj.iNUM5);
        }
        else if (type == 2) //3d
        {
            str += itemobj.iNUM1;
            str += itemobj.iNUM2;
            str += itemobj.iNUM3;
        }
        else if (type == 4) //pk10
        {
            str += numStr(itemobj.iNUM1);
            str += numStr(itemobj.iNUM2);
            str += numStr(itemobj.iNUM3);
            str += numStr(itemobj.iNUM4);
            str += numStr(itemobj.iNUM5);
            str += numStr(itemobj.iNUM6);
            str += numStr(itemobj.iNUM7);
            str += numStr(itemobj.iNUM8);
            str += numStr(itemobj.iNUM9);
            str += numStr(itemobj.iNUM10);
        }

        return str;
    }

    string numStr(int x)  //为了显示字体，10转成a 11转成b
    {
        if (x == 10)
            return "a";
        if (x == 11)
            return "b";
        return x.ToString();
    }

    void BuyBtnClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        if (recordObj!=null)
            goBuy(recordObj.lotteryId);
    }

    void ShowMoreClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        if (recordObj == null)
            return;

        //todo
        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("MoreRecordPanel");
        GameObject ItemObj = Instantiate(asset) as GameObject;
        ItemObj.transform.parent = root;
        ItemObj.transform.localScale = Vector3.one;

        MoreRecordPanel script = ItemObj.GetComponent<MoreRecordPanel>();

        script.CreateItems(recordObj);
    }
    // Use this for initialization
    void Start()
    {
        UIEventListener.Get(GoBuyBtn).onClick = BuyBtnClick;
        UIEventListener.Get(Morebtn).onClick = ShowMoreClick;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
