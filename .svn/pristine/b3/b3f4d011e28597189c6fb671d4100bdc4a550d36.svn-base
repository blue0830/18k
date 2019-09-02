using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.signal.impl;

public class ConfirmPanel : MonoBehaviour {


 
    public GameObject ReturnBtn;
    public UILabel TitleLabel;

    public UIScrollView scrollview;

    public GameObject DeleteAllBtn;
    public GameObject ConfirmBtn;

    public UILabel ConfirmLabel;

    public GameObject addNewBtn;

    public GameObject zhuihaoBtn;


    public UIGrid Grid;

    public UIInput zhuiqi;
    public UIInput beishu;

    public UIToggle zhuihaoToggle;
    public UIToggle stopToggle;

    public GameObject beishuAdd;
    public GameObject beishuSub;

    //subUI
    List<GameObject> itemList = new List<GameObject>();

    public Signal<ConfirmPanelObj> deleteSignal = new Signal<ConfirmPanelObj>();

    public void CreateItems(List<ConfirmPanelObj> itemobjs)
    {
        Clear();

        if (itemobjs.Count == 0)
            return;
        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("ConfirmItem");
        GameObject ItemObj = Instantiate(asset) as GameObject;
        ItemObj.transform.parent = Grid.transform;
        ItemObj.transform.localScale = Vector3.one;
        ItemObj.name = "0";
        itemList.Add(ItemObj);

        for (int i = 1; i < itemobjs.Count; ++i)
        {

            GameObject go = Instantiate(ItemObj);
            go.transform.parent = ItemObj.transform.parent;
            go.transform.localScale = Vector3.one;

            go.name = i.ToString();

            itemList.Add(go);
        }

        for (int i = 0; i < itemList.Count; ++i)
        {
            ConfirmListItem clItem = itemList[i].GetComponent<ConfirmListItem>();
            clItem.FillContent(itemobjs[i]);
            UIEventListener.Get(clItem.DeleteBtn).onClick = OnDeleteItem;
        }

   
        Grid.repositionNow = true;
      
        StartCoroutine(process());
    }

    void OnEnable()
    {

    }

    void Clear()
    {
        for (int i = 0; i < itemList.Count; ++i)
        {
            Destroy(itemList[i]);
        }
        itemList.Clear();
 
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

    public void DeleteAll()
    {
        Clear();
    }

    void OnDeleteItem(GameObject item)
    {
        AudioController.Instance.SoundPlay("active_item");

        GameObject parentGo = item.transform.parent.gameObject;

        ConfirmListItem temp = parentGo.GetComponent<ConfirmListItem>();
        deleteSignal.Dispatch(temp.obj);

      
        itemList.Remove(parentGo);
        Destroy(parentGo);
        Grid.repositionNow = true;
 

        StartCoroutine(process());
    }
        
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
