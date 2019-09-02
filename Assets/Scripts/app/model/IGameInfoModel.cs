using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IGameInfoModel
{
    MSG_GP_USER_GetUserInfoBack userinfo { get; set; }

    List<GetBankInfo> GetBankInfos();

	List<ComNameInfo> QpGameInfos();

	List<ComRoomInfo> QpRoomInfos();

	void AddGetBankInfo(GetBankInfo info);

	void AddQpGameInfo(ComNameInfo info);

	void AddQpRoomInfo(ComRoomInfo info);

	void ClearBankInfo();

	void ClearQpGameInfos();

	void ClearQpRoomInfos();

    TouzhuXiangxi xiangxiObj { get; set; }

    void Setordervalue(int i, string s);

    SortedList<int, string> GetOrderValue();

    RecordBackObj recordBackObj { get; set; }

    void SetRecordchData(int i, string s);

    SortedList<int, string> GetRecordchData();

    void ClearOrderValueList();

    void ClearRecordBackSlist();
}
