using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using FairyGUI.Utils;

namespace baijiale
{
	public class UI_LudanItem : GComponent {

		public GImage m_selected;
		float baseX;
		float baseY;

		public const string URL = "ui://nyvoaldgn144n4s";

		public static UI_LudanItem CreateInstance()
		{
			return (UI_LudanItem)UIPackage.CreateObject("baijiale","ludanItem");
		}

		public UI_LudanItem()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_selected = (GImage)this.GetChild("selected");
			baseX = m_selected.position.x;
			baseY = m_selected.position.y;
		}

		/// <summary>
		/// 初始化对号显示
		/// </summary>
		/// <param name="type">0:庄,1:闲,2:平</param>
		public void SetSelected(int type){
			float size = this.size.y / 3;
			m_selected.SetPosition (baseX, baseY+size * type, 0);

		}
	}
}
