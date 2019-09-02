﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
//会员资料
public class XiaJiChongZhiPanel : UserSubPanelBase {

    public const byte byRord =31;
    //返回
    public GameObject ReturnBtn;
    //筛选
    public GameObject shaixuanBtn;

    //下一页
    public GameObject NextBtn;
    //上一页
    public GameObject PreBtn;

    public UILabel pageLabel;

    public ShaiXuanXiaJiChongZhi shaixuanpanel;


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
    public static int lookuserId;

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
        ProcessGameObjs(0);
        dataObjs.Clear();
        _RecordBackObj = obj;
        shaixuanpanel.gameObject.SetActive(false); //隐藏筛选


        string[] labelstrs = obj.chData.Split('~');
        int col = _RecordBackObj.byColumn;
        int length = labelstrs.Length / col ;
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
            XiaJiChongZhiItem script = itemGameObjs[i].GetComponent<XiaJiChongZhiItem>();
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


    void ProcessGameObjs(int count)
    {
        if (itemGameObjs.Count == count)
            return;
        int n = Mathf.Abs(count - itemGameObjs.Count);
        if (itemGameObjs.Count < count)
        {
            GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("subPanelMember/Items/XiaJiChongZhiItem");
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
            NetworkManager.Instance.LookupRecord(byRord, (byte)page, byRord, page, XiaJiChongZhiPanel.chName, XiaJiChongZhiPanel.startDate, XiaJiChongZhiPanel.endDate, XiaJiChongZhiPanel.lookuserId);
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
            NetworkManager.Instance.LookupRecord(byRord, (byte)page, byRord, page, XiaJiChongZhiPanel.chName, XiaJiChongZhiPanel.startDate, XiaJiChongZhiPanel.endDate, XiaJiChongZhiPanel.lookuserId);
            ProcessGameObjs(0);
        }
        else
        {
            msgSignal.Dispatch(new MsgPara("已无更多记录", 2));
        }
    }


}
