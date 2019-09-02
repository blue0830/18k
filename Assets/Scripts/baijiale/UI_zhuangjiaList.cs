/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace baijiale
{
	public class UI_zhuangjiaList : GComponent
	{
		public GTextField m_name;
		public GImage m_n1;
		public GTextField m_number;

		public const string URL = "ui://nyvoaldgdfnf3v";

		public static UI_zhuangjiaList CreateInstance()
		{
			return (UI_zhuangjiaList)UIPackage.CreateObject("baijiale","zhuangjiaList");
		}

		public UI_zhuangjiaList()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_name = (GTextField)this.GetChildAt(0);
			m_n1 = (GImage)this.GetChildAt(1);
			m_number = (GTextField)this.GetChildAt(2);
		}

		public void OnData(string name,string num){
			m_name.text = name;
			m_number.text = num;
		}
	}
}