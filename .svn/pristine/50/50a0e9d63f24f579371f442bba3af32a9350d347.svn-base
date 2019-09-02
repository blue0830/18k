using UnityEngine;
using System.Collections.Generic;

public class GongGaoPanel : MonoBehaviour {
    public UILabel Label;

    GongGaoFinishDelegate ggFinish;

    string text;

    public void SetData(string str, GongGaoFinishDelegate callback)
    {
        text = str;
        ggFinish = callback;
    }

    void Destory()
    {
        ggFinish();
        Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        Label.text = text;
        TweenPosition tw = gameObject.AddComponent<TweenPosition>();
        tw.from = new Vector3(640 + Label.localSize.x, 0, 0);
        tw.to = new Vector3(-640, 0, 0);
        tw.duration = (Label.localSize.x+640)/100;
        EventDelegate.Add(tw.onFinished, Destory);
    }
	
	// Update is called once per frame
	void Update () {
	}
}