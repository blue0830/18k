using UnityEngine;
using System.Collections.Generic;

public class GameInfoModel : IGameInfoModel
{
	List<ComRoomInfo> QpRoomInfoList = new List<ComRoomInfo>();

	List<GetBankInfo> BankInfoList = new List<GetBankInfo>();

	List<ComNameInfo> QpGameInfoList = new List<ComNameInfo>();

	SortedList<int, string> orderValueSlist = new SortedList<int, string>();

	SortedList<int, string> recordBackSlist = new SortedList<int, string>();

    public MSG_GP_USER_GetUserInfoBack userinfo { get; set; }
    
    public void AddGetBankInfo(GetBankInfo info)
    {
        BankInfoList.Add(info);
    }

	public void AddQpGameInfo(ComNameInfo info)
	{
		QpGameInfoList.Add(info);
	}

	public void AddQpRoomInfo(ComRoomInfo info)
	{
		QpRoomInfoList.Add(info);
	}

    public List<GetBankInfo> GetBankInfos()
    {
        return BankInfoList;
    }

	public List<ComNameInfo> QpGameInfos()
	{
		return QpGameInfoList;
	}

	public List<ComRoomInfo> QpRoomInfos()
	{
		return QpRoomInfoList;
	}

    public void ClearBankInfo()
    {
        BankInfoList.Clear();
    }

	public void ClearQpGameInfos()
	{
		QpGameInfoList.Clear();
	}

	public void ClearQpRoomInfos()
	{
		QpRoomInfoList.Clear();
	}

    //投注详细
    public TouzhuXiangxi xiangxiObj { get; set; }

    public void ClearOrderValueList()
    {
        orderValueSlist.Clear();
    }

    public void Setordervalue(int i, string s)
    {
        if(!orderValueSlist.ContainsKey(i))
            orderValueSlist.Add(i, s);
    }

    public SortedList<int, string> GetOrderValue()
    {
        return orderValueSlist;
    }

    //记录查询
    public RecordBackObj recordBackObj { get; set; }

    public void ClearRecordBackSlist()
    {
        recordBackSlist.Clear();
    }

    public void SetRecordchData(int i, string s)
    {
        if (!recordBackSlist.ContainsKey(i))
            recordBackSlist.Add(i, s);
    }

    public SortedList<int, string> GetRecordchData()
    {
        return recordBackSlist;
    }

}
