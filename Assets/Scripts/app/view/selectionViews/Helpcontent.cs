using UnityEngine;
using System.Collections;

public class Helpcontent : MonoBehaviour {

    public GameObject leftarrow;
    public GameObject rightarrow;
    public GameObject middlearrow;

    public UILabel textlabel;
    public GameObject bgbtn;

	// Use this for initialization
	void Start () {
        UIEventListener.Get(bgbtn).onClick = close;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //type 1 help 2 exp 3award
    public void Show(int type,string str)
    {
        if (type == 1)
        {
            leftarrow.SetActive(false);
            rightarrow.SetActive(true);
            middlearrow.SetActive(false);
        }
        else if (type == 2)
        {
            leftarrow.SetActive(true);
            rightarrow.SetActive(false);
            middlearrow.SetActive(false);
        }
        else
        {
            leftarrow.SetActive(false);
            rightarrow.SetActive(false);
            middlearrow.SetActive(true);
        }

        textlabel.text = str;
        gameObject.SetActive(true);

    }

    void close( GameObject go)
    {
        gameObject.SetActive(false);
    }
}
