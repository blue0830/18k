using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZhuihaoPanel : MonoBehaviour {

    public GameObject returnBtn;
    public UILabel title;

    public UIPopupList qishulist;
    public UIInput beishu;
      
    public UIToggle beitouToggle;
    public UIToggle stopToggle;

    public UILabel botzje;
    public GameObject botBtn;


    public GameObject beishuAdd;
    public GameObject beishuSub;

    public UIScrollView scrollview;
    public UIGrid grid;

    public GameObject zhuihaoItem;

    ZhuihaoOrderObj zhOrderObj;
    List<QihaoObj> qhList = null;


    List<GameObject> itemObjList = new List<GameObject>();

    double _zje;  //总金额
    int _zhqs;  //追号期数
    // Use this for initialization
    void Start()
    {
      
        UIEventListener.Get(returnBtn).onClick = OnCloseClick;
        UIEventListener.Get(botBtn).onClick = OnConfirmClick;

        UIEventListener.Get(beitouToggle.gameObject).onClick = PlaySound;
        UIEventListener.Get(stopToggle.gameObject).onClick = PlaySound;

        EventDelegate.Add(qishulist.onChange, OnQishuListChange);
        EventDelegate.Add(beitouToggle.onChange, OnBeitouchange);
        EventDelegate.Add(beishu.onChange, OnBeishuInputChange);

        UIEventListener.Get(beishuAdd).onClick = OnChangeBeishu;
        UIEventListener.Get(beishuSub).onClick = OnChangeBeishu;

        stopToggle.value = true;

    }

    // Update is called once per frame
    void Update () {
	
	}

    public void Close()
    {
        Destroy(gameObject); 
    }

    void OnCloseClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        Destroy(gameObject);
    }

    void OnConfirmClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        OnConfirmZhuihao();

    }

    void OnChangeBeishu(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");

        if (go.name == "add")
        {
            int bs = int.Parse(beishu.value);
            bs++;
            beishu.value = bs.ToString();

        }
        else
        {
            int bs = int.Parse(beishu.value);
            if (bs > 1)
            {
                bs--;
                beishu.value = bs.ToString();
            }
        }
        FillData(false);
    }

    void PlaySound(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
    }

    public void SetZhuihaoObj(ZhuihaoOrderObj zobj)
    {
        zhOrderObj = zobj;

        title.text = zhOrderObj.cfirmObj.lCfg.name;
    }

    public void OnGetZhuihaoList(List<QihaoObj> qihaoList)
    {
        qhList = qihaoList;

        if (zhOrderObj == null)
            return;

        beishu.value = "1";
        PrepareItemObjs();
        FillData(true);
    }

    void PrepareItemObjs()
    {
        if (qhList == null)
            return;

        int chooseLength = int.Parse(qishulist.value); //用户选择的显示期数
        int length = qhList.Count > chooseLength ? chooseLength : qhList.Count;

        if (itemObjList.Count > 0)
        {
            for (int i = 1; i < itemObjList.Count; ++i)
            {
                Destroy(itemObjList[i]);
            }
            zhuihaoItem = itemObjList[0];
            itemObjList.Clear();
        }

        itemObjList.Add(zhuihaoItem);
        for (int i = 1; i < length; ++i)
        {
            GameObject go = Instantiate(zhuihaoItem);
            go.transform.parent = grid.transform;
            go.transform.localScale = Vector3.one;

            itemObjList.Add(go);
        }

        grid.repositionNow = true;
        StartCoroutine(process());
    }

    void FillData(bool isreset)
    {
        if (qhList == null||qhList.Count<itemObjList.Count)
            return;

        int beitou = 1;
        for (int i = 0; i < itemObjList.Count; ++i)
        {
            ZhuihaoItem zhItem = itemObjList[i].GetComponent<ZhuihaoItem>();

            zhItem.updateBot = UpdateBotInfo;
            zhItem.singleJine = zhOrderObj.zhzje.ToString();
            zhItem.id = qhList[i].id;
            zhItem.qihao.text = qhList[i].date + qhList[i].code;

            if(isreset)
                zhItem.selectToggle.value = true;

            beitou = (int)Mathf.Pow(2, i);
            if (beitouToggle.value)
            {
                int bsvalue = beitou * int.Parse(beishu.value);
                if (bsvalue > 999)
                    bsvalue = 999;
                zhItem.beishu.value = bsvalue.ToString();
            }
            else
            {
                zhItem.beishu.value = beishu.value;
            }

            zhItem.OnbeishuChange();

        }
        UpdateBotInfo();
    }


    IEnumerator process()
    {

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < itemObjList.Count; ++i)
        {
            itemObjList[i].SetActive(false);
            itemObjList[i].SetActive(true);
        }

        scrollview.ResetPosition();
    }

    void OnConfirmZhuihao()
    {

        ConfirmPanelObj obj = zhOrderObj.cfirmObj;

        int acCount = _zhqs; //

        int singleMoney = 0;
        if (obj.model == 1)
        {
            singleMoney = 200;

        }
        else if (obj.model == 2)
        {
            singleMoney = 20;

        }
        else if (obj.model == 3)
        {
            singleMoney = 2;

        }
        double intje = _zje;

        int bingoStop = stopToggle.value ? 1 : 0;

        NetworkManager.Instance.TakerZhuihaoOrder(obj.lCfg.lotteryId, (int)obj.subCfg.subModeId, acCount, singleMoney, (int)intje, obj.zs, bingoStop, obj.tzbs, obj.contents);

        Loading.GetInstance().ShowLoading(3);
    }


    public void OnZhuihaoRtn(int id)
    {
       
        if (zhOrderObj == null || qhList == null)
            return;

        List<int> ids = new List<int>();

        string beishuStr = "";
      
        for (int i = 0; i < itemObjList.Count; ++i)
        {
            ZhuihaoItem zhItem = itemObjList[i].GetComponent<ZhuihaoItem>();

            if (zhItem.selectToggle.value)
            {
                ids.Add(zhItem.id);
				beishuStr += zhItem.beishu.value + ",";
            }
        }
		beishuStr = beishuStr.Substring (0,beishuStr.Length-1);

        int acCount = _zhqs;

        NetworkManager.Instance.SentZhuihao(id, acCount, ids, beishuStr);
    }


    void OnQishuListChange()
    {
        PrepareItemObjs();
        FillData(true);
    }



    void OnBeishuInputChange()
    {
        if (string.IsNullOrEmpty(beishu.value))
        {
            beishu.value = "1";
        }
        int a = 0;
        if (int.TryParse(beishu.value, out a))
        {
            if (a <= 0)
            {
                beishu.value = "1";
            }
        }
        else
        {
            beishu.value = "1";
        }

        FillData(false);
       
    }

    void OnBeitouchange()
    {
        FillData(false);
    }

    void UpdateBotInfo()
    {
        double jine=0;
        int qs = 0;
        for (int i = 0; i < itemObjList.Count; ++i)
        {
            ZhuihaoItem zhItem = itemObjList[i].GetComponent<ZhuihaoItem>();
            if (zhItem.selectToggle.value)
            {
                qs++;
                jine = MathUtil.calculate(jine.ToString(), zhItem.jine.text, '+');
            }
        }

        botzje.text = string.Format("总金额[F96502FF]{0}[-]元", jine);

        _zje = MathUtil.calculate(jine.ToString(), "100", '*');
        _zhqs = qs;

    }

}
