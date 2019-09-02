/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using DG.Tweening;
using UnityEngine;

namespace baijiale
{
	public class UI_InfoMsg : GComponent
	{
		public static void Show(string message){
			UI_InfoMsg tmp = UI_InfoMsg.CreateInstance ();
			tmp.SetMessage (message);

			tmp.x = (GRoot.inst.width - tmp.width) / 2;
			tmp.y = (GRoot.inst.height - tmp.height) / 2;

			GRoot.inst.AddChild (tmp);
		}

		public GTextField m_Message;

		public const string URL = "ui://nyvoaldgppcyn5g";

		public static UI_InfoMsg CreateInstance()
		{
			return (UI_InfoMsg)UIPackage.CreateObject("baijiale","InfoMsg");
		}

		public UI_InfoMsg()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_Message = (GTextField)this.GetChild("Message");

			DOTween.To (() => {
				return this.position.y;
			}, (y) => {
				this.position = new Vector3(this.position.x,y,this.position.z);
			}, -10, 2f).OnComplete(()=>{
				this.Dispose();
			});

		}

		public void SetMessage(string message){
			m_Message.text = message;
		}
	}
}