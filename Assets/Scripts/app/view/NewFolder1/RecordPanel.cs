using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecordPanel : MonoBehaviour {

    public GameObject returnbtn;

    public UIGrid Grid;
    public UIScrollView scrollview;
    public GameObject RecordItem;

    List<GameObject> itemList = new List<GameObject>();

    List<RecordItem> _sscList = new List<RecordItem>();
    List<RecordItem> _115List = new List<RecordItem>();
    List<RecordItem> _3dList = new List<RecordItem>();
    List<RecordItem> _pk10List = new List<RecordItem>();

    public void CreateItems(List<LotteryConfig> lcfg )
    {
        
         
        itemList.Add(RecordItem);

        for (int i = 1; i < lcfg.Count; ++i)
        {

            GameObject go = Instantiate(RecordItem);
            go.transform.parent = RecordItem.transform.parent;
            go.transform.localScale = Vector3.one;

            itemList.Add(go);
        }

        for (int i = 0; i < itemList.Count; ++i)
        {
 
            NetworkManager.Instance.GetRecord(lcfg[i].lotteryId);

            RecordItem clItem = itemList[i].GetComponent<RecordItem>();

            clItem.root = transform.parent;

            if (lcfg[i].lotteryType == 1)
            {
                _sscList.Add(clItem);
            }
            else if (lcfg[i].lotteryType == 2)
            {
                _3dList.Add(clItem);
            }
            else if (lcfg[i].lotteryType == 3)
            {
                _115List.Add(clItem);
            }
            else if (lcfg[i].lotteryType == 4)
            {
                _pk10List.Add(clItem);
            }
        }


        Grid.repositionNow = true;

        StartCoroutine(process());
    }

    public void UpdateInfo(RecordObj robj, GoBuyDelegate OngoBuy)
    {

        if (robj.lotteryType == 1)
        {
            for (int i = 0; i < _sscList.Count; ++i)
            {
                if (_sscList[i].lotteryId == -1)
                {
                    _sscList[i].FillContent(robj, OngoBuy);
                    break;
                }
            }
        }
        else if (robj.lotteryType == 2)
        {
            
            for (int i = 0; i < _3dList.Count; ++i)
            {
            
                if (_3dList[i].lotteryId == -1)
                {
                    _3dList[i].FillContent(robj, OngoBuy);
                    break;
                }
            }
        }
        else if (robj.lotteryType == 3)
        {
            
            for (int i = 0; i < _115List.Count; ++i)
            {
                
                if (_115List[i].lotteryId == -1)
                {
                    _115List[i].FillContent(robj, OngoBuy);
                    break;
                }
            }
        }
        else if (robj.lotteryType == 4)
        {
            
            for (int i = 0; i < _pk10List.Count; ++i)
            {
       
                if (_pk10List[i].lotteryId == -1)
                {
                    _pk10List[i].FillContent(robj, OngoBuy);
                    break;
                }
            }
        }
    }

    IEnumerator process()
    {

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < itemList.Count; ++i)
        {
            itemList[i].SetActive(false);
            itemList[i].SetActive(true);
        }

        scrollview.transform.localPosition = Vector3.zero;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
