using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class MSG_GP_USER_ResultRecord_Handler : IHandler<MSG_GP_USER_ResultRecord>
{
    [Inject]
    public IGameInfoModel model { get; set; }

    [Inject]
    public RecordBackSignal Signal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_USER_ResultRecord para)
    {
		if (para.byMainType != Global.AppQueryMainType || Global.AppQuerySubType != para.byZiType)
        {
			return;//子标识 主标识不符 丢弃
        }
        if (para.byCueData == 0||para.byCueData==para.byCountData)
        {
            model.SetRecordchData(para.byCueData, para.GetChData());
            RecordBackObj obj = new RecordBackObj();
            obj.byMainType = para.byMainType;
            obj. byZiType=para.byZiType;
            obj. byColumn=para.byColumn;    
            obj. byLine=para.byLine;      
            obj. iCountRecord=para.iCountRecord;
            obj.byPages = para.byPages ;
            obj.iCuePage = para.iCuePage;
            
            string chData = "";
            SortedList<int, string> sortedList = model.GetRecordchData();
            foreach (var item in sortedList)
            {
                chData += item.Value;
            }
            obj.chData = chData;
            model.recordBackObj = obj;
			model.ClearRecordBackSlist ();
            Signal.Dispatch(obj);
        }
        else
        {
            model.SetRecordchData(para.byCueData, para.GetChData());
        }
    }
}

