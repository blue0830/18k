using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		if (!Global.IsStarted) {
			Global.IsStarted = true;
			GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("GlobelPanel/ContextView");
			GameObject panel = GameObject.Instantiate(asset) as GameObject;
			panel.transform.localScale = Vector3.one;
			panel.name = "ContextView";
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
