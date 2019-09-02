using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using FairyGUI.Utils;

public class PokeBar {
	GComponent bar;
	GComponent zp1;
	GComponent zp2;
	GComponent zp3;
	GComponent xp1;
	GComponent xp2;
	GComponent xp3;

	GTextField ZDian;
	GTextField XDian;


	PokePai[] pais = new PokePai[6];
	public void Init () {
		if (bar != null) {
			return;
		}

		bar = UIPackage.CreateObject ("baijiale", "pai").asCom;
		HideMouse (bar);



		zp1 = bar.GetChild ("ZP1").asCom;
		zp2 = bar.GetChild ("ZP2").asCom;
		zp3 = bar.GetChild ("ZP3").asCom;
		xp1 = bar.GetChild ("XP1").asCom;
		xp2 = bar.GetChild ("XP2").asCom;
		xp3 = bar.GetChild ("XP3").asCom;

		ZDian = bar.GetChild ("ZDian").asTextField;
		XDian = bar.GetChild ("XDian").asTextField;

		zp1.RemoveChildren ();
		zp2.RemoveChildren ();
		zp3.RemoveChildren ();
		xp1.RemoveChildren ();
		xp2.RemoveChildren ();
		xp3.RemoveChildren ();

		GComponent[] paiCom = new GComponent[]{zp1,zp2,zp3,xp1,xp2,xp3 };
		for (int i = 0; i < 6; i++) {
			PokePai tmp = new PokePai ("", paiCom[i]);
			tmp.SetPerspective();
			tmp.opened = false;
			pais [i] = tmp;

			pais [i].Visible = false;
		}
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

	public void fucFanPai(List<Card> zCards,int z2point,int z3point,int zToD,List<Card> xCards,int x2point,int x3point,int xToD){
		string[] color = new string[]{"","f","m","h","t" };
		//庄
		for (int i = 0; i < 3; i++) {
			if (i < zCards.Count) {
				string c = color [zCards [i].color];
				string pai = c + zCards [i].point;

				pais [i].SetPai (pai);
			} else {
				pais [i].SetPai ("");
			}
		}

		//闲
		for (int i = 0; i < 3; i++) {
			if (i < xCards.Count) {
				string c = color [xCards [i].color];
				string pai = c + xCards [i].point;

				pais [i + 3].SetPai (pai);
			} else {
				pais [i + 3].SetPai ("");
			}
		}

		for (int i = 0; i < 6; i++) {
			pais [i].SetPerspective();
			pais [i].opened = false;
			pais [i].Visible = false;
		}

		CoroutineTool.inst.StartCoroutine (Trun (z2point,z3point,zToD,x2point,x3point,xToD));
	}

	IEnumerator Trun(int z2point,int z3point,int zToD,int x2point,int x3point,int xToD){
		ZDian.text = "--";
		XDian.text = "--";

		for (int i = 0; i < 3; i++) {
			bool isTrun = false;
			if (pais [i].pai != "") {
				pais [i].opened = false;
				pais [i].Turn ();
				isTrun = true;

				if(Global.GameEffectMusic)
					AudioController.Instance.SoundGamePlay("baccarat/open_card");
				
				if (i == 1) {
					ChangeDian (ZDian, z2point, zToD);
				}
				if (i == 2) {
					ChangeDian (ZDian, z3point, zToD);
				}
			}
			if (pais [i+3].pai != "") {
				pais [i+3].opened = false;
				pais [i+3].Turn ();
				isTrun = true;

				if(Global.GameEffectMusic)
					AudioController.Instance.SoundGamePlay("baccarat/open_card");

				if (i == 1) {
					ChangeDian (XDian, x2point, xToD);
				}
				if (i == 2) {
					ChangeDian (XDian, x3point, xToD);
				}
			}

			if (isTrun) {
				yield return new WaitForSeconds (1f);
			}
		}

		yield return new WaitForSeconds (5f);
		this.Close ();
	}

	void ChangeDian(GTextField dian,int point,int tod){
		switch (tod) {
		case 1: //天王
			dian.text = "天王("+point+")";
			break;
		case 2: //对子
			dian.text = "对子("+point+")";
			break;
		case 3: //天王对子
			dian.text = "天王对子("+point+")";
			break;
		default:
			dian.text = point.ToString () + "点";
			break;
		}
	}

	public void Open(){
		GRoot.inst.AddChild (bar);
		bar.SetSize(GRoot.inst.width,GRoot.inst.height);
	}

	public void Close(){
		GRoot.inst.RemoveChild (bar);
	}
}

