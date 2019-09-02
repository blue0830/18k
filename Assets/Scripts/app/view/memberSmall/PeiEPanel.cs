using UnityEngine;
using System.Collections;

public class PeiEPanel : MonoBehaviour {

    public UILabel _30show;
    public UIInput _30input;

	public GameObject obj;
	public GameObject obj2;

    public UILabel _31show;
    public UIInput _31input;

    public GameObject btn;
    public GameObject returnBtn;

    public UILabel title;

    MSG_GP_USER_GETPLAYPE savepara;

    public void Show(MSG_GP_USER_GETPLAYPE para)
    {
        savepara = para;
        title.text = string.Format("修改配额({0})", para.dwUserID);
		if (para.iCountPe == 2) {
			obj.SetActive (true);
			obj2.SetActive (true);
			_30show.text = string.Format ("3.0配额分配(您的剩余配额{0})", para.mePoint27.ToString ());
			_31show.text = string.Format ("3.1配额分配(您的剩余配额{0})", para.mePoint28.ToString ());
			_30input.value = para.Point27+"";
			_31input.value = para.Point28+"";
		} else {
			_30show.text = string.Format ("3.0配额分配(您的剩余配额{0})", para.mePoint27.ToString ());
			_30input.value = para.Point27+"";
			obj.SetActive (true);
		}
        gameObject.SetActive(true);
    }
    // Use this for initialization
    void Start () {
        UIEventListener.Get(btn).onClick = OnChange;
        UIEventListener.Get(returnBtn).onClick = OnReturn;
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnChange(GameObject go)
    {
        NetworkManager.Instance.ChangePeiE(savepara.dwUserID, int.Parse(_30input.value), int.Parse(_31input.value));
    }

    void OnReturn(GameObject go)
    {
        gameObject.SetActive(false);
    }
}
