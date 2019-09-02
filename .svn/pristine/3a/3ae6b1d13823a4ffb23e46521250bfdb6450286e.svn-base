/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace baijiale
{
	public class UI_SetUI : GComponent
	{

		static UI_SetUI instance;
		public static void Show(){
			if (instance == null) {
				instance = UI_SetUI.CreateInstance ();
			}

			GRoot.inst.AddChild (instance);
			instance.SetSize(GRoot.inst.width,GRoot.inst.height);
		}

		public GButton m_okBtn;
		public GButton m_canelBtn;
		public GButton m_musicBtn;
		public GButton m_audioBtn;

		public const string URL = "ui://nyvoaldgbnnfn5b";

		public static UI_SetUI CreateInstance()
		{
			return (UI_SetUI)UIPackage.CreateObject("baijiale","SetUI");
		}

		public UI_SetUI()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_musicBtn = (GButton)this.GetChild("musicBtn");
			m_audioBtn = (GButton)this.GetChild("audioBtn");
			m_okBtn = (GButton)this.GetChild("okBtn");
			m_canelBtn = (GButton)this.GetChild("canelBtn");

			InitBtn ();
		}

		bool oldMusic;
		bool oldAudio;

		void InitBtn(){
			oldMusic = Global.GameBackgroundMusic;
			oldAudio = Global.GameEffectMusic;

			m_okBtn.onClick.Add (() => {
				Global.GameBackgroundMusic = m_musicBtn.selected;
				Global.GameEffectMusic = m_audioBtn.selected;
				GRoot.inst.RemoveChild (this);
			});

			m_canelBtn.onClick.Add (() => {
				Global.GameBackgroundMusic = oldMusic;
				Global.GameEffectMusic = oldAudio;
				GRoot.inst.RemoveChild (this);
			});

			m_musicBtn.onClick.Add (() => {
				Global.GameBackgroundMusic = m_musicBtn.selected;
			});

			m_audioBtn.onClick.Add (() => {
				Global.GameEffectMusic = m_audioBtn.selected;
			});

			m_musicBtn.selected = Global.GameBackgroundMusic;
			m_audioBtn.selected = Global.GameEffectMusic;
		}
	}
}