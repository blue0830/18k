using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class Scrollview_V : MonoBehaviour {


    public UIGrid Grid_v;

 

    List<GameObject> items = new List<GameObject>();
    //subUI

    public List<NumbersItem> NumsItems = new List<NumbersItem>();

    

    public void CreateScrollItems(LotterySubModeCfg ltySubCf, NumClickDelegate OnNumClick )
    {
        Clear();

        List<RowModecfg> rmcs = ltySubCf.rowModecfgs;

 
        if (rmcs == null || rmcs.Count == 0)
        {
            Debug.LogError("Scrollview_V: rmc == null || rmc.Count == 0");
            return;
        }


        int quantity = rmcs[0].numTo- rmcs[0].numFrom+1; //有多少个数字圈要显示，用来确定每行高度
        int height = 80 * ((quantity / 6) + 1)+40;

        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("NumbersItem");
        GameObject ItemObj = Instantiate(asset) as GameObject;
        ItemObj.transform.parent = Grid_v.transform;
        ItemObj.transform.localScale = Vector3.one;

        Vector3 newsize = ItemObj.GetComponent<BoxCollider>().size;
        newsize.y = height;
        ItemObj.GetComponent<BoxCollider>().size = newsize;


        ItemObj.name = "0";
        items.Add(ItemObj);
        for (int i = 1; i < rmcs.Count; ++i)
        {

            GameObject go = Instantiate(ItemObj);
            go.transform.parent = ItemObj.transform.parent;
            go.transform.localScale = Vector3.one;

            go.name = i.ToString();
 
            items.Add(go);
        }

        for (int i = 0; i < items.Count; ++i)
        {
            NumbersItem temp = items[i].GetComponent<NumbersItem>();
            temp.CreateRow(ltySubCf, rmcs[i],i, OnNumClick, DantuoClick);
            NumsItems.Add(temp);
        }

  
        Grid_v.cellHeight = height;
        Grid_v.repositionNow = true;

 


        StartCoroutine(process());
    }

    //胆拖 点击 两行不能重复
    void DantuoClick(int lInx ,string ClickNum)
    {
        if (NumsItems.Count == 2)
        {
            if (lInx == 0)  //第一行
            {
                NumsItems[1].RemoveSelection(ClickNum);
            }
            else
            {
                NumsItems[0].RemoveSelection(ClickNum);
            }
        }
    }

    IEnumerator process()
    {

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < items.Count; ++i)
        {
            items[i].SetActive(false);
            items[i].SetActive(true);
        }
        gameObject.GetComponent<UIScrollView>().ResetPosition();
    }

    void Clear()
    {
        for (int i = 0; i < items.Count; ++i)
        {
            Destroy(items[i]);
        }
        items.Clear();

        NumsItems.Clear();
    }

    public void ClearSelection()
    {
        for (int i = 0; i < NumsItems.Count; ++i)
        {
            NumsItems[i].ClearSelection();
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
