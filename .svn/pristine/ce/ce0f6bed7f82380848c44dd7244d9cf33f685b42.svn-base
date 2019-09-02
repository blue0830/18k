using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Security.Cryptography;

public class Lopackage
{
    //结构体序列化     
    public NetMessageHead head;
    public byte[] objectData; 
}

public class Sendpackage
{
    //结构体序列化     
    public uint msgId;
    public uint assId;
    public object msgPackge;

}

//网络数据包结构头
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class NetMessageHead
{
    public uint uMessageSize;                     //数据包大小
    public uint bMainID;                          //处理主类型
    public uint bAssistantID;                     //辅助处理类型 ID
    public uint bHandleCode;                      //数据包处理代码
    public uint bReserve;                         //保留字段
};

//用户登陆（帐号）结构
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_S_LogonByNameStruct
{
    public uint uRoomVer;                          //大厅版本
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 61, ArraySubType = UnmanagedType.Struct)]
    private byte[] szName=new byte[61];                            //登陆名字
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.Struct)]
    private byte[] TML_SN = new byte[128];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
    private byte[] szMD5Pass = new byte[50];                     //登陆密码
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.Struct)]
    private byte[] szMathineCode=new byte[64];                //本机机器码 zxj 2009-11-10 锁定机器
    public int gsqPs;

    public byte[] SzName
    {
        get
        {
            return szName;
        }
    }

    public void SetSzName(string str)
    {
        szName = Util.StrToFixLenByte(str, szName);
    }

    public byte[] TML_SN1
    {
        get
        {
            return TML_SN;
        }
    }

    public byte[] SzMD5Pass
    {
        get
        {
            return szMD5Pass;
        }
    }

    public void SetSzMD5Pass(string strMd5)
    {
        Util.StrToFixLenByte(strMd5, szMD5Pass);
    }


    public byte[] SzMathineCode
    {
        get
        {
            return szMathineCode;
        }
    }
};

//大厅登陆返回数据包
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_R_LogonResult
{
    public int dwUserID;//用户ID 
	public int dwGamePower;//游戏权限
	public int dwMasterPower;//管理权限
	public int dwMobile;//手机号码
	public int dwAccID;//Acc 号码
	public uint dwLastLogonIP;//上次登陆IP
	public uint dwNowLogonIP;//现在登陆IP
	public uint bLogoID;//用户头像
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 61, ArraySubType = UnmanagedType.Struct)]
    byte[] szName = new byte[61];//用户登录名
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.Struct)]
    byte[] TML_SN = new byte[128];//数字签名
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
    byte[] szMD5Pass = new byte[50];//用户密码
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.Struct)]
    byte[] nickName = new byte[32];//用户昵称
    public int dwMoney;//用户金币
    public int dwBank;//用户财富
	public int dwFascination;//魅力
	public int dwTimeIsMoney;//上次登陆时长所换取的金币
	public int iVipTime;
	public int iDoublePointTime;//双倍积分时间
	public int iProtectTime;//护身符时间，保留
	public byte bLoginBulletin;//是否有登录公告，Fred Huang,2008-05-20
	public int iLockMathine;//当前帐号是否锁定了某台机器，为锁定，为未锁定zxj 2009-11-13
	public int iUserType;//玩家类型，表示普通玩家，表示代理
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
    byte[] point = new byte[8];
	public int SpareValue;//棋牌返点
	public int IsSafe;//是否设置密保
	public ulong tCueLogoTm;//本次登录时间
	public ulong tLastLogoTm;//最后登录的时间
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 15, ArraySubType = UnmanagedType.Struct)]
    char[] chSerialNo = new char[15];//版本
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 15, ArraySubType = UnmanagedType.Struct)]
    char[] chGrQQ = new char[15];

	public string GetNickName()
	{
		return Util.BytetoString(nickName);
	}

    public string GetUserName()
    {
        return Encoding.UTF8.GetString(szName);
    }

	public void SetNickName(string name)
	{
		nickName = Util.StringtoFixedByte_GB2312(name, nickName);
	}

	public double GetPoint()
	{
		return BitConverter.ToDouble(point, 0);
	}

};

//玩家下注的数据
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_SSC_PLAYNOTE
{
    uint dwUserID;                     //玩家ID
    int iClassId;                     //彩种ID
    int iActivityID;                  //期号ID
    int iOrderType;                   //玩法类型ID
    byte byCueSend;                    //当前发送的次数
    byte byAllSend;                    //一共要发送的次数
    int iSingleMoney;                 //单注金额
    int iMultiple;                    //倍数
    int iAmount;                      //总的金额
    int iCountDataSize;               //投注的数据包的大小
    int iCountNote;                   //注数
    int iCountAllChar;                //总的字符串数
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
    char[] chRx = new char[10];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3001, ArraySubType = UnmanagedType.Struct)]
    char[] OrderValue= new char[3001];             //投注的内容
};

//玩家批量下单
//玩家下注的数据
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_SSC_PLAYBATCHNOTE
{
    public uint dwUserID;                     //玩家ID
    public int iClassId;                     //彩种ID
    public int iActivityID;                  //期号ID
    public int iAmountTotal;                 //总金额
    public int iCountDataSize;               //投注的数据包的大小
    public int iOrderCount;                  //订单笔数
    public byte byCueSend;                    //当前发送的次数
    public byte byAllSend;                    //一共要发送的次数
    public int iCountAllChar;                //总的字符串数
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3000, ArraySubType = UnmanagedType.Struct)]
    byte[] chOrderValue=new byte[3000];            //下单内容

    public void SetOrderValue(byte[] charAr)
    {
        Array.Copy(charAr, 0, chOrderValue, 0, charAr.Length);
    }
};

///以下是时时彩数据,3d,p3
//获取当前时间期号信息，如投注时间、等待时间
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_SSC_GETCUE
{
    public int iActivityID;                  //期号ID
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.Struct)]
    char[] chTitle = new char[11];                 //当前投注日期
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
    char[] chTitleConde = new char[10];             //投注期号
    public int iOrderTm;                    //投注时间
    public int iWaitTm;                     //封单时间

    public string GetDateStr()
    {
        return Util.FixLenChartoString(chTitle);
    }
    public string GetActivityStr()
    {
        return Util.FixLenChartoString(chTitleConde);
    }

};

[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_SAND_GETCUE
{
    public int iActivityID;                  //期号ID
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.Struct)]
    char[] chTitle = new char[11];                 //当前投注日期
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
    char[] chTitleConde = new char[10];             //投注期号
    public int iOrderTm;                    //投注时间
    public int iWaitTm;                     //封单时间
    public int iNextBeginTm;

    public string GetDateStr()
    {
        return Util.FixLenChartoString(chTitle);
    }
    public string GetActivityStr()
    {
        return Util.FixLenChartoString(chTitleConde);
    }
};

[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_PK10_GETCUE
{
    public int iActivityID;                  //期号ID
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.Struct)]
    char[] chTitle = new char[11];                 //当前投注日期
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 21, ArraySubType = UnmanagedType.Struct)]
    char[] chTitleConde = new char[21];             //投注期号
    public int iOrderTm;                    //投注时间
    public int iWaitTm;                     //封单时间
    public int iOpenWaitTM;

    public string GetDateStr()
    {
        return Util.FixLenChartoString(chTitle);
    }
    public string GetActivityStr()
    {
        return Util.FixLenChartoString(chTitleConde);
    }

};

//获取玩家的奖金和返点结果
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class SScPlayGetPointResult
{
    int iClassID;
    int iOrderType;
    public int iPeiLv;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
    byte[] fPoint=new byte[8];
    public int iMaxPeiLv;
    public int iMinPeiLv;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
    byte[] fMaxPoint = new byte[8];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
    byte[] fPeiLvChangeUnit = new byte[8];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
    byte[] fPointChangeUnit = new byte[8];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
    public int[] iAllPeiLv=new int[6];  //特殊玩家的奖金
 
    public double Getfpoint()
    {
        return BitConverter.ToDouble(fPoint, 0);
    }
    public double GetfMaxPoint()
    {
        return BitConverter.ToDouble(fMaxPoint, 0);
    }
    public double GetfPeiLvChangeUnit()
    {
        return BitConverter.ToDouble(fPeiLvChangeUnit, 0);
    }
    public void SetfPeiLvChangeUnit(double unit)
    {
        fPeiLvChangeUnit = BitConverter.GetBytes(unit);
    }
    public double GetfPointChangeUnit()
    {
        return BitConverter.ToDouble(fPointChangeUnit, 0);
    }
};

[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class SScPlayGetPeilvPoint
{
    public uint dwUserID;
    public int iClassID;
    public int iOrderType;
};

[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class SScSetPlayPoint
{
    public uint dwUserID;
    public int iClassID;
    public int iOrderType;
    public int iPeiLv;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
    byte[] dPoint = new byte[8];

    public void SetDp(double pt)
    {
        byte[] temp =  BitConverter.GetBytes(pt);
        Array.Copy(temp, 0, dPoint, 0, temp.Length);
    }
};

[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_UER_GETBACKInfo
{
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.Struct)]
    byte[] chDescribe = new byte[256];                         //取款描述
    public int byReturn;                                //返回值
    public int iOutMoney;                              //返回扣取的实际金额
    public byte byGetTypeBack;                           //返回的类型标志是哪个返回

    public string GetDesc()
    {
		return Util.BytetoString(chDescribe);
    }
};


//通过彩种获取剩余期数
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_GETLASTQISHU
{
    public int iClassID;
};

//追好获取剩余期数结果dddhh
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_GETLASTQISHURESULT
{
    int iCountQiShu;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 101, ArraySubType = UnmanagedType.Struct)]
    int[] iID=new int[101];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 1111, ArraySubType = UnmanagedType.Struct)]
    char[] chTitle=new char[1111];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 1111, ArraySubType = UnmanagedType.Struct)]
    char[] chTitleCode = new char[1111];

    public List<QihaoObj> GetQISHUList()
    {
        List<QihaoObj> tempList = new List<QihaoObj>();
        for (int i = 0; i < iCountQiShu; ++i)
        {
            QihaoObj obj = new QihaoObj();
            obj.id = iID[i];

            char[] array = new char[11];
			for (int j = i*11,end = i*11+11,index =0; j<chTitle.Length&&j < end; ++j,++index)
			{
				array[index] = chTitle[j];
			}
			obj.date = Util.FixLenChartoString(array);

			array = new char[11];
			for (int j = i*11,end = i*11+11,index =0; j<chTitle.Length&&j < end; ++j,++index)
			{
				array[index] = chTitleCode[j];
			}
			obj.code = Util.FixLenChartoString(array);

            tempList.Add(obj);
        }
        return tempList;
    }
};

//追号投注的数据
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_ZHNOTEVALUE
{
    public uint dwUserID;
    public int iClassID;    //彩种ID
    public int iActCount;   //追号期数
    public int iOrderType;  //玩法类型ID
    public int iSingleMoney; //单注金额
    public int Amount;        //投注总金额
    public int iOrderCount;   //注数
    public int BingoIsStop;   //中奖后是否停止：0-不停止，1-停止
    public int iCountDataSize;               //投注的数据包的大小
    public byte byCueSend;                    //当前发送的次数
    public byte byAllSend;                    //一共要发送的次数
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
    byte[] chRx=new byte[10];    //
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3000, ArraySubType = UnmanagedType.Struct)]
    byte[] chOrderValue=new byte[3000]; //投注内容

    public void SetChaRx(string str)
    {
        chRx = Util.StrToFixLenByte(str, chRx);
    }

    public void SetOrderValue(byte[] charAr)
    {
        Array.Copy(charAr, 0, chOrderValue, 0, charAr.Length); ;
    }
};

//追号数据
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_ZHUIHAOQISHU
{
    public int iSLID;
    public uint dwUserID;
    public int iActCount;   //追号期数
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 300, ArraySubType = UnmanagedType.Struct)]
    int[] iActivityIDs = new int[300];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 1000, ArraySubType = UnmanagedType.Struct)]
    byte[] chMultiples=new byte[1000];

    public void SetActivityIds(List<int> ids)
    {
        int[] idarr = ids.ToArray();
        Array.Copy(idarr, 0, iActivityIDs, 0, idarr.Length);
    }

    public void SetchMultiples(string str)
    {
        chMultiples= Util.StrToFixLenByte(str, chMultiples);
    }
};
//====================================================================================================================

//获取最近一期的开奖信息
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct MSG_GP_SSC_LASTOPNENUM
{
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.Struct)]
    char[] chTitle;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
    char[] chTitleCode;
    public int iWanWei;
    public int iQianWei;
    public int iBaiWei;
    public int iShiWei;
    public int iGeWei;

    public string GetRecordTitle()
    {
        string str = Util.FixLenChartoString(chTitle).Trim() + Util.FixLenChartoString(chTitleCode).Trim();

        return str.Trim();
    }
};



//PK10最近一期的开奖信息
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct MSG_GP_SYXW_LASTOPENNUM
{
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.Struct)]
    char[] chTitle ;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
    char[] chTitleCode ;
    public int iWanWei;
    public int iQianWei;
    public int iBaiWei;
    public int iShiWei;
    public int iGeWei;

    public string GetRecordTitle()
    {
        string str = Util.FixLenChartoString(chTitle).Trim() + Util.FixLenChartoString(chTitleCode).Trim();

        return str.Trim();
    }

};

//获取最近一期的开奖信息
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct MSG_GP_PK10_LASTOPNENUM
{
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.Struct)]
    char[] chTitle;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.Struct)]
    char[] chTitleCode;
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

    public string GetRecordTitle()
    {
        string str = Util.FixLenChartoString(chTitle).Trim() + Util.FixLenChartoString(chTitleCode).Trim();

        return str.Trim();
    }
};




//3D一期的开奖数据
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct MSG_GP_SD_OPENNUM
{
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.Struct)]
    char[] chTitle;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
    char[] chTitleCode;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.Struct)]
    char[] chTitle2;
    public int iBaiWei;
    public int iShiWei;
    public int iGeWei;

    public string GetRecordTitle()
    {
        string str = Util.FixLenChartoString(chTitle).Trim() + Util.FixLenChartoString(chTitleCode).Trim();

        return str.Trim();
    }
};

//最近五期的开奖信息
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_SSC_LASTFIVEOPNENUM
{
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 30, ArraySubType = UnmanagedType.Struct)]
    public MSG_GP_SSC_LASTOPNENUM[] LastFiveNum= new MSG_GP_SSC_LASTOPNENUM[30];
  
};

//3d
//最近五期的开奖信息
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_SD_LASTFIVEOPENNUM
{
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 30, ArraySubType = UnmanagedType.Struct)]
    public MSG_GP_SD_OPENNUM[] LastFiveNum = new MSG_GP_SD_OPENNUM[30];
};




//十一选五最近五期的开奖信息
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_SYXW_LASTFIVEOPENNUM
{
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 30, ArraySubType = UnmanagedType.Struct)]
    public MSG_GP_SYXW_LASTOPENNUM[] LastFiveNum = new MSG_GP_SYXW_LASTOPENNUM[30];
};


//PK10最近30期的开奖信息
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_PK10_LASTFIVEOPENNUM
{
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 30, ArraySubType = UnmanagedType.Struct)]
    public MSG_GP_PK10_LASTOPNENUM[] LastFiveNum = new MSG_GP_PK10_LASTOPNENUM[30];
};


//公告
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_GongGao
{
    uint iCountData;
    public byte byCueGg;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 800, ArraySubType = UnmanagedType.Struct)]
    byte[] chGg=new byte[800];

    public string GetchGg()
    {
		return Util.BytetoString(chGg);
    }
    
};

//盈亏推送
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class SScGetPlayYingKuiRecord
{
    public int iCountNote;          //一共有多少投注记录 
	public int iResultMoney;       //盈亏的金额
    int iIsWin;
    public uint udwUserID;          //玩家ID
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 21, ArraySubType = UnmanagedType.Struct)]
	byte[] className=new byte[21];       //彩种
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 21, ArraySubType = UnmanagedType.Struct)]
    char[] activityName=new char[21];    //期号
    
	public string GetClassName()
	{
		return Util.BytetoString(className);
	}

	public string GetActivityName()
	{
		string str = Util.FixLenChartoString(activityName).Trim();
		return str.Trim();
	}
};

//充值和取款提示
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class SScPlayCZorQKCue
{
	public uint dwUserID;
    public ulong AddTime;
	public int iDataSize;
	public int iCzOrQk;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 1024, ArraySubType = UnmanagedType.Struct)]
	byte[] chTipInfo=new byte[1024];
     
	public string GetChTipInfo()
	{
		return Util.BytetoString(chTipInfo);
	}
};

#region
//zjl 2016-01-10  start
//个人资金请求结构体
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MAG_GP_USER_GETWDZHGRJBXX
{
	public uint udwUserID;
	public byte bIsTime;
	public ulong tStartTime;
	public ulong tEndTime;
};


//个人资金返回结构体
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_GETBACKWDZHJBXX
{
    public int iAmount;
    public int iBingo;
    public int iPoint;
    public int iRecharge;
    public int iExchange;
    public int ixjPoint;
    public int iPlayAd;
    public int iPlayQpYk;
    public int iPlayQpFanDian;
};

//我的账号里团队信息获取
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MAG_GP_USER_GETTEAMJBXX
{
	public uint  udwUserID;
    public byte bIsTime;
    public ulong sStartTime;
    public ulong tEndTime;
};

//团队基本信息返回
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_TEAM_GETBACKJBXX
{
	public long     iteamTdYeZ;               //团队棋牌余额
	public long     iteamTdqpye;               //团队彩票余额
	public int         iteamJrXzc;                //今日新注册
	public int         iteamJrXcz;                //今日新充值
	public long     itempTdCz;                //团队充值
	public int         iteamTdrs;                //团队人数
	public int         itempZxhy;                //在线会员
	public long     iteamTdqk;                //团队取款
	public long     iteamCptzze;              //彩票投注总额
	public long     iteamCpzjze;              //彩票中奖总额
	public long     iteamtdcpfd;              //团队彩票返点
	public long     iwdcpfd;                  //我的彩票返点
	public long     iwdcpcdze;                //我的撤单总额
	public long     iteamtdcpyk;              //团队彩票盈亏
	public long     iteamqpxml;               //棋牌洗码量（赢）
	public long     iteamtdqpyk;               //团队棋牌盈亏
	public long     iteamtdbrqpfd;             //团队棋牌返点（百人）
	public long     iteamtddzqpfd;             //团队棋牌返点（对战）
	public long     iwdqpfd;                   //我的棋牌返点
	public long     iteamtdyjze;               //团队佣金总额

};

//获取个人信息返回结果,银行卡等信息
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_GetUserInfoBack
{
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 51, ArraySubType = UnmanagedType.Struct)]
	byte[] backName = new byte[51];          //开户行
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 51, ArraySubType = UnmanagedType.Struct)]
	byte[]  trueName = new byte[51];          //开户名
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 101, ArraySubType = UnmanagedType.Struct)]
    char[]  backAccount = new char[101];      //银行账号
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 51, ArraySubType = UnmanagedType.Struct)]
	byte[]  backAddress = new byte[51];     //银行地址
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 101, ArraySubType = UnmanagedType.Struct)]
    char[]  alipayAccount = new char[101];   //支付宝
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 101, ArraySubType = UnmanagedType.Struct)]
    char[]  tenpayAccount = new char[101];  //财富通
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
    char[]  agencyQQNum = new char[50];     //上级QQ
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
    char[]  QQNum = new char[50];           //个人QQ
	public byte  isLockBank;        //是否锁定
	//chQkBackInfo[30][20]
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 600, ArraySubType = UnmanagedType.Struct)]
	byte[]   chQkBackInfo= new byte[600];   //可以设置的取款银行
	byte  byCountQkBank;         //可以设置多少张取款银行

	public string GetBackName()
	{
		return Util.BytetoString(backName);
	}

	public string GetBackAccount()
	{
		string str = Util.FixLenChartoString(backAccount).Trim();
		return str.Trim();
	}

	public string GetTrueName()
	{
		return Util.BytetoString(trueName);
	}

	public string GetBackAddress()
	{
		return Util.BytetoString(backAddress);
	}

	public string GetAlipayAccount()
	{
		string str = Util.FixLenChartoString(alipayAccount).Trim();
		return str.Trim();
	}

	public string GetTenpayAccount()
	{
		string str = Util.FixLenChartoString(tenpayAccount).Trim();
		return str.Trim();
	}

	public string GetAgencyQQNum()
	{
		string str = Util.FixLenChartoString(agencyQQNum).Trim();
		return str.Trim();
	}

	public string GetQQNum()
	{
		string str = Util.FixLenChartoString(QQNum).Trim();
		return str.Trim();
	}

	public void SetQQNum(string qqNum)
	{
		QQNum = Util.StrToFixLenChar (qqNum,QQNum);
	}

	public List<string> GetChQkBackInfos(){
		List<string> list = new List<string> ();
		for(var i =0;i<30;i++){
			byte [] bArray = new byte[20];
			Array.Copy(chQkBackInfo,i * 20 , bArray, 0, 20);
			list.Add (Util.BytetoString (bArray));
		}
		return list;
	}
};

//修改银行信息或者修改并且锁定
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_ChangeBakeInfoOrLock
{
	public uint  dwUserId;           // 需要修改的玩家id
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
	byte [] szMD5OldPass=new byte[50];        //设置取款账户填写的密码
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 51, ArraySubType = UnmanagedType.Struct)]
	byte [] BackName=new byte[51];          //开户行
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 51, ArraySubType = UnmanagedType.Struct)]
	byte [] TrueName=new byte[51];          //开户名
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 101, ArraySubType = UnmanagedType.Struct)]
	byte [] backAccount=new byte[101];      //银行账号
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 51, ArraySubType = UnmanagedType.Struct)]
	byte [] backAddress=new byte[51];     //银行地址
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 101, ArraySubType = UnmanagedType.Struct)]
	byte [] AlipayAccount=new byte[101];   //支付宝
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 101, ArraySubType = UnmanagedType.Struct)]
	byte [] TenpayAccount=new byte[101];  //财富通
	byte  IsLock=1;             //是否锁定

    public void SetszMD5OldPass(string str)
    {
        string passMd5 = Util.GetMd5Hash(str);
		szMD5OldPass = Util.StringtoFixedByte_GB2312(passMd5, szMD5OldPass);
    }

    public void SetBackName(string str)
    {
		BackName = Util.StringtoFixedByte_GB2312(str, BackName);
    }
    public void SetTrueName(string str)
    {
		TrueName = Util.StringtoFixedByte_GB2312(str, TrueName);
    }
    public void SetbackAccount(string str)
    {
		backAccount = Util.StringtoFixedByte_GB2312(str, backAccount);
    }
    public void SetbackAddress(string str)
    {
		backAddress = Util.StringtoFixedByte_GB2312(str, backAddress);
    }
    public void SetAlipayAccount(string str)
    {
		AlipayAccount = Util.StringtoFixedByte_GB2312(str, AlipayAccount);
    }
    public void SetTenpayAccount(string str)
    {
		TenpayAccount = Util.StringtoFixedByte_GB2312(str, TenpayAccount);
    }
}

//修改银行信息返回结果
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class  MSG_GP_USER_ChangeBakeInfoOrLockResult
{
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.Struct)]
	byte []chData=new byte[256];
	int  iInMoney;
	public byte  bIsSuc;

	public string GetChData()
	{
		Debug.LogError (Util.BytetoString(chData));
		return Util.BytetoString(chData);
	}
};

//密码保护问题设置
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class  MSG_GP_USER_ZHGLSETMB
{
	public uint  dwUserID;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
    char []  Md5twoPass=new char[50];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.Struct)]
	byte [] chMb1=new byte[60];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.Struct)]
	byte [] chda1=new byte[60];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.Struct)]
	byte [] chMb2=new byte[60];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.Struct)]
	byte [] chda2=new byte[60];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.Struct)]
	byte [] chMb3=new byte[60];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 60, ArraySubType = UnmanagedType.Struct)]
	byte [] chda3=new byte[60];

    public void SetMd5twoPass(string str)
    {
        string passMd5 = Util.GetMd5Hash(str);
        Md5twoPass = Util.StrToFixLenChar(passMd5, Md5twoPass);
    }

    public void SetQA(string[] q, string[] a)
    {
		chMb1 = Util.StringtoFixedByte_GB2312(q[0], chMb1);
		chda1 = Util.StringtoFixedByte_GB2312(a[0], chda1);

		chMb2 = Util.StringtoFixedByte_GB2312(q[1], chMb2);
		chda2 = Util.StringtoFixedByte_GB2312(a[1],chda2);

		chMb3 = Util.StringtoFixedByte_GB2312(q[2], chMb3);
		chda3 = Util.StringtoFixedByte_GB2312(a[2], chda3);
    }
};

//修改密码
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class  MSG_GP_USER_changePassWord
{
	public uint    dwUserID;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
	byte [] szMD5OldPass=new byte[50];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
	byte [] szMD5NewPass=new byte[50];	
	public int    isChangeType;                               //修改的是登陆密码还是取款密码

    public void SetOldPass(string str)
    {
        string passMd5 = Util.GetMd5Hash(str);
		szMD5OldPass = Util.StrToFixLenByte(passMd5, szMD5OldPass);
    }
    public void SetNewPass(string str)
    {
        string passMd5 = Util.GetMd5Hash(str);
		szMD5NewPass = Util.StrToFixLenByte(passMd5, szMD5NewPass);
    }

    public void SetQQ(string qq)
    {
		szMD5OldPass = Util.StrToFixLenByte(qq, szMD5OldPass);
    }

    public void SetNickname(string name)
    {
		szMD5NewPass = Util.StringtoFixedByte_GB2312(name, szMD5NewPass);
    }
};


//修改密码信息返回结果
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class  MSG_GP_USER_ChangeUserPassWordResult
{
	public byte  byCangeType;                         //修改类型
	public byte  bIsSuc;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 15, ArraySubType = UnmanagedType.Struct)]
    char []  chGrQQ=new char[15];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
	byte []  chNickName=new byte[50];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.Struct)]
	byte [] chData=new byte[256];

	public string GetChGrQQ()
	{
		string str = Util.FixLenChartoString(chGrQQ).Trim();
		return str.Trim();
	}

	public bool GetBIsSuc()
	{
		return bIsSuc != 0;
	}

	public string GetChNickName()
	{
		return Util.BytetoString(chNickName);
	}

	public string GetChData()
	{
		return Util.BytetoString (chData);
	}
}

//玩家获得充值的银行卡信息
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class  MSG_GP_USER_GetCZbankInfo
{
	public byte isEnd;                               //银行卡资料信息是否已经结束
	public byte byCountBank;                         //一共有多少张银行卡的资料
	public byte byCueBank;                           //发送的是第几张银行卡资料
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
	byte []  TrueName=new byte[50];                        //开户名
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
	byte []  BankName=new byte[50];                        //开户银行
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 100, ArraySubType = UnmanagedType.Struct)]
    char []  BankAccount=new char[100];                    //银行卡账号
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 100, ArraySubType = UnmanagedType.Struct)]
    char []  BankUrl=new char[100];                        //银行卡链接地址  19 20 直接加 username
	public int iShowBankType;                       //银行卡显示的图片 19 20 隐藏下面
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 600, ArraySubType = UnmanagedType.Struct)]
	byte []  Remark=new byte[600];                         //描述数据

	public string GetTrueName()
	{
		return Util.BytetoString (TrueName);
	}

	public string GetBankName()
	{
		return Util.BytetoString (BankName);
	}

	public string GetBankAccount()
	{
		string str = Util.FixLenChartoString(BankAccount).Trim();
		return str.Trim();
	}

	public string GetBankUrl()
	{
		string str = Util.FixLenChartoString(BankUrl).Trim();
		return str.Trim();
	}

	public string GetRemark()
	{
		return Util.BytetoString(Remark);
	}
};
	
//玩家提交取款信息
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_TJQKInfo
{
	public uint  dwUserID;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
	byte [] MD5pass=new byte[50];
	public int   iMoney;
	byte  byAccountType=1;

    public void Setpass(string str)
    {
        string passMd5 = Util.GetMd5Hash(str);
		MD5pass = Util.StrToFixLenByte(passMd5, MD5pass);
    }
}
	
//查询标识
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class  MSG_GP_USER_Record
{
	public byte  byMainType;
	public byte  byZiType;
	public byte  byRord;
	public uint  dwUserId;
	public int  byCuePage;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 150, ArraySubType = UnmanagedType.Struct)]
	byte[] chGameName=new byte[150];
	public byte  bIsOther;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
	byte[] chBySj=new byte[50];
	public ulong startdatatime;
	public ulong enddatatime;

	public void SetChGameName(string str)
	{
		//发现这个字段传到服务器，现在测试是安卓手机，发过去的chGameName乱骂，干脆直接用byte[]算了
        //chGameName = Util.StrToFixLenChar(str, chGameName);
		chGameName = Util.StrToFixLenByte(str,chGameName);
	}

	public void SetChBySj(string str)
	{
		//发现这个字段传到服务器，现在测试是安卓手机，发过去的chGameName乱骂，干脆直接用byte[]算了
		//chBySj = Util.StrToFixLenChar(str, chBySj);
		chBySj = Util.StrToFixLenByte(str,chBySj);
	}
};

//因为查询的数据比较大，超过了缓存，超出部分多次发送。
//查询返回值（查询返回值用统一的结构）
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_ResultRecord
{
    public	byte  byMainType;
    public	byte  byZiType;
    public	byte  byColumn;                      //返回值有多少列
    public	byte  byLine;                        //有多少行
    public	int   iCountRecord;                 //总共有多少条记录
    public	byte  byPages;                      //总共有多少页       
    public	byte  byCountData;                  //一共分几次发送
    public	byte  byCueData;                    //发送的是第几次数据
    public  int  iCuePage;                     //当前页数
	public	uint  uCountchData;               //消息包的大小

    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2500, ArraySubType = UnmanagedType.Struct)]
    byte[] chData = new byte[2500];                //默认发送的最大消息包

	public string GetChData()
	{
		string str = Util.BytetoString(chData).Trim();
		return str.Trim();
	}
};


//获取投注详细结果
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class SScGetPlayNoteResult
{
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 21, ArraySubType = UnmanagedType.Struct)]
	byte[] UserName=new byte[21];    //玩家账号
	public int    OrderCount;     //投注数量
	public int    iNoteID;        //订单号
    public int SingleMoney;    //单注金额
    public int Multiple;       //倍数
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 21, ArraySubType = UnmanagedType.Struct)]
    char[]  ActivityName=new char[21]; //期号
    public int     Amount;         //投注总额
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 21, ArraySubType = UnmanagedType.Struct)]
	byte[]  ClassName=new byte[21];   //彩种
    public int BingoCount;       //中奖注数
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 51, ArraySubType = UnmanagedType.Struct)]
	byte[]  OrderTypeName=new byte[51]; //玩法
    public int PeiLv;             //单注中奖金额
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 21, ArraySubType = UnmanagedType.Struct)]
	char[] OpenNum=new char[21];        //开奖号码
    public int BingoMoney;        //中奖金额
    public ulong AddTM;    //下单时间
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
    byte[]  Point = new byte[8];             //返点
	public int     PointMoney;       //返点金额
	public int     IsBingo;          //状态
	public int     ResultMoney;      //盈亏
	public byte    byCueSend;        //当前发送到的次数
	public byte    byAllSend;        //一共要发送的次数
	public byte    bIsFinishSend;
	public int     iSendDataSize;    //一共发送的大小
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2001, ArraySubType = UnmanagedType.Struct)]
    char[]   OrderValue=new char[2001];  //投注内容

	public string GetUserName()
	{
		string str = Util.BytetoString(UserName).Trim();
		return str.Trim();
	}

	public string GetActivityName()
	{
		string str = Util.FixLenChartoString(ActivityName).Trim();
		return str.Trim();
	}

	public string GetClassName()
	{
		return Util.BytetoString(ClassName);
	}

	public string GetOrderTypeName()
	{
		string str = Util.BytetoString(OrderTypeName).Trim();
		return str.Trim();
	}

	public string GetOpenNum()
	{
		string str = Util.FixLenChartoString(OpenNum).Trim();
		return str.Trim();
	}

	public string GetOrderValue()
	{
		string str = Util.FixLenChartoString(OrderValue).Trim();
		return str.Trim();
	}

    public double GetPoint()
    {
        return BitConverter.ToDouble(Point, 0);
    }
}

//金额转换

[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_DHZHJINER
{
    public uint dwUserID;
    public int iMoney;
    public int iMoneyType;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
	byte[] chTwoPass = new byte[50];

    public void SetTwoPass(string str)
    {
		string passMd5 = Util.GetMd5Hash(str);
		chTwoPass = Util.StrToFixLenByte(passMd5, chTwoPass);
    }
};

//金额转换 返回
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_PLAYJINERZH
{
   public int iReturn;
   public int izhmoney;
   public int iqpmoney;
   public int icpmoney;

    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
    byte[] chResult=new byte[50];

    public string GetChResult()
    {
        string str = Util.BytetoString(chResult);

        return str;
    }

};
//zjl 2016-01-10  end
#endregion


//获取投注详细
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class SScGetPlayNote
{
    public int iNoteID;         //投注ID
    public byte bIsMm;
    public uint dwUserID;
};


//玩家取消下注
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class SScPlayCaclePlayNote
{
    public uint dwUserID;
    public int iNoteID;          //投注ID

};

[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_GETUSERINFO
{
    public uint dwUserId;

};

//金额刷新
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_SHUAXINRESULT
{
   public int iWalletMoney;	//棋牌余额	int		
   public int iMoney;	//彩票金额	int
}

//版本结果
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSC_GP_S_UserGetVisionResult
{
	public int iReturn;//返回值 等于0才成功
	public int imainNum;//主版本号		
	public int isubNum;//次版本号		
	public int byfixNum;//修订版本号		
	public byte bIsAnOrIos;//true为安卓版本，false为IOS版本
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 300, ArraySubType = UnmanagedType.Struct)]
    char[] chdownLoadUrl = new char[300];//下载更新地址

	public string GetChdownLoadUrl()
	{
		string str = Util.FixLenChartoString(chdownLoadUrl).Trim();
		return str.Trim();
	}
}

//版本请求
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSC_GP_S_UserGetVision
{
	public byte bIsAndroidOrios;	//true 安卓|false IOS
}

//心跳
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MDM_CONNECT_OBJ
{
}

//活动中心7天乐获取规则
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_HDZXHQ7TLRULE
{
   public  uint dwUserID;
};

//活动中心7天乐规则
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_HDZX7TLRULERESULT
{
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.Struct)]
    public int[] iXifei = new int[7];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 7, ArraySubType = UnmanagedType.Struct)]
    public int[] iSong = new int[7];
    public int iCueNumBtn;  //第几个按钮可以触发
    public int iCurDayXF;   //当前消费金额
    public int iSingedCount; //连续签到天数
    public int BtnLingjiangVisible;  //是否可以领奖
 
};


//7天乐签到
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_HDZXHQ7TLQD
{
    public uint dwUserID;
    public int iBtnNumber;
};

//7天乐领奖
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_HDZX7TLLJ
{
    public uint dwUserID;
};


//新人充值送活动
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_HDZXGETXRCZS
{
    public uint dwUserID;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
    byte[] xrczsAccessIP=new byte[16];	      //领取IP
};

//活动中心新人充值送规则结果
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_HDZXXRCZSRULET
{
   public ulong timesatarTM;
   public ulong timeendTM;
   public int iczMoney1;
   public int iczMoney2;
   public int ihbmoney1;
   public int ihbmoney2;
   public int IsCanLq;
};

//新人充值送领取红包
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_HDZXXRCZSLQHB
{
    public uint dwUserID;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
    byte[] xrczsAccessIP=new byte[16];	      //领取IP

};

//新人充值赠送领取红包
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_HDZXXRCZZSLQHBRESULT
{
    public int iReturn;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 100, ArraySubType = UnmanagedType.Struct)]
	byte[] chResult=new byte[100];

	public string GetChResult()
	{
		return Util.BytetoString(chResult);
	}
};


//永久推广奖励
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_HDZXYJTGJL
{
    public uint dwUserID;
};

//活动中心永久推广奖励规则
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_HDZXYJTGJLRUSULT
{
   public  int iplaydrcz;
   public  int iplaydrczhxx;
   public  int iagencyjl;
   public  int iplaydrxx;
   public  int isjjl;
   public  int issjjl;
   public  int isssjjl;
   public  int iplaydrxx_ks;
   public  int isjjl_ks;
   public  int issjjl_ks;
   public  int isssjjl_ks;
};


//会员管理包 req
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_ADDMONEY
{
    public byte byType;
    public uint dwMeUserID;

    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 21, ArraySubType = UnmanagedType.Struct)]
    byte[] destUserName = new byte[21];

    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.Struct)]
    byte[] point = new byte[11];     //金额或者分水的比例


    public void SetUserName(string uname)
    {
        destUserName = Util.StringtoFixedByte_GB2312(uname, destUserName);

    }
    public void SetPoint(string pt)
    {
        point = Util.StringtoFixedByte_GB2312(pt, point);
    }


};


//获取指定玩家配额
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_PLAY_GETPLAYPE
{
    public uint dwMeUserID;
    public uint dwUserID;
};

//获取玩家配额
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_GETPLAYPE
{
   public int iCountPe;
   public int dwUserID;
   public int dwDesUserID;
   public double fPoint;
   public int Point29;
   public int Point28;
   public int Point27;
   public int Point26;
   public int Point25;
   public int mePoint29;
   public int mePoint28;
   public int mePoint27;
   public int mePoint26;
   public int mePoint25;
   public int iReturn;

   [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 200, ArraySubType = UnmanagedType.Struct)]
   byte[] chResult = new byte[200];

    public string GetChResult()
    {
        return Util.BytetoString(chResult);
    }
 
};

//提交玩家配额
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_PLAY_SETPLAYPE
{
   public int dwMeUserID;
   public int dwUserID;
   public int iPe25;
   public int iPe26;
   public int iPe27;
   public int iPe28;
   public int iPe29;
};

//设定玩家配额返回
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_SETPLAYPERESULT
{
    public int dwUserID;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 200, ArraySubType = UnmanagedType.Struct)]
    byte[] chResult = new byte[200];
    public int iReturn;


    public string GetchResult()
    {
        return Util.BytetoString(chResult);
    }
};

//代理添加玩家的获取最高的返点结果
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_GETPLAYMAXPOINTRESULT
{
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
    byte[] dMaxPoint = new byte[8];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
    byte[] dWebPoint = new byte[8];   //注册返点

    byte byCountZc;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 100, ArraySubType = UnmanagedType.Struct)]
    byte[] chDescribe = new byte[100];

    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2400, ArraySubType = UnmanagedType.Struct)]
    byte[] chUrlZc = new byte[2400]; //TCHAR chUrlZc[30][80];


    public double GetdMaxPoint()
    {
        return BitConverter.ToDouble(dMaxPoint, 0);
    }

    public double GetdWebPoint()
    {
        return BitConverter.ToDouble(dWebPoint, 0);
    }

    public string GetDescribe()
    {
        return Util.BytetoString(chDescribe);
    }

    public List<string> GetUrlList()
    {
        List<string> urlList = new List<string>();
        for (int i = 0; i < byCountZc; ++i)
        {
            byte[] urlbyte = new byte[80];
            Array.Copy(chUrlZc, 80*i, urlbyte, 0, 80);
            string url = Util.BytetoString(urlbyte);
            urlList.Add(url);
        }

        return urlList;
    }

};

//代理添加玩家或者下级代理
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_TJPLAYORDL
{
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
    byte[] chUserName =new byte[20];   //游戏账号
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
    byte[] nickName = new byte[50];     //昵称  
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
    byte[] pass = new byte[50];         //登陆密码
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
    byte[] twopass = new byte[50];      //取款密码

    public int WalletMoney;      //增加金额
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
    byte[] chSpare = new byte[10];           //分水比例
    public int iAgencyID;        //上级代理id

    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
    byte[] qqNum = new byte[50];        //qq号码
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
    byte[] Email = new byte[50];        //电子邮件
    public int iUserType;        //帐号类型:0-玩家，1-代理


    public void SetUserName(string uname)
    {
        chUserName = Util.StringtoFixedByte_GB2312(uname, chUserName);

        SetNickName(uname);
    }

    void SetNickName(string nName)
    {
        nickName = Util.StringtoFixedByte_GB2312(nName, nickName);
    }

    public void Setpass(string ps)
    {
		string md5 = Util.GetMd5Hash(ps);
		pass = Util.StringtoFixedByte_GB2312(md5, pass);
    }

    public void Settwopass(string tpas)
    {
		string md5 = Util.GetMd5Hash(tpas);
		twopass = Util.StringtoFixedByte_GB2312(md5, twopass);
    }


    public void SetchSpare(string ss)
    {
        chSpare = Util.StringtoFixedByte_GB2312(ss, chSpare);
    }

    public void SetqqNum(string qq)
    {

        qqNum = Util.StringtoFixedByte_GB2312(qq, qqNum);
    }

    public void SetEmail(string em)
    {
        Email = Util.StringtoFixedByte_GB2312(em, Email);
    }

};

//玩家转账
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_USER_ZHGLZZ
{
    public uint dwMeUserId;
    public uint DestUserID;
    public int iTranMoney;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
    byte[] chUserName= new byte[50];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
    byte[] Md5twoPass = new byte[50];
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 100, ArraySubType = UnmanagedType.Struct)]
    byte[] Remark = new byte[100];

    public void SetchUserName(string ch)
    {
        chUserName = Util.StringtoFixedByte_GB2312(ch, chUserName);
    }

    public void SetMd5twoPass(string twopass)
    {
        string md5 = Util.GetMd5Hash(twopass);
        Md5twoPass = Util.StringtoFixedByte_GB2312(md5, Md5twoPass);
    }


};

//代理设定网页注册返点
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_PLAY_SDWEBFD
{
    public uint dwUserID;
    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.Struct)]
    byte[] chFd=new byte[10];

    public void SetChfd(string s)
    {
        chFd = Util.StringtoFixedByte_GB2312(s, chFd);
    }
};

//错误类型
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_S_SQL_Error
{
	public byte byErrorType;
};

//游戏列表辅助结构
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class  AssistantHead
{
	public uint uSize;//数据大小
	public uint bDataType;//类型标识
};

//游戏类型结构
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class  ComKindInfo//加入游戏类型AddTreeData
{
	AssistantHead Head;
	public uint uKindID;//游戏类型 ID 号码
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 61, ArraySubType = UnmanagedType.Struct)]
	byte[] szKindName =new byte[61];//游戏类型名字

	public string GetSzKindName()
	{
		return Util.BytetoString(szKindName);
	}
};

//游戏名称结构
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class  ComNameInfo
{
	AssistantHead Head;
	public uint uKindID;//游戏类型 ID 号码
	public uint uNameID;//游戏名称 ID 号码
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 61, ArraySubType = UnmanagedType.Struct)]
	byte[] szGameName =new byte[61];//游戏名称
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 31, ArraySubType = UnmanagedType.Struct)]
	byte[] szGameProcess =new byte[31];//游戏进程名
	public uint m_uOnLineCount;//在线人数
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.Struct)]
	byte[] version =new byte[5];//游戏版本
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 300, ArraySubType = UnmanagedType.Struct)]
	byte[] chdownLoadUrl =new byte[300];//下载更新地址

	public string GetSzGameName()
	{
		return Util.BytetoString(szGameName);
	}

	public string GetSzGameProcess()
	{
		return Util.BytetoString(szGameProcess);
	}

	public string GetVersion()
	{
		return Util.BytetoString(version);
	}

	public string GetChdownLoadUrl()
	{
		return Util.BytetoString(chdownLoadUrl);
	}
};

//获取游戏房间数据包
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GP_SR_GetRoomStruct
{
	public uint uKindID;//类型 ID
	public uint uNameID;//名字 ID
};

//游戏房间列表结构
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class  ComRoomInfo
{
	MSG_GP_SR_GetRoomStruct RoomBuf;
	AssistantHead Head;
	public uint uComType;//游戏类型
	public uint uKindID;//游戏类型 ID 号码
	public uint uNameID;//游戏名称 ID 号码
	public uint uRoomID;//游戏房间 ID 号码
	public uint uPeopleCount;//游戏在线人数
	public uint uDeskPeople;//每桌游戏人数
	public uint uDeskCount;//游戏大厅桌子数目
	public uint uServicePort;//大厅服务端口
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 25, ArraySubType = UnmanagedType.Struct)]
	byte[] szServiceIP = new byte[25];//服务器 IP 地址
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 61, ArraySubType = UnmanagedType.Struct)]
	byte[] szGameRoomName =new byte[61];//游戏房间名称		
	public int uVirtualUser;
	public int uVirtualGameTime;
	public uint uVer;//版本
	public uint dwRoomRule;//游戏房间规则
	public uint uBattleRoomID;
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 31, ArraySubType = UnmanagedType.Struct)]
	char [] szBattleGameTable=new char[31];//混战场房间信息表
	public int iLessPoint;//最少进入房间的门槛

	public string GetSzServiceIP()
	{
		return Util.BytetoString(szServiceIP);
	}

	public string GetSzGameRoomName()
	{
		return Util.BytetoString(szGameRoomName);
	}

	public string GetSzBattleGameTable()
	{
		string str = Util.FixLenChartoString(szBattleGameTable).Trim();
		return str.Trim();
	}
};