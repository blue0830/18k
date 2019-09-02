using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class ASS_GP_3DGETRECORD_Handler : IHandler<MSG_GP_SD_LASTFIVEOPENNUM>
{
    [Inject]
    public ILotteryModel model { get; set; }

    [Inject]
    public LoRecordSignal lrSignal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_SD_LASTFIVEOPENNUM para)
    {

        Debug.Log("assID" + head.bAssistantID + " ASS_GP_3DGETRECORD_Handler ");


        List<RecordItemObj> list = new List<RecordItemObj>();

        for (int i = 0; i < para.LastFiveNum.Length; ++i)
        {
            string title = para.LastFiveNum[i].GetRecordTitle();
            if (string.IsNullOrEmpty(title))
            {
                continue;
            }
            RecordItemObj obj = new RecordItemObj();
            obj.titleStr = title;

            obj.iNUM1 = para.LastFiveNum[i].iBaiWei;
            obj.iNUM2 = para.LastFiveNum[i].iShiWei;
            obj.iNUM3 = para.LastFiveNum[i].iGeWei;


            list.Add(obj);
        }

        RecordObj reobj = new RecordObj();

        reobj.lotteryType = 2;
        reobj.recordItems = list;

        int id = -1;
        LotteryConfigLoader lloader = ConfigManager.Instance.GetLotteryCfgLoader();
        if (lloader != null)
            id = lloader.GetLIdByRecordRspId((int)head.bAssistantID);
        reobj.lotteryId = id;   
        lrSignal.Dispatch(reobj);


    }
}

