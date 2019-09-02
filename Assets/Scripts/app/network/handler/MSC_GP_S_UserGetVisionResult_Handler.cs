using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class MSC_GP_S_UserGetVisionResult_Handler : IHandler<MSC_GP_S_UserGetVisionResult>
{
	[Inject]
	public VisionSignal signal { get; set; }

    [Inject]
    public MsgSignal msignal { get; set; }

    public void OnReceive(NetMessageHead head, MSC_GP_S_UserGetVisionResult para)
	{
        byte IsAndriod = 1;

		#if UNITY_ANDROID
        	IsAndriod = 1;
		#elif UNITY_IPHONE
			IsAndriod = 0;
		#endif

        if (IsAndriod != para.bIsAnOrIos)
        {
            return;
        }
        string versionStr = "";
        if (IsAndriod == 1)
        {
            versionStr = Constant.ANDROID_VERSION;
        } else
        {
            versionStr = Constant.IOS_VERSION;
        }
        string[] versions = versionStr.Split('.');

        if (int.Parse(versions[0]) != para.imainNum || int.Parse(versions[1]) != para.isubNum  )  //修订版本不弹窗
        {
			if (!NetworkManager.Instance.dontUpdate) {
				msignal.Dispatch (new MsgPara ("您的版本过低，\n点击确认下载最新版本", 1, () => {
					Application.OpenURL (para.GetChdownLoadUrl ());
				}));
			}
        }
        else if(int.Parse(versions[2]) != para.byfixNum)
        {
			if (!NetworkManager.Instance.dontUpdate) {
				msignal.Dispatch (new MsgPara ("您的版本过低，\n点击确认下载最新版本", 1, () => {
					Application.OpenURL (para.GetChdownLoadUrl ());
				}, () => {
					NetworkManager.Instance.dontUpdate = true;
				}));
			}
        }
    }
}

