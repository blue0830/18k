/// An example view
/// ==========================
/// 

using System;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;


public class SelectionConfirmView : View
{
    [Inject]
    public MsgSignal MsgSignal { get; set; }

    GameObject panel;
    ConfirmPanel panelScript;

    List<ConfirmPanelObj> ConfirmObjList = new List<ConfirmPanelObj>();

    public Signal<List<ConfirmPanelObj>> orderSign = new Signal<List<ConfirmPanelObj>>();

    public Signal<ZhuihaoOrderObj> zhuihaoSignal = new Signal<ZhuihaoOrderObj>();

    internal void init()
    {
        Transform parent = UIRootFinder.uiRootTran;
        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("SelectionConfrimPanel");
        panel = Instantiate(asset) as GameObject;
        panel.transform.parent = parent;
        panel.transform.localScale = Vector3.one;

        panelScript = panel.GetComponent<ConfirmPanel>();

        UIEventListener.Get(panelScript.ReturnBtn).onClick = OnReturnClick;
        UIEventListener.Get(panelScript.addNewBtn).onClick = OnReturnClick;
        UIEventListener.Get(panelScript.DeleteAllBtn).onClick = OnDeleteAll;
        UIEventListener.Get(panelScript.ConfirmBtn).onClick = OnConfirmClick;

        EventDelegate.Add(panelScript.zhuihaoToggle.onChange, OnZhuihaoToggleChange);
        EventDelegate.Add(panelScript.stopToggle.onChange, OnStopToggleChange);

        UIEventListener.Get(panelScript.zhuihaoToggle.gameObject).onClick = OnZhuihaoToggleClick;
        UIEventListener.Get(panelScript.stopToggle.gameObject).onClick = OnStopToggleClick;

        panelScript.zhuiqi.selectAllTextOnFocus = true;
        panelScript.beishu.selectAllTextOnFocus = true;

        //EventDelegate.Add(panelScript.zhuiqi.onChange, OnZhuihaoInputChange);
        //EventDelegate.Add(panelScript.beishu.onChange, OnBeishuInputChange);
		EventDelegate.Add(panelScript.zhuiqi.onSubmit, OnZhuihaoInputChange);
		EventDelegate.Add(panelScript.beishu.onSubmit, OnBeishuInputChange);
       

        UIEventListener.Get(panelScript.beishuAdd).onClick = OnChangeBeishu;
        UIEventListener.Get(panelScript.beishuSub).onClick = OnChangeBeishu;

        UIEventListener.Get(panelScript.zhuihaoBtn).onClick = OnZHBtn;

        panelScript.deleteSignal.AddListener(OnItemDelete);
    
        panel.SetActive(false);
    }

    public void ShowPanel(ConfirmPanelObj obj)
    {
        if (obj != null)
        {
            ConfirmObjList.Add(obj);
            panelScript.TitleLabel.text = obj.lCfg.name + " 投注列表";
        }
        else
        {
            panelScript.TitleLabel.text = "投注列表";
        }
        panel.SetActive(true);
        panelScript.CreateItems(ConfirmObjList);
        UpdateBotInfo();
    }

    void OnDeleteAll(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        DeleteAll();
    }

    public void DeleteAll()
    {
        panelScript.DeleteAll();
        ConfirmObjList.Clear();
        panelScript.zhuihaoToggle.value = false;
        panelScript.zhuiqi.enabled = false;
        panelScript.beishu.enabled = false;
        UpdateBotInfo();
    }

    void UpdateBotInfo()
    {
        int zzs = 0;
        double zje = Getzje(ref zzs);
        panelScript.ConfirmLabel.text = string.Format("总注数[F96502FF]{0}[-]注\r\n总金额[F96502FF]{1}[-]元", zzs, zje);
    }

    //通过一个下单数据金额 计算追号后的总金额
    double Getzje(ref int zzs)
    {
        zzs = 0;
        double zje = 0;

        for (int i = 0; i < ConfirmObjList.Count; ++i)
        {
            zzs += ConfirmObjList[i].zs;
            zje = MathUtil.calculate(zje.ToString(), ConfirmObjList[i].amount, '+');
        }

        int zhuiqi = 0, beishu = 0;
        if (panelScript.zhuihaoToggle.value)
        {
            zhuiqi = int.Parse(panelScript.zhuiqi.value);
            beishu = int.Parse(panelScript.beishu.value);
        }
        int zs = zzs;
        zzs += zs * zhuiqi;


        double danzhuje = zje;

        zje = MathUtil.calculate(danzhuje.ToString(), beishu.ToString(), '*');
        zje = MathUtil.calculate(zje.ToString(), zhuiqi.ToString(), '*');
        zje = MathUtil.calculate(danzhuje.ToString(), zje.ToString(), '+');

        return zje;
    }

    void OnChangeBeishu(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");

        if (panelScript.zhuihaoToggle.value)
        {
            if (go.name == "add")
            {
                int beishu = int.Parse(panelScript.beishu.value);
                beishu++;
                panelScript.beishu.value = beishu.ToString();

            }
            else
            {
                int beishu = int.Parse(panelScript.beishu.value);
                if (beishu > 1)
                {
                    beishu--;
                    panelScript.beishu.value = beishu.ToString();
                }
            }
            UpdateBotInfo();
        }
    }

    void OnZHBtn(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");

        if (ConfirmObjList.Count > 1)
        {
            MsgSignal.Dispatch(new MsgPara("追号只支持单条投注记录", 2));
            return;
        }
        else if (ConfirmObjList.Count == 0)
        {
            MsgSignal.Dispatch(new MsgPara("请先下单", 2));
            return;
        }
        else if (ConfirmObjList[0].lCfg.lotteryId==5|| ConfirmObjList[0].lCfg.lotteryId == 4)
        {
            MsgSignal.Dispatch(new MsgPara("该彩种没有追号功能", 2));
            return;
        }
        int zzs = 0;
        ZhuihaoOrderObj obj = new ZhuihaoOrderObj();
        obj.cfirmObj = ConfirmObjList[0];
        obj.bingoStop = 0;// = panelScript.stopToggle.value ? 1 : 0;  //由于逻辑改变这里的值没有用了
        obj.beishu = 0;// int.Parse(panelScript.beishu.value); //由于逻辑改变这里的值没有用了
        obj.qishu = 0; //int.Parse(panelScript.zhuiqi.value); //由于逻辑改变这里的值没有用了
        obj.zhzje = Getzje(ref zzs); //由于逻辑改变这里的值为投注的总金额
        zhuihaoSignal.Dispatch(obj);
    }

    void OnConfirmClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        if (panelScript.zhuihaoToggle.value)  //追号下单  改变逻辑后这里永远为false 
        {
            int zzs = 0;
            ZhuihaoOrderObj obj = new ZhuihaoOrderObj();
            obj.cfirmObj = ConfirmObjList[0];
            obj.bingoStop = panelScript.stopToggle.value?1:0;
            obj.beishu = int.Parse(panelScript.beishu.value);
            obj.qishu = int.Parse(panelScript.zhuiqi.value);
            obj.zhzje = Getzje(ref zzs);
            zhuihaoSignal.Dispatch(obj);
			DeleteAll();//去追号的话，还是需要删除
        }else   //普通下单
        {
            if (ConfirmObjList.Count > 100)
            {
                MsgSignal.Dispatch(new MsgPara("一次提交的订单过多,请重新下单",2));
                return;
            }
            if (ConfirmObjList.Count == 0)
            {
                MsgSignal.Dispatch(new MsgPara("请先下单",2));
                return;
            }
			Loading.GetInstance ().ShowLoading ("数据正在加载中.......");
            orderSign.Dispatch(ConfirmObjList);
			TimeManager.Instance().Register("checkOrder", 1, 1000, 15000, (c, t) =>
			{
				if(!Global.IsAppOrderRsp){
					Loading.GetInstance ().HideLoading();
					Global.IsAppOrderRsp = false;
					MsgSignal.Dispatch(new MsgPara("抱歉,由于网络波动下单有可能异常,请查看您的下单记录进行确认", 2));
				}
			});
        }
    }

    public void HidePanel()
    {
        panel.SetActive(false);
    }

    void OnZhuihaoToggleClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
    }
    void  OnStopToggleClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
    }

    void OnZhuihaoToggleChange()
    {
        if (panelScript.zhuihaoToggle.value)
        {

            if (ConfirmObjList.Count > 1)
            {
                panelScript.zhuihaoToggle.value = false;
                MsgSignal.Dispatch(new MsgPara("追号只支持单条投注记录",2));
                return;
            }
            panelScript.stopToggle.value = true;

            panelScript.zhuiqi.enabled = true;
            panelScript.beishu.enabled = true;

            panelScript.zhuiqi.value = "1";
            panelScript.beishu.value = "1";

        }
        else
        {
            panelScript.stopToggle.value = false;

            panelScript.zhuiqi.enabled = false;
            panelScript.beishu.enabled = false;

            panelScript.zhuiqi.value = "1";
            panelScript.beishu.value = "1";
        }

        UpdateBotInfo();
    }

    void OnStopToggleChange()
    {
        if (panelScript.stopToggle.value)
        {
            if (!panelScript.zhuihaoToggle.value)
                panelScript.stopToggle.value = false;
        }
    }

    void OnZhuihaoInputChange()
    {

        if (string.IsNullOrEmpty(panelScript.zhuiqi.value))
        {
            panelScript.zhuiqi.value = "1";
        }
        int a = 0;
        if (int.TryParse(panelScript.zhuiqi.value, out a))
        {
            if (a <= 0)
            {
                panelScript.zhuiqi.value = "1";
            }
            else if (a > 100)
            {
                panelScript.zhuiqi.value = "100";
            }
        }
        else
        {
            panelScript.zhuiqi.value = "1";
        }

        UpdateBotInfo();
    }

    void OnBeishuInputChange()
    {
        if (string.IsNullOrEmpty(panelScript.beishu.value))
        {
            panelScript.beishu.value = "1";
        }
        int a = 0;
        if (int.TryParse(panelScript.beishu.value, out a))
        {
            if (a <= 0)
            {
                panelScript.beishu.value = "1";
            }      
        }
        else
        {
            panelScript.beishu.value = "1";
        }

        UpdateBotInfo();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        Destroy(panel);
    }

    void OnReturnClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        HidePanel();
    }

    public void close()
    {
        panelScript.deleteSignal.RemoveListener(OnItemDelete);
		TimeManager.Instance().UnRegister("checkOrder");
        Destroy(gameObject);
    }

    void OnItemDelete(ConfirmPanelObj itemObj)
    {
        ConfirmObjList.Remove(itemObj);
        UpdateBotInfo();
    }
}


