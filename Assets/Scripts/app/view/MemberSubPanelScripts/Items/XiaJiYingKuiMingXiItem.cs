using UnityEngine;
using System.Collections;

public class XiaJiYingKuiMingXiItem : MonoBehaviour {

    public UILabel[] Labels;

	void Start () {


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
}
