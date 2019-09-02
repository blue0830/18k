using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SubModeItem : MonoBehaviour {


    public UIGrid Grid_h;
    public GameObject ItemObj;
    public UILabel modeNameLabel;

    const int Colum = 2;
    //int row = modeCfgs.Count / Colum + 1;


    public List<GameObject> items = new List<GameObject>();
    //subUI

    public void CreateScrollItems(List<LotterySubModeCfg> SubModeCfgs)
    {
        if (ItemObj == null)
        {
            Debug.LogError("Scrollview_H: ItemObj == null");
            return;
        }
        if (SubModeCfgs == null|| SubModeCfgs.Count==0)
        {
            Debug.LogError("Scrollview_H: SubModeCfgs == null|| SubModeCfgs.Count==0");
            return;
        }
        items.Clear();

 
        ItemObj.name = SubModeCfgs[0].subModeId.ToString();
        ItemObj.transform.Find("UILabel").GetComponent<UILabel>().text = SubModeCfgs[0].name;
        ItemObj.transform.Find("selected").gameObject.SetActive(true);
        items.Add(ItemObj);
        for (int i = 1; i < SubModeCfgs.Count; ++i)
        {
            GameObject go = Instantiate(ItemObj);
            go.transform.parent = ItemObj.transform.parent;
            go.transform.localScale = Vector3.one;

            go.name = i.ToString();

            go.name = SubModeCfgs[i].subModeId.ToString();
            go.transform.Find("UILabel").GetComponent<UILabel>().text = SubModeCfgs[i].name;
            go.transform.Find("selected").gameObject.SetActive(false);
            items.Add(go);
        }

        
        Grid_h.repositionNow = true;

        gameObject.GetComponent<UIScrollView>().ResetPosition();


        StartCoroutine(process());

    }

    IEnumerator process()
    {

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < items.Count; ++i)
        {
            items[i].SetActive(false);
            items[i].SetActive(true);
        }
    }

    public void ClearItems()
    {

        for (int i = 1; i < items.Count; ++i)
        {
            Destroy(items[i]);
        }

   
        Grid_h.Reposition();

    }

    public void ChooseSubMode(int id)
    {
        for (int i = 0; i < items.Count; ++i)
        {
            if (items[i].name == id.ToString())
            {
                items[i].transform.Find("selected").gameObject.SetActive(true);
            }
            else
            {
                items[i].transform.Find("selected").gameObject.SetActive(false);
            }
        }

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
