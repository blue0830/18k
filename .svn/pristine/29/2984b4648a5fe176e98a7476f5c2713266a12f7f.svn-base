using UnityEngine;
using System.Text;
using System.Collections;

public class UInfoModel : IUInfoModel
{
    private MSG_GP_R_LogonResult userInfo = null;

	public MSG_GP_R_LogonResult GetUserinfo()
	{
		return userInfo;
	}

    public void SetUserinfo(MSG_GP_R_LogonResult uinfo)
    {
        userInfo = uinfo;
    }

    public int GetUserID()
    {
        if (userInfo == null)
        {
            Debug.LogError("you must set userinfo first");
        }

        return userInfo.dwUserID;
    }

    public string GetUserName()
    {
        if (userInfo == null)
        {
            Debug.LogError("you must set userinfo first");
        }
        return userInfo.GetUserName();
    }

	public string GetNickName()
	{
		if (userInfo == null)
		{
			Debug.LogError("you must set userinfo first");
		}
		return userInfo.GetNickName();
	}

	public int GetQpFd(){
		if (userInfo == null)
		{
			Debug.LogError("you must set userinfo first");
		}
		return userInfo.SpareValue;
	}

	public double GetCpFd(){
		if (userInfo == null)
		{
			Debug.LogError("you must set userinfo first");
		}
		return userInfo.GetPoint();
	}

    public int GetGold()
    {
        if (userInfo == null)
        {
            Debug.LogError("you must set userinfo first");
        }

        return userInfo.dwMoney;
    }
    public int GetMoney()
    {
        if (userInfo == null)
        {
            Debug.LogError("you must set userinfo first");
        }

        return userInfo.dwBank;
    }


	public string GetLastLoginIp(){
		if (userInfo == null)
		{
			Debug.LogError("you must set userinfo first");
		}
		return IntToIp(userInfo.dwLastLogonIP);
	}

	public string GetCurrLoginIp(){
		if (userInfo == null) {
			Debug.LogError ("you must set userinfo first");
		}
		return IntToIp (userInfo.dwNowLogonIP);
	}

	public string GetLastLoginTime(){
		return TimeHelper.yyyyMMddHHmmss (userInfo.tLastLogoTm);
	}

	public string GetCurrLoginTime(){
		return TimeHelper.yyyyMMddHHmmss (userInfo.tCueLogoTm);
	}

    public void SetGold(int g)
    {
        userInfo.dwMoney = g;
    }

    public void SetMoney(int m)
    {
        userInfo.dwBank = m;
    }

	public void SetNickName(string nickename){
		userInfo.SetNickName(nickename);
	}

	public static string IntToIp(uint ipInt)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(ipInt & 0xFF).Append(".");
		sb.Append((ipInt >> 8) & 0xFF).Append(".");
		sb.Append((ipInt >> 16) & 0xFF).Append(".");
		sb.Append((ipInt >> 24) & 0xFF);
		return sb.ToString();
	}

	public static uint IpToInt(string ip)
	{
		char[] separator = new char[] { '.' };
		string[] items = ip.Split(separator);
		return uint.Parse(items[0]) << 24
			| uint.Parse(items[1]) << 16
			| uint.Parse(items[2]) << 8 
			| uint.Parse(items[3]);
	}
}