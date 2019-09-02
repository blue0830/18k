using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.signal.impl;

public class NumbersItem : MonoBehaviour {

    //public Signal<string,int>  

    public UILabel title;
    public GameObject NumberObj;

    public UIGrid NumGrid;

    NumClickDelegate OnNumClick;
    DantuoDelegate danTuoClk;

    List<GameObject> NumberObjs= new List<GameObject>();

    public List<string> SelectedNums = new List<string>();

    LotterySubModeCfg ltySubCfg = null;
    RowModecfg rowCfg = null;

    List<NumberItem> dantuolist = new List<NumberItem>();

    //大小单双全清
    public GameObject fcbtnRoot;
    public GameObject[] buttons;

    //<!-- 类型text|select|longhu|hezhi|reXuan|reXuanHeZhi|danShuang|daXiao|dS|dingDanShuang|caiZhongWei|reXuanDanTuo -->
    public void CreateRow(LotterySubModeCfg lsc , RowModecfg rmc ,int lInx, NumClickDelegate onNumClk , DantuoDelegate DantuoClick)
    {
        if(rmc.option)
            fcbtnRoot.SetActive(true);
        else
            fcbtnRoot.SetActive(false);

        ltySubCfg = lsc;
        rowCfg = rmc;
        OnNumClick = onNumClk;
        danTuoClk = DantuoClick;


        title.text = rmc.name;

        Clear();

        if (lsc.type == "select" || lsc.type == "hezhi" || lsc.type == "reXuanHeZhi" || lsc.type == "reXuanDanTuo" || lsc.type == "caiZhongWei")
        {
            CreateNumbers( rmc.numFrom, rmc.numTo, lInx, lsc.isShowTwo);
        }
        else if (lsc.type == "danShuang")
        {
            Daxiaodanshuang(1);
        }
        else if (lsc.type == "daXiao")
        {
            Daxiaodanshuang(2);
        }
        else if (lsc.type == "dS")
        {
            Daxiaodanshuang(3);
        }
        else if (lsc.type == "longhu")
        {
            Longhu();
        }
        else if (lsc.type == "dingDanShuang")
        {
            Dingdanshuang();
        }

    }

    //select  hezhi
    void CreateNumbers(int from, int to ,int lInx, bool twoNumber)
    {
        if (NumberObj == null)
        {
            Debug.LogError("NumberItem: NumberObj == null");
            return;
        }
        if (from > to)
        {
            return;
        }
    

        NumberObj.name = from.ToString();
        NumberObjs.Add(NumberObj);
        //创建 length-1个因为已经存在一个
        for (int i = from+1; i <= to; ++i)
        {
            GameObject go =  Instantiate(NumberObj);  
            go.transform.parent = NumberObj.transform.parent;
            go.transform.localScale = Vector3.one;

            go.name = i.ToString();
            NumberObjs.Add(go);
        }

        for (int i = 0; i < NumberObjs.Count; ++i)
        {
            UIEventListener.Get(NumberObjs[i]).onClick = OnClickNum;
            FillNumber(NumberObjs[i], int.Parse(NumberObjs[i].name), lInx, twoNumber);
        }

        NumGrid.repositionNow = true;
        StartCoroutine(process());

    }

    //danshuang
    //1：大小单双 四个球   2 大小 两个球   3 单双 两个球
    void Daxiaodanshuang(int type)  //9,1,3,2
    {
        if (NumberObj == null)
        {
            Debug.LogError("NumberItem: NumberObj == null");
            return;
        }

       

        if (type == 1)
        {
            NumberObjs.Add(NumberObj);
            for (int i = 0; i < 3; ++i)
            {
                GameObject go = Instantiate(NumberObj);
                go.transform.parent = NumberObj.transform.parent;
                go.transform.localScale = Vector3.one;

                NumberObjs.Add(go);
            }
            NumberObjs[0].GetComponent<NumberItem>().numberLabel.text = "大"; //9
            NumberObjs[0].GetComponent<NumberItem>().Number = 9;
            NumberObjs[1].GetComponent<NumberItem>().numberLabel.text = "小"; //1
            NumberObjs[1].GetComponent<NumberItem>().Number = 1;
            NumberObjs[2].GetComponent<NumberItem>().numberLabel.text = "单"; //3
            NumberObjs[2].GetComponent<NumberItem>().Number = 3;
            NumberObjs[3].GetComponent<NumberItem>().numberLabel.text = "双"; //2
            NumberObjs[3].GetComponent<NumberItem>().Number = 2;
        }
        else if (type == 2)
        {
            NumberObjs.Add(NumberObj);
            for (int i = 0; i < 1; ++i)
            {
                GameObject go = Instantiate(NumberObj);
                go.transform.parent = NumberObj.transform.parent;
                go.transform.localScale = Vector3.one;

                NumberObjs.Add(go);
            }
            NumberObjs[0].GetComponent<NumberItem>().numberLabel.text = "大"; //9
            NumberObjs[0].GetComponent<NumberItem>().Number = 9;
            NumberObjs[1].GetComponent<NumberItem>().numberLabel.text = "小"; //1
            NumberObjs[1].GetComponent<NumberItem>().Number = 1;

        }
        else if (type == 3)
        {
            NumberObjs.Add(NumberObj);
            for (int i = 0; i < 1; ++i)
            {
                GameObject go = Instantiate(NumberObj);
                go.transform.parent = NumberObj.transform.parent;
                go.transform.localScale = Vector3.one;

                NumberObjs.Add(go);
            }
            NumberObjs[0].GetComponent<NumberItem>().numberLabel.text = "单"; //3
            NumberObjs[0].GetComponent<NumberItem>().Number = 3;
            NumberObjs[1].GetComponent<NumberItem>().numberLabel.text = "双"; //2
            NumberObjs[1].GetComponent<NumberItem>().Number = 2;
        }


        for (int i = 0; i < NumberObjs.Count; ++i)
        {
            UIEventListener.Get(NumberObjs[i]).onClick = OnClickNum;
            NumberObjs[i].GetComponent<NumberItem>().SelectedSprite.SetActive(false);
        }

        NumGrid.repositionNow = true;
        StartCoroutine(process());

    }

    //longhu
    void Longhu()
    {
        if (NumberObj == null)
        {
            Debug.LogError("NumberItem: NumberObj == null");
            return;
        }


        NumberObjs.Add(NumberObj);

        GameObject go = Instantiate(NumberObj);
        go.transform.parent = NumberObj.transform.parent;
        go.transform.localScale = Vector3.one;

        NumberObjs.Add(go);

        if (ltySubCfg.valtype == 1) //万位vs个位
        {
            NumberObjs[0].GetComponent<NumberItem>().Numtext = "lw"; //lw lq
            NumberObjs[1].GetComponent<NumberItem>().Numtext = "hg"; //hg hs
        }
        else if(ltySubCfg.valtype == 2)
        {
            NumberObjs[0].GetComponent<NumberItem>().Numtext = "lq"; //lw lq
            NumberObjs[1].GetComponent<NumberItem>().Numtext = "hs"; //hg hs
        }

        NumberObjs[0].GetComponent<NumberItem>().numberLabel.text = "龙"; //lw lq
        NumberObjs[1].GetComponent<NumberItem>().numberLabel.text = "虎"; //hg hs


        for (int i = 0; i < NumberObjs.Count; ++i)
        {
            UIEventListener.Get(NumberObjs[i]).onClick = OnClickNum;
            NumberObjs[i].GetComponent<NumberItem>().SelectedSprite.SetActive(false);
        }

        NumGrid.repositionNow = true;

        StartCoroutine(process());

    }

    void Dingdanshuang()
    {
        if (NumberObj == null)
        {
            Debug.LogError("NumberItem: NumberObj == null");
            return;
        }

        //NumberItem item = NumberObj.GetComponent<NumberItem>();
        //item.bgSprite.spriteName = "7";
        //item.SelectedSprite.GetComponent<UISprite>().spriteName = "7 - 1";
        //NumGrid.cellWidth = 140;


        NumberObjs.Add(NumberObj);
        for (int i = 0; i < 5; ++i)
        {
            GameObject go = Instantiate(NumberObj);
            go.transform.parent = NumberObj.transform.parent;
            go.transform.localScale = Vector3.one;
            NumberObjs.Add(go);
        }

        NumberObjs[0].GetComponent<NumberItem>().numberLabel.text = "5.0"; //3
        NumberObjs[0].GetComponent<NumberItem>().Number = 5;
        NumberObjs[1].GetComponent<NumberItem>().numberLabel.text = "4.1"; //2
        NumberObjs[1].GetComponent<NumberItem>().Number = 4;
        NumberObjs[2].GetComponent<NumberItem>().numberLabel.text = "3.2"; //2
        NumberObjs[2].GetComponent<NumberItem>().Number = 3;
        NumberObjs[3].GetComponent<NumberItem>().numberLabel.text = "2.3"; //2
        NumberObjs[3].GetComponent<NumberItem>().Number = 2;
        NumberObjs[4].GetComponent<NumberItem>().numberLabel.text = "1.4"; //2
        NumberObjs[4].GetComponent<NumberItem>().Number = 1;
        NumberObjs[5].GetComponent<NumberItem>().numberLabel.text = "0.5"; //2
        NumberObjs[5].GetComponent<NumberItem>().Number = 0;


        for (int i = 0; i < NumberObjs.Count; ++i)
        {
            UIEventListener.Get(NumberObjs[i]).onClick = OnClickNum;
            NumberObjs[i].GetComponent<NumberItem>().SelectedSprite.SetActive(false);
        }

        NumGrid.repositionNow = true;

        StartCoroutine(process());
    }


    IEnumerator process()
    {

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < NumberObjs.Count; ++i)
        {
            NumberObjs[i].SetActive(false);
            NumberObjs[i].SetActive(true);
        }
    }

    public void Clear()
    {
        var transform1 = NumGrid.transform;
        if (transform1.childCount > 1)
        {
            for (int i = 1; i < transform1.childCount; i++)
            {
                Destroy(transform1.GetChild(i).gameObject);
            }
            NumberObj = transform1.GetChild(0).gameObject;
        }
        NumberObjs.Clear();

        NumberItem item= NumberObj.GetComponent<NumberItem>();
        item.bgSprite.spriteName = "selqiuno";
        item.SelectedSprite.GetComponent<UISprite>().spriteName = "selqiu";

        NumGrid.cellWidth = 80;
    }


    public void ClearSelection()//清除选择状态
    {
        for (int i = 0; i < NumberObjs.Count; ++i)
        {
            NumberItem tempItem = NumberObjs[i].GetComponent<NumberItem>();
            tempItem.SelectedSprite.SetActive(false);
            SelectedNums.Remove(tempItem.Number.ToString());
        }
    }
    public void RemoveSelection(string str)
    {
        for (int i = 0; i < NumberObjs.Count; ++i)
        {
            NumberItem tempItem = NumberObjs[i].GetComponent<NumberItem>();
            if (str == tempItem.Number.ToString())
            {
                tempItem.SelectedSprite.SetActive(false);
                SelectedNums.Remove(tempItem.Number.ToString());
                break;
            }
        }
    }

    void OnClickNum(GameObject go)
    {
        NumberItem goItem = go.GetComponent<NumberItem>();
        AudioController.Instance.SoundPlay("active_item");
        OnNumTapped(goItem);
    }

    void OnNumTapped(NumberItem goItem)
    {
        if (ltySubCfg.type == "longhu") // 龙虎不能复选
        {
            if (goItem.SelectedSprite.activeSelf)
            {
                goItem.SelectedSprite.SetActive(false);
                SelectedNums.Remove(goItem.Numtext);
            }
            else
            {
                for (int i = 0; i < NumberObjs.Count; ++i)
                {
                    NumberItem tempItem = NumberObjs[i].GetComponent<NumberItem>();
                    tempItem.SelectedSprite.SetActive(false);
                    SelectedNums.Remove(tempItem.Numtext);
                }
                goItem.SelectedSprite.SetActive(true);
                SelectedNums.Add(goItem.Numtext);
            }
        }
        else if (ltySubCfg.type == "reXuanDanTuo")
        {
            if (goItem.SelectedSprite.activeSelf)
            {
                goItem.SelectedSprite.SetActive(false);
                SelectedNums.Remove(goItem.Number.ToString());
                dantuolist.Remove(goItem);
            }
            else
            {
                goItem.SelectedSprite.SetActive(true);
                SelectedNums.Add(goItem.Number.ToString());
                dantuolist.Add(goItem);
                if (dantuolist.Count > ltySubCfg.mincnt - 1 && goItem.LineIndex == 0)
                {
                    NumberItem removeitem = dantuolist[0];
                    dantuolist.Remove(removeitem);
                    removeitem.SelectedSprite.SetActive(false);
                    SelectedNums.Remove(removeitem.Number.ToString());
                }
            }
        }
        else
        {
            if (goItem.SelectedSprite.activeSelf)
            {
                goItem.SelectedSprite.SetActive(false);
                SelectedNums.Remove(goItem.Number.ToString());
            }
            else
            {
                goItem.SelectedSprite.SetActive(true);
                SelectedNums.Add(goItem.Number.ToString());
            }
        }

        SelectedNums.Sort();

        if (OnNumClick != null)
        {
            OnNumClick();
        }
        if (ltySubCfg.type == "reXuanDanTuo")
        {
            danTuoClk(goItem.LineIndex, goItem.Number.ToString());
        }

    }

    void FillNumber(GameObject go , int num, int lInx, bool twoNum)
    {
        NumberItem goItem = go.GetComponent<NumberItem>();
        goItem.SelectedSprite.SetActive(false);
        goItem.Number = num;
        goItem.LineIndex = lInx;
        if (twoNum)
        {
            if (num < 10)
            {
                goItem.numberLabel.text = "0"+ num.ToString();
            }
        }
        else
        {
            goItem.numberLabel.text = num.ToString();
        }
       
    }
    void OnFunction(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        //先清了再选
        for (int i = 0; i < NumberObjs.Count; ++i)
        {
            GameObject NumberObj = NumberObjs[i];
            NumberItem goItem = NumberObj.GetComponent<NumberItem>();
            {
                goItem.SelectedSprite.SetActive(false);
                SelectedNums.Remove(goItem.Number.ToString());
            }
        }
        int first= NumberObjs[0].GetComponent<NumberItem>().Number;

        switch (go.name)
        {
            case "da":
                for (int i = 0; i < NumberObjs.Count; ++i)
                {
                    GameObject NumberObj = NumberObjs[i];
                    NumberItem goItem = NumberObj.GetComponent<NumberItem>();
                    if (goItem.Number >= 5+ first)
                    {
                        OnNumTapped(goItem);
                    }
    
                }
                break;
            case "xiao":
                for (int i = 0; i < NumberObjs.Count; ++i)
                {
                    GameObject NumberObj = NumberObjs[i];
                    NumberItem goItem = NumberObj.GetComponent<NumberItem>();
                    if (goItem.Number < 5 + first)
                    {
                        OnNumTapped(goItem);
                    }
            
                }
                break;
            case "quan":
                for (int i = 0; i < NumberObjs.Count; ++i)
                {
                    GameObject NumberObj = NumberObjs[i];
                    NumberItem goItem = NumberObj.GetComponent<NumberItem>();
                    OnNumTapped(goItem);
                }
                break;
            case "dan":
                for (int i = 0; i < NumberObjs.Count; ++i)
                {
                    GameObject NumberObj = NumberObjs[i];
                    NumberItem goItem = NumberObj.GetComponent<NumberItem>();
                    if (goItem.Number%2==1)
                    {
                        OnNumTapped(goItem);
                    }
   
                }
                break;
            case "shuang":
                for (int i = 0; i < NumberObjs.Count; ++i)
                {
                    GameObject NumberObj = NumberObjs[i];
                    NumberItem goItem = NumberObj.GetComponent<NumberItem>();
                    if (goItem.Number % 2 == 0)
                    {
                        OnNumTapped(goItem);
                    }
  
                }
                break;
            case "qing":
                
                break;

        }
		if (OnNumClick != null)
		{
			OnNumClick();
		}
    }

	// Use this for initialization
	void Start () {
      
        for (int i = 0; i < 6;  ++i)
        {
            UIEventListener.Get(buttons[i]).onClick = OnFunction;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
