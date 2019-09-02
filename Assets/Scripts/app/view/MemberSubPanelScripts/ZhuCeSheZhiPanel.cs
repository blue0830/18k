using UnityEngine;
using System.Collections;
//会员资料
public class ZhuCeSheZhiPanel : UserSubPanelBase {
	 
	//按钮
	public GameObject ReturnBtn;
 

	void Start () {
		UIEventListener.Get(ReturnBtn).onClick = OnReturn;
 
	}

	// Update is called once per frame
	void Update () {
	}

	void OnReturn(GameObject go)
	{
		AudioController.Instance.SoundPlay("active_item");
		gameObject.SetActive(false);
	}

	 
}
