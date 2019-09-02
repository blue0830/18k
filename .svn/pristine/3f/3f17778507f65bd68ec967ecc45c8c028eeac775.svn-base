/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace baijiale
{
	public class UI_Head : GComponent
	{
		public GImage m_n0;
		public GButton m_setbtn;
		public GButton m_quitBtn;
		public GTextField m_userName;
		public GTextField m_userId;
		public GTextField m_goldText;
		public GTextField m_goldaddText;

		public const string URL = "ui://nyvoaldgsazt3u";

		public static UI_Head CreateInstance()
		{
			return (UI_Head)UIPackage.CreateObject("baijiale","Head");
		}

		public UI_Head()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_n0 = (GImage)this.GetChildAt(0);
			m_setbtn = (GButton)this.GetChildAt(1);
			m_quitBtn = (GButton)this.GetChildAt(2);
			m_userName = (GTextField)this.GetChildAt(3);
			m_userId = (GTextField)this.GetChildAt(4);
			m_goldText = (GTextField)this.GetChildAt(5);
			m_goldaddText = (GTextField)this.GetChildAt(6);

			m_setbtn.onClick.Add (() => {
				UI_SetUI.Show();
			});
		}

		//设置金币和成绩
		public void initGoldData(string gold,string achievement){
			m_goldText.text = gold;
			m_goldaddText.text = achievement;
		}

		//设置用户id和用户名
		public void initPlayData(string userId,string userName){
			m_userId.text = userId;
			m_userName.text = userName;
		}
	}
}