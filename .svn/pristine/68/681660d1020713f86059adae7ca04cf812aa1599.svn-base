
using System.Collections.Generic;

public class ConfirmPanelObj
{
    public LotteryConfig lCfg;                    
    public LotterySubModeCfg subCfg;

    public string showContent;  
                                
    public string contents;                //选中值
    public int lid;                       //当前期数
    public int zs;                             // 注数
    public int bs;                              // 倍数
    public int model;                          //  单位
    public string amount;                       // 金额
    public string  tzbs;                    //万位千位百位
}

public class ZhuihaoOrderObj
{
    public ConfirmPanelObj cfirmObj;
    public int bingoStop = 1;
    public int qishu;
    public int beishu;
    public double zhzje;

}

public class QihaoObj
{
    public int id;  //期号id
    public string date;  //期数日期
    public string code;    //期数code

}

public class RecordItemObj
{
    public string titleStr;
    public int iNUM1;
    public int iNUM2;
    public int iNUM3;
    public int iNUM4;
    public int iNUM5;
    public int iNUM6;
    public int iNUM7;
    public int iNUM8;
    public int iNUM9;
    public int iNUM10;
}

public class RecordObj
{
    public int lotteryId;
    //前面的int 代表type 目前支持4种： 1：重庆， 2：3D， 3：11选5，4：Pk10
    public int lotteryType;
    public List<RecordItemObj> recordItems;
    
}

public delegate void GoBuyDelegate(int lotteryId);

public delegate void NumClickDelegate();

public delegate void DantuoDelegate(int lInx ,string numStr);

public delegate void GongGaoFinishDelegate();

public delegate void UpdateBotDelegate();

public delegate void ShowFandian(uint id,string username,string fandian);
