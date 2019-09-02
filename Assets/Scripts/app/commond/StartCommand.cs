using System;
using System.Collections;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;


class StartCommand:Command
{
    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject contextview { get; set; }

    public StartCommand()
    {
    }

    public override void Execute()
    {

		GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("GlobelPanel/Network");
		GameObject panel = GameObject.Instantiate(asset) as GameObject;
		panel.transform.localScale = Vector3.one;

		asset = (GameObject)AssetManager.Instance.LoadPrefab("GlobelPanel/Sound");
		panel = GameObject.Instantiate(asset) as GameObject;
		panel.transform.localScale = Vector3.one;


        GameObject go = new GameObject();
        go.name = "LoginView";
        go.AddComponent<LoginView>();
        go.transform.parent = contextview.transform;
	 
		if(LoSocket.GetInstance().Isconnected())
			go.SetActive (false);
		

        GameObject msgGo = new GameObject();
        msgGo.name = "MessageView";
        msgGo.AddComponent<MessageView>();
        msgGo.transform.parent = contextview.transform;

        GameObject mainGo = new GameObject();
        mainGo.name = "MainView";
        mainGo.AddComponent<MainView>();
        mainGo.transform.parent = contextview.transform;
    }
}