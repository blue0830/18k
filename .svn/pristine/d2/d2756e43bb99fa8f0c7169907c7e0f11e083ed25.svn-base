using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SScGetPlayNoteResult_Handler : IHandler<SScGetPlayNoteResult>
{
    [Inject]
    public IGameInfoModel model { get; set; }

    [Inject]
    public TouzhuXiangxiSignal GSignal { get; set; }

    public void OnReceive(NetMessageHead head, SScGetPlayNoteResult para)
    {

		if (para.byCueSend==0||para.byCueSend==para.byAllSend)
        {
            model.Setordervalue(para.byCueSend, para.GetOrderValue());

            TouzhuXiangxi obj = new TouzhuXiangxi();

			obj.UserName = Global.CurrentUserName;    //玩家账号
            obj.OrderCount = para.OrderCount;     //投注数量
            obj.iNoteID = para.iNoteID;        //订单号


			double d = para.SingleMoney *1.0/100;
			obj.SingleMoney = String.Format("¥{0:0.00}", d);    //单注金额
            obj.Multiple = para.Multiple;       //倍数

            obj.ActivityName = para.GetActivityName(); //期号

			d = para.Amount *1.0/100;
			obj.Amount = String.Format("¥{0:0.00}", d);         //投注总额

            obj.ClassName = para.GetClassName();   //彩种
            obj.BingoCount = para.BingoCount;       //中奖注数

            obj.OrderTypeName = para.GetOrderTypeName(); //玩法
			d = para.PeiLv *1.0/100;
			obj.PeiLv = String.Format("¥{0:0.00}", d);             //单注中奖金额

            obj.OpenNum = para.GetOpenNum();        //开奖号码
			d = para.BingoMoney *1.0/100;
			obj.BingoMoney = String.Format("¥{0:0.00}", d);        //中奖金额
            obj.AddTM = para.AddTM;    //下单时间


			obj.Point = String.Format("{0:0.00}%", para.GetPoint());             //返点
			d = para.PointMoney *1.0/100;
			obj.PointMoney = String.Format("¥{0:0.00}", d);       //返点金额
            obj.IsBingo = para.IsBingo;          //状态
			d = para.ResultMoney *1.0/100;
			obj.ResultMoney = String.Format("¥{0:0.00}", d);      //盈亏


            string orderValue = "";
            SortedList<int, string> sortedList = model.GetOrderValue();
            foreach (var item in sortedList)
            {
                orderValue += item.Value;
            }
            obj.OrderValue = orderValue;

            model.xiangxiObj = obj;
			model.ClearOrderValueList ();
            GSignal.Dispatch();
        }
        else
        {
            model.Setordervalue(para.byCueSend, para.GetOrderValue());
        }

    }
}

