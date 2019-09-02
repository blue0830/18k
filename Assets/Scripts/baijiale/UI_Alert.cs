/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System;
using FairyGUI;
using FairyGUI.Utils;

namespace baijiale
{
	public class UI_Alert : GComponent
	{
		static UI_Alert instance;
		public static void ShowMsg(string message,Action confirm = null,Action cancel = null){
			if (instance == null) {
				instance = UI_Alert.CreateInstance ();
			}

			GRoot.inst.AddChild (instance);
			instance.SetSize(GRoot.inst.width,GRoot.inst.height);
			instance.SetMsg (message, confirm, cancel);
		}

		public GButton m_okBtn;
		public GButton m_canelBtn;
		public GTextField m_alertText;

		public const string URL = "ui://nyvoaldgbnnfn54";

		public static UI_Alert CreateInstance()
		{
			return (UI_Alert)UIPackage.CreateObject("baijiale","Alert");
		}

		public UI_Alert()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_okBtn = (GButton)this.GetChild("okBtn");
			m_canelBtn = (GButton)this.GetChild("canelBtn");
			m_alertText = (GTextField)this.GetChild("alertText");

			InitBtn ();
		}

		Action confirm;
		Action cancel;

		void InitBtn(){
			m_okBtn.onClick.Add (() => {
				GRoot.inst.RemoveChild (this);
				if(confirm!=null){
					confirm.Invoke();
					confirm = null;
				}
			});

			m_canelBtn.onClick.Add (() => {
				GRoot.inst.RemoveChild (this);
				if(cancel != null){
					cancel.Invoke();
					cancel = null;
				}
			});
		}

		public void SetMsg(string message,Action confirm = null,Action cancel = null){
			this.confirm = confirm;
			this.cancel = cancel;
			m_alertText.text = message;
		}
	}
}