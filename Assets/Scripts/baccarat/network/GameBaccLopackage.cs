using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Security.Cryptography;

//游戏房间连接成功回包
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GameConnSuccess
{
	uint dwVer1;
	uint dwRes1;
	uint dwVer2;
	uint dwRes2;
};

//游戏房间登陆
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class  MSG_GR_S_RoomLogon
{
	public uint uNameID;//游戏名字ID
	public uint dwUserID; //用户ID
	public uint uRoomVer;//大厅版本
	public uint uGameVer;//游戏版本
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.Struct)]
	private byte[] szMD5Pass = new byte[50];//登陆密码

	public void SetSzMD5Pass(string strMd5)
	{
		Util.StrToFixLenByte(strMd5, szMD5Pass);
	}
}

//用户信息结构
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class UserInfoStruct
{
	public int dwUserID; //ID 号码
	public int dwExperience; //经验值
	public int dwAccID; //ACC 号码
	public int dwPoint; //分数
	public int dwMoney; //金币
	public int dwBank; //银行//游戏房间ID号码
	public uint uWinCount; //胜利数目
	public uint uLostCount;//输数目  //每桌游戏人数
	public uint uCutCount;//强退数目 //游戏大厅桌子数目
	public uint uMidCount;//和局数目 //大厅服务端口
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 61, ArraySubType = UnmanagedType.Struct)]
	byte[] szName = new byte[61];//登录名 //服务器IP 地址
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 61, ArraySubType = UnmanagedType.Struct)]
	byte[] szClassName = new byte[61];//游戏社团 //游戏房间名称
	uint bLogoID;//头像 ID 号码
	public byte bDeskNO;//游戏桌号
	public byte bDeskStation;//桌子位置
	public byte bUserState;//用户状态 //游戏房间规则
	public byte bMember;//会员等级
	public byte bGameMaster;//管理等级
	public uint dwUserIP;//登录IP地址
	public byte bBoy;//性别
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 61, ArraySubType = UnmanagedType.Struct)]
	byte[] nickName = new byte[61];//用户昵称
	public uint uDeskBasePoint;//设置的桌子倍数
	public int dwFascination;//魅力
	public int iVipTime;//会员时间
	public int	 iDoublePointTime;//双倍积分时间
	public int iProtectTime;//护身符时间，保留
	public int	 isVirtual;//是否是扩展机器人 //20081211 , Fred Huang
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 61, ArraySubType = UnmanagedType.Struct)]
	byte[] szOccupation = new byte[61];//玩家职业
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 61, ArraySubType = UnmanagedType.Struct)]
	byte[] szProvince = new byte[61];//玩家所在的省
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 61, ArraySubType = UnmanagedType.Struct)]
	byte[] szCity = new byte[61];//玩家所在的市
	public byte bHaveVideo; //是否具有摄像头
	public int userType;//0 ,普通玩家;1 ,电视比赛玩家;2 ,VIP玩家;3 ,电视比赛VIP玩家
	public uint userInfoEx1;//扩展字段1
	public uint userInfoEx2;//扩展字段2

	public string GetSzName()
	{
		return Util.BytetoString(szName);
	}

	public string GetSzClassName()
	{
		return Util.BytetoString(szClassName);
	}

	public string GetNickName()
	{
		return Util.BytetoString(nickName);
	}

	public string GetSzOccupation()
	{
		return Util.BytetoString(szOccupation);
	}

	public string GetSzProvince()
	{
		return Util.BytetoString(szProvince);
	}

	public string GetSzCity()
	{
		return Util.BytetoString(szCity);
	}

	public override bool Equals(object obj)
	{
		UserInfoStruct e = obj as UserInfoStruct;
		return this.dwUserID == e.dwUserID;
	}

	public override int GetHashCode()
	{
		return this.dwUserID.GetHashCode();
	}
};

//用户坐下
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GR_S_UserSit
{
	public byte bDeskIndex;//桌子索引
	public byte bDeskStation;//桌子位置
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 61, ArraySubType = UnmanagedType.Struct)]
	byte [] szPassword = new byte[61];//桌子密码

	public void SetSzPassword(string pwd)
	{
		Util.StrToFixLenByte(pwd, szPassword);
	}
};

//用户坐下回包
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GR_S_UserSit_Error
{
};

//用户坐下或者起来
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GR_R_UserSit
{
	public uint dwUserID; //用户 ID
	public byte bLock; //是否密码
	public byte bDeskIndex; //桌子索引
	public byte bDeskStation; //桌子位置
	public byte bUserState; //用户状态
	public byte bIsDeskOwner; //台主离开
};

//游戏信息
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class  MSG_GM_S_GameInfo
{
	public byte bGameStation;//游戏状态
	public byte bWatchOther;//允许旁观
	public byte bWaitTime;//等待时间
	public byte bReserve;//保留字段
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 1000, ArraySubType = UnmanagedType.Struct)]
	byte[] szMessage = new byte[1000];//系统消息

	public string GetSzMessage()
	{
		return Util.BytetoString(szMessage);
	}
};

//游戏场景
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class  MSG_GM_S_GameStation
{
	public byte bGameStation;//游戏状态
	public byte bWatchOther;//允许旁观
	public byte bWaitTime;//等待时间
	public byte bReserve;//保留字段
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 1000, ArraySubType = UnmanagedType.Struct)]
	byte[] szMessage = new byte[1000];//系统消息


	public string GetSzMessage()
	{
		return Util.BytetoString(szMessage);
	}
};


//断线重连数据包
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class ChongLian
{
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
	public int [] zhuangstation = new int[4];		//庄家列表
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
	public ushort [] pai = new ushort[6];			//庄闲的牌，0为庄，1为闲
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iZPaiXing = new int[5];		//庄家牌型,元素0前两张牌的值，元素1总牌值，元素2天王，元素3对子，元素4和
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iXPaiXing = new int[5];		//闲家牌型
	public int  m_iZhuangFen;			//庄家的得分
	public int  m_iXianFen;			//闲家的得分
	public int  m_iUserFen;			//当前玩家的得分
	public int  m_iWinner;				//赢家1 庄，2闲，3和，本赢方
	public int  m_iKaiPai;				//本把开牌区域：1庄，2庄天王，3庄对子，4闲，5闲天王，6闲对子，和，同点和
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
	public byte [] m_CardCount = new byte[2];		//双方的牌张数   
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iNTdata = new int[4];			//庄家的位置,总分（当前金币数），成绩(赢的总金币)，局数（坐庄的局数）
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 120, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iResultInfo = new int[120];	//最近30局的信息
	public int  m_iShangZhuangLimit;	//上庄需要的最少金币
	public int  m_iBaSHu;				//本局（30把一局）进行的把数
	public int  m_iZhuangBaShu;		//庄家进行了几把
	public int  m_iNowNtStation;		//当前庄家的位置
	public int  m_iGameCount;			//当前已经进行了几把
	public int  m_iSYTime;             //剩余时间
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
	public int []	m_iMaxZhu = new int[8];			//每个区域能下的最大注
	public int m_iZhongZhu;			//本把当前总注额
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iQuYuZhu = new int[8];			//本把每个区域下的注额
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 48, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iQuYuJinBi = new int[48];	//每区域下各类（共6类）金币的个数
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iMeZhu = new int[9];			//我在每个区域下的注 和总注 0-7每个区域的下注，8总注lym 
	public int  m_iXiaZhuTime ;		//下注时间
	public int  m_iKaiPaiTime ;		//开牌时间
	public int  m_iFreeTime ;			//空闲时间
	public int  m_iSendInterval;		//发牌间隔时间

	public List<Bet> getBets(){
		List<Bet> bets = new List<Bet> ();
		for (int i = 0; i < 6; ++i)
		{
			for (int j = i*6,end = i*6+6,index =0; j<m_iQuYuJinBi.Length&&j < end; ++j,++index)
			{
				Bet bet = new Bet ();
				bet.type = i+1;
				bet.area = index;
				bet.money = m_iQuYuJinBi [index];
				bets.Add (bet);
				Debug.LogWarning ("下注区域:"+index+", 下注类型:"+bet.type+", 下注金额:"+m_iQuYuJinBi [index]);
			}
		}
		return bets;
	} 

	public Cards getCards(){
		Cards cards = new Cards();
		for (int i = 0; i < 2; ++i)
		{
			List<Card> list = new List<Card>();
			if (i == 0) {
				cards.zCards = list;
			} else {
				cards.xCards = list;
			}
			for (int j = i*3,end = i*3+3,index =0; j<pai.Length&&j < end; ++j,++index)
			{
				Card card = new Card();
				card.point = pai[j]&0x000F;
				card.color = pai[j]>>4&0x000F;
				if(list.Count<(int)m_CardCount[i])//实际牌数
					list.Add (card);
			}
		}
		return cards;
	}
};

public class Bet
{
	public int area;//下注区域
	public int type;//下注类型
	public int money;//下注金额
}

public class Cards
{
	public List<Card> zCards;//庄牌数
	public List<Card> xCards;//闲牌数
}

public class Card
{
	public int point;//点数
	public int color;//花色：0表示大小王或无花色， 1方块，2梅花，3红桃，4黑桃
}

public class BaccaraHistory
{
	public int zPoint;//庄点数
	public int xPoint;//闲点数
	public int winner;//1庄2闲3和
}

//空包结构
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class Empty
{
}

//百家乐游戏开始信息
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class BeginData
{
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iNTdata = new int[4]; //庄家的位置,总分（当前金币数），成绩(赢的总金币)，局数（坐庄的局数）
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iNTlist = new int[4]; //庄家列表的位置
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 120, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iResultInfo = new int[120];//最近30局的信息
	public int  m_iShangZhuangLimit; //上庄需要的最少金币
	public int  m_iBaSHu;            //本局（30把一局）进行的把数
	public int  m_iZhuangBaShu;      //庄家进行了几把
	public int  m_iNowNtStation;     //当前庄家的位置
	public int  m_iGameCount;        //当前已经进行了几把
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iMaxZhu = new int[8];        //每个区域能下的最大注
	public int  m_iXiaZhuTime ;		//下注时间
	public int  m_iKaiPaiTime ;		//开牌时间
	public int  m_iFreeTime ;			//空闲时间	public long m_allAiMoney;//统计机器人总额        
	public long m_allplaymoney;//统计真人的金币的 
};


//下注数据包
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class XiaZhu
{
	public int station;//位置
	public int money;//金额
	public int type;//下注区域0-7  0 庄下注区域 1庄天王下注区域 2庄对子下注区域 3 闲下注区域 4闲天王下注区域 5闲对子下注区域 6和下注区域 7同点和下注区域
	public int moneytype;//筹码类型：1:1元，2:10元，3:100元，4:100元，5:5000元，6:1w元
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iMaxZhu = new int[8];//每个区域还能下多少注
	public int m_iZhongZhu;//本把当前总注额
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iQuYuZhu = new int[8];//本把每个区域下的注额
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
	public byte [] point = new byte[8];////筹码坐标
	public byte bisAI; //是否机器人在下注

	public int getXSeed(){
		int i = point[0];
		i += point[1];
		i += point[2];
		i += point[3];
		return i;
	}

	public int getYSeed(){
		int i = point[4];
		i += point[5];
		i += point[6];
		i += point[7];
		return i;
	}
};

//lym0512
//开牌数据包
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class KaiPai
{
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
	public ushort [] pai = new ushort[6];        //庄闲的牌，0为庄，1为闲
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iZPaiXing = new int[5];   //庄家牌型,元素0前两张牌的值，元素1总牌值，元素2天王，元素3对子，元素4和
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iXPaiXing = new int[5];   //闲家牌型
	public int  m_iZhuangFen;     //庄家的得分
	public int  m_iXianFen;	   //闲家的得分
	public int  m_iUserFen;		//当前玩家的得分
	public int  m_iWinner;			//赢家1 庄，2闲，3和，本赢方
	public int  m_iKaiPai;			//本把开牌区域：1庄，2庄天王，3庄对子，4闲，5闲天王，6闲对子，7和，8同点和
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 120, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iResultInfo = new int[120]; //牌信息
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
	public int [] zhuangstation = new int[4];//庄家列表
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iWinQuYu = new int[8]; //游戏的赢钱区域 0庄，1庄天王，2庄对子，3闲，4先天王，5闲对子，6和，7同点和
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.Struct)]
	public byte [] m_CardCount = new byte[2];//双方的牌张数

	public Cards getCards(){
		Cards cards = new Cards();
		for (int i = 0; i < 2; ++i)
		{
			List<Card> list = new List<Card>();
			if (i == 0) {
				cards.zCards = list;
			} else {
				cards.xCards = list;
			}
			for (int j = i*3,end = i*3+3,index =0; j<pai.Length&&j < end; ++j,++index)
			{
				Card card = new Card();
				card.point = pai[j]&0x000F;
				card.color = pai[j]>>4&0x000F;
				if(list.Count<(int)m_CardCount[i])//实际牌数
					list.Add (card);
			}
		}
		return cards;
	}
};


//结算数据包lym1204
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class JieSuan
{
	public int  m_iZhuangFen;//庄家的得分
	public int  m_iXianFen;//闲家的得分
	public int  m_iUserFen;//当前玩家的得分 
	public int  m_iNtWin;//当前庄家赢的金币（成绩
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iZPaiXing = new int[5];   //庄家牌型,元素0前两张牌的值，元素1总牌值，元素2天王，元素3对子，元素4和
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iXPaiXing = new int[5];   //闲家牌型
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 120, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iResultInfo = new int[120]; //牌信息
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
	public int [] zhuangstation = new int[4];//庄家列表
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.Struct)]
	public int [] m_iWinQuYu = new int[8];//游戏的赢钱区域 0庄，1庄天王，2庄对子，3闲，4先天王，5闲对子，6和，7同点和
	public int  m_iMeFanFen;//本把玩家返还的分，开和时出现
	public long m_allAiMoney;//增加统计机器人的总额             
	public long m_allplaymoney;//增加统计真人的金币的总额
	public long m_aiLostorwin;//机器人输赢的金额
	public long m_iAiwin;//机器人输赢区间
	public long m_iailost;
};

//上庄数据包
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class ShangZhuang
{
	public int station; //申请的位置
	public byte shang;	 //true为上庄，false 为下庄
	[MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.Struct)]
	public int [] zhuangstation = new int[4];//庄家列表
	public byte success;//是否失败
	public byte isAI;//是否机器人在申请做庄
};

//游戏信息
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GM_S_ClientInfo
{
	public byte bEnableWatch;//允许旁观
};


//用户经验值
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GR_R_UserPoint
{
	public uint dwUserID;//用户 ID
	public int dwPoint;//用户经验值
	public int dwMoney;//用户金币
	public byte bWinCount;//胜局
	public byte bLostCount;//输局
	public byte bMidCount;//平局
	public byte bCutCount;//逃局
};

//用户离开结构
[System.Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_GR_R_UserLeft
{
	public int dwUserID; //ID 号码
};