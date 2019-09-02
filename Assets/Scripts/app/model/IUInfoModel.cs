using UnityEngine;
using System.Collections;

public interface IUInfoModel {

    void SetUserinfo(MSG_GP_R_LogonResult uinfo);

	MSG_GP_R_LogonResult GetUserinfo();

    int GetUserID();

    string GetUserName();

	string GetNickName();

    int GetGold();

    int GetMoney();

	int GetQpFd();

	double GetCpFd();

	string GetLastLoginIp();

	string GetCurrLoginIp();

	string GetLastLoginTime();

	string GetCurrLoginTime();

    void  SetGold(int g);

    void  SetMoney(int m);

	void SetNickName(string name);
}
