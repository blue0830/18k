using System;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using System.Text.RegularExpressions;

public class SelecterView : View
{
    [Inject]
    public MsgSignal MsgSignal { get; set; }

    [Inject]
    public ShowCfirmPSignal ShowConfirmSignal { get; set; }

    [Inject]
    public ILotteryModel lModel { get; set; }

    GameObject panel;
    SelectionPanel panelScript;

    public LotteryConfig lotteryCfg = null;

    LotteryModeCfg currentMode = null;  //当前的模式
    LotterySubModeCfg currentSubMode = null; //当前的子模式




    int valCutterIndex = 22;
    int yuanjiaofen = 1;  //1元 2角 3分

    int activityID = 0; //当前期数

    public bool isBlockOrder;

    SScPlayGetPointResult getPtObj;

    RecordObj _recordobj;

    internal void init()
    {
        Transform parent = UIRootFinder.uiRootTran;
        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("SelectionPanel");
        panel = Instantiate(asset) as GameObject;
        panel.transform.parent = parent;
        panel.transform.localScale = Vector3.one;

        panelScript = panel.GetComponent<SelectionPanel>();

        UIEventListener.Get(panelScript.ReturnBtn).onClick = OnReturnClick;
        UIEventListener.Get(panelScript.ModeBtn).onClick = OnModeClick;

        UIEventListener.Get(panelScript.ClearBtn).onClick = OnClearClick;
        UIEventListener.Get(panelScript.ConfirmBtn).onClick = OnConfirmClick;
        UIEventListener.Get(panelScript.goConfirmBtn).onClick = OnGoConfirmClick;

        UIEventListener.Get(panelScript.helpBtn).onClick = OnHelpClick;
        UIEventListener.Get(panelScript.expBtn).onClick = OnExpClick;
        UIEventListener.Get(panelScript.awardBtn).onClick = OnAwardClick;

        UIEventListener.Get(panelScript.beishuAdd).onClick = OnChangeBeishu;
		UIEventListener.Get(panelScript.beishuSub).onClick = OnChangeBeishu;

        panelScript.ModeTriangle.spriteName = "xs";
        panelScript.lotteryNameLabel.text = lotteryCfg.name;

        panelScript.lotteryModeLabel.text = currentSubMode.name;
        initModeBtns();
        //initSubMode();

        initSelecter();

        //元角分处理
        yuanjiaofen = PlayerPrefs.GetInt("yuanjiaofen", 1);
        ProcessToggles(yuanjiaofen);

        EventDelegate.Add(panelScript.YuanToggle.onChange, OnYJFChange);
        EventDelegate.Add(panelScript.JiaoToggle.onChange, OnYJFChange);
        EventDelegate.Add(panelScript.FenToggle.onChange, OnYJFChange);

        UIEventListener.Get(panelScript.YuanToggle.gameObject).onClick =OnYJFClick;
        UIEventListener.Get(panelScript.JiaoToggle.gameObject).onClick = OnYJFClick;
        UIEventListener.Get(panelScript.FenToggle.gameObject).onClick = OnYJFClick;

        EventDelegate.Add(panelScript.textInput.onChange, OnNumClick);

        for (int i = 0; i < panelScript.CheckBoxes.Length; ++i)
        {
            EventDelegate.Add(panelScript.CheckBoxes[i].onChange, OnNumClick);
        }

        clearPendingChange();

        //请求期数 时间
        NetworkManager.Instance.GetQishuTime(lotteryCfg.lotteryId);
        NetworkManager.Instance.GetRecord(lotteryCfg.lotteryId);

        isBlockOrder = false;

        UIEventListener.Get(panelScript.MoreRecordBtn).onClick = OnMoreRecordClick;
        panelScript.MoreRecordBtn.SetActive(false);
        string space = "                  ";
        string record = null;
		if (lotteryCfg.lotteryType == 4) {
			record = "0000000000";
		} else if (lotteryCfg.lotteryType == 2) {
			record = "000";
		} else {
            record = "0000";
        }
        
        panelScript.qishuLabel.text = string.Format("第[F96502FF]{0}[-]期", space);
		panelScript.timeLabel.text = "--时间 [F4C303FF]--:--:--[-]";
        panelScript.recordQishu.text = string.Format("第[F96502FF]{0}[-]期", space);
        panelScript.recordresult.text = string.Format("第[F96502FF]{0}[-]期", record);

        panelScript.YuELabel.text = "彩票余额:-------";

        EventDelegate.Add(panelScript.slider.onChange, OnSliderChange);
        panelScript.slider.onDragFinished= OnSliderFinish;
        UIEventListener.Get(panelScript.arrowLeftBtn).onClick = OnArrowClick;
        UIEventListener.Get(panelScript.arrowRightBtn).onClick = OnArrowClick;

        lModel.lotteryCfg = lotteryCfg;
        lModel.currentMode = currentMode;
        lModel.currentSubMode = currentSubMode;
        //倍数
        EventDelegate.Add(panelScript.mutipleLabel.onSubmit, OnBeishuChange);
		//EventDelegate.Add(panelScript.mutipleLabel.onChange, OnBeishuChange);
    }

    void OnApplicationPause(bool Pause)
    {
        if (Pause)
        {
           
        }
        else
        {
            RequestTime();
        }
    }

    public void RequestTime()
    {
        if (lotteryCfg != null)
        {
            NetworkManager.Instance.GetQishuTime(lotteryCfg.lotteryId);
            NetworkManager.Instance.GetRecord(lotteryCfg.lotteryId);
        }
        if (lotteryCfg != null && currentSubMode != null)
            NetworkManager.Instance.GetAward(lotteryCfg.lotteryId, currentSubMode.subModeId);
    }

    public void SetLotteryCfg(LotteryConfig cfg)
    {
        lotteryCfg = cfg;
        currentMode = cfg.modecfgs[0];
        currentSubMode = currentMode.subModecfgs[0];

    }


    public void UpdateYuelabel(int yue)
    {
        double d = yue * 1.0 / 100;
        panelScript.YuELabel.text = String.Format("彩票余额:{0:0.00}", d);
    }

    void OnBeishuChange()
    {
        
        if (string.IsNullOrEmpty(panelScript.mutipleLabel.value))
        {
            panelScript.mutipleLabel.value = "1";
        }
        int a = 0;
        if (int.TryParse(panelScript.mutipleLabel.value, out a))
        {
            if (a <= 0)
            {
                panelScript.mutipleLabel.value = "1";
            }
        }
        else
        {
            panelScript.mutipleLabel.value = "1";
        }
        
        OnNumClick();
    }

	void OnChangeBeishu(GameObject go)
	{
        AudioController.Instance.SoundPlay("active_item");
        if (go.name == "add")
		{
			int beishu = int.Parse(panelScript.mutipleLabel.value);
			beishu++;
			panelScript.mutipleLabel.value = beishu.ToString();

		}
		else
		{
			int beishu = int.Parse(panelScript.mutipleLabel.value);
			if (beishu > 1)
			{
				beishu--;
				panelScript.mutipleLabel.value = beishu.ToString();
			}
		}
		OnNumClick();
	}

    void OnHelpClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        panelScript.helpcontent.Show(1, currentSubMode.help);
    }

    void OnExpClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        panelScript.helpcontent.Show(2, currentSubMode.example);
    }

    void OnAwardClick(GameObject go)
    {
        string show = "加载中请稍后。。。";

        if (getPtObj != null)
        {
            if (currentSubMode.subModeId == 39&& getPtObj.iAllPeiLv.Length>=6)
            {
                double d = getPtObj.iAllPeiLv[0] * 1.0 / 100;
                string num0 = string.Format("{0:0.00}", d);
                d = getPtObj.iAllPeiLv[1] * 1.0 / 100;
                string num1 = string.Format("{0:0.00}", d);
                d = getPtObj.iAllPeiLv[2] * 1.0 / 100;
                string num2 = string.Format("{0:0.00}", d);
                d = getPtObj.iAllPeiLv[3] * 1.0 / 100;
                string num3 = string.Format("{0:0.00}", d);
                d = getPtObj.iAllPeiLv[4] * 1.0 / 100;
                string num4 = string.Format("{0:0.00}", d);
                d = getPtObj.iAllPeiLv[5] * 1.0 / 100;
                string num5 = string.Format("{0:0.00}", d);

                show = string.Format("对应奖金 3单2双：{0}  2单3双：{1}  4单1双：{2}  1单4双：{3}  5单0双：{4}  0单5双：{5}",num0,num1,num2,num3,num4,num5);
            }
            else if(currentSubMode.subModeId == 45 && getPtObj.iAllPeiLv.Length >= 4)
            {
                double d = getPtObj.iAllPeiLv[0] * 1.0 / 100;
                string num0 = string.Format("{0:0.00}", d);
                d = getPtObj.iAllPeiLv[1] * 1.0 / 100;
                string num1 = string.Format("{0:0.00}", d);
                d = getPtObj.iAllPeiLv[2] * 1.0 / 100;
                string num2 = string.Format("{0:0.00}", d);
                d = getPtObj.iAllPeiLv[3] * 1.0 / 100;
                string num3 = string.Format("{0:0.00}", d);

                show = string.Format("对应奖金 (6)：{0}   (5,7)：{1}   (4,8)：{2}   (3,9)：{3}", num0, num1, num2, num3);
            }

        }
        panelScript.helpcontent.Show(3, show);
        AudioController.Instance.SoundPlay("active_item");
    }

    void OnMoreRecordClick(GameObject obj)
    {
        if (_recordobj == null)
            return;

        AudioController.Instance.SoundPlay("active_item");
        //todo
        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("MoreRecordPanel");
        GameObject ItemObj = Instantiate(asset) as GameObject;
        ItemObj.transform.parent = UIRootFinder.uiRootTran;
        ItemObj.transform.localScale = Vector3.one;

        MoreRecordPanel script = ItemObj.GetComponent<MoreRecordPanel>();

        script.CreateItems(_recordobj);
    }

    //更新开奖记录
    public void updateRecord(RecordObj obj)
    {
        if (lotteryCfg.lotteryId == obj.lotteryId&& obj.recordItems!=null&&obj.recordItems.Count>0)
        {
            RecordItemObj item = obj.recordItems[0];
            panelScript.recordQishu.gameObject.SetActive(true);
            panelScript.recordresult.gameObject.SetActive(true);
            panelScript.recordQishu.text = string.Format("第[F96502FF]{0}[-]期", item.titleStr);

            _recordobj = obj;
            panelScript.MoreRecordBtn.SetActive(true);
            
            panelScript.recordresult.text = getResultStr_big(obj.lotteryType, item);
        }
    }

    string getResultStr_big(int type, RecordItemObj itemobj)
    {
        string str = "";
        if (type == 1 || type == 3)
        {
            str += numStr(itemobj.iNUM1);
            str += numStr(itemobj.iNUM2);
            str += numStr(itemobj.iNUM3);
            str += numStr(itemobj.iNUM4);
            str += numStr(itemobj.iNUM5);
        }
        else if (type == 2) //3d
        {
            str += itemobj.iNUM1;
            str += itemobj.iNUM2;
            str += itemobj.iNUM3;
        }
        else if (type == 4) //pk10
        {
            str += numStr(itemobj.iNUM1);
            str += numStr(itemobj.iNUM2);
            str += numStr(itemobj.iNUM3);
            str += numStr(itemobj.iNUM4);
            str += numStr(itemobj.iNUM5);
            str += numStr(itemobj.iNUM6);
            str += numStr(itemobj.iNUM7);
            str += numStr(itemobj.iNUM8);
            str += numStr(itemobj.iNUM9);
            str += numStr(itemobj.iNUM10);
        }

        return str;
    }

    public void OnRecordBack(RecordBackObj obj)
    {
        panelScript.OnRecordBack(obj);
    }

    string numStr(int x)  //为了显示字体，10转成a 11转成b
    {
        if (x == 10)
            return "a";
        if (x == 11)
            return "b";
        return x.ToString();
    }


    #region 奖金返点处理
    public void SetAwardPt(SScPlayGetPointResult obj )
    {
        if (obj == null)
            return;

        getPtObj = obj;

 
        CaculatePrcessBar();
    }

    void CaculatePrcessBar()
    {
        int range = getPtObj.iMaxPeiLv - getPtObj.iMinPeiLv;
        int step = (int)MathUtil.calculate(range.ToString(), getPtObj.GetfPeiLvChangeUnit().ToString(), '/');
        double temp = MathUtil.calculate(getPtObj.iPeiLv.ToString(), getPtObj.iMinPeiLv.ToString(), '-');
        float value = 0;
        if (range != 0)
        {
            value = (float)MathUtil.calculate(temp.ToString(), range.ToString(), '/');
        }
        ProcessSlider(value, step);
        SetAwardpoint(getPtObj.iPeiLv, getPtObj.Getfpoint());
    }

    void OnArrowClick(GameObject go)
    {
        if (getPtObj.iMaxPeiLv == getPtObj.iMinPeiLv)
        {
            return;
        }

        int award = getPtObj.iPeiLv;
        double pl = getPtObj.Getfpoint();
        if (go.name == "arrowleft")
        {
            if (award > getPtObj.iMinPeiLv)
            {
                award -= (int)getPtObj.GetfPeiLvChangeUnit();

                pl = MathUtil.calculate(pl.ToString(), getPtObj.GetfPointChangeUnit().ToString(), '+') ;
            }
        }
        else
        {
            if (award < getPtObj.iMaxPeiLv)
            {
                award += (int)getPtObj.GetfPeiLvChangeUnit();
                pl = MathUtil.calculate(pl.ToString(), getPtObj.GetfPointChangeUnit().ToString(), '-');
            }
        }
	 

		if (lotteryCfg.lotteryType == 1) {
			if ((int)currentSubMode.subModeId == 1 || (int)currentSubMode.subModeId == 10 || (int)currentSubMode.subModeId == 23) {
				return;
			}
		}else if(lotteryCfg.lotteryType == 2)
		{
			if ((int)currentSubMode.subModeId == 35 || (int)currentSubMode.subModeId == 39) {
				return;
			}
		}
		else if(lotteryCfg.lotteryType == 3)
		{
			if ((int)currentSubMode.subModeId == 45) {
				return;
			}
		}
			
        NetworkManager.Instance.SetAward(lotteryCfg.lotteryId,(int)currentSubMode.subModeId, award, pl);

    }

    void ProcessSlider(float value, int steps)
    {
        panelScript.slider.numberOfSteps = steps;
        panelScript.slider.value = value;
    }

    void OnSliderChange()
    {
       

        if (getPtObj != null)
        {
            if (getPtObj.iMaxPeiLv == getPtObj.iMinPeiLv)
            {
                CaculatePrcessBar();
                return;
            }

            int steps = Mathf.RoundToInt(panelScript.slider.value * panelScript.slider.numberOfSteps);

            int award = getPtObj.iMinPeiLv + Mathf.RoundToInt(steps * (float)getPtObj.GetfPeiLvChangeUnit());

            double temp = MathUtil.calculate(steps.ToString(), getPtObj.GetfPointChangeUnit().ToString(), '*');
            double pl = MathUtil.calculate(getPtObj.GetfMaxPoint().ToString(), temp.ToString(), '-');

            SetAwardpoint(award, pl);
        }
    }

    void OnSliderFinish()
    {
        if (getPtObj.iMaxPeiLv == getPtObj.iMinPeiLv)
        {
            CaculatePrcessBar();
            return;
        }

        int steps = Mathf.RoundToInt(panelScript.slider.value * panelScript.slider.numberOfSteps);

        int award = getPtObj.iMinPeiLv + Mathf.RoundToInt(steps * (float)getPtObj.GetfPeiLvChangeUnit());

        double temp = MathUtil.calculate(steps.ToString(), getPtObj.GetfPointChangeUnit().ToString(), '*');
        double pl = MathUtil.calculate(getPtObj.GetfMaxPoint().ToString(), temp.ToString(), '-');

		if (lotteryCfg.lotteryType == 1) {
			if ((int)currentSubMode.subModeId == 1 || (int)currentSubMode.subModeId == 10 || (int)currentSubMode.subModeId == 23) {
				return;
			}
		}else if(lotteryCfg.lotteryType == 2)
		{
			if ((int)currentSubMode.subModeId == 35 || (int)currentSubMode.subModeId == 39) {
				return;
			}
		}
		else if(lotteryCfg.lotteryType == 3)
		{
			if ((int)currentSubMode.subModeId == 45) {
				return;
			}
		}
        NetworkManager.Instance.SetAward(lotteryCfg.lotteryId, (int)currentSubMode.subModeId, award, pl);
    }


    void SetAwardpoint(int awad, double pt)
    {
        double af = awad;
        if (yuanjiaofen == 1)
        {
            af /= 100;

        }
        else if (yuanjiaofen == 2)
        {
            af /= 1000;

        }
        else {
            af /= 10000;
        }
      

        panelScript.awardLabel.text = af.ToString();
        panelScript.proportionLabel.text = pt.ToString();
    }
#endregion 
    void ProcessToggles(int yuanjiaofen)
    {
        if (yuanjiaofen == 1)
        {
            panelScript.YuanToggle.value = true;

        } else if (yuanjiaofen == 2)
        {
            panelScript.JiaoToggle.value = true;

        } else if (yuanjiaofen == 3)
        {
            panelScript.FenToggle.value = true;

        }
 
    }

    void OnYJFClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
    }

    void OnYJFChange()
    {
        int oldYuanjiaofen = yuanjiaofen;

        if (panelScript.YuanToggle.value)
        {
            yuanjiaofen = 1;

        }
        else if (panelScript.JiaoToggle.value)
        {
            yuanjiaofen = 2;

        }
        else if (panelScript.FenToggle.value)
        {
            yuanjiaofen = 3;

        }

        PlayerPrefs.SetInt("yuanjiaofen", yuanjiaofen);

        //变更奖金
        if (getPtObj != null)
        SetAwardpoint(getPtObj.iPeiLv, getPtObj.Getfpoint());
        //变更底部金额显示
        if (!string.IsNullOrEmpty(panelScript.jineString))
        {
            float je = float.Parse(panelScript.jineString);

            if (oldYuanjiaofen == 1)
            {

                if (yuanjiaofen == 2)
                {
                    je = je / 10;

                }
                else if (yuanjiaofen == 3)
                {
                    je = je / 100;

                }
            }
            else if (oldYuanjiaofen == 2)
            {
                if (yuanjiaofen == 1)
                {
                    je = je * 10;

                }       
                else if (yuanjiaofen == 3)
                {
                    je = je / 10;

                }

            }
            else if (oldYuanjiaofen == 3)
            {
                if (yuanjiaofen == 1)
                {
                    je = je * 100;

                }
                else if (yuanjiaofen == 2)
                {
                    je = je * 10;
                }
            }
            UpdateBotInfo(panelScript.zsSave, je.ToString()); 
        }
    }

    public void SetQishuTime(string qishu,string timeStr)
    {
        panelScript.qishuLabel.gameObject.SetActive(true);
        panelScript.timeLabel.gameObject.SetActive(true);

        if (!string.IsNullOrEmpty(qishu))
        {
            string str = string.Format("第[F96502FF]{0}[-]期", qishu);
            panelScript.qishuLabel.text = str;
        }
        panelScript.timeLabel.text = timeStr;
    }

    //模式设置
    void initModeBtns()
    {
        //panelScript.modePanel.CreateBtn(lotteryCfg.modecfgs);
       

    }


    void OnModeClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");

        if (!panelScript.modePanel.gameObject.activeSelf)
            ShowModePanel();
        else
            HideModePanel();
    }

    void OnClearClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        ClearSelect();
    }

    void OnGoConfirmClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        ShowConfirmSignal.Dispatch(null);
    }

    /// 根据彩种不同选择调用
    void OnConfirmClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");

        if (isBlockOrder)
        {
            MsgSignal.Dispatch(new MsgPara("抱歉，已经超过了投注截止时间！",2));
            return;
        }

        if (lotteryCfg.lotteryType == 1)
        {
            addNumbersChongqing(2, currentSubMode, int.Parse(panelScript.mutipleLabel.value), yuanjiaofen);
        }
        else if (lotteryCfg.lotteryType == 2)
        {
            addNumbers3D(2, currentSubMode, int.Parse(panelScript.mutipleLabel.value), yuanjiaofen);
        }
        else if (lotteryCfg.lotteryType == 3)
        {
            addNumbers11_5(2, currentSubMode, int.Parse(panelScript.mutipleLabel.value), yuanjiaofen);
        }
        else if (lotteryCfg.lotteryType == 4)
        {
            addNumbersPK10(2, currentSubMode, int.Parse(panelScript.mutipleLabel.value), yuanjiaofen);
        }
    }

    void OnNumClick()
    {
        if (lotteryCfg.lotteryType == 1)
        {
            addNumbersChongqing(1, currentSubMode, int.Parse(panelScript.mutipleLabel.value), yuanjiaofen);
        }
        else if (lotteryCfg.lotteryType == 2)
        {
            addNumbers3D(1, currentSubMode, int.Parse(panelScript.mutipleLabel.value), yuanjiaofen);
        }
        else if (lotteryCfg.lotteryType == 3)
        {
            addNumbers11_5(1, currentSubMode, int.Parse(panelScript.mutipleLabel.value), yuanjiaofen);
        }
        else if (lotteryCfg.lotteryType == 4)
        {
            addNumbersPK10(1, currentSubMode, int.Parse(panelScript.mutipleLabel.value), yuanjiaofen);
        }
    }

    void ShowModePanel()
    {
        panelScript.modePanel.Show(currentMode.modeId,lotteryCfg.lotteryType);
        List<UIToggle> toggles = panelScript.modePanel.modeToggles;
        for (int i = 0; i < toggles.Count; ++i)
        {
            EventDelegate.Add(toggles[i].onChange, OnModeToggleChange);
            UIEventListener.Get(toggles[i].gameObject).onClick = OnModeToggleClick;
        }
        panelScript.ModeTriangle.spriteName = "downla";
    }

    void HideModePanel()
    {
        panelScript.modePanel.gameObject.SetActive(false);
        panelScript.ModeTriangle.spriteName = "xs";
    }
    void OnModeToggleClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");

        TimeManager.Instance().UnRegister("HideModePanel");
        TimeManager.Instance().Register("HideModePanel", 1, 1, 200, (c, t) => { HideModePanel(); });
    }
    void OnModeToggleChange()
    {
        GameObject go = null;
        List<UIToggle> toggles = panelScript.modePanel.modeToggles;
        for (int i = 0; i < toggles.Count; ++i)
        {
            if (toggles[i].value)
            {
                go = toggles[i].gameObject;
            }
        }
        if (go == null)
            return;
        string id = go.name;
        string[] ids = id.Split('_');

        int modeId = int.Parse(ids[0]);
        int submodeId = int.Parse(ids[1]);

        if (currentSubMode.subModeId != submodeId)
        {
            currentMode = lotteryCfg.GetModeCfg(modeId);
            OnModeChange(submodeId);
        }

    }

 

    void OnModeChange(int subModeId)
    {
       

        //模式改变刷新子模式按钮
        //panelScript.scrollview_h.ClearItems();
        //initSubMode();
    
        lModel.currentMode = currentMode;
     
        //重新设置子模式
        SetSubMode(currentMode.GetSubModeCfg(subModeId));

    }

    //子模式设置
    //void initSubMode() //初始化子模式
    //{
    //    panelScript.scrollview_h.CreateScrollItems(currentMode.subModecfgs);

    //    List<GameObject> gos = panelScript.scrollview_h.items;
    //    for (int i = 0; i < gos.Count; ++i)
    //    {
    //        UIEventListener.Get(gos[i]).onClick = OnSubModeBtnClick;
    //    }

    //}
    public void ClearText()
    {
        panelScript.textInput.value = "";
    }

    void OnSubModeBtnClick(GameObject go)
    {
        int submodeId = int.Parse(go.name);
        SetSubMode(currentMode.GetSubModeCfg(submodeId));
    }

    void SetSubMode(LotterySubModeCfg subModeCfg)
    {
        if (currentSubMode!= subModeCfg)
        {
          
            currentSubMode = subModeCfg;
            //panelScript.scrollview_h.ChooseSubMode((int)currentSubMode.subModeId);
            panelScript.lotteryModeLabel.text = currentSubMode.name;

            //子模式变更会导致 选择模块变更
            initSelecter();

            ClearSelect();

            //这里处理一下胆拖的奖金显示
            if (lotteryCfg.lotteryType == 3)
            {
                if (currentSubMode.subModeId == 45 || currentSubMode.subModeId == 39)
                {
                    panelScript.awardBtn.SetActive(true);
                    panelScript.helpGrid.Reposition();
                }
                else
                {
                    panelScript.awardBtn.SetActive(false);
                    panelScript.helpGrid.Reposition();
                }

            }
            else
            {
                panelScript.awardBtn.SetActive(false);
                panelScript.helpGrid.Reposition();
            }

        }
       
    }

    //选择模块处理  //text|select|longhu|hezhi|reXuan|reXuanHeZhi|danShuang
    void initSelecter() //初始化选中模块 根据type
    {
        // 请求返点 奖金
        NetworkManager.Instance.GetAward(lotteryCfg.lotteryId, currentSubMode.subModeId);
        lModel.currentSubMode = currentSubMode;

        string type = currentSubMode.type;
        if (lotteryCfg.lotteryType == 1) //重庆
        {
            if (type == "text" || type == "reXuan")
            {
                ShowText();
            }
            else
            {
                ShowNo();
                panelScript.scrollview_v.CreateScrollItems(currentSubMode, OnNumClick);
            }

            if (type == "reXuan" || type == "reXuanHeZhi")
            {
                panelScript.CheckBoxRoot.SetActive(true);
            }
            else
            {
                panelScript.CheckBoxRoot.SetActive(false);
            }
        }
        else if (lotteryCfg.lotteryType == 2) //3D
        {
            panelScript.CheckBoxRoot.SetActive(false);

            if (type == "text")
            {
                ShowText();
            }
            else
            {
                ShowNo();
                panelScript.scrollview_v.CreateScrollItems(currentSubMode, OnNumClick);
            }

        }
        else if (lotteryCfg.lotteryType == 3) //11选5
        {
            panelScript.CheckBoxRoot.SetActive(false);

            if (type == "text")
            {
                ShowText();
            }
            else
            {
                ShowNo();
                panelScript.scrollview_v.CreateScrollItems(currentSubMode, OnNumClick);
            }

        }
        else if (lotteryCfg.lotteryType == 4) //pk10
        {
            panelScript.CheckBoxRoot.SetActive(false);

            if (type == "text")
            {
                ShowText();
            }
            else
            {
                ShowNo();
                panelScript.scrollview_v.CreateScrollItems(currentSubMode, OnNumClick);
            }

        }
    }


    void ShowText()
    {
        panelScript.textroot.SetActive(true);
        panelScript.scrollview_v.gameObject.SetActive(false);
    }

    void ShowNo()
    {
        panelScript.textroot.SetActive(false);
        panelScript.scrollview_v.gameObject.SetActive(true);
    }

    //====
    void UpdateBotInfo(int zs, string je) //更新底部按钮信息
    {
        // F96502FF
        panelScript.ConfirmLabel.text = string.Format("共[F96502FF]{0}[-]注 共[F96502FF]{1}[-]元", zs, je);

        panelScript.jineString = je;
        panelScript.zsSave = zs;
    }

    void ClearSelect() //清除所有选择状态
    {
        //倍数设为1
        panelScript.mutipleLabel.value = "1";

        panelScript.textInput.value="";

        panelScript.scrollview_v.ClearSelection();
        clearPendingChange();
    }


    void clearPendingChange()
    {
        UpdateBotInfo(0, "0");
    }



    string getJE(int zs, int beishu, int unit)
    {
        double miane = 2;
        double je = 0;
        if (1 == unit)
            je = miane * beishu * zs;
        else if (2 == unit)
            je = (beishu * zs * miane / 10);
        else if (3 == unit)
            je = (beishu * zs * miane / 100);
        return String.Format("{0:0.00}", je);
    }


    char[] intersect(char[] a, char[]b)
    {
        List<char> tempA = new List<char>();
        for (int i = 0; i < a.Length; i++)
        {
            if (!tempA.Contains(a[i]))
            {
                tempA.Add(a[i]);
            }
        }

        List<char> tempB = new List<char>();
        for (int i = 0; i < b.Length; i++)
        {
            if (!tempB.Contains(b[i]))
            {
                tempB.Add(b[i]);
            }
        }



        List<char> temp = new List<char>();

        for (int i = 0; i < tempA.Count; ++i)
        {
            for (int j = 0; j < tempB.Count; ++j)
            {
                if (tempA[i] == tempB[j])
                {
                    temp.Add(tempA[i]);
                }
            }
        }

        return temp.ToArray();
    }

    //组合数计算
    int  Combination(int n, int m)
    {
 
        if (m < 0 || n < 0)
        {
            return -1;  //原来是false
        }
        if (m == 0 || n == 0)
        {
            return 1;
        }
        if (m > n)
        {
            return 0;
        }
        if (m > n / 2.0)
        {
            m = n - m;
        }
        double result = 0.0;
        for (int i = n; i >= (n - m + 1); i--)
        {
            result += Math.Log(i);
        }
        for (int i = m; i >= 1; i--)
        {
            result -= Math.Log(i);
        }
        result = Math.Exp(result);
        return (int)Math.Round(result);
    }

    //三个号码都不重复
    bool checkNotSameInThree(string str)
    {
        var cnt = 0;
        str = str.Trim();
        if (str.Length != 3)
            return false;
        else
        {
            Dictionary<int, int> dct = new Dictionary<int, int>();
            for (var i = 0; i < 3; i++)
            {
                var tmp = str.ToCharArray()[i];
                cnt = 1;
                for (var j = 0; j < 3; j++)
                {
                    if (j != i && tmp == str.ToCharArray()[j])
                        cnt++;
                }
                dct[i] = cnt;
            }
            cnt = 0;
            foreach (KeyValuePair<int,int> attr in dct)
            {
                if (attr.Value > 1)
                {
                    return false;
                }
            }
            return true;
        }
    }

    //判断是否重复需要的个数
    bool checkTwoSameInThree(string str)
    {
        var cnt = 0;
        str = str.Trim();
        if (str.Length != 3)
            return false;
        else
        {
            Dictionary<int, int> dct = new Dictionary<int, int>();
            for (var i = 0; i < 3; i++)
            {
                var tmp = str.ToCharArray()[i];
                cnt = 1;
                for (var j = 0; j < 3; j++)
                {
                    if (j != i && tmp == str.ToCharArray()[j])
                        cnt++;
                }
                dct[i] = cnt;
            }
            cnt = 0;
            foreach (KeyValuePair<int, int> attr in dct)
            {
                if (attr.Value == 2)
                {
                    cnt++;
                }
            }
            return cnt == 2;
        }
    }

    //判断是否存在重复
    bool ckeckNotRepeat(string str)
    {
        str = str.Trim();
        var len = str.Length;
        for (var i = 0; i < len; i++)
        {
            var tmp = str.ToCharArray()[i];
            for (var j = 0; j < len; j++)
            {
                if (j != i && tmp == str.ToCharArray()[j])
                    return false;
            }
        }
        return true;
    }

    //三星直选和值注数
    int getSxZxhzZs(int num)
    {
        var zs = 0;
        switch (num)
        {
            case 0:
            case 27:
                zs = 1;
                break;
            case 1:
            case 26:
                zs = 3;
                break;
            case 2:
            case 25:
                zs = 6;
                break;
            case 3:
            case 24:
                zs = 10;
                break;
            case 4:
            case 23:
                zs = 15;
                break;
            case 5:
            case 22:
                zs = 21;
                break;
            case 6:
            case 21:
                zs = 28;
                break;
            case 7:
            case 20:
                zs = 36;
                break;
            case 8:
            case 19:
                zs = 45;
                break;
            case 9:

            case 18:
                zs = 55;
                break;
            case 10:
            case 17:
                zs = 63;
                break;
            case 11:
            case 16:
                zs = 69;
                break;
            case 12:
            case 15:
                zs = 73;
                break;
            case 13:
            case 14:
                zs = 75;
                break;
        }
        return zs;
    }

 
//二星直选和值注数
    int  getExZxhzZs(int num)
    {
        var zs = 0;
        switch (num)
        {
            case 0:
            case 18:
                zs = 1;
                break;
            case 1:
            case 17:
                zs = 2;
                break;
            case 2:
            case 16:
                zs = 3;
                break;
            case 3:
            case 15:
                zs = 4;
                break;
            case 4:
            case 14:
                zs = 5;
                break;
            case 5:
            case 13:
                zs = 6;
                break;
            case 6:
            case 12:
                zs = 7;
                break;
            case 7:
            case 11:
                zs = 8;
                break;
            case 8:
            case 10:
                zs = 9;
                break;
            case 9:
                zs = 10;
                break;
        }
        return zs;
    }


    int getRenXuanAndZuXuan(int m,int n)
    {
        if (((m < 0) || (n < 0)) || (m < n))
        {
            return 0;
        }
        n = (n < (m - n)) ? n : (m - n);
        if (n == 0)
        {
            return 1;
        }
        var num = m;
        var num2 = 2;
        for (var i = num - 1; num2 <= n; i--)
        {
            num = (num * i) / num2;
            num2++;
        }
        return num;
    }





    #region 销毁
    protected override void OnDestroy()
    {
        base.OnDestroy();

        Destroy(panel);
    }

    void OnReturnClick(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        close();
    }

    public void close()
    {
        Destroy(gameObject);
    }
    #endregion



    //添加选中号码

    void addNumbersChongqing(int args, LotterySubModeCfg modeCfg, int bs, int unitpara)
    {

        LotterySubModeCfg seletedType = modeCfg;

        int beiShu = bs;
        int unit = unitpara; //1元 2角 3分
        //var showMianE = " 元",
       
        var  tzbs = "0";

        int zs = 0;
        string je = "";

        string rType = seletedType.type;
        var val = "";

        var selectedVal = seletedType.subModeId.ToString();


        if (rType == "select")
        {

            var canNotAll = (seletedType.cannotall == 1) ? true : false;
            var cnt = seletedType.cnt;

            var selectedCnt = 0;
            //必须全部选择
            var divs = panelScript.scrollview_v.NumsItems;
            for (var i = 0; i < divs.Count; i++)
            {
                var div = divs[i];
                var selecteds = div.SelectedNums;
                var exit = false;
                for (var j = 0; j < selecteds.Count; j++)
                {
                    var selected = div.SelectedNums[j];

                    val += selected;
                    exit = true;
                }
                if (i != divs.Count - 1)
                    val += ',';

                if (exit == false && canNotAll == false)
                {//存在没选，而且每行都要选
                    if (args == 2)  //2代表下单操作
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
                if (exit)
                    selectedCnt++;
            }

            if (canNotAll == true && cnt > -1 && selectedCnt != cnt)
            {
                if (args == 2)
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                    return;
                }
                else
                {
                    clearPendingChange();
                    return;
                }
            }
            if ("2" == selectedVal || "33" == selectedVal || "21" == selectedVal)
            { //后三直选复式\中三直选复式\前三直选复式
                var argsArray = val.Split(',');
                zs = argsArray[0].Length * argsArray[1].Length * argsArray[2].Length;
                je = getJE(zs, beiShu, unit);
            }
            else if ("4" == selectedVal || "36" == selectedVal || "24" == selectedVal)
            {  //后三组三复式\中三组三复式\前三组三复
                var argsArray = val.Split(',');
                zs = argsArray[0].Length * (argsArray[0].Length - 1);
                je = getJE(zs, beiShu, unit);
            }
            else if ("6" == selectedVal || "38" == selectedVal || "26" == selectedVal)
            {  //后三组六复式\中三组六复式\前三组六复
                var argsArray = val.Split(',');
                zs = argsArray[0].Length * (argsArray[0].Length - 1) * (argsArray[0].Length - 2) / 6;
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "16" || selectedVal == "14")
            {  // 二星直选
                var argsArray = val.Split(',');
                zs = argsArray[0].Length * argsArray[1].Length;
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "18" || selectedVal == "28")
            {
                var argsArray = val.Split(',');
                if (argsArray[0].Length < 2)
                {
                    if (args == 2)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
                zs = argsArray[0].Length * (argsArray[0].Length - 1) / 2;
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "1" || selectedVal == "23" || selectedVal == "35")
            {  // 不定位
                var argsArray = val.Split(',');
                zs = argsArray[0].Length;
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "44")
            {     //五星直选复式
                var argsArray = val.Split(',');
                zs = argsArray[0].Length * argsArray[1].Length * argsArray[2].Length * argsArray[3].Length * argsArray[4].Length;
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "48" || selectedVal == "46")
            {  // 四星直选
                var argsArray = val.Split(',');
                zs = argsArray[0].Length * argsArray[1].Length * argsArray[2].Length * argsArray[3].Length;
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "40" || selectedVal == "42" || selectedVal == "58")
            {     //任三直选复式\任二直选复式\任四星直选复
                var argsArray = val.Split(',');
                tzbs = "";
                if (argsArray[0].Length > 0)
                {      //万位
                    tzbs += "1";
                }
                if (argsArray[1].Length > 0)
                {
                    tzbs += "2";
                }
                if (argsArray[2].Length > 0)
                {
                    tzbs += "3";
                }
                if (argsArray[3].Length > 0)
                {
                    tzbs += "4";
                }
                if (argsArray[4].Length > 0)
                {
                    tzbs += "5";
                }
                zs = (argsArray[0].Length == 0 ? 1 : argsArray[0].Length) * (argsArray[1].Length == 0 ? 1 : argsArray[1].Length) * (argsArray[2].Length == 0 ? 1 : argsArray[2].Length) * (argsArray[3].Length == 0 ? 1 : argsArray[3].Length) * (argsArray[4].Length == 0 ? 1 : argsArray[4].Length);
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "9")
            {  //五星定位胆
                var argsArray = val.Split(',');
                zs = argsArray[0].Length + argsArray[1].Length + argsArray[2].Length + argsArray[3].Length + argsArray[4].Length;
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "60")
            { //组选120
                var argsArray = val.Split(',');
                if (argsArray[0].Length < 5)
                {
                    if (args == 2)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
                zs = Combination(argsArray[0].Length, 5);
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "61" || selectedVal == "62" || selectedVal == "63" || selectedVal == "64" || selectedVal == "65")
            { //组选60
                int[] minchosen = { 1, 2, 1, 1 };
                int[] minchosen1 = { 3, 1, 2, 1 };
                var argsArray = val.Split(',');
                var status2 = argsArray[0].ToCharArray();
                int index = 0;
                var tmp_nums = 0;

                if (selectedVal == "61")
                {
                    index = 0;
                }
                else if (selectedVal == "62")
                {
                    index = 1;
                }
                else if (selectedVal == "63")
                {
                    index = 2;
                }
                else if (selectedVal == "64")
                {
                    index = 3;
                }
                else if (selectedVal == "65")
                {
                    index = 3;
                }
                if (status2.Length < minchosen[index])
                {
                    if (args == 2)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
                var status3 = argsArray[1].ToCharArray();
                if (status3.Length < minchosen1[index])
                {
                    if (args == 2)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
                if (status2.Length >= minchosen[index] && status3.Length >= minchosen1[index])
                {
                    var h = intersect(status2, status3).Length;  //计算两个集合的交集
                    tmp_nums = Combination(status2.Length, minchosen[index]) * Combination(status3.Length, minchosen1[index]);
                    if (h > 0)
                    {
                        if (selectedVal == "61")
                        {
                            tmp_nums -= Combination(h, 1) * Combination(status3.Length - 1, 2);
                        }
                        else
                        {
                            if (selectedVal == "62")
                            {
                                tmp_nums -= Combination(h, 2) * Combination(2, 1);
                                if (status2.Length - h > 0)
                                {
                                    tmp_nums -= Combination(h, 1) * Combination(status2.Length - h, 1);
                                }
                            }
                            else
                            {
                                if (selectedVal == "63")
                                {
                                    tmp_nums -= Combination(h, 1) * Combination(status3.Length - 1, 1);

                                }
                                else
                                {
                                    if (selectedVal == "64" || selectedVal == "65")
                                    {
                                        tmp_nums -= Combination(h, 1);
                                    }
                                }
                            }
                        }
                    }
                    zs += tmp_nums;
                    je = getJE(zs, beiShu, unit);
                }
            }
            else if (selectedVal == "66")
            {     //组选24
                var argsArray = val.Split(',');
                if (argsArray[0].Length < 4)
                {
                    if (args == 2)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
                zs = Combination(argsArray[0].Length, 4);
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "67" || selectedVal == "69")
            {     //组选12
                int[] minchosen = { 1, 2, 1, 1 };
                int[] minchosen1 = { 3, 1, 2, 1 };
                var argsArray = val.Split(',');
                var status2 = argsArray[0].ToCharArray();
                int index = 0, tmp_nums=0;
                if (selectedVal == "67")
                {
                    index = 2;
                }
                else if (selectedVal == "69")
                {
                    index = 3;
                }
                if (status2.Length < minchosen[index])
                {
                    if (args == 2)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
                var status3 = argsArray[1].ToCharArray();
                if (status3.Length < minchosen1[index])
                {
                    if (args == 2)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
                if (status2.Length >= minchosen[index] && status3.Length >= minchosen1[index])
                {
                    var h = intersect(status2, status3).Length;  //计算两个集合的交集
                    tmp_nums = Combination(status2.Length, minchosen[index]) * Combination(status3.Length, minchosen1[index]);
                    if (h > 0)
                    {
                        if (selectedVal == "67")
                        {
                            tmp_nums -= Combination(h, 1) * Combination(status3.Length - 1, 1);
                            }
                        else if (selectedVal == "69")
                        {
                            tmp_nums -= Combination(h, 1);
                            }
                    }
                    zs += tmp_nums;
                    je = getJE(zs, beiShu, unit);
                }
            }
            else if (selectedVal == "68")
            {     //组选6
                var argsArray = val.Split(',');
                if (argsArray[0].Length < 1)
                {
                    if (args == 2)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
                zs = Combination(argsArray[0].Length, 2);
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "70" || selectedVal == "71" || selectedVal == "72" || selectedVal == "73")
            {     //一帆风顺、好事成双、三星报喜、四季发财
                var argsArray = val.Split(',');
                zs = argsArray[0].Length;
                je = getJE(zs, beiShu, unit);
            }
            if (args == 2)
            {
                ClearSelect();
            }
            if (val != ",,,," || (cnt > 0 && selectedCnt == cnt))
            {//不需要每行选，但是一个都没选
                if (args == 2)
                {

                    if (zs == 0)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入", 2));
                        return;
                    }

                    ConfirmPanelObj cpObj = new ConfirmPanelObj();
                    cpObj.lCfg = lotteryCfg;
                    cpObj.subCfg = currentSubMode;

                    cpObj.showContent = (val.Length > valCutterIndex ? val.Substring(0, valCutterIndex) + "..." : val);

                    cpObj.contents = val;
                    cpObj.lid = activityID;
                    cpObj.zs = zs;
                    cpObj.bs = beiShu;
                    cpObj.model = unit;
                    cpObj.amount = je;
                    cpObj.tzbs = tzbs;

                    ShowConfirmSignal.Dispatch(cpObj);

                }
            }
        }
        else if (rType == "text")
        {
            var cnt = seletedType.cnt;
            var canRepeat = seletedType.canrepeat;
            var str = panelScript.textInput.value;
            if (string.IsNullOrEmpty(str))
            {
                if (args == 2)
                {
                    return;
                }
                else
                {
                    clearPendingChange();
                    return;
                }
            }
            //首先将非数字的作为分隔符分解字符串
            List<string> arrList = new List<string>();
            foreach (Match match in Regex.Matches(str, @"[\d]+"))
                arrList.Add(match.Value);
            var arr = arrList.ToArray();

            

            var list = new List<string>();
            //判断每个分解的字符串是否符合规则
            if (selectedVal == "52" || selectedVal == "53" || selectedVal == "54")
            {
                for (var s = 0; s < arr.Length; s++)
                {
                    if (arr[s] != "")
                    {
                        var flag = (checkNotSameInThree(arr[s]) || checkTwoSameInThree(arr[s]));
                        if (flag == false)
                        {
                            if (args == 2)
                            {
                                MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                                return;
                            }
                            else
                            {
                                clearPendingChange();
                                return;
                            }
                        }
                        else
                            list.Add(arr[s]);
                    }
                }
            }
            else
            {
                //处理其他，重复或者不能重复，按照数目来判断
                for (var s = 0; s < arr.Length; s++)
                {
                    if (arr[s].Length != cnt)
                    {
                        if (args == 2)
                        {
                            MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                            return;
                        }
                        else
                        {
                            clearPendingChange();
                            return;
                        }
                    }
                    else
                    {
                        var flag = true;
                        if (canRepeat == 0)
                        {
                            flag = ckeckNotRepeat(arr[s]);
                        }
                        if (flag == false)
                        {
                            if (args == 2)
                            {
                                MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                                return;
                            }
                            else
                            {
                                clearPendingChange();
                                return;
                            }
                        }
                        else
                            list.Add(arr[s]);
                    }
                }
            }
            zs = list.Count;
            je = getJE(zs, beiShu, unit);
            val = string.Join("$", list.ToArray());
            if (args == 2)
            {
                if (string.IsNullOrEmpty(val))
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                    return;
                }

                ConfirmPanelObj cpObj = new ConfirmPanelObj();
                cpObj.lCfg = lotteryCfg;
                cpObj.subCfg = currentSubMode;

                cpObj.showContent = (val.Length > valCutterIndex ? val.Substring(0, valCutterIndex) + "..." : val);

                cpObj.contents = val;
                cpObj.lid = activityID;
                cpObj.zs = zs;
                cpObj.bs = beiShu;
                cpObj.model = unit;
                cpObj.amount = je;
                cpObj.tzbs = tzbs;

                ShowConfirmSignal.Dispatch(cpObj);
            }
        }
        else if (rType == "danShuang")
        {
            
                var divs = panelScript.scrollview_v.NumsItems;
                for (var i = 0; i < divs.Count; i++)
               {
                var div = divs[i];
                var selecteds = div.SelectedNums;
                //倒序排序
                //selecteds.Sort(Compare);

                var exit = false;
                for (var j = 0; j < selecteds.Count; j++)
                {
                    var selected = selecteds[j];
                   
                    val += selected;
                    exit = true;
                }
                if (i != divs.Count - 1)
                    val += ',';
                if (exit == false)
                {
                    //alert('号码输入有误，请重新输入');
                    if (args == 2)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
            }
            if (  args == 2 )
            {
                ClearSelect();
            }
            var argsArray = val.Split(',');
            zs = argsArray[0].Length * argsArray[1].Length;
            je = getJE(zs, beiShu, unit);
            if (args == 2)
            {

                if (zs == 0)
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入", 2));
                    return;
                }

                ConfirmPanelObj cpObj = new ConfirmPanelObj();
                cpObj.lCfg = lotteryCfg;
                cpObj.subCfg = currentSubMode;

                cpObj.showContent = (val.Length > valCutterIndex ? val.Substring(0, valCutterIndex) + "..." : val);
                cpObj.showContent = cpObj.showContent.Replace("9", "大");
                cpObj.showContent = cpObj.showContent.Replace("1", "小");
                cpObj.showContent = cpObj.showContent.Replace("3", "单");
                cpObj.showContent = cpObj.showContent.Replace("2", "双");


                cpObj.contents = val;
                cpObj.lid = activityID;
                cpObj.zs = zs;
                cpObj.bs = beiShu;
                cpObj.model = unit;
                cpObj.amount = je;
                cpObj.tzbs = tzbs;

                ShowConfirmSignal.Dispatch(cpObj);
            }
        }
        else if (rType == "longhu")
        {
            
            var divs = panelScript.scrollview_v.NumsItems;
            var text = "";
            for (var i = 0; i < divs.Count; i++)
            {
                var div = divs[i];
                    var selecteds = div.SelectedNums;
                    var exit = false;
                    for (var j = 0; j < selecteds.Count; j++)
                {
                    var selected = selecteds[j];
                    
                    val += selected;

                    if (val == "lw" || val == "hg")
                    {
                        text += "万位VS个位";
                        if (val == "lw")
                        {
                            text += "-万位";
                        }
                        else
                        {
                            text += "-个位";
                        }
                    } else
                    {
                        text += "千位VS十位";
                        if (val == "lq")
                        {
                            text += "-千位";
                        }
                        else
                        {
                            text += "-十位";
                        }
                    }
                    

                    exit = true;
                }
                if (i != divs.Count - 1)
                {
                    val += ',';
                    text += ',';
                }
                if (exit == false)
                {
                    if (args == 2)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
            }
            if (args == 2)
            {
                ClearSelect();
            }
            var argsArray = val.Split(',');
            zs = 1;
            je = getJE(zs, beiShu, unit);
            if (args == 2)
            {

                if (zs == 0)
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入", 2));
                    return;
                }

                ConfirmPanelObj cpObj = new ConfirmPanelObj();
                cpObj.lCfg = lotteryCfg;
                cpObj.subCfg = currentSubMode;

                


                cpObj.showContent = (val.Length > valCutterIndex ? val.Substring(0, valCutterIndex) + "..." : val);
                cpObj.showContent = cpObj.showContent.Replace("lw", "龙");
                cpObj.showContent = cpObj.showContent.Replace("lq", "龙");
                cpObj.showContent = cpObj.showContent.Replace("hg", "虎");
                cpObj.showContent = cpObj.showContent.Replace("hs", "虎");

                cpObj.contents = val;
                cpObj.lid = activityID;
                cpObj.zs = zs;
                cpObj.bs = beiShu;
                cpObj.model = unit;
                cpObj.amount = je;
                cpObj.tzbs = tzbs;

                ShowConfirmSignal.Dispatch(cpObj);
            }
        }
        else if (rType == "hezhi")
        {
            var list = new List<string>();

            var divs = panelScript.scrollview_v.NumsItems;
            for (var i = 0; i < divs.Count; i++)
            {
                var div = divs[i];
                var selecteds = div.SelectedNums;
                var exit = false;
                for (var j = 0; j < selecteds.Count; j++)
                {
                    var selected = selecteds[j];

                    list.Add(selected);

                    exit = true;
                }
                if (exit == false)
                {
                    if (args == 2)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
            }
            for (var i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    val = list[i];
                }
                else
                {
                    val += "$" + list[i];
                }
                
                if (selectedVal == "8" || selectedVal == "55")
                {  //直选和值三星
                    zs += getSxZxhzZs(int.Parse(list[i]));
                }
                else if (selectedVal == "56" || selectedVal == "57")
                {  //直选和值二星
                    zs += getExZxhzZs(int.Parse(list[i]));
                }
            }

            je = getJE(zs, beiShu, unit);
            if (args == 2)
            {
                ClearSelect();

                if (zs == 0)
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入", 2));
                    return;
                }

                ConfirmPanelObj cpObj = new ConfirmPanelObj();
                cpObj.lCfg = lotteryCfg;
                cpObj.subCfg = currentSubMode;

                cpObj.showContent = (val.Length > valCutterIndex ? val.Substring(0, valCutterIndex) + "..." : val);

                cpObj.contents = val;
                cpObj.lid = activityID;
                cpObj.zs = zs;
                cpObj.bs = beiShu;
                cpObj.model = unit;
                cpObj.amount = je;
                cpObj.tzbs = tzbs;

                ShowConfirmSignal.Dispatch(cpObj);
            }
        }
        else if (rType == "reXuan")
        {
            var cnt = seletedType.cnt;
            //var chkLen = $('[name="chkWeiXuanZe"]:checked').Length;
            var chkLen = 0;
            for (int i = 0; i < panelScript.CheckBoxes.Length; ++i)
            {
                if (panelScript.CheckBoxes[i].value)
                {
                    chkLen++;
                }
            }


            if (chkLen < cnt)
            {
                if (args == 2)
                {
 
                    MsgSignal.Dispatch(new MsgPara(string.Format("请先至少选择{0}个位",cnt),2));
                    return;
                }
                else
                {
                    clearPendingChange();
                    return;
                }
            }
            if (chkLen > cnt && !(selectedVal == "41" || selectedVal == "43" || selectedVal == "59" || selectedVal == "77" || selectedVal == "80"))
            {
                if (args == 2)
                {
                    MsgSignal.Dispatch(new MsgPara(string.Format("必须投注{0}位数的球位，请重新选择！", cnt),2));
                    return;
                }
                else
                {
                    clearPendingChange();
                    return;
                }
            }
            var str = panelScript.textInput.value;
            if (string.IsNullOrEmpty(str))
                return;

            List<string> arrList = new List<string>();
            foreach (Match match in Regex.Matches(str, @"[\d]+"))
                arrList.Add(match.Value);
            var arr = arrList.ToArray();

 
            var list = new List<string>();
            //判断每个分解的字符串是否符合规则
            for (var s = 0; s < arr.Length; s++)
            {
                if (arr[s] != "")
                {
                    if (arr[s].Length != cnt)
                    {
                        if (args == 2)
                        {
                            MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                            return;
                        }
                        else
                        {
                            clearPendingChange();
                            return;
                        }
                    }
                    else
                        list.Add(arr[s]);
                }
            }
            var argsArray = val.Split(',');
            tzbs = "";
            if (panelScript.CheckBoxes[0].value) {      //万位
                tzbs += "1";
            }
            if (panelScript.CheckBoxes[1].value) {
                tzbs += "2";
            }
            if (panelScript.CheckBoxes[2].value) {
                tzbs += "3";
            }
            if (panelScript.CheckBoxes[3].value) {
                tzbs += "4";
            }
            if (panelScript.CheckBoxes[4].value) {
                tzbs += "5";
            }
            if (selectedVal == "41" || selectedVal == "77")
            {
                zs = getRenXuanAndZuXuan(tzbs.Length, 3) * list.Count;
            }
            else if (selectedVal == "43" || selectedVal == "80")
            {
                zs = getRenXuanAndZuXuan(tzbs.Length, 2) * list.Count;
            }
            else if (selectedVal == "59")
            {
                zs = getRenXuanAndZuXuan(tzbs.Length, 4) * list.Count;
            }
            else
            {
                zs = list.Count;
            }
            je = getJE(zs, beiShu, unit);

            val = string.Join("$", list.ToArray());

            if (args == 2)
            {

                if (zs == 0)
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入", 2));
                    return;
                }

                ConfirmPanelObj cpObj = new ConfirmPanelObj();
                cpObj.lCfg = lotteryCfg;
                cpObj.subCfg = currentSubMode;

                cpObj.showContent = (val.Length > valCutterIndex ? val.Substring(0, valCutterIndex) + "..." : val);

                cpObj.contents = val;
                cpObj.lid = activityID;
                cpObj.zs = zs;
                cpObj.bs = beiShu;
                cpObj.model = unit;
                cpObj.amount = je;
                cpObj.tzbs = tzbs;

                ShowConfirmSignal.Dispatch(cpObj);
            }
        }
        else if (rType == "reXuanHeZhi")
        {
            var cnt = seletedType.cnt;
            var chkLen = 0;
            for (int i = 0; i < panelScript.CheckBoxes.Length; ++i)
            {
                if (panelScript.CheckBoxes[i].value)
                {
                    chkLen++;
                }
            }
            if (chkLen < cnt)
            {
                if (args == 2)
                {
                    MsgSignal.Dispatch(new MsgPara(string.Format("请先至少选择{0}个位", cnt),2));
                    return;
                }
                else
                {
                    clearPendingChange();
                    return;
                }
            }
            if (chkLen > cnt && !(selectedVal == "75" || selectedVal == "76" || selectedVal == "79" || selectedVal == "78" || selectedVal == "74"))
            {
                if (args == 2)
                {
                    MsgSignal.Dispatch(new MsgPara(string.Format("必须投注{0}位数的球位，请重新选择！", cnt),2));
                    return;
                }
                else
                {
                    clearPendingChange();
                    return;
                }
            }

            var list = new List<string>();
            var divs = panelScript.scrollview_v.NumsItems;
            var connector = "";
            for (var i = 0; i < divs.Count; i++)
            {
                var div = divs[i];
                var selecteds = div.SelectedNums;
                var exit = false;
                for (var j = 0; j < selecteds.Count; j++)
                {
                    var selected = selecteds[j];
                    list.Add(selected);
                    exit = true;
                }
                if (exit == false)
                {
                    if (args == 2)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
            }
            if (selectedVal == "75" || selectedVal == "79")
            {
                if (list.Count < 2)
                {
                    if (args == 2)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
            }
            else if (selectedVal == "76")
            {
                if (list.Count < 3)
                {
                    if (args == 2)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
            }
            tzbs = "";
            if (panelScript.CheckBoxes[0].value)
            {      //万位
                tzbs += "1";
            }
            if (panelScript.CheckBoxes[1].value)
            {
                tzbs += "2";
            }
            if (panelScript.CheckBoxes[2].value)
            {
                tzbs += "3";
            }
            if (panelScript.CheckBoxes[3].value)
            {
                tzbs += "4";
            }
            if (panelScript.CheckBoxes[4].value)
            {
                tzbs += "5";
            }
            for (var i = 0; i < list.Count; i++)
            {
                if (selectedVal == "74")
                {  //任三直选和值
                    zs += getSxZxhzZs(int.Parse(list[i]));
                    connector = "$";
                }
                else if (selectedVal == "75")
                {
                    connector = "";
                }
                else if (selectedVal == "78")
                {  //任二直选和值
                    zs += getExZxhzZs(int.Parse(list[i]));
                    connector = "$";
                }
                if (i == 0)
                {
                    val = list[i];
                }
                else
                {
                    val += connector + list[i];
                }

            }
            if (selectedVal == "75")
            {      //任三组三复式
                zs = list.Count * (list.Count - 1);
                if (tzbs.Length == 4)
                    zs = zs * 4;
                else if (tzbs.Length == 5)
                    zs = zs * 10;
            }
            else if (selectedVal == "76")
            {     //任三组六复式
                zs = list.Count * (list.Count - 1) * (list.Count - 2) / 6;
                if (tzbs.Length == 4)
                    zs = zs * 4;
                else if (tzbs.Length == 5)
                    zs = zs * 10;
            }
            else if (selectedVal == "79")
            {     //任二组选复式
                zs = list.Count * (list.Count - 1) / 2;
                if (tzbs.Length == 3)
                    zs = zs * 3;
                else if (tzbs.Length == 4)
                    zs = zs * 6;
                else if (tzbs.Length == 5)
                    zs = zs * 10;
            }
            else if (selectedVal == "78")
            {
                if (tzbs.Length == 3)
                {
                    zs = zs * 3;
                }
                else if (tzbs.Length == 4)
                {
                    zs = zs * 6;
                }
                else if (tzbs.Length == 5)
                {
                    zs = zs * 10;
                }
            }
            else if (selectedVal == "74")
            {
                if (tzbs.Length == 4)
                {
                    zs = zs * 4;
                }
                else if (tzbs.Length == 5)
                {
                    zs = zs * 10;
                }
            }
            je = getJE(zs, beiShu, unit);
            if (args == 2)
            {
                ClearSelect();

                if (zs == 0)
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入", 2));
                    return;
                }

                ConfirmPanelObj cpObj = new ConfirmPanelObj();
                cpObj.lCfg = lotteryCfg;
                cpObj.subCfg = currentSubMode;

                cpObj.showContent = (val.Length > valCutterIndex ? val.Substring(0, valCutterIndex) + "..." : val);

                cpObj.contents = val;
                cpObj.lid = activityID;
                cpObj.zs = zs;
                cpObj.bs = beiShu;
                cpObj.model = unit;
                cpObj.amount = je;
                cpObj.tzbs = tzbs;

                ShowConfirmSignal.Dispatch(cpObj);
            }
        }
        if (args == 2)
        {//添加到列表
            //caculateMoney(zs, je, "+");
            //    $("#txtBeiShu").val(1);
            //    $("#pbUnit").text("0");
            //    $("#pbMoney").text("0.00");
        }
        else
        {//预添加
        
            UpdateBotInfo(zs, je);
        }
    }

    //添加选中号码
    void addNumbers3D(int args, LotterySubModeCfg modeCfg, int bs, int unitpara)
    {
        LotterySubModeCfg seletedType = modeCfg;

        int beiShu = bs;
        int unit = unitpara; //1元 2角 3分
                             //var showMianE = " 元",

        var tzbs = "0";

        int zs = 0;
        string je = "";

        string rType = seletedType.type;
        var val = "";
        var selectedVal = seletedType.subModeId.ToString();
        if (rType == "select")
        {
            var canNotAll = (seletedType.cannotall == 1) ? true : false;
            var cnt = seletedType.cnt;

            var selectedCnt = 0;
            //必须全部选择
            var divs = panelScript.scrollview_v.NumsItems;
            //必须全部选择
            for (var i = 0; i < divs.Count; i++)
            {
                var div = divs[i];
                var selecteds = div.SelectedNums;
                var exit = false;
                for (var j = 0; j < selecteds.Count; j++)
                {
                    var selected = div.SelectedNums[j];

                    val += selected;
                    exit = true;
                }
                if (i != divs.Count - 1)
                    val += ',';

                if (exit == false && canNotAll == false)
                {//存在没选，而且每行都要选
                    if (args == 2)  //2代表下单操作
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
                if (exit)
                    selectedCnt++;
            }
            if (canNotAll == true && cnt > -1 && selectedCnt != cnt)
            {
                if (args == 2)
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                    return;
                }
                else
                {
                    clearPendingChange();
                    return;
                }
            }
            if ("1" == selectedVal)
            { //三星直选复
                var argsArray = val.Split(',');
                zs = argsArray[0].Length * argsArray[1].Length * argsArray[2].Length;
                je = getJE(zs, beiShu, unit);
            }
            else if ("3" == selectedVal)
            {  //三星组选三复式
                var argsArray = val.Split(',');
                zs = argsArray[0].Length * (argsArray[0].Length - 1);
                je = getJE(zs, beiShu, unit);
            }
            else if ("5" == selectedVal)
            {  //三星组选六复式
                var argsArray = val.Split(',');
                zs = argsArray[0].Length * (argsArray[0].Length - 1) * (argsArray[0].Length - 2) / 6;
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "7" || selectedVal == "10" || selectedVal == "11")
            {  // 前一直选复式\三星不定位复\后一直选复式
                var argsArray = val.Split(',');
                zs = argsArray[0].Length;
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "8" || selectedVal == "12")
            {  // 前二直选复式/后二直选复式
                var argsArray = val.Split(',');
                zs = argsArray[0].Length * argsArray[1].Length;
                je = getJE(zs, beiShu, unit);
            }
            if (args == 2)
            {
                ClearSelect();
            }
            if (val != ",,,," || (cnt > 0 && selectedCnt == cnt))
            {//不需要每行选，但是一个都没选
                if (args == 2)
                {

                    if (zs == 0)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入", 2));
                        return;
                    }

                    ConfirmPanelObj cpObj = new ConfirmPanelObj();
                    cpObj.lCfg = lotteryCfg;
                    cpObj.subCfg = currentSubMode;

                    cpObj.showContent = (val.Length > valCutterIndex ? val.Substring(0, valCutterIndex) + "..." : val);

                    cpObj.contents = val;
                    cpObj.lid = activityID;
                    cpObj.zs = zs;
                    cpObj.bs = beiShu;
                    cpObj.model = unit;
                    cpObj.amount = je;
                    cpObj.tzbs = tzbs;

                    ShowConfirmSignal.Dispatch(cpObj);
                }
            }
        }
        else if (rType == "text")
        {
            var cnt = seletedType.cnt;
            var canRepeat = seletedType.canrepeat;
            var str = panelScript.textInput.value;
            if (string.IsNullOrEmpty(str))
            {
                if (args == 2)
                {
                    return;
                }
                else
                {
                    clearPendingChange();
                    return;
                }
            }
            //首先将非数字的作为分隔符分解字符串
            List<string> arrList = new List<string>();
            foreach (Match match in Regex.Matches(str, @"[\d]+"))
                arrList.Add(match.Value);
            var arr = arrList.ToArray();
            var list = new List<string>();
            //判断每个分解的字符串是否符合规则
            if (selectedVal == "4")
            {//三单式
                for (var s = 0; s < arr.Length; s++)
                {
                    if (arr[s] != "")
                    {
                        var flag = checkTwoSameInThree(arr[s]);
                        if (flag == false)
                        {
                            if (args == 2)
                            {
                                MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                                return;
                            }
                            else
                            {
                                clearPendingChange();
                                return;
                            }
                        }
                        else
                            list.Add(arr[s]);
                    }
                }
            }
            else if (selectedVal == "6")
            {
                for (var s = 0; s < arr.Length; s++)
                {
                    if (arr[s] != "")
                    {
                        var flag = checkNotSameInThree(arr[s]);
                        if (flag == false)
                        {
                            if (args == 2)
                            {
                                MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                                return;
                            }
                            else
                            {
                                clearPendingChange();
                                return;
                            }
                        }
                        else
                            list.Add(arr[s]);
                    }
                }
            }
            else if (selectedVal == "52" || selectedVal == "53" || selectedVal == "54")
            {
                for (var s = 0; s < arr.Length; s++)
                {
                    if (arr[s] != "")
                    {
                        var flag = (checkNotSameInThree(arr[s]) || checkTwoSameInThree(arr[s]));
                        if (flag == false)
                        {
                            if (args == 2)
                            {
                                MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                                return;
                            }
                            else
                            {
                                clearPendingChange();
                                return;
                            }
                        }
                        else
                            list.Add(arr[s]);
                    }
                }
            }
            else
            {
                //处理其他，重复或者不能重复，按照数目来判断
                for (var s = 0; s < arr.Length; s++)
                {
                    if (arr[s].Length != cnt)
                    {
                        if (args == 2)
                        {
                            MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                            return;
                        }
                        else
                        {
                            clearPendingChange();
                            return;
                        }
                    }
                    else
                    {
                        var flag = true;
                        if (canRepeat == 0)
                        {
                            flag = ckeckNotRepeat(arr[s]);
                        }
                        if (flag == false)
                        {
                            if (args == 2)
                            {
                                MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                                return;
                            }
                            else
                            {
                                clearPendingChange();
                                return;
                            }
                        }
                        else
                            list.Add(arr[s]);
                    }
                }
            }
            zs = list.Count;
            je = getJE(zs, beiShu, unit);
            val = string.Join("$", list.ToArray());
            if (args == 2)
            {
                if (string.IsNullOrEmpty(val))
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                    return;
                }

                ConfirmPanelObj cpObj = new ConfirmPanelObj();
                cpObj.lCfg = lotteryCfg;
                cpObj.subCfg = currentSubMode;

                cpObj.showContent = (val.Length > valCutterIndex ? val.Substring(0, valCutterIndex) + "..." : val);

                cpObj.contents = val;
                cpObj.lid = activityID;
                cpObj.zs = zs;
                cpObj.bs = beiShu;
                cpObj.model = unit;
                cpObj.amount = je;
                cpObj.tzbs = tzbs;

                ShowConfirmSignal.Dispatch(cpObj);
            }
        }
        else if (rType == "hezhi")
        {
            var list = new List<string>();
            var divs = panelScript.scrollview_v.NumsItems;
            for (var i = 0; i < divs.Count; i++)
            {
                var div = divs[i];
                var selecteds = div.SelectedNums;
                var exit = false;
                for (var j = 0; j < selecteds.Count; j++)
                {
                    var selected = selecteds[j];
                    list.Add(selected);
                    exit = true;
                }
                if (exit == false)
                {
                    if (args == 2)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
            }
            for (var i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    val = list[i];
                }
                else
                {
                    val += "$" + list[i];
                }
                if (selectedVal == "14")
                {  //直选和值三星
                    zs += getSxZxhzZs(int.Parse(list[i]));
                }
            }
            je = getJE(zs, beiShu, unit);
            if (args == 2)
            {
                ClearSelect();

                if (zs == 0)
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入", 2));
                    return;
                }

                ConfirmPanelObj cpObj = new ConfirmPanelObj();
                cpObj.lCfg = lotteryCfg;
                cpObj.subCfg = currentSubMode;

                cpObj.showContent = (val.Length > valCutterIndex ? val.Substring(0, valCutterIndex) + "..." : val);

                cpObj.contents = val;
                cpObj.lid = activityID;
                cpObj.zs = zs;
                cpObj.bs = beiShu;
                cpObj.model = unit;
                cpObj.amount = je;
                cpObj.tzbs = tzbs;

                ShowConfirmSignal.Dispatch(cpObj);
            }
        }
        if (args == 2)
        {//添加到列表
            
        }
        else
        {//预添加
            UpdateBotInfo(zs, je);
        }
    }

    //11选5
    void addNumbers11_5(int args, LotterySubModeCfg modeCfg, int bs, int unitpara)
    {
        LotterySubModeCfg seletedType = modeCfg;

        int beiShu = bs;
        int unit = unitpara; //1元 2角 3分
                             //var showMianE = " 元",

        var tzbs = "0";

        int zs = 0;
        string je = "";

        string rType = seletedType.type;
        var val = "";

        var selectedVal = seletedType.subModeId.ToString();
        
        if (rType == "select")
        {
             
            var selectedNumList = new Dictionary<int ,List<string>>();

            var minCnt = seletedType.mincnt;
            var checkCombin = seletedType.checkcombin;


            var canNotAll = (seletedType.cannotall == 1) ? true : false;
            var cnt = seletedType.cnt;

            var selectedCnt = 0;
            //必须全部选择
            var divs = panelScript.scrollview_v.NumsItems;
            for (var i = 0; i < divs.Count; i++)
            {
                var selectNum = 0;
                selectedNumList[i] = new List<string>();

                var div = divs[i];
                var selecteds = div.SelectedNums;
                var exit = false;

                for (var j = 0; j < selecteds.Count; j++)
                {
                    var selected = div.SelectedNums[j];
                   
                    var selVal = int.Parse(selected);
                    if (selVal < 10)
                    {
                        val += "0" + selVal;
                        selectedNumList[i].Add("0" + selVal);
                    }
                    else
                    {
                        val += "" + selVal;
                        selectedNumList[i].Add("" + selVal);
                    }
                    if (j != selecteds.Count - 1)
                        val += ",";
                    selectNum++;
                    exit = true;
                }
                if (i != divs.Count - 1)
                    val += '|';
                if (minCnt != -1 && selectNum < minCnt)//存在至少选择号码
                {
                    if (args == 2)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
                if (exit == false && canNotAll == false)
                {//存在没选，而且每行都要选
                    if (args == 2)  //2代表下单操作
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
                if (exit)
                    selectedCnt++;
            }
            if (canNotAll == true && cnt > -1 && selectedCnt != cnt)
            {
                if(args == 2)
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                    return;
                }
                else
                {
                    clearPendingChange();
                    return;
                }
            }
            //debugger;
            if (checkCombin == 1)
            {
                //判断组合数 begin
                var resultArr = combin(selectedNumList);
                var resultList = new List<string>();

                //for (i in resultArr)
                //{
                //    resultList.push(resultArr[i].join(','));
                //}
                foreach (KeyValuePair<int, List<string>> pair in resultArr)
                {
                    resultList.Add(string.Join(",", pair.Value.ToArray()));
                }
                var sameCnt = getExitSameNumberCnt(resultList);
                if (sameCnt == resultList.Count)
                {
                    if (args == 2)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
                //判断组合数 end
            }
            if (selectedVal == "1")
            {  //任选一中一复式
                var argsArray = val.Split(',');
                zs = argsArray.Length;
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "2" || selectedVal == "21")
            {  //任选二中二复式\前二组选复式
                var argsArray = val.Split(',');
                zs = comb(argsArray.Length, 2);
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "4" || selectedVal == "23")
            {  //任选三中三复式\前三组选复式
                var argsArray = val.Split(',');
                zs = comb(argsArray.Length, 3);
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "6")
            {  //任选四中四复式
                var argsArray = val.Split(',');
                zs = comb(argsArray.Length, 4);
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "8")
            {  //任选五中五复式
                var argsArray = val.Split(',');
                zs = comb(argsArray.Length, 5);
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "10")
            {  //任选六中五复式
                var argsArray = val.Split(',');
                zs = comb(argsArray.Length, 6);
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "12")
            {  //任选七中五复式
                var argsArray = val.Split(',');
                zs = comb(argsArray.Length, 7);
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "14")
            {  //任选八中五复式
                var argsArray = val.Split(',');
                zs = comb(argsArray.Length, 8);
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "19")
            {  //前三直选复式
                var argsArray = val.Split('|');
                var wanwei = argsArray[0].Split(',');
                var qianwei = argsArray[1].Split(',');
                var baiwei = argsArray[2].Split(',');
                zs = wanwei.Length * qianwei.Length * baiwei.Length;
                for (var i = 0; i < wanwei.Length; i++)
                {
                    for (var j = 0; j < qianwei.Length; j++)
                    {
                        for (var k = 0; k < baiwei.Length; k++)
                        {
                            var a = wanwei[i];
                            var b = qianwei[j];
                            var c = baiwei[k];
                            if (a == b || a == c || b == c)
                            {
                                zs--;
                            }
                        }
                    }
                }
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "17")
            {  //前二直选复式
                var argsArray = val.Split('|');
                var wanwei = argsArray[0].Split(',');
                var qianwei = argsArray[1].Split(',');
                zs = wanwei.Length * qianwei.Length;
                for (var i = 0; i < wanwei.Length; i++)
                {
                    for (var j = 0; j < qianwei.Length; j++)
                    {
                        var a = wanwei[i];
                        var b = qianwei[j];
                        if (a == b)
                        {
                            zs--;
                        }
                    }
                }
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "16")
            {  //定位胆
                var argsArray = val.Split('|');
                //var wanwei = argsArray[0].replace(/,/ g, "");
                //var qianwei = argsArray[1].replace(/,/ g, "");
                //var baiwei = argsArray[2].replace(/,/ g, "");
                //var shiwei = argsArray[3].replace(/,/ g, "");
                //var gewei = argsArray[4].replace(/,/ g, "");
                var wanwei = argsArray[0].Replace(",", "");
                var qianwei = argsArray[1].Replace(",", "");
                var baiwei = argsArray[2].Replace(",", "");
                var shiwei = argsArray[3].Replace(",", "");
                var gewei = argsArray[4].Replace(",", "");

                zs = (wanwei.Length + qianwei.Length + baiwei.Length + shiwei.Length + gewei.Length) / 2;
                je = getJE(zs, beiShu, unit);
            }
            if (args == 2)
            {
                ClearSelect();
            }
            if (-1 < val.IndexOf("|"))
            {
                //val = val.replace(/,/ g, "");
                //val = val.replace(/\|/ g, ",");

                val = val.Replace(",", "");
                val = val.Replace("|", ",");
            }
            if (val != "||||" || (cnt > 0 && selectedCnt == cnt))
            {//不需要每行选，但是一个都没选
                if (args == 2)
                {
                    if (zs == 0)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入", 2));
                        return;
                    }

                    ConfirmPanelObj cpObj = new ConfirmPanelObj();
                    cpObj.lCfg = lotteryCfg;
                    cpObj.subCfg = currentSubMode;

                    cpObj.showContent = (val.Length > valCutterIndex ? val.Substring(0, valCutterIndex) + "..." : val);

					if (selectedVal == "16") {//定位胆  服务端需要特殊处理  （比如选择是地一位现在是1和11 第二位选择是2和3 其他没有选 提交到服务端格式就是1，11|2，3|||）
						//01,10|01,10|||
						var newVal = "";
						for (var i = 0; i < divs.Count; i++)
						{
							var list = selectedNumList[i];
							for (var j = 0; j < list.Count; j++)
							{
								if(j!=list.Count-1)
									newVal += list [j]+",";
								else
									newVal += list [j];
							}
							if (i != divs.Count - 1)
								newVal += '|';
						}
						cpObj.contents = newVal;
					} else {
						cpObj.contents = val;
					}
                    cpObj.lid = activityID;
                    cpObj.zs = zs;
                    cpObj.bs = beiShu;
                    cpObj.model = unit;
                    cpObj.amount = je;
                    cpObj.tzbs = tzbs;

                    ShowConfirmSignal.Dispatch(cpObj);

                }
            }
        }
        else if (rType == "text")
        {
            var cnt = seletedType.cnt;
            var canRepeat = seletedType.canrepeat;
            var str = panelScript.textInput.value;
            if (string.IsNullOrEmpty(str))
            {
                if (args == 2)
                {
                    return;
                }
                else
                {
                    clearPendingChange();
                    return;
                }
            }
            //首先将非数字的作为分隔符分解字符串
            List<string> arrList = new List<string>();
            foreach (Match match in Regex.Matches(str, @"[\d]+"))
                arrList.Add(match.Value);
            var arr = arrList.ToArray();
            
            var ds_iNum = 0;
            if (selectedVal == "24" || selectedVal == "20")
            {
                ds_iNum = 6;
            }
            else if (selectedVal == "18" || selectedVal == "22")
            {
                ds_iNum = 4;
            }
            else if (selectedVal == "31")
            {
                ds_iNum = 2;
            }
            else if (selectedVal == "3")
            {
                ds_iNum = 4;
            }
            else if (selectedVal == "5")
            {
                ds_iNum = 6;
            }
            else if (selectedVal == "7")
            {
                ds_iNum = 8;
            }
            else if (selectedVal == "9")
            {
                ds_iNum = 10;
            }
            else if (selectedVal == "11")
            {
                ds_iNum = 12;
            }
            else if (selectedVal == "13")
            {
                ds_iNum = 14;
            }
            else if (selectedVal == "15")
            {
                ds_iNum = 16;
            }

            if (!isCheck_syxw(arr, ds_iNum, selectedVal, (arr.Length > 1 ? 2 : 1)))
            {
                if (args == 2)
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                    return;
                }
                else
                {
                    clearPendingChange();
                    return;
                }
            }
            val = string.Join("$", arr);
            zs = arr.Length;
            je = getJE(zs, beiShu, unit);
            if (args == 2)
            {
                if (string.IsNullOrEmpty(val))
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                    return;
                }

                ConfirmPanelObj cpObj = new ConfirmPanelObj();
                cpObj.lCfg = lotteryCfg;
                cpObj.subCfg = currentSubMode;

                cpObj.showContent = (val.Length > valCutterIndex ? val.Substring(0, valCutterIndex) + "..." : val);

                cpObj.contents = val;
                cpObj.lid = activityID;
                cpObj.zs = zs;
                cpObj.bs = beiShu;
                cpObj.model = unit;
                cpObj.amount = je;
                cpObj.tzbs = tzbs;

                ShowConfirmSignal.Dispatch(cpObj);
            }
        }
        else if (rType == "dingDanShuang")
        {
            var divs = panelScript.scrollview_v.NumsItems[0].SelectedNums;
            
            if (divs.Count == 0)
            {
                return;
            }

            for (var i = 0; i < divs.Count; i++)
            {
                val += divs[i];
                if (i != divs.Count - 1)
                {
                    val += "$";
                }
            }
            zs = divs.Count;
            je = getJE(zs, beiShu, unit);
            if (args == 2)
            {
                ClearSelect();

                if (zs == 0)
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入", 2));
                    return;
                }

                ConfirmPanelObj cpObj = new ConfirmPanelObj();
                cpObj.lCfg = lotteryCfg;
                cpObj.subCfg = currentSubMode;

                cpObj.showContent = (val.Length > valCutterIndex ? val.Substring(0, valCutterIndex) + "..." : val);

                cpObj.contents = val;
                cpObj.lid = activityID;
                cpObj.zs = zs;
                cpObj.bs = beiShu;
                cpObj.model = unit;
                cpObj.amount = je;
                cpObj.tzbs = tzbs;

                ShowConfirmSignal.Dispatch(cpObj);
            }
        }
        else if (rType == "caiZhongWei")
        {
            var divs = panelScript.scrollview_v.NumsItems[0].SelectedNums;
            if (divs.Count == 0)
            {
                return;
            }
            for (var i = 0; i < divs.Count; i++)
            {
                val += divs[i];
                if (i != divs.Count - 1)
                {
                    val += "$";
                }
            }
            zs = divs.Count;
            je = getJE(zs, beiShu, unit);
            if (args == 2)
            {
                ClearSelect();
                if (zs == 0)
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入", 2));
                    return;
                }

                ConfirmPanelObj cpObj = new ConfirmPanelObj();
                cpObj.lCfg = lotteryCfg;
                cpObj.subCfg = currentSubMode;

                cpObj.showContent = (val.Length > valCutterIndex ? val.Substring(0, valCutterIndex) + "..." : val);

                cpObj.contents = val;
                cpObj.lid = activityID;
                cpObj.zs = zs;
                cpObj.bs = beiShu;
                cpObj.model = unit;
                cpObj.amount = je;
                cpObj.tzbs = tzbs;

                ShowConfirmSignal.Dispatch(cpObj);
            }
        }
        else if (rType == "reXuanDanTuo")
        {
            var cnt = seletedType.cnt;
            var parentDiv = panelScript.scrollview_v.NumsItems;
            var firstDiv = parentDiv[0];
            var firstDivLis = firstDiv.SelectedNums;

            if (firstDivLis.Count < 1)
            {
                if (args == 2)
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                    return;
                }
                else
                {
                    clearPendingChange();
                    return;
                }
            }
            var secondDiv = parentDiv[1];
            var secondDivLis = secondDiv.SelectedNums;
            var total = (firstDivLis.Count + secondDivLis.Count);
            if (total < cnt)
            {
                if (args == 2)
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                    return;
                }
                else
                {
                    clearPendingChange();
                    return;
                }
            }
             val = "";
            for (var i = 0; i < firstDivLis.Count; i++)
            {
                var tmp =firstDivLis[i];
                val += padLeft(tmp, 2);
                if (i != firstDivLis.Count - 1)
                    val += ',';
                else
                    val += ']';
                //if (args == addNumbersArgs.CalculationAndPrompt)
                //{
                //    tmp.removeClass('current');
                //}
            }
            for (var i = 0; i < secondDivLis.Count; i++)
            {
                var tmp = secondDivLis[i];
                val += padLeft(tmp, 2);
                if (i != secondDivLis.Count - 1)
                    val += ',';
                //if (args == addNumbersArgs.CalculationAndPrompt)
                //{
                //    tmp.removeClass('current');
                //}
            }
            var maxCnt = 0;
            if (selectedVal == "32")
            {  //任选二中二胆拖
                maxCnt = 2;
            }
            else if (selectedVal == "33")
            { //任选三中三胆拖
                maxCnt = 3;
            }
            else if (selectedVal == "34")
            { //任选四中四胆拖
                maxCnt = 4;
            }
            else if (selectedVal == "35")
            { //任选五中五胆拖
                maxCnt = 5;
            }
            else if (selectedVal == "36")
            { //任选六中五胆拖
                maxCnt = 6;
            }
            else if (selectedVal == "37")
            { //任选七中五胆拖
                maxCnt = 7;
            }
            else if (selectedVal == "38")
            { //任选八中五胆拖
                maxCnt = 8;
            }

            var argsArray = val.Split(']');
            var danma = argsArray[0].Split(',');
            var tuoma = argsArray[1].Split(',');
            zs = (danma.Length == 0 || tuoma.Length == 0) ? 0 : getDTZs(tuoma.Length, maxCnt - danma.Length);
            je = getJE(zs, beiShu, unit);
            if (args == 2)
            {
                ClearSelect();
                if (zs == 0)
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入", 2));
                    return;
                }

                ConfirmPanelObj cpObj = new ConfirmPanelObj();
                cpObj.lCfg = lotteryCfg;
                cpObj.subCfg = currentSubMode;

                cpObj.showContent = (val.Length > valCutterIndex ? val.Substring(0, valCutterIndex) + "..." : val);

                cpObj.contents = val;
                cpObj.lid = activityID;
                cpObj.zs = zs;
                cpObj.bs = beiShu;
                cpObj.model = unit;
                cpObj.amount = je;
                cpObj.tzbs = tzbs;

                ShowConfirmSignal.Dispatch(cpObj);
            }
        }
        if (args == 2)
        {//添加到列表
            //caculateMoney(zs, je, "+");
            //    $("#txtBeiShu").val(1);
            //    $("#pbUnit").text("0");
            //    $("#pbMoney").text("0.00");
        }
        else
        {//预添加

            UpdateBotInfo(zs, je);
        }
    }

    string padLeft(string str, int lenght)
    {
        if (str.Length >= lenght)
            return str;
        else
            return padLeft("0" + str, lenght);
    }
    string padRight(string str, int lenght)
    {
        if (str.Length >= lenght)
            return str;
        else
            return padRight(str + "0", lenght);
    }

    //判断数组存在相同的项有多少个
    int  getExitSameNumberCnt(List<string> list)
    {
        var sameCnt = 0;
        for (var i = 0; i < list.Count; i++)
        {
            var tmp = list[i];
            var arr = tmp.Split(',');
            var arrLen = arr.Length;
            var cnt = 1;
            for (var k = 0; k < arrLen; k++)
            {
                var tmp2 = arr[k];
                var exit = false;
                for (var j = 0; j < arrLen; j++)
                {
                    if (j != k && tmp2 == arr[j])
                    {
                        cnt++;
                        sameCnt++;
                        exit = true;
                        break;
                    }
                }
                if (exit == true)
                    break;
            }
        }
        return sameCnt;
    }


    //组合数
    Dictionary<int, List<string>> combin(Dictionary<int, List<string>>  CombinList)
    {
        /*
        var test = new Array();
        test[0] = new Array('a1', 'a2');
        test[1] = new Array('b1', 'b2', 'b3');
        test[2] = new Array('c1', 'c2');
        */
        var Result = new Dictionary<int , List<string>>();
        var CombineCount = 1;
        for (int  i=0; i< CombinList.Count;++i)
        {
            CombineCount *= CombinList[i].Count;
        }
        var RepeatTime = CombineCount;
        for (int i = 0; i < CombinList.Count; ++i)
        {
            //var ClassNo = i;
            var StudentList = CombinList[i];
            RepeatTime = RepeatTime / StudentList.Count;
            var StartPosition = 1;
            for (int j = 0; j < StudentList.Count; ++j)
            {
                var TempStartPosition = StartPosition;
                var SpaceCount = CombineCount / StudentList.Count / RepeatTime;
                for (int J = 1; J <= SpaceCount; J++)
                {
                    for (int I = 0; I < RepeatTime; I++)
                    {
                        if(!Result.ContainsKey(TempStartPosition + I))
                        {
                            Result[TempStartPosition + I] = new List<string>();
                        }
                        Result[TempStartPosition + I].Add(StudentList[j]);
                    }
                    TempStartPosition += RepeatTime * StudentList.Count;
                }
                StartPosition += RepeatTime;
            }
        }
        return Result;
    }

    bool isInteger(string value)
    {
        return Regex.IsMatch(value, @"^[0-9]*$");
    }

    bool isInteger(string[] value)
    {
        return Regex.IsMatch(value[0], @"^[0-9]*$");
    }

    bool isCheck_syxw(string[] adddsinfo, int ds_iNum, string Type,int iNum)
    {
        int typeInt = int.Parse(Type);
        if (typeInt == 31 || typeInt == 3 || typeInt == 5 || typeInt == 7 || typeInt == 9 || typeInt == 11 || typeInt == 13 || typeInt == 1 || typeInt == 15 || typeInt == 18 || typeInt == 20 || typeInt == 22 || typeInt == 24)
        {  //11选5 任一 至  任八
            if (iNum == 1)
            {//单注
                if (adddsinfo[0].Length != ds_iNum || !isInteger(adddsinfo))
                {
                    return false;
                }
                else
                {
                    if (!isRepeat(adddsinfo[0]))
                    {
                        return false;
                    }
                }
            }
            else
            {//多注
                for (var i = 0; i < adddsinfo.Length; i++)
                {
                    if (adddsinfo[i].Length != ds_iNum || !isInteger(adddsinfo[i]))
                    {
                        return false;
                    }
                    if (!isRepeat(adddsinfo[i]))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    int comb(int m, int n)
    {
        if (m < 0 || n < 0 || m < n)
        {
            return 0;
        }
        n = n < (m - n) ? n : m - n;
        if (n == 0)
        {
            return 1;
        }

        var result = m;

		for (int i = 2,j = result - 1; i <= n; i++,j--)
        {
            result = result * j / i; // 得到C(m,i)
        }

        return result;
    }

    //胆拖注数
    int getDTZs(int m,int n)
    {
        if (((m < 0) || (n < 0)) || (m < n))
        {
            return 0;
        }
        n = (n < (m - n)) ? n : (m - n);
        if (n == 0)
        {
            return 1;
        }
        var num = m;
        var num2 = 2;
        for (int i = num - 1; num2 <= n; i--)
        {
            num = (num * i) / num2;
            num2++;
        }
        return num;
    }


    bool isRepeat(string str)
    {
        var strtemp = "";
        var iNum = str.Length / 2;
     
        for (var i = 1; i <= iNum; i++)
        {
            if (i == 1)
                strtemp = str.Substring(i * 2 - 2, 2);
            else
                strtemp = strtemp + '|' + str.Substring(i * 2 - 2, 2);
        }
        var ary = strtemp.Split('|');

        for (var j = 0; j < ary.Length; j++)
        {
            if (int.Parse(ary[j]) > 11)
            {
                return false;
            }
            if (countInstances(strtemp, ary[j]) > 1)
            {
                return false;
            }
        }
        return true;
    }

 
    int countInstances(string mainStr,string subStr)
    {
        var count = 0; var offset = 0;
        do
        {
            offset = mainStr.IndexOf(subStr, offset);
            if (offset != -1)
            {
                count++;
                offset += subStr.Length;
            }
        } while (offset != -1);

        return count;
    }


    //添加选中号码
    void addNumbersPK10(int args, LotterySubModeCfg modeCfg, int bs, int unitpara)
    {

        LotterySubModeCfg seletedType = modeCfg;

        int beiShu = bs;
        int unit = unitpara; //1元 2角 3分
                             //var showMianE = " 元",

        var tzbs = "0";

        int zs = 0;
        string je = "";

        string rType = seletedType.type;
        var val = "";

        var selectedVal = seletedType.subModeId.ToString();


        if (rType == "select")
        {
            
            var selectedNumList = new Dictionary<int, List<string>> ();

            //var divs = detail.find('div.detail_c');
            var selectedCnt = 0;
            var canNotAll = (seletedType.cannotall == 1) ? true : false;
            var cnt = seletedType.cnt;
            //必须全部选择
            var divs = panelScript.scrollview_v.NumsItems;

            //必须全部选择
            for (var i = 0; i < divs.Count; i++)
            {
                var div = divs[i];
                var selecteds = div.SelectedNums;
                selectedNumList[i] = new List<string>();
                var exit = false;
                for (var j = 0; j < selecteds.Count; j++)
                {
                    var selected = selecteds[j];
                    
                    var selVal = int.Parse(selected);
                    if (selVal < 10)
                    {
                        val += "0" + selVal;
                        selectedNumList[i].Add("0" + selVal);
                    }
                    else
                    {
                        val += "" + selVal;
                        selectedNumList[i].Add(""+selVal);
                    }
                    exit = true;
                }
                if (i != divs.Count - 1)
                    val += ',';

                if (exit == false && canNotAll == false)
                {//存在没选，而且每行都要选
                    if (args == 2)  //2代表下单操作
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
                if (exit)
                    selectedCnt++;
            }

            if (canNotAll == true && cnt > -1 && selectedCnt != cnt)
            {
                if (args == 2)  //2代表下单操作
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                    return;
                }
                else
                {
                    clearPendingChange();
                    return;
                }
            }
            if (selectedVal == "2")
            {  //猜冠亚军
                var guan = selectedNumList[0];
                var ya = selectedNumList[1];
                if (guan.Count > 0 && ya.Count > 0)
                {
                    for (int i = 0; i < guan.Count; i++)
                    {
                        for (int j = 0; j < ya.Count; j++)
                        {
                            if (guan[i] != ya[j])
                            {
                                zs++;
                            }
                        }
                    }
                }
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "4")
            {  //猜前三名
                var guan = selectedNumList[0];
                var ya = selectedNumList[1];
                var ji = selectedNumList[2];
                if (guan.Count > 0 && ya.Count > 0 && ji.Count > 0)
                {
                    for (int i = 0; i < guan.Count; i++)
                    {
                        for (int j = 0; j < ya.Count; j++)
                        {
                            for (int k = 0; k < ji.Count; k++)
                            {
                                if (guan[i] != ya[j] && guan[i] != ji[k] && ya[j] != ji[k])
                                {
                                    zs++;
                                }
                            }
                        }
                    }
                }
                je = getJE(zs, beiShu, unit);
            }
            else if (selectedVal == "6" || selectedVal == "7")
            {
                zs = selectedNumList[0].Count + selectedNumList[1].Count + selectedNumList[2].Count + selectedNumList[3].Count + selectedNumList[4].Count;
                je = getJE(zs, beiShu, unit);
            }
            else
            {
                zs = selectedNumList[0].Count;
                je = getJE(zs, beiShu, unit);
            }

            if (args == 2)
            {
                ClearSelect();
            }
            if (val != "," || (cnt > 0 && selectedCnt == cnt))
            {//不需要每行选，但是一个都没选
                if (args == 2)
                {
                    if (zs == 0)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入", 2));
                        return;
                    }

                    ConfirmPanelObj cpObj = new ConfirmPanelObj();
                    cpObj.lCfg = lotteryCfg;
                    cpObj.subCfg = currentSubMode;

                    cpObj.showContent = (val.Length > valCutterIndex ? val.Substring(0, valCutterIndex) + "..." : val);

                    cpObj.contents = val;
                    cpObj.lid = activityID;
                    cpObj.zs = zs;
                    cpObj.bs = beiShu;
                    cpObj.model = unit;
                    cpObj.amount = je;
                    cpObj.tzbs = tzbs;

                    ShowConfirmSignal.Dispatch(cpObj);

                }
            }
        }
        else if (rType == "text")
        {
            
            var cnt = seletedType.cnt;
            var canRepeat = seletedType.canrepeat;
            var str = panelScript.textInput.value;
            if (string.IsNullOrEmpty(str))
            {
                if (args == 2)
                {
                    return;
                }
                else
                {
                    clearPendingChange();
                    return;
                }
            }
            //首先将非数字的作为分隔符分解字符串
            List<string> arrList = new List<string>();
            foreach (Match match in Regex.Matches(str, @"[\d]+"))
                arrList.Add(match.Value);
            var arr = arrList.ToArray();

            if (!isCheck_pk10(arr, cnt, (arr.Length > 1 ? 2 : 1)))
            {
                if (args == 2)
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                    return;
                }
                else
                {
                    clearPendingChange();
                    return;
                }
            }
            zs = arr.Length;
            je = getJE(zs, beiShu, unit);
            val = string.Join("$", arr);
            if (args == 2)
            {
                if (string.IsNullOrEmpty(val))
                {
                    MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                    return;
                }

                ConfirmPanelObj cpObj = new ConfirmPanelObj();
                cpObj.lCfg = lotteryCfg;
                cpObj.subCfg = currentSubMode;

                cpObj.showContent = (val.Length > valCutterIndex ? val.Substring(0, valCutterIndex) + "..." : val);

                cpObj.contents = val;
                cpObj.lid = activityID;
                cpObj.zs = zs;
                cpObj.bs = beiShu;
                cpObj.model = unit;
                cpObj.amount = je;
                cpObj.tzbs = tzbs;

                ShowConfirmSignal.Dispatch(cpObj);
            }
        }
        else if (rType == "daXiao" || rType == "dS")
        {
          
            var selectedNumList = new List<List<string>>();
            var divs = panelScript.scrollview_v.NumsItems;
            var selectedCnt = 0;
            var showText = "";

            for (var i = 0; i < divs.Count; i++)
            {
                var div = divs[i];
                var selecteds = div.SelectedNums;
                //倒序排序
                selecteds.Sort(Compare);

                selectedNumList.Add( new List<string>());
                var exit = false;
                for (var j = 0; j < selecteds.Count; j++)
                {
                    var selected = selecteds[j];
                  
                    var selVal = int.Parse(selected);
                    if ((rType == "daXiao" && (selVal == 9 || selVal == 1)) ||
                        (rType == "dS" && (selVal == 3 || selVal == 2))
                        )
                    {
                        if (selVal < 10)
                        {
                            val += "0" + selVal;
                            showText += selected;
                            selectedNumList[i].Add("0" + selVal);
                        }
                        else
                        {
                            val += "" + selVal;
                            showText += selected;
                            selectedNumList[i].Add(""+ selVal);
                        }
                    }
                    exit = true;
                }
                if (i != divs.Count - 1)
                    val += ',';

                if (exit == false)
                {
                    if (args == 2)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入",2));
                        return;
                    }
                    else
                    {
                        clearPendingChange();
                        return;
                    }
                }
                if (exit)
                    selectedCnt++;
            }

            zs = selectedNumList[0].Count;
            je = getJE(zs, beiShu, unit);

            //ClearSelect();

            if (val != "," )
            {//不需要每行选，但是一个都没选
                if (args == 2)
                {
                    ClearSelect();

                    if (zs == 0)
                    {
                        MsgSignal.Dispatch(new MsgPara("号码输入有误，请重新输入", 2));
                        return;
                    }

                    ConfirmPanelObj cpObj = new ConfirmPanelObj();
                    cpObj.lCfg = lotteryCfg;
                    cpObj.subCfg = currentSubMode;

                    cpObj.showContent = (val.Length > valCutterIndex ? val.Substring(0, valCutterIndex) + "..." : val);

                    cpObj.contents = val;
                    cpObj.lid = activityID;
                    cpObj.zs = zs;
                    cpObj.bs = beiShu;
                    cpObj.model = unit;
                    cpObj.amount = je;
                    cpObj.tzbs = tzbs;

                    ShowConfirmSignal.Dispatch(cpObj);
                }
            }
        }

        if (args == 2)
        {//添加到列表
            //caculateMoney(zs, je, "+");
            //    $("#txtBeiShu").val(1);
            //    $("#pbUnit").text("0");
            //    $("#pbMoney").text("0.00");
        }
        else
        {//预添加

            UpdateBotInfo(zs, je);
        }
    }

    bool isCheck_pk10(string[] adddsinfo, int ds_iNum, int iNum)
    {
        if (iNum == 1)
        {//单注
            if (adddsinfo[0].Length != ds_iNum || !isInteger(adddsinfo))
            {
                return false;
            }
            else
            {
                if (!isRepeat(adddsinfo[0]))
                {
                    return false;
                }
            }
        }
        else
        {//多注
            for (var i = 0; i < adddsinfo.Length; i++)
            {
                if (adddsinfo[i].Length != ds_iNum || !isInteger(adddsinfo[i]))
                {
                    return false;
                }
                if (!isRepeat(adddsinfo[i]))
                {
                    return false;
                }
            }
        }
        return true;
    }



    public int Compare(string x, string y)
    {
        if (int.Parse(x) > int.Parse(y))
            return -1;
        else
            return 1;
    }
}


