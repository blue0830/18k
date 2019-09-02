using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LotteryModel : ILotteryModel
{
    //当前状态
    public LotteryConfig lotteryCfg { get; set; }
    public LotteryModeCfg currentMode { get; set; } //当前的模式
    public LotterySubModeCfg currentSubMode { get; set; } //当前的子模式
	//追期
	//奖金 返点
	SScPlayGetPointResult pointAward;
    //期号 时间
    #region 期号 时间
    MSG_GP_SSC_GETCUE activitycueInfo;
    MSG_GP_SAND_GETCUE activitycueInfo_2;
    MSG_GP_PK10_GETCUE activitycueInfo_pk10;

    public bool IscurLottery(int assId)
    {
        if (lotteryCfg==null || lotteryCfg.timeRspId!= assId)
            return false;
        return true;
    }

    public void SetQishuTime(MSG_GP_SSC_GETCUE acInfo)
    {
 
        activitycueInfo = acInfo;
        activitycueInfo_2 = null;
        activitycueInfo_pk10 = null;
    }

    public void SetQishuTime(MSG_GP_SAND_GETCUE acInfo)
    {
         
        activitycueInfo_2 = acInfo;
        activitycueInfo = null;
        activitycueInfo_pk10 = null;
    }

    public void SetQishuTime(MSG_GP_PK10_GETCUE acInfo)
    {
         
        activitycueInfo_pk10 = acInfo;
        activitycueInfo_2 = null;
        activitycueInfo = null;
    }

    public string GetQishuStr()
    {
        string str = "";
        if (activitycueInfo != null)
        {
            str = activitycueInfo.GetDateStr().Trim() + activitycueInfo.GetActivityStr().Trim();
        }
        else if(activitycueInfo_2!=null)
        {
            str = activitycueInfo_2.GetDateStr().Trim() + activitycueInfo_2.GetActivityStr().Trim();
        }
        else if (activitycueInfo_pk10 != null)
        {
            str = activitycueInfo_pk10.GetDateStr().Trim() + activitycueInfo_pk10.GetActivityStr().Trim();
        }

        str = str.Replace(" ", "");
        return str;
    }

    //当前期倒计时
    public int GetTimer(ref string labelStr)
    {
        int timer = 0;
        if (activitycueInfo != null)
        {
            if (activitycueInfo.iOrderTm > 0)
            {
                timer = activitycueInfo.iOrderTm;
                labelStr = "下单时间";
            }
            else if (activitycueInfo.iWaitTm > 0)
            {
                timer = activitycueInfo.iWaitTm;
                labelStr = "封单时间";
            }
           
        }
		//3D和p3 首先判断orderTm是不是大于0 如果大于就是下单时间，再判断NextBeginTm是不是大于0，如果大于0表示 距离---期 开始时间。  如果orderTm和NextBeginTm都是等于0，就表示封单时间。
        else if (activitycueInfo_2 != null)
        {
            if (activitycueInfo_2.iOrderTm > 0)
            {
                timer = activitycueInfo_2.iOrderTm;
                labelStr = "下单时间";
            }
            else if (activitycueInfo_2.iWaitTm > 0)
            {
                timer = activitycueInfo_2.iWaitTm;
                labelStr = "封单时间";
            }
            else
            {
                timer = activitycueInfo_2.iNextBeginTm;
                labelStr = "下单时间";
            }
        }
        else if (activitycueInfo_pk10 != null)
        {
            if (activitycueInfo_pk10.iOrderTm > 0)
            {
                timer = activitycueInfo_pk10.iOrderTm;
                labelStr = "下单时间";
            }
            else if (activitycueInfo_pk10.iWaitTm > 0)
            {
                timer = activitycueInfo_pk10.iWaitTm;
                labelStr = "封单时间";
            }
        }

        return timer;
    }
		
    public int GetActivityId()
    {
        if (activitycueInfo != null)
        {

            return activitycueInfo.iActivityID;
        }
        else if (activitycueInfo_2 != null)
        {

            return activitycueInfo_2.iActivityID;
        }
        else if (activitycueInfo_pk10 != null)
        {
            return activitycueInfo_pk10.iActivityID;
        }

        return 0;
    }
    #endregion

    public void SetAwardinfo(SScPlayGetPointResult ssr)
    {
        pointAward = ssr;
    }
		
    public SScPlayGetPointResult GetAwardinfo()
    {
        return pointAward;
    }
}
