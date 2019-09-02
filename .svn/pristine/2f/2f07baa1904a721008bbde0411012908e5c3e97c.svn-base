using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class NetworkManager
{
	private volatile static NetworkManager instance = null;

	LoSocket socket = null;

	public bool dontUpdate = false;

	private static readonly object locker = new object();

    private NetworkManager()
    {
        socket = LoSocket.GetInstance();
    }

    public static NetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                        instance = new NetworkManager();
                }
            }
            return instance;
        }
        set
        {
        }
    }
		
    //心跳
    public void Heartbeat()
    {
		socket.Heartbeat();
    }

    public void ReLogin()
    {
		Login(Global.CurrentUserName, Global.CurrentUserPwd,true);
    }
		
	public void Login(string userName, string password,bool noLoading=false)
    {
        MSG_GP_S_LogonByNameStruct data = new MSG_GP_S_LogonByNameStruct();
        data.SetSzName(userName);
        data.SetSzMD5Pass(password);
		data.gsqPs = 5471;
        data.uRoomVer = 1;
		Global.CurrentUserName = userName;
		Debug.LogWarning("Login ==== "+password);
		Global.CurrentUserPwd = password;
        socket.SendMessage(100,1,data);
    }

    //请求时间期数
    public void GetQishuTime(int lId)
    {
        LotteryConfig lcfg = ConfigManager.Instance.GetLotteryCfgLoader().GetLotteryConfig(lId);
        socket.SendMessage(120, (uint)lcfg.timeResId);
    }

    //请求开奖记录
    public void GetRecord(int lId)
    {
        LotteryConfig lcfg = ConfigManager.Instance.GetLotteryCfgLoader().GetLotteryConfig(lId);
        socket.SendMessage(120, (uint)lcfg.recordResId);
    }

    public void GetAward(int lId, uint ModeId)
    {
        SScPlayGetPeilvPoint obj = new SScPlayGetPeilvPoint();
        obj.dwUserID = Global.CurrentUserId;
        obj.iClassID = lId;
        obj.iOrderType = (int)ModeId;
        socket.SendMessage(120, 4, obj);
    }

    public void SetAward(int lId, int ModeId, int award, double fpoint)
    {
        SScSetPlayPoint obj = new SScSetPlayPoint();
        obj.dwUserID = Global.CurrentUserId;
        obj.iClassID = lId;
        obj.iOrderType = ModeId;
        obj.iPeiLv = award;
        obj.SetDp(fpoint);
        socket.SendMessage(120, 5, obj);
    }

    /// <summary>
    /// 下订单
    /// </summary>
    /// <param name="cId">彩种id</param>
    /// <param name="aId">期号id</param>
    /// <param name="Amount">总金额</param>
 
    /// <param name="count">订单个数</param>
    /// <param name="orderContent">内容</param>
    /// 
    public void TakeOrder(int cId , int aId, int Amount, int count ,  string orderContent)
    {
        Debug.Log("orderContent； " + orderContent);
        List<byte[]> orders = new List<byte[]>();
        Split(orderContent, ref orders,3000);
        orders.Reverse();
        for (int i = 0; i < orders.Count; i++)
        {
            byte[] cCharAry = orders[i];
            MSG_GP_SSC_PLAYBATCHNOTE obj = new MSG_GP_SSC_PLAYBATCHNOTE();
            obj.dwUserID = Global.CurrentUserId;
            obj.iClassId = cId;
            obj.iActivityID = aId;
            obj.iAmountTotal = Amount;
            obj.iOrderCount = count;
            obj.byCueSend = (byte)i;
            obj.byAllSend = orders.Count==1?(byte)0:(byte)orders.Count;
            obj.iCountDataSize = Marshal.SizeOf(obj);
            obj.iCountAllChar = cCharAry.Length;obj.SetOrderValue(cCharAry);
            socket.SendMessage(120, 50, obj);
        }
    }
    
    void Split(string ori,ref List<byte[]> list,int SplitLen)
    {
        if (ori.Length > SplitLen)
        {
            int subLength = ori.Length - SplitLen;
            string subStr = ori.Substring(SplitLen, subLength);
            char[] subContent = subStr.ToCharArray();
            ori = ori.Substring(0, SplitLen);
            Split( subStr, ref list, SplitLen);

        }
        list.Add(Encoding.UTF8.GetBytes(ori));
    }
		
    //首先先要获取可以追号的期数
    public void GetZhuihaoQshu(int lotteryId)
    {
        MSG_GP_USER_GETLASTQISHU obj = new MSG_GP_USER_GETLASTQISHU();
        obj.iClassID = lotteryId;
        socket.SendMessage(120, 15, obj);
    }
		
    public void TakerZhuihaoOrder(int lotteryId,int submodeId,int actCount,  int singleMoney, int amount,int orderCount, int bingoStop ,string chRx, string orderContent)
    {
		List<byte[]> orders = new List<byte[]>();
        Split(orderContent, ref orders, 3000);
        orders.Reverse();
        for (int i = 0; i < orders.Count; i++)
        {
            byte[] cCharAry = orders[i];
            MSG_GP_ZHNOTEVALUE obj = new MSG_GP_ZHNOTEVALUE();
            obj.dwUserID = Global.CurrentUserId;
            obj.iClassID = lotteryId;
            obj.iOrderType = submodeId;
            obj.iActCount = actCount;
            obj.iSingleMoney = singleMoney;
            obj.Amount = amount;
            obj.iOrderCount = orderCount;
            obj.BingoIsStop = bingoStop;
            obj.byCueSend = (byte)i;
            obj.byAllSend = orders.Count == 1 ? (byte)0 : (byte)orders.Count;
            obj.iCountDataSize = Marshal.SizeOf(obj);
            obj.SetChaRx(chRx);
			obj.SetOrderValue(cCharAry);
            socket.SendMessage(120, 16, obj);
        }
    }

    public void SentZhuihao(int zhuihaoID ,int actCount ,List<int> actIds, string beishuStr)
    {

        MSG_GP_USER_ZHUIHAOQISHU obj = new MSG_GP_USER_ZHUIHAOQISHU();
        obj.iSLID = zhuihaoID;
        obj.dwUserID = Global.CurrentUserId;
        obj.iActCount = actCount;
        obj.SetActivityIds(actIds);
        obj.SetchMultiples(beishuStr);
        socket.SendMessage(120, 17, obj);
    }
		
    //获取公告
    public void GetGongGao()
    {
        socket.SendMessage(101, 6);
    }
		
    //获取个人资金
    public void GetMyMoney(ulong Tstart , ulong Tend)
    {
        MAG_GP_USER_GETWDZHGRJBXX obj = new MAG_GP_USER_GETWDZHGRJBXX();
        obj.udwUserID = Global.CurrentUserId;
        obj.bIsTime = 1;
        obj.tStartTime = Tstart;
        obj.tEndTime = Tend;
        socket.SendMessage(107, 22,obj);
    }

    //团队数据接口
    public void GetTeamData(ulong Tstart, ulong Tend)
    {
        MAG_GP_USER_GETTEAMJBXX obj = new MAG_GP_USER_GETTEAMJBXX();
        obj.udwUserID = Global.CurrentUserId;
        obj.bIsTime = 1;
        obj.sStartTime = Tstart;
        obj.tEndTime = Tend;
        socket.SendMessage(107, 23, obj);
    }
		
    //用户信息获取
    public void GetUserData()
    {
        MSG_GP_GETUSERINFO obj = new MSG_GP_GETUSERINFO();
        obj.dwUserId = Global.CurrentUserId;
        socket.SendMessage(107, 7, obj);
    }

    //设置提款账号
    public void SetBankAccount(string passwd, string bankname , string truename , string bankAccount , string bankAddress)
    {
        MSG_GP_USER_ChangeBakeInfoOrLock obj = new MSG_GP_USER_ChangeBakeInfoOrLock();
        obj.dwUserId = Global.CurrentUserId;
        obj.SetszMD5OldPass(passwd);
        obj.SetBackName(bankname);
        obj.SetTrueName(truename);
        obj.SetbackAccount(bankAccount);
        obj.SetbackAddress(bankAddress);
        obj.SetAlipayAccount("");
        obj.SetTenpayAccount("");
        socket.SendMessage(107, 5,obj);
    }

    //设置账号保护 q answer 长度必须是3
    public void SetAccountProtect(string twopass, string[] q ,string[] answer)
    {
        MSG_GP_USER_ZHGLSETMB obj = new MSG_GP_USER_ZHGLSETMB();
        obj.dwUserID = Global.CurrentUserId;
        obj.SetMd5twoPass(twopass);
        obj.SetQA(q, answer);
        socket.SendMessage(107, 16,obj);
    }

    //修改登录密码 type = 1 修改取款密码 type = 2   修改qq昵称 type = 3      // 
    public void ChangePasswd(int type, string oldpass , string newpass) //后两个参数也可作为qq号和昵称
    {
        MSG_GP_USER_changePassWord obj = new MSG_GP_USER_changePassWord();
        obj.dwUserID = Global.CurrentUserId;
		obj.isChangeType = type;
        if (type == 3)
        {
            obj.SetQQ(oldpass);
			obj.SetNickname(newpass);
        }
        else
        {
            obj.SetOldPass(oldpass);
            obj.SetNewPass(newpass);
        }
        socket.SendMessage(107, 8, obj);
    }

    //获取充值接口
    public void GetTopupInfo()
    {
        socket.SendMessage(107, 10);
    }

    //取款接口
    public void Withdraw(string passwd , int money)
    {
        MSG_GP_USER_TJQKInfo obj = new MSG_GP_USER_TJQKInfo();
        obj.dwUserID = Global.CurrentUserId;
        obj.Setpass(passwd);
        obj.iMoney = money;
        socket.SendMessage(107, 12, obj);
    }
		
    //记录查询  
    public void LookupRecord(byte main, byte zitype, byte byrod, int cupage, string chGameName, ulong Tstart, ulong Tend, int uId = -1 , string chbysj="")
    {
		Global.AppQueryMainType = main;
		Global.AppQuerySubType = zitype;
        MSG_GP_USER_Record obj = new MSG_GP_USER_Record();
		if (uId == 0)
		{
			obj.dwUserId = 0;
			obj.bIsOther = 0;
		}
        else if (uId == -1)
        {
            obj.dwUserId = Global.CurrentUserId;
            obj.bIsOther = 0;
        }
        else
        {
            obj.dwUserId = (uint)uId;
            obj.bIsOther = 1;
        }
        obj.byMainType = main;
        obj.byZiType = zitype;
        obj.byRord = byrod;
        obj.byCuePage = cupage;
        obj.SetChGameName(chGameName);
		obj.SetChBySj(chbysj);
        obj.startdatatime = Tstart;
        obj.enddatatime = Tend;
        socket.SendMessage(107, 6, obj);
    }

    //投注明细接口
    public void TouchuMingxi(int noteId , byte ismy=1)
    {
        SScGetPlayNote obj = new SScGetPlayNote();
        obj.dwUserID = Global.CurrentUserId;
        obj.iNoteID = noteId;
        obj.bIsMm = ismy;
        socket.SendMessage(120, 7, obj);
    }
		
    //投注撤单接口
    public void TouchuCheDan(int noteId)
    {
        SScPlayCaclePlayNote obj = new SScPlayCaclePlayNote();
        obj.dwUserID = Global.CurrentUserId;
        obj.iNoteID = noteId;
        socket.SendMessage(120, 20, obj);
    }

    //余额转换
    //type 1为彩票转棋牌，2为棋牌转彩票
    public void TransferMoney(int type , int money, string twopass)
    {
        MSG_GP_USER_DHZHJINER obj = new MSG_GP_USER_DHZHJINER();
        obj.dwUserID = Global.CurrentUserId;
        obj.iMoney = money;
        obj.iMoneyType = type;
        obj.SetTwoPass(twopass);
        socket.SendMessage(120, 35, obj);
    }

    //金额刷新
    public void RefreshMoney()
    {
        socket.SendMessage(107, 20);
    }

    //版本查询
    public void LookupVersion()
    {
        MSC_GP_S_UserGetVision obj = new MSC_GP_S_UserGetVision();
		#if UNITY_ANDROID
        	obj.bIsAndroidOrios = 1;
		#elif UNITY_IPHONE
			obj.bIsAndroidOrios = 0;
		#endif
        socket.SendMessage(107, 24, obj);
    }
		
    //拉取 7天乐信息
    public void GetSevenDayInfo()
    {
        MSG_GP_USER_HDZXHQ7TLRULE obj = new MSG_GP_USER_HDZXHQ7TLRULE();
        obj.dwUserID = Global.CurrentUserId;
        socket.SendMessage(120, 65, obj);
    }

    //7天 签到 天数从1 开始
    public void SevenDaySign(int day)
    {
        MSG_GP_USER_HDZXHQ7TLQD obj = new MSG_GP_USER_HDZXHQ7TLQD();
        obj.dwUserID = Global.CurrentUserId;
        obj.iBtnNumber = day;
        socket.SendMessage(120, 66, obj);
    }

    //7天领奖
    public void SevenDayGetAward()
    {
        MSG_GP_USER_HDZX7TLLJ obj = new MSG_GP_USER_HDZX7TLLJ();
        obj.dwUserID = Global.CurrentUserId;
        socket.SendMessage(120, 67, obj);
    }

	//充值送活动
	public void GetChongZhiSongInfo()
	{
		MSG_GP_USER_HDZXGETXRCZS obj = new MSG_GP_USER_HDZXGETXRCZS();
		obj.dwUserID = Global.CurrentUserId;
		socket.SendMessage(120, 39, obj);
	}

	//充值送活动 领奖
	public void ChongZhiSongGetAward()
	{
		MSG_GP_USER_HDZXXRCZSLQHB obj = new MSG_GP_USER_HDZXXRCZSLQHB();
		obj.dwUserID = Global.CurrentUserId;
		socket.SendMessage(120, 40, obj);
	}
		
	//永久推广活动
	public void GetPermanentPromotionInfo()
	{
		MSG_GP_USER_HDZXYJTGJL obj = new MSG_GP_USER_HDZXYJTGJL();
		obj.dwUserID = Global.CurrentUserId;
		socket.SendMessage(120, 68, obj);
	}


    //会员管理-会员资料-修改返点
    public void ChangeFanDian(string acc,string point)
    {
        MSG_GP_USER_ADDMONEY obj = new MSG_GP_USER_ADDMONEY();
        obj.byType = 1;
        obj.dwMeUserID = Global.CurrentUserId;
        obj.SetUserName(acc);
        obj.SetPoint(point);
        socket.SendMessage(107, 17, obj);
    }

    //会员管理-会员资料-分配配额
    int saveotherId=0;
    public void FenPeiPeiE(uint otherID)
    {
        if (otherID == 0)
        {
            if (saveotherId == 0)
                return;
            else
                otherID = (uint)saveotherId;
        }
        saveotherId = (int)otherID;
        MSG_PLAY_GETPLAYPE obj = new MSG_PLAY_GETPLAYPE();
        obj.dwMeUserID = Global.CurrentUserId;
        obj.dwUserID = otherID;
        socket.SendMessage(107, 27, obj);
    }

    //会员管理-会员资料-修改配额
    public void ChangePeiE(int desiD,int _30,int _31)
    {
        MSG_PLAY_SETPLAYPE obj = new MSG_PLAY_SETPLAYPE();
        obj.dwMeUserID = (int)Global.CurrentUserId;
        obj.dwUserID = desiD;
        obj.iPe27 = _30;
        obj.iPe28 = _31;
        socket.SendMessage(107, 28, obj);
    }

    public void GetAddMemberInfo()
    {
        socket.SendMessage(107, 21);
    }

    public void AddMember(int type , string account, string pass, string twopass, string fenshui)
    {
        MSG_GP_USER_TJPLAYORDL obj = new MSG_GP_USER_TJPLAYORDL();
        obj.iUserType = type;
        obj.WalletMoney = 0;
        obj.iAgencyID = (int)Global.CurrentUserId;
        obj.SetUserName(account);
        obj.Setpass(pass);
        obj.Settwopass(twopass);
        obj.SetchSpare(fenshui);
        socket.SendMessage(107, 13, obj);
    }

    //money 单位为分
    public void MemberTransfer(int money ,string twoPass, uint recId, string acc="")
    {
        MSG_GP_USER_ZHGLZZ obj = new MSG_GP_USER_ZHGLZZ();
        obj.dwMeUserId = Global.CurrentUserId;
        obj.DestUserID = recId;
        obj.iTranMoney = money;
        obj.SetchUserName(acc);
        obj.SetMd5twoPass(twoPass);
        socket.SendMessage(107, 15, obj);
    }

    public void SetWebFandian(string fandian)
    {
        MSG_PLAY_SDWEBFD obj = new MSG_PLAY_SDWEBFD();
        obj.dwUserID = Global.CurrentUserId;
        obj.SetChfd(fandian);
        socket.SendMessage(107, 26, obj);
    }

    public void LogOut()
	{
		try
		{
			Debug.LogWarning("LogOut ==== ");
			Global.CurrentUserPwd = null;
			dontUpdate = false;
			socket.ManualShutDown ();
		}
		catch (Exception e)
		{
			Debug.Log("logout error: " + e);
		}
    }

	public void GetGameTypes()
	{
		socket.SendMessage(101, 1);
	}

	public void GetGameRooms(uint uKindID,uint uNameID)
	{
		MSG_GP_SR_GetRoomStruct room = new MSG_GP_SR_GetRoomStruct ();
		room.uKindID = uKindID;
		room.uNameID = uNameID;
		socket.SendMessage(101, 3,room);
	}
}