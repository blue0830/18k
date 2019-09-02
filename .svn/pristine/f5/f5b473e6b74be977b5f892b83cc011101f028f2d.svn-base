using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class GameNetworkManager
{
	private volatile static GameNetworkManager instance = null;

	private GameSocket socket = null;

	private static readonly object locker = new object();

    private GameNetworkManager()
    {
        socket = GameSocket.GetInstance();
    }

    public static GameNetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                        instance = new GameNetworkManager();
                }
            }
            return instance;
        }
        set
        {
        }
    }
		
    //游戏心跳
    public void Heartbeat()
    {
        socket.SendHeartbeat();
    }
 
	//游戏登录
	public void Login()
    {
		MSG_GR_S_RoomLogon data = new MSG_GR_S_RoomLogon();
		data.dwUserID = Global.CurrentUserId;
		data.uNameID = Global.CurrentGameId;
		Debug.LogWarning("Login:"+Global.CurrentUserPwd);
		data.SetSzMD5Pass(Global.CurrentUserPwd);
        socket.SendMessage(100,5,data);
    }

	//游戏坐下
	public void Sit(byte bDeskStation)
	{
		MSG_GR_S_UserSit data = new MSG_GR_S_UserSit();
		data.bDeskIndex = 0;
		data.bDeskStation = bDeskStation;
		data.SetSzPassword("");
		socket.SendMessage(102,2,data);
	}

	//游戏下注
	public void bet(int betType,int moneyType)
	{
		XiaZhu data = new XiaZhu();
		data.station = 0;
		data.money = 0;
		data.type = betType;
		data.moneytype = moneyType;
		socket.SendMessage(180,130,data);
	}

	//游戏上庄
	public void shangZhuang(bool flag)
	{
		ShangZhuang data = new ShangZhuang();
		data.station = 0;
		data.shang = flag?(byte)1:(byte)0;
		socket.SendMessage(180,133,data);
	}

	//游戏状态
	public void getGameStatus()
	{
		MSG_GM_S_ClientInfo data = new MSG_GM_S_ClientInfo();
		data.bEnableWatch = 0;
		socket.SendMessage(150,1,data);
	}

	//离开桌子
	public void getUp()
	{
		socket.SendMessage(102,1);
	}

	//游戏退出
	public void getQuit()
	{
		socket.SendMessage(150,3);
	}
}