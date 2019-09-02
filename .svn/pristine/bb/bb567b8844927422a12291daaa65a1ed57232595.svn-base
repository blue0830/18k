using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Reflection;

public class UntiySocket : MonoBehaviour {

    LoSocket socket = null;

    public LotteryRoot root = null;
    LotteryContext context = null;

    MsgConfigLoader msgCfgLoader = null;

    //游戏config
	MsgConfigLoader baccaratMsgConfigLoader = null;

    GameSocket gameSocket = null;

    void Awake()
    {
        TextAsset TXTFile = Resources.Load("Config/msgConfig") as TextAsset;
        msgCfgLoader = XmlHelper.XmlDeserialize<MsgConfigLoader>(TXTFile.text, System.Text.Encoding.UTF8);

        //游戏config去load
		TXTFile = Resources.Load("Config/baccaratMsgConfig") as TextAsset;
        baccaratMsgConfigLoader = XmlHelper.XmlDeserialize<MsgConfigLoader>(TXTFile.text, System.Text.Encoding.UTF8);
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
		root = GameObject.Find("ContextView").GetComponent<LotteryRoot>();
        context = (LotteryContext)root.context;   //这种方法不太好，以后再改进
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    void FixedUpdate()
    {
        ProcessSocketMsg();
		ProcessGameSocketMsg ();
    }

    void ProcessSocketMsg()
    {
        if (socket == null)
        {
            socket = LoSocket.GetInstance();
        }

        if (socket.rcevPackages != null && socket.rcevPackages.Count > 0)
        {
            Lopackage package = null;
            lock (socket.syncObj)
            {
                package = socket.rcevPackages.Dequeue();
            }
            AnalzyPackage(package);
        }
    }

	void ProcessGameSocketMsg()
	{
		if (gameSocket == null)
		{
			gameSocket = GameSocket.GetInstance();
		}

		gameSocket.SendPkgQueue();

		if (gameSocket.rcevPackages != null && gameSocket.rcevPackages.Count > 0)
		{
			Lopackage package = null;
			lock (socket.syncObj)
			{
				package = gameSocket.rcevPackages.Dequeue();
			}
			AnalzyPackage(package,true);
		}
	}

    void AnalzyPackage(Lopackage pkg, bool isGameSocket=false)
    {
        if (pkg == null)
        {
            return;
        }
        NetMessageHead head = pkg.head;
        //获取消息配置
        MsgConfig msgCfg = msgCfgLoader.GetMsgConfig(head.bMainID);
        if (isGameSocket)
        {
			switch (Global.CurrentGameId)
            {
				case 10301800://百家乐
                if (baccaratMsgConfigLoader != null)
                {
                    msgCfg = baccaratMsgConfigLoader.GetMsgConfig(head.bMainID);
                }
                break;
            }

        }
        if (msgCfg == null)
        {
			if(isGameSocket)
				Debug.LogWarning("this messageid: " +head.bMainID+" this bAssistantID: " + head.bAssistantID + " doesn't has its config");
            return;
        }
        MsgSubCfg subCfg = msgCfg.GetMsgSubCfg(head.bAssistantID);
        if (subCfg == null)
        {
			if(isGameSocket)
				Debug.LogWarning("this messageid: " +head.bMainID+" this bAssistantID: " + head.bAssistantID + " doesn't has its config");
            return;
        }
        //根据消息配置 获得处理类 以及 返回数据类型
        Type rspType = Type.GetType(subCfg.rspType);
        if (rspType == null)
        {
			Debug.LogError("this messageid: " +head.bMainID+" this bAssistantID: " + head.bAssistantID + " do not have subCfg.rspType");
        }
        Type InterfaceType = typeof(IHandler<>);
        InterfaceType = InterfaceType.MakeGenericType(rspType);

        Type handlerType = Type.GetType(subCfg.handler);
        if (handlerType == null)
        {
			Debug.LogError("this messageid: " +head.bMainID+" this bAssistantID: " + head.bAssistantID + " subCfg.handler error");
        }
        object obj =  Activator.CreateInstance(handlerType);
        //将处理类注入
        context.injectionBinder.injector.Inject(obj);
        //调用处理类的 回调
        object[] args = new object[2];
        args[0] = head;
        if (subCfg.hasData)
        {
            if (pkg.objectData == null)
            {
				Debug.LogError("this messageid: " +head.bMainID+" this bAssistantID: " + head.bAssistantID+" pkg.objectData==null");
				args[1] = null;//特殊处理
			}
			else if (head.uMessageSize != pkg.objectData.Length + 20)
			{
				Debug.LogError("this messageid: " +head.bMainID+" this bAssistantID: " + head.bAssistantID+" pkg.head.uMessageSize != pkg.objectData.Length");
				args[1] = null;//特殊处理
			}
			else if (head.uMessageSize == pkg.objectData.Length + 20)
			{
				object dataObj = LoSocket.BytesToStruct(pkg.objectData, rspType);
				args[1] = dataObj;
			}
			else
			{
				Debug.LogError("this messageid: " +head.bMainID+" this bAssistantID: " + head.bAssistantID+" other error");
				args[1] = null;
			}
        }
        else
        {
            args[1] = null;
        }
        InterfaceType.InvokeMember("OnReceive", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, args);
        obj = null;
    }


    void OnDestroy()
    {
        if (socket != null)
        {
            socket.ManualShutDown();
        }
    }

 

    public string getProperties<T>(T t)
    {
        string tStr = string.Empty;
        if (t == null)
        {
            return tStr;
        }
        System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

        if (properties.Length <= 0)
        {
            return tStr;
        }
        foreach (System.Reflection.PropertyInfo item in properties)
        {
            string name = item.Name;
            object value = item.GetValue(t, null);
            if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
            {
                tStr += string.Format("{0}:{1},", name, value);
            }
            else
            {
                tStr += string.Format("{0}:{1},", name, System.Text.Encoding.UTF8.GetString((byte[])value));
                //getProperties(value);
            }
        }
        return tStr;
    }


}
