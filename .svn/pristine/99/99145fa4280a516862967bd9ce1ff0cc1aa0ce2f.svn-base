using UnityEngine;
using System.Collections;

public class ActivityPanel : MonoBehaviour {

 
    public GameObject returnbtn;

    public GameObject _7daybtn;
    public GameObject hongbaobtn;
    public GameObject tuiguangbtn;

    public SevenDayPanel _7daypanel;
    public ChongZHiSongPanel chongzhisongpanel;
    public TuiGuangPanel tuiguangpanel;


    public void Open7dayPanel(MSG_GP_USER_HDZX7TLRULERESULT para)
    {
        _7daypanel.gameObject.SetActive(true);
        _7daypanel.Open7dayPanel(para);
    }

	public void OpenChongZhiSongPanel(MSG_GP_USER_HDZXXRCZSRULET para)
	{
		chongzhisongpanel.gameObject.SetActive(true);
		chongzhisongpanel.OpenChongZhiSongPanel(para);
	}

	public void OpenPermanentPromotionPanel(MSG_GP_USER_HDZXYJTGJLRUSULT para)
	{
		tuiguangpanel.gameObject.SetActive(true);
		tuiguangpanel.OpenTuiGuangPanel(para);
	}

    // Use this for initialization
    void Start () {

        UIEventListener.Get(_7daybtn).onClick = OpenSevenDay;
		UIEventListener.Get(hongbaobtn).onClick = OpenChongZhiSong;
		UIEventListener.Get(tuiguangbtn).onClick = OpenPermanentPromotion;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OpenSevenDay(GameObject go)
    {
        AudioController.Instance.SoundPlay("active_item");
        NetworkManager.Instance.GetSevenDayInfo();
    }

	void OpenChongZhiSong(GameObject go)
	{
        AudioController.Instance.SoundPlay("active_item");
        NetworkManager.Instance.GetChongZhiSongInfo ();
	}

	void OpenPermanentPromotion(GameObject go)
	{
        AudioController.Instance.SoundPlay("active_item");
        NetworkManager.Instance.GetPermanentPromotionInfo ();
	}
}
