using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class ASS_GP_PK10GETRECORD_Handler : IHandler<MSG_GP_PK10_LASTFIVEOPENNUM>
{
    [Inject]
    public ILotteryModel model { get; set; }

    [Inject]
    public LoRecordSignal lrSignal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_PK10_LASTFIVEOPENNUM para)
    {

        Debug.Log("assID" + head.bAssistantID + " ASS_GP_PK10GETRECORD_Handler ");


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
            obj.iNUM1 = para.LastFiveNum[i].iNUM1;
            obj.iNUM2 = para.LastFiveNum[i].iNUM2;
            obj.iNUM3 = para.LastFiveNum[i].iNUM3;
            obj.iNUM4 = para.LastFiveNum[i].iNUM4;
            obj.iNUM5 = para.LastFiveNum[i].iNUM5;
            obj.iNUM6 = para.LastFiveNum[i].iNUM6 ;
            obj.iNUM7 = para.LastFiveNum[i].iNUM7 ;
            obj.iNUM8 = para.LastFiveNum[i].iNUM8 ;
            obj.iNUM9 = para.LastFiveNum[i].iNUM9 ;
            obj.iNUM10 =para.LastFiveNum[i].iNUM10;


            list.Add(obj);
        }

        RecordObj reobj = new RecordObj();

        reobj.lotteryType = 4;
        reobj.recordItems = list;

        int id = -1;
        LotteryConfigLoader lloader = ConfigManager.Instance.GetLotteryCfgLoader();
        if (lloader != null)
            id = lloader.GetLIdByRecordRspId((int)head.bAssistantID);
        reobj.lotteryId = id;   
        lrSignal.Dispatch(reobj);


    }
}

