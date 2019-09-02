/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using UnityEngine;

namespace baijiale
{
	public class UI_Settlement : GComponent
	{
		public GButton m_closeBtn;
		public GTextField m_mfen;
		public GTextField m_zfen;
		public GTextField m_mfan;
		public GTextField m_zfan;
		float startTime;

		public const string URL = "ui://nyvoaldgn144n4u";

		public static UI_Settlement CreateInstance()
		{
			return (UI_Settlement)UIPackage.CreateObject("baijiale","Settlement");
		}

		public UI_Settlement()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_closeBtn = (GButton)this.GetChildAt(1);
			m_mfen = (GTextField)this.GetChildAt(2);
			m_zfen = (GTextField)this.GetChildAt(3);
			m_mfan = (GTextField)this.GetChildAt(4);
			m_zfan = (GTextField)this.GetChildAt(5);

			InitClose ();

			EventMgr.ins.AddEventListener (EventMgr.EnterFrame, OnEnterFrame);
			startTime = Time.time;

			HideMouse (this);
		}

		void HideMouse(GComponent obj){
			if (obj == null) {
				return;
			}
			obj.touchable = false;
			for (int i = 0; i < obj.numChildren; i++) {
				GObject item = obj.GetChildAt (i);
				item.touchable = false;
				if (item is GComponent) {
					HideMouse ((GComponent)item);
				}
			}
		}

		void OnEnterFrame(string e,System.Object obj){
			if (Time.time - startTime > 7) {
				OnClose ();
			}
		}

		void InitClose(){
			m_closeBtn.onClick.Add (OnClose);
			m_closeBtn.visible = false;
		}

		void OnClose() {
			EventMgr.ins.RemoveEventListner (EventMgr.EnterFrame, OnEnterFrame);
			this.Dispose();
		}

		public void OnData(string bfMoney,string fhMoney,string zjMoney){
			m_mfen.text = bfMoney;
			m_mfan.text = fhMoney;
			m_zfen.text = zjMoney;
			m_zfan.text = "";
		}
	}
}