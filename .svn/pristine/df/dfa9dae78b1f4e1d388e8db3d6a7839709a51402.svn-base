using UnityEngine;
using System.Collections.Generic;

public class ModePanel : MonoBehaviour {

    public GameObject[] modeSelections;
    ModeSubpanel currentscript;
    //subUI

    //80  60
    public List<UIToggle> modeToggles = new List<UIToggle>();

    public void Show(int currentModeId,int lotteryType)
    {
        modeToggles.Clear();

        int index = lotteryType-1;

        for (int i = 0; i < modeSelections.Length; ++i)
        {
            if (i == index)
            {
                modeSelections[i].SetActive(true);
                currentscript = modeSelections[i].GetComponent<ModeSubpanel>();
            }
            else
            {
                modeSelections[i].SetActive(false);
            }
        }

        SubModeItemNew[] smItems = currentscript.SubModeitems;

        for (int i = 0; i < smItems.Length; i++)
        {
            for (int j = 0; j < smItems[i].subModeObjs.Length; ++j)
            {
                modeToggles.Add(smItems[i].subModeObjs[j]);
            }
        }
         


        gameObject.SetActive(true);
    }

    public bool IsShowing()
    {
        for (int i = 0; i < modeSelections.Length; ++i)
        {
            if (modeSelections[i].activeSelf)
                return true;

        }
        return false;
    }

 
	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
