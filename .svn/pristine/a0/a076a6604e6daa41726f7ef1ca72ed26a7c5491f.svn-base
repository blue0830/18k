using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MainPanel : MonoBehaviour {
    public GameObject[] subBtns;
    public UIGrid IconGrid;
    public GameObject IconObj;
    public Transform GongGaoRoot;
    public List<GameObject> Icons = new List<GameObject>();
    public UIToggle[] mainToggle;
    public UIScrollView scorllview;
    public UICenterOnChild centerChild;
    public Transform[] childTans;

	public GameObject gamelistPanel;

    public void CreateIcons(List<LotteryConfig> LotteryCfgs)
    {
        if (IconObj == null)
        {
            Debug.LogError("MainPanel: IconObj == null");
            return;
        }
        if (LotteryCfgs == null || LotteryCfgs.Count == 0)
        {
            Debug.LogError("MainPanel: LotteryCfgs == null || LotteryCfgs.Count == 0");
            return;
        }
        ClearIcon();
        Icons.Add(IconObj);
        //创建 Length-1个因为已经存在一个
        for (int i = 1; i < LotteryCfgs.Count; ++i)
        {
            GameObject go = Instantiate(IconObj);
            go.transform.parent = IconObj.transform.parent;
            go.transform.localScale = Vector3.one;
            Icons.Add(go);
        }
        for (int i = 0; i < LotteryCfgs.Count; ++i)
        {
            GameObject go = Icons[i];
            go.name = LotteryCfgs[i].lotteryId.ToString();
            go.GetComponent<UISprite>().spriteName = LotteryCfgs[i].iconName;
            if (LotteryCfgs[i].hot)
            {
                go.GetComponent<MainItem>().hot.SetActive(true);
            }
            else
            {
                go.GetComponent<MainItem>().hot.SetActive(false);
            }
        }
        for (int i = 0; i < Icons.Count; ++i)
        {

            Icons[i].SetActive(false);
        }
        IconGrid.repositionNow = true;
        scorllview.ResetPosition();
        StartCoroutine(process());
    }

    void ClearIcon()
    {
        for (int i = 1; i < Icons.Count; ++i)
        {
            Destroy(Icons[i]);
        }
        if(Icons.Count>0)
            IconObj = Icons[0];
        Icons.Clear();
    }

    IEnumerator process()
    {

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < Icons.Count; ++i)
        {
           
            Icons[i].SetActive(true);
        }
        //gameObject.GetComponent<UIScrollView>().ResetPosition();
    }

    int index = 0;
    // Use this for initialization
    void Start () {

        TimeManager.Instance().UnRegister("scrollview");
        TimeManager.Instance().Register("scrollview", 0, 5000, 0, (c,t) =>
        {
            if (index < 2)
            {
                index++;
            }
            else if (index == 2)
            {
                index = 0;
            }

            centerChild.CenterOn(childTans[index]);

        });


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
