using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoreRecordPanel : MonoBehaviour {

    public GameObject returnbtn;
    public UILabel titleLable;

    public UIScrollView scrollview;
    public UIGrid grid;

    public GameObject MRItem;

    List<GameObject> itemList = new List<GameObject>();

    public void CreateItems(RecordObj obj)
    {

        titleLable.text = ConfigManager.Instance.GetLotteryCfgLoader().GetLotteryConfig(obj.lotteryId).name;


        itemList.Add(MRItem);

        List<RecordItemObj> rcItems = obj.recordItems;

        for (int i = 1; i < rcItems.Count; ++i)
        {

            GameObject go = Instantiate(MRItem);
            go.transform.parent = grid.transform;
            go.transform.localScale = Vector3.one;

            itemList.Add(go);
        }

        for (int i = 0; i < rcItems.Count; ++i)
        {
            MoreRecordItem clItem = itemList[i].GetComponent<MoreRecordItem>();
            clItem.titleLabel.text = string.Format("第[E4B62FFF]{0}[-]期", rcItems[i].titleStr) ;
            clItem.resultLabel.text = getResultStr(obj.lotteryType, rcItems[i]);

        }


        grid.repositionNow = true;

        StartCoroutine(process());
    }

    string getResultStr(int type, RecordItemObj itemobj)
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

    IEnumerator process()
    {

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < itemList.Count; ++i)
        {
            itemList[i].SetActive(false);
            itemList[i].SetActive(true);
        }

        scrollview.ResetPosition();
    }

    // Use this for initialization
    void Start()
    {
        UIEventListener.Get(returnbtn).onClick = OnCloseClick;
       
    }


    void OnCloseClick(GameObject go)
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
