using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class LotteryConfig
{
    public int lotteryId;

    public string name;
    
	public int lotteryType;//目前支持4种： 1：重庆， 2：3D， 3：11选5，4：Pk10

    public string iconName;

    public int timeResId; //时间期数请求id

    public int timeRspId;  //时间期数返回id

    public int recordResId; //开奖记录请求id

    public int recordRspId; // 开奖记录返回Id

    public bool hot;
    
    [XmlElement]
    public List<LotteryModeCfg> modecfgs { get; set; }

    public LotteryModeCfg GetModeCfg(int mId)
    {
        if (modecfgs == null)
        {
            Debug.LogError("message sub configlist is null");
        }
        for (int i = 0; i < modecfgs.Count; ++i)
        {
            if (modecfgs[i].modeId == mId)
            {
                return modecfgs[i];
            }
        }

        return null;
    }
}

public class LotteryModeCfg
{
    public int modeId;
    public string name;

    [XmlElement]
    public List<LotterySubModeCfg> subModecfgs { get; set; }

    public LotterySubModeCfg GetSubModeCfg(int mId)
    {
        if (subModecfgs == null)
        {
            Debug.LogError("message sub configlist is null");
        }
        for (int i = 0; i < subModecfgs.Count; ++i)
        {
            if (subModecfgs[i].subModeId == mId)
            {
                return subModecfgs[i];
            }
        }

        return null;
    }
}

public class LotterySubModeCfg
{
    public uint subModeId;
    public string name;
    public string type;

    public string example;
    public string help;
    public string gold;

    public int cnt=-1;
    public int cannotall;
    public int canrepeat=-1;
    public int valtype;

    public bool isShowTwo;

    public int mincnt=-1;
    public int checkcombin=-1;

    [XmlElement] 
    public List<RowModecfg> rowModecfgs { get; set; }


}

public class RowModecfg
{
    public string name;
    public int numFrom;
    public int numTo;
    public bool option;
}

public class LotteryConfigLoader
{
    [XmlElement]
    public List<LotteryConfig> lotteryConfigs { get; set; }

    public LotteryConfig GetLotteryConfig(int Id)
    {
        if (lotteryConfigs == null)
        {
            Debug.LogError("message configlist is null");
        }
        for (int i = 0; i < lotteryConfigs.Count; ++i)
        {
            if (lotteryConfigs[i].lotteryId == Id)
            {
                return lotteryConfigs[i];
            }
        }

        return null;
    }
		
    public int GetLIdByRecordRspId(int RecordrspId)
    {
        if (lotteryConfigs == null)
        {
            Debug.LogError("message configlist is null");
        }
        for (int i = 0; i < lotteryConfigs.Count; ++i)
        {
            if (lotteryConfigs[i].recordRspId == RecordrspId)
            {
                return lotteryConfigs[i].lotteryId;
            }
        }

        return -1;
    }
}