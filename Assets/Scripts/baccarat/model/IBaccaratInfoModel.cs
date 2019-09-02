using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IBaccaratInfoModel
{
	List<UserInfoStruct> OnLineUserInfos();

	void AddOnLineUserInfo(UserInfoStruct info);

	void ClearOnLineUserInfos();

	List<UserInfoStruct> OffLineUserInfos ();

	void AddOffLineUserInfo (UserInfoStruct info);

	void ClearOffLineUserInfos ();
}
