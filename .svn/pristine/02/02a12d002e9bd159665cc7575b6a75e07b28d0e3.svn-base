/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace baijiale
{
	public class UI_xiafenList : GComponent
	{
		public GTextField m_name;
		public GImage m_n1;
		public GTextField m_number;

		public const string URL = "ui://nyvoaldgdfnf3w";

		public static UI_xiafenList CreateInstance()
		{
			return (UI_xiafenList)UIPackage.CreateObject("baijiale","xiafenList");
		}

		public UI_xiafenList()
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