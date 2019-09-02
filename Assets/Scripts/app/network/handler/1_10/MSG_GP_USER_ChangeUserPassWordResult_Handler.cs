using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
 
public class MSG_GP_USER_ChangeUserPassWordResult_Handler : IHandler<MSG_GP_USER_ChangeUserPassWordResult>
{
	[Inject]
	public IUInfoModel model { get; set; }

	[Inject]
	public IGameInfoModel GameModel { get; set; }

    [Inject]
	public QQMiMaQuKuanMiMaSignal GSignal { get; set; }

    public void OnReceive(NetMessageHead head, MSG_GP_USER_ChangeUserPassWordResult para)
    {
		GSignal.Dispatch (para);
		//为了全局更新，暂时写这里 蛋疼的方式
		if ((int)para.byCangeType == 3) {
			if (para.GetBIsSuc ()) {
				model.SetNickName (para.GetChNickName());
				GameModel.userinfo.SetQQNum (para.GetChGrQQ());
			}
		}
    }
}

