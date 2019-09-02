using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using FairyGUI.Utils;
using DG.Tweening;

public class PokePai {
	GComponent _back;
	GComponent _front;

	public string pai {
		get;
		protected set;
	}

	GComponent parent;
	bool isOpen = false;

	public PokePai (string pai,GComponent parent) {
		this.parent = parent;

		_back = UIPackage.CreateObject ("baijiale", "pokeBack").asCom;
		_back.pivot = new Vector2 (0.5f, 0.5f);
		parent.AddChild (_back);

		SetPai (pai);
	}

	public void SetPai(string pai){
		this.pai = pai;

		if (_front != null) {
			_front.Dispose ();
			_front = null;
		}

		if (pai != "") {
			_front = UIPackage.CreateObject ("baijiale", pai).asCom;
			_front.pivot = new Vector2 (0.5f, 0.5f);
			parent.AddChild (_front);
			_front.displayObject.perspective = _back.displayObject.perspective;
		}

		Visible = Visible;
	}

	public bool Visible{
		get{ 
			return _back.visible;
		}
		set{ 
			_back.visible = value;
			if (_front != null) {
				_front.visible = value;
			}
		}
	}

	public bool opened
	{
		get
		{
			return isOpen;
		}

		set
		{
			isOpen = value;

			if (DOTween.IsTweening(this))
				DOTween.Kill(this);

			if (_front != null) {
				_front.visible = value;
			}

			_back.visible = !value;
		}
	}

	public void SetPerspective()
	{
		if (_front != null) {
			_front.displayObject.perspective = true;
		}
		_back.displayObject.perspective = true;
	}

	public void Turn()
	{
		if (DOTween.IsTweening(this))
			return;

		bool toOpen = !opened;
		DOTween.To(() => 0, x =>
			{
				if (toOpen)
				{
					_back.rotationY = x;
					_front.rotationY = -180 + x;
					if (x > 90)
					{
						_front.visible = true;
						_back.visible = false;
					}
				}
				else
				{
					_back.rotationY = -180 + x;
					_front.rotationY = x;
					if (x > 90)
					{
						_front.visible = false;
						_back.visible = true;
					}
				}
			}, 180, 0.8f).SetTarget(this).SetEase(Ease.OutQuad);
	}
}
