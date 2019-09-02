using UnityEngine;
using System.Collections;

public interface ILotteryModel
{
    LotteryConfig lotteryCfg { get; set; }
    LotteryModeCfg currentMode { get; set; } //当前的模式
    LotterySubModeCfg currentSubMode { get; set; } //当前的子模式


    void SetQishuTime(MSG_GP_SSC_GETCUE acInfo);
    void SetQishuTime(MSG_GP_SAND_GETCUE acInfo);
    void SetQishuTime(MSG_GP_PK10_GETCUE acInfo);
    bool IscurLottery(int assId);

    string GetQishuStr();
    //当前期倒计时
    int GetTimer(ref string labelStr);
    //void SetTimer(int timer, string labelStr);

    int GetActivityId();

    void SetAwardinfo(SScPlayGetPointResult ssr);

    SScPlayGetPointResult GetAwardinfo( );

}
