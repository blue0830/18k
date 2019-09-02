using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using strange.extensions.signal.impl;

public class GameListPanel : MonoBehaviour {
	public Signal<MsgPara> msgSignal = new Signal<MsgPara> ();

	public GameObject ReturnBtn;

	public UIGrid IconGrid;
 
	private List<GameObject> Icons = new List<GameObject>();

	public UIScrollView scorllview;
 
	public void Start()
	{
		UIEventListener.Get (ReturnBtn).onClick = OnReturn;
	}

	void OnReturn(GameObject go)
	{
		ClearIcon();
		gameObject.SetActive (false);
	}
		

	public void CreateIcons(List<ComRoomInfo> ComRoomInfos)
	{
		if (ComRoomInfos == null || ComRoomInfos.Count == 0)
		{
			Debug.LogError("MainPanel: ComRoomInfos == null || ComRoomInfos.Count == 0");
			return;
		}

		ClearIcon();

		GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("GameListItem");

		//创建 Length-1个因为已经存在一个
		for (int i = 0; i < ComRoomInfos.Count; ++i)
		{
			GameObject go = Instantiate(asset);
			go.transform.parent = IconGrid.transform;
			go.transform.localScale = Vector3.one;
			Icons.Add(go);
			Icons[i].SetActive(false);
			ComRoomInfo room = ComRoomInfos [i];
			go.name = room.uRoomID.ToString();
			GamelistItem item = go.GetComponent<GamelistItem> ();
			int count = (int)room.uPeopleCount;
			if (count < 30) {
				item.people.text = "在线人数:空闲";
			} else if (count > 30 && count <= 60) {
				item.people.text = "在线人数:良好";
			} else if (count > 60 && count <= 90) {
				item.people.text = "在线人数:活跃";
			} else if (count > 90 && count <= 120) {
				item.people.text = "在线人数:繁忙";
			} else {
				item.people.text = "在线人数:火爆";
			}
			item.limit.text = System.String.Format("房间限制:{0:0.00}",room.iLessPoint*1.0/100);
			item.comRoomInfo = room;
			UIEventListener.Get (Icons [i]).onClick = OnIconClick;
		}
		IconGrid.repositionNow = true;
		scorllview.ResetPosition();
		StartCoroutine(process());
	}

	IEnumerator process()
	{
		yield return new WaitForEndOfFrame();
		for (int i = 0; i < Icons.Count; ++i)
		{
			Icons[i].SetActive(true);
		}
	}

	void OnIconClick(GameObject go)
	{
		GamelistItem item = go.GetComponent<GamelistItem> ();
		Global.CurrentSelGameRoom = item.comRoomInfo;
		if (Global.user.dwMoney >= item.comRoomInfo.iLessPoint) {
			if(Global.CurrentGameId==10301800){//百家乐游戏
				Loading.GetInstance ().ShowLoading ("游戏加载中......");
				TimeManager.Instance().Register("loadGame", 0, 3000, 3000, (c, t) =>{
					msgSignal.Dispatch(new MsgPara("游戏加载失败",2));
					Loading.GetInstance ().HideLoading();
				});
				SceneManager.LoadScene ("baijiale");
			}
		} else {
			string limitStr = System.String.Format("您的棋牌金额不足:{0:0.00}请兑换或充值",item.comRoomInfo.iLessPoint*1.0/100);
			msgSignal.Dispatch(new MsgPara(limitStr,2));
		}
	}

	void ClearIcon()
	{
		for (int i = 0; i < Icons.Count; ++i)
		{
			Destroy(Icons[i]);
		}
		Icons.Clear ();
	}
		
}
