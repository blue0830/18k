using UnityEngine;
using System.Collections.Generic;
using System.Collections;
//会员资料
public class XiaJiYouXiYingKuiPanel : UserSubPanelBase {

    public const byte byRord = 35;
    //返回
    public GameObject ReturnBtn;
    //筛选
    public GameObject shaixuanBtn;

    //下一页
    public GameObject NextBtn;
    //上一页
    public GameObject PreBtn;

    public UILabel pageLabel;

    public ShaiXuanXiaJiYouXiYingKui shaixuanpanel;

    //新加的
    public UIGrid grid;
    List<RecordLookItemObj> dataObjs = new List<RecordLookItemObj>();
    List<GameObject> itemGameObjs = new List<GameObject>();

    RecordBackObj _RecordBackObj;

    public GameObject norecord;
    //筛选保存
    public static ulong startDate;
    public static ulong endDate;
    public static string chName;
    public static string chBySj;
    public static int lookuserId;


    //查看下级保存
    public static List<uint> lookedID = new List<uint>();
    public static List<string> lookedName = new List<string>();

    List<GameObject> nameObjs = new List<GameObject>();
    public UIGrid namegrid;
    public UIScrollView nameScrollview;

    void Start()
    {
        UIEventListener.Get(ReturnBtn).onClick = OnReturn;
        UIEventListener.Get(shaixuanBtn).onClick = Onshaixuan;

        UIEventListener.Get(NextBtn).onClick = OnNext;
        UIEventListener.Get(PreBtn).onClick = OnPre;
    }



    // Update is called once per frame
    void Update()
    {


    }

    public void show(RecordBackObj obj)
    {
        gameObject.SetActive(true);

        ProcessYonghuming();
        FillYonghuming();

        ProcessGameObjs(0);
        dataObjs.Clear();
        _RecordBackObj = obj;
        shaixuanpanel.gameObject.SetActive(false); //隐藏筛选


        string[] labelstrs = obj.chData.Split('~');
        int col = _RecordBackObj.byColumn;
        int length = labelstrs.Length / col;
        for (int i = 0; i < length; ++i)
        {
            //Debug.Log (length+"  "+i);
            RecordLookItemObj dataObj = new RecordLookItemObj();
            for (int j = 0; j < col; j++)
            {
                //Debug.Log (col+"  "+i);
                dataObj.data.Add(labelstrs[col * i + j]);
            }

            dataObjs.Add(dataObj);
        }

        if (length > 0)
        {
            norecord.SetActive(false);
        }
        else
        {
            norecord.SetActive(true);
        }

        ProcessGameObjs(dataObjs.Count);

        for (int i = 0; i < itemGameObjs.Count; ++i)
        {
            XiaJiYouXiYingKuiItem script = itemGameObjs[i].GetComponent<XiaJiYouXiYingKuiItem>();
            script.FillData(dataObjs[i]);
        }

        
        grid.transform.parent.GetComponent<UIScrollView>().ResetPosition();

        int totalRecord = _RecordBackObj.iCountRecord;
        int pageSize = (int)_RecordBackObj.byPages;
        int totalPages = totalRecord % pageSize > 0 ? 1 + (totalRecord / pageSize) : 0 + (totalRecord / pageSize);
        if (totalPages == 0)
            totalPages = 1;
        pageLabel.text = string.Format("第{0}页 共{1}页", _RecordBackObj.iCuePage, totalPages);
    }

    void ProcessYonghuming()
    {
        if (lookedID.Count == 0)
        {
			AddNameId(Global.CurrentUserName, Global.CurrentUserId);
        }
        if (nameObjs.Count == lookedID.Count)
            return;
        int n = Mathf.Abs(lookedID.Count - nameObjs.Count);
        if (nameObjs.Count < lookedID.Count)
        {
            GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("subPanelMember/Items/nameItem");
            for (int i = 0; i < n; ++i)
            {
                GameObject go = Instantiate(asset) as GameObject;
                go.transform.parent = namegrid.transform;
                go.transform.localScale = Vector3.one;
                go.name = i.ToString();
                nameObjs.Add(go);
            }
        }
        else
        {
            for (int i = 0; i < n; ++i)
            {
                Destroy(nameObjs[0]);
                nameObjs.RemoveAt(0);
            }
        }

        namegrid.repositionNow = true;
        StartCoroutine(process1());
    }

    IEnumerator process1()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < nameObjs.Count; ++i)
        {
            nameObjs[i].SetActive(false);
            nameObjs[i].SetActive(true);
        }
        nameScrollview.ResetPosition();
    }

    void FillYonghuming()
    {
        for (int i = 0; i < nameObjs.Count; ++i)
        {
            nameObjs[i].name = lookedID[i].ToString();
            nameItem script = nameObjs[i].GetComponent<nameItem>();
            script.label.text = lookedName[i];
            script.id = lookedID[i];

            UIEventListener.Get(nameObjs[i]).onClick = OnClickId;
        }

    }

    public static void AddNameId(string name, uint id)
    {
        lookedName.Add(name);
        lookedID.Add(id);
    }

    public static uint GetLastId()
    {
        return lookedID[lookedID.Count - 1];
    }

    void OnClickId(GameObject go)
    {
        int id = int.Parse(go.name);
        int index = lookedID.IndexOf((uint)id);
        if (index == lookedID.Count - 1)
            return;

        lookedID.RemoveRange(index + 1, lookedID.Count - index - 1);
        lookedName.RemoveRange(index + 1, lookedName.Count - index - 1);

        NetworkManager.Instance.LookupRecord(byRord, 2, byRord, 1, chName, startDate, endDate, id, chBySj);
    }

    void ProcessGameObjs(int count)
    {
        if (itemGameObjs.Count == count)
            return;
        int n = Mathf.Abs(count - itemGameObjs.Count);
        if (itemGameObjs.Count < count)
        {
            GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("subPanelMember/Items/XiaJiYouXiYinKuiItem");
            for (int i = 0; i < n; ++i)
            {
                GameObject go = Instantiate(asset) as GameObject;
                go.transform.parent = grid.transform;
                go.transform.localScale = Vector3.one;
                go.name = i.ToString();
                itemGameObjs.Add(go);
            }
        }
        else
        {
            for (int i = 0; i < n; ++i)
            {
                Destroy(itemGameObjs[0]);
                itemGameObjs.RemoveAt(0);
            }
        }

        StartCoroutine(process());
        grid.repositionNow = true;

    }

    IEnumerator process()
    {

        yield return new WaitForEndOfFrame();

        grid.transform.parent.GetComponent<UIScrollView>().ResetPosition();
    }

    void OnReturn(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        gameObject.SetActive(false);
    }

    void Onshaixuan(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        shaixuanpanel.Show();
    }

    void OnNext(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        int totalRecord = _RecordBackObj.iCountRecord;
        int pageSize = (int)_RecordBackObj.byPages;
        int totalPages = totalRecord % pageSize > 0 ? 1 + (totalRecord / pageSize) : 0 + (totalRecord / pageSize);
        if (_RecordBackObj.iCuePage < totalPages)
        {
            int page = _RecordBackObj.iCuePage + 1;
            NetworkManager.Instance.LookupRecord(byRord, (byte)page, byRord, page, XiaJiYouXiYingKuiPanel.chName, XiaJiYouXiYingKuiPanel.startDate, XiaJiYouXiYingKuiPanel.endDate, (int)XiaJiYouXiYingKuiPanel.GetLastId(), XiaJiYouXiYingKuiPanel.chBySj);
            ProcessGameObjs(0);
        }
        else
        {
            msgSignal.Dispatch(new MsgPara("已无更多记录", 2));
        }

    }

    void OnPre(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");

        if (_RecordBackObj.iCuePage > 1)
        {
            int page = _RecordBackObj.iCuePage - 1;
            NetworkManager.Instance.LookupRecord(byRord, (byte)page, byRord, page, XiaJiYouXiYingKuiPanel.chName, XiaJiYouXiYingKuiPanel.startDate, XiaJiYouXiYingKuiPanel.endDate, (int)XiaJiYouXiYingKuiPanel.GetLastId(), XiaJiYouXiYingKuiPanel.chBySj);
            ProcessGameObjs(0);
        }
        else
        {
            msgSignal.Dispatch(new MsgPara("已无更多记录", 2));
        }
    }


}
