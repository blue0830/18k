using UnityEngine;
using System.Collections.Generic;

public class BaccaratInfoModel : IBaccaratInfoModel
{
	List<UserInfoStruct> onLineuserInfos = new List<UserInfoStruct>();

	List<UserInfoStruct> offLineUserInfos = new List<UserInfoStruct>();

	public void AddOnLineUserInfo(UserInfoStruct info)
	{
		onLineuserInfos.Add(info);
	}
		
	public List<UserInfoStruct> OnLineUserInfos()
	{
		return onLineuserInfos;
	}

	public void ClearOnLineUserInfos()
	{
		onLineuserInfos.Clear();
	}

	public void AddOffLineUserInfo(UserInfoStruct info)
	{
		offLineUserInfos.Add(info);
	}

	public List<UserInfoStruct> OffLineUserInfos()
	{
		return offLineUserInfos;
	}

	public void ClearOffLineUserInfos()
	{
		offLineUserInfos.Clear();
	}
}
