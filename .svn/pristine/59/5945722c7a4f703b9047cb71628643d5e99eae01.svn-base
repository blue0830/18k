using UnityEngine;
using System.Collections;

public class XiaJiTouZhuMingXiItem : MonoBehaviour {

    public UILabel[] Labels;
 
    public GameObject mingxi;

	void Start () {

        UIEventListener.Get(mingxi).onClick = Onmingxi;
    }

	// Update is called once per frame
	void Update () {


	}

	public void FillData(RecordLookItemObj obj)
	{
        for (int i = 0; i < Labels.Length; ++i)
        {
            Labels[i].text = obj.data[i];
        }
	}

    void Onmingxi(GameObject go)
    {
        NetworkManager.Instance.TouchuMingxi(int.Parse(Labels[0].text));
    }
}
