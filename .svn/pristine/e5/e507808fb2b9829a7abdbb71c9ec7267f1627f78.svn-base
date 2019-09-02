/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using System.Collections.Generic;

namespace baijiale
{
	public class UI_ludan : GComponent
	{
		public GButton m_closeBtn;
		public GList m_ludanList;
		public GTextField m_zText;
		public GTextField m_xText;
		public GTextField m_pText;
		List<int> m_ludanData;

		public const string URL = "ui://nyvoaldgsazt3d";

		public static UI_ludan CreateInstance()
		{
			return (UI_ludan)UIPackage.CreateObject("baijiale","ludan");
		}

		public UI_ludan()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_closeBtn = (GButton)this.GetChild ("closeBtn");
			m_ludanList = (GList)this.GetChild ("ludanList");

			m_zText = (GTextField)this.GetChild ("zText");
			m_xText = (GTextField)this.GetChild ("xText");
			m_pText = (GTextField)this.GetChild ("pText");

			InitClose ();

			m_ludanList.itemRenderer = RendererLudanList;
			m_ludanList.SetVirtual();
			m_ludanList.numItems = 0;
		}

		void RendererLudanList(int index,GObject obj){
			UI_LudanItem item = (UI_LudanItem)obj;

			if (m_ludanData!=null && index < m_ludanData.Count) {
				int tmp = m_ludanData [index];
				item.SetSelected (tmp);
			}
		}

		public void SetLudanData(List<int> ludanData){
			this.m_ludanData = ludanData;
			m_ludanList.numItems = this.m_ludanData.Count;

			int z = 0;
			int x = 0;
			int p = 0;
			for (int i = 0; i < ludanData.Count; i++) {
				switch (ludanData [i]) {
				case 0:
					z++;
					break;
				case 1:
					x++;
					break;
				case 2:
					p++;
					break;
				}
			}

			m_zText.text = z.ToString ();
			m_xText.text = x.ToString ();
			m_pText.text = p.ToString ();
		}

		void InitClose(){
			m_closeBtn.onClick.Add (() => {
				GRoot.inst.RemoveChild(this);
			});
		}
	}
}