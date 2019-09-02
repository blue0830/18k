using UnityEngine;
using System.Collections;

public class MessageShow : MonoBehaviour {

    float TimeDestory = 2;

    public UILabel message;
    public float showTime;
	// Use this for initialization
	void Start () {

        showTime = Time.time;

    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time - showTime >= TimeDestory)
        {
            Destroy(gameObject);
        }
	}
}
