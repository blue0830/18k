using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Loading
{
    //单例模式     
    private static Loading instance;
    public static Loading GetInstance()
    {
        if (instance == null)
        {
            instance = new Loading();
        }
        return instance;
    }
    //单例的构造函数     

    GameObject panel;

	LoadingPanel loadingpanel;

    GameObject webViewPanel;
    Loading()
    {
        Transform parent = UIRootFinder.uiRootTran;
        GameObject asset = (GameObject)AssetManager.Instance.LoadPrefab("LoadingPanel");
        panel = GameObject.Instantiate(asset) as GameObject;
		loadingpanel = panel.GetComponent<LoadingPanel>();
        panel.transform.parent = parent;
        panel.transform.localScale = Vector3.one;
        panel.SetActive(false);
        webViewPanel = GameObject.Find("webMediator");
    }

    //Stack<int> loadingStack = new Stack<int>();

    public void ShowLoading()
    {
        //loadingStack.Push(0);
        //panel.SetActive(true);
    }

    public void HideLoading()
    {
       //loadingStack.Pop();
       //if(loadingStack.Count==0)
       panel.SetActive(false);
    }

    public void ShowLoading(string msg)
    {
        //loadingStack.Push(0);
        loadingpanel.elDesc.text = msg;
        panel.SetActive(true);
    }


    public void ShowLoading(int second)
    {
        panel.SetActive(true);
		loadingpanel.elDesc.text = "数据正在加载中.......";
        TimeManager.Instance().Register("HideLoading", 1, 5000, second*1000, (c, t) =>
        {
            panel.SetActive(false);
        });
    }

	public void ShowLoading(int second,EventDelegate.Callback callback)
	{
		panel.SetActive(true);
		loadingpanel.elDesc.text = "数据正在加载中.......";
		TimeManager.Instance().Register("HideLoading", 1, 5000, second*1000, (c, t) =>
		{
			bool flag = panel.activeSelf;
			if(flag){
				panel.SetActive(false);
				callback();
			}
		});
	}

	public void ShowLoading(int second,string msg,EventDelegate.Callback callback)
	{
		panel.SetActive(true);
		loadingpanel.elDesc.text = msg;
        TimeManager.Instance().UnRegister("HideLoading");
		TimeManager.Instance().Register("HideLoading", 1, 5000, second*1000, (c, t) =>
			{
				bool flag = panel.activeSelf;
				if(flag){
					panel.SetActive(false);
					callback();
				}
			});
	}
		
    public void OpenURL(string url)
    {
        string http = "http://";
        if (!url.Contains(http))
        {
            url = http + url;
        }
        //WebMediator webView = webViewPanel.GetComponent<WebMediator>();
        //webView.Show(url);
    }


}
