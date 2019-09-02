using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using strange.extensions.signal.impl;



public class GameSocket
{
    public const short HEADSIZE = 20;

    public const int MAXRETRYTTIMES = 3;

	public const int SOCKET_CONNOUT_TIMES = 3000;

	Thread connectThread;

	bool runConnect = true;

	Thread receiveThread;

	bool runReceive = true;

	bool runSend = false;

	bool IsAppPaused = false;

	private IPEndPoint ipEndpoint;

	bool HasRequested = false; //是否发过请求

    //Socket客户端对象     
	private Socket gameSocket;

    //在与服务器交互的时候会传递这个结构体     
    //当客户端接到到服务器返回的数据包时，我把结构体add存在链表中。     
    public Queue<Lopackage> rcevPackages = new Queue<Lopackage>();

    public Queue<Sendpackage> sendPackages=new Queue<Sendpackage>();

    private byte[] buffer = null;  //作为缓存只供接收线程使用

    public readonly object _syncObj = new object();

    public readonly object _syncSendObj = new object();

    private bool isConnecting = false;
    
	private static GameSocket instance;

	GameSocket(){}

	public static GameSocket GetInstance()
    {
        if (instance == null)
        {
			instance = new GameSocket();
        }
        return instance;
    }

	public void Connect(String ip=null,int port=0)
    {
		if (ip != null && port != 0) {
			ipEndpoint = Hostname2ip (ip, port);
		}
        if (!isConnecting)
        {
			Debug.Log("Game DoConnect ReTryTimes:" + Global.GameTryReConnTimes);
            //连接服务器
			HasRequested = true;
            isConnecting = true;
            connectThread = new Thread(new ThreadStart(DoConnect));
            connectThread.IsBackground = true;
            runConnect = true;
            connectThread.Start();
        }
    }

    void DoConnect()
    {
		if (ipEndpoint != null) {
			//创建Socket对象， 这里我的连接类型是TCP     
			gameSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			//这是一个异步的建立连接，当连接建立成功时调用connectCallback方法     
			IAsyncResult result = gameSocket.BeginConnect (ipEndpoint, new AsyncCallback (connectCallback), gameSocket);
			//这里做一个超时的监测，当连接超过5秒还没成功表示超时     
			bool success = result.AsyncWaitHandle.WaitOne (SOCKET_CONNOUT_TIMES, true);  //这里会造成线程阻塞
			if (!success) {
				//超时  
				Debug.Log ("Game Connect Time Out");
				isConnecting = false;
				if (runConnect)
					OnSocketProblem ();
			}
			isConnecting = false;
		}
    }

    IPEndPoint Hostname2ip(string hostname,int port)
    {
        List<IPEndPoint> ipList = new List<IPEndPoint>();
        try
        {
            IPAddress ip;
            if (IPAddress.TryParse(hostname, out ip))
                ipList.Add(new IPEndPoint(ip,port));
            else
            {
                for (int i = 0; i < Dns.GetHostEntry(hostname).AddressList.Length; i++)
                {
                    ipList.Add(new IPEndPoint(Dns.GetHostEntry(hostname).AddressList[i], port));
                }

            }
            return ipList[0];
        }
        catch (Exception)
        {
			throw new Exception("Game IP Address Error");
        }
    }

    private void connectCallback(IAsyncResult asyncConnect)
    {
        try
        {
            gameSocket.EndConnect(asyncConnect);
        }
        catch (SocketException e)
        {
			Debug.Log("Game ClientSocket EndConnect Error." + e);
            isConnecting = false;
            OnSocketProblem();
        }

        if (gameSocket.Connected)
        {
			Debug.Log("Game Socket ConnectSuccess "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            ReSetConnectPara();
               
            receiveThread = new Thread(new ThreadStart(ReceiveSorket));
            receiveThread.IsBackground = true;
            runReceive = true;
            receiveThread.Start();

            runSend = true;
        }
    }

    private void ReceiveSorket()
    {
        //在这个线程中接受服务器返回的数据     
        while (runReceive){
            if (gameSocket==null||!gameSocket.Connected){
				Debug.Log("Game Failed to ClientSocket Server.");
                break;
            }
            try{
                byte[] bytesRcv = new byte[4096];
                int size = gameSocket.Receive(bytesRcv);
                if (size <= 0){
                    gameSocket.Close();
                    break;
                }
                byte[] bytes = new byte[size];
                Array.Copy(bytesRcv, 0, bytes, 0, bytes.Length);
                SplitPackage(bytes);
            }catch (Exception e){
				Debug.Log("Game Failed to clientSocket error." + e);
                OnSocketProblem();
                break;
            }
        }
    }

    private void SplitPackage(byte[] bytespara)
    {
        byte[] bytes;
        if (buffer != null && buffer.Length > 0){
            bytes = new byte[buffer.Length + bytespara.Length];
            Array.Copy(buffer, 0, bytes, 0, buffer.Length);
            Array.Copy(bytespara, 0, bytes, buffer.Length, bytespara.Length);
            buffer = null;
        }else{
            bytes = bytespara;
        }

        int index = 0;
        while (true)
        {

            if (bytes.Length - index < HEADSIZE)  //剩下的byte数不够一个包头长度  
            {
                //如果包头为0表示没有包了，那么跳出循环    
                buffer = new byte[bytes.Length - index];
                Array.Copy(bytes, index, buffer, 0, buffer.Length);
                //Debug.LogWarning("不够包头跳出循环 buffer大小: " + buffer.Length);
                break;
            }

            //包头    
            byte[] head = new byte[HEADSIZE];
            int headLengthIndex = index + HEADSIZE;
            //把数据包的包头拷贝出来     
            Array.Copy(bytes, index, head, 0, HEADSIZE);
            NetMessageHead headobj = new NetMessageHead();
            headobj = (NetMessageHead)LoSocket.BytesToStruct(head, headobj.GetType());
            short dataLength = (short)(headobj.uMessageSize- HEADSIZE);

            //当包体的长度大于0 那么需要依次把相同长度的byte数组拷贝出来 作为包体解析               
            if (dataLength > 0)
            {
                if (bytes.Length - headLengthIndex < dataLength)  ////不够解析包体 这种情况为了处理方便连之前的包头也要放到缓存里
                {
                    buffer = new byte[bytes.Length - index];

                    Array.Copy(bytes, index, buffer, 0, buffer.Length);
                    //Debug.LogWarning("跳出循环 buffer大小: " + buffer.Length);
                    break;
                }
            }
            //将索引指向下一个包的包头     
            index = headLengthIndex + dataLength;

            // 这里将包头和包存在链表中
            Lopackage package = new Lopackage();
            package.head = headobj;
            if (dataLength > 0)
            {
                package.objectData = new byte[dataLength];
                Array.Copy(bytes, headLengthIndex, package.objectData, 0, dataLength);
            }
			Debug.LogWarning("接收到数据： Game mainId: "+headobj.bMainID +" bAssistantID: "+headobj.bAssistantID+ "  uMessageSize:" + headobj.uMessageSize+"  dataSize:" + bytes.Length);
            
            lock (_syncObj)
            rcevPackages.Enqueue(package);
        }
    }

    //按顺序发送数据包
    public void SendPkgQueue()
    {
		if (!runSend)
			return;
		
        if (gameSocket == null || !gameSocket.Connected)
        {
			Debug.Log("Game SendPkgQueue " + gameSocket.Connected+" "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            OnSocketProblem();
            return;
        }
 
        if (sendPackages.Count > 0) 
        {
            lock (_syncSendObj)
            {
                Sendpackage sendpkg = sendPackages.Peek();
                if (sendpkg != null)
                {
                    try
                    {
						Debug.Log("Game start send packege mainId:" + sendpkg.msgId+" assId:"+sendpkg.assId);
                        byte[] newByte=null;
                        if (sendpkg.msgPackge != null)  //有包体 包头
                        {
							byte[] head = LoSocket.StructToBytes(GenerateHead(sendpkg));
                            //把结构体对象转换成数据包，也就是字节数组    
                            object sendobj = sendpkg.msgPackge;
							byte[] data = LoSocket.StructToBytes(sendobj);

                            newByte = new byte[head.Length + data.Length];
                            Array.Copy(head, 0, newByte, 0, head.Length);
                            Array.Copy(data, 0, newByte, head.Length, data.Length);
                        }
                        else  //只有包头
                        {
                            NetMessageHead headObj = new NetMessageHead();
                            headObj.bMainID = sendpkg.msgId;
                            headObj.uMessageSize = 20;
                            headObj.bAssistantID = sendpkg.assId;
                            headObj.bHandleCode = 0;
							//bReserve=7891 表示IOS发送，bReserve=7892表示安卓发送
							#if UNITY_ANDROID
								headObj.bReserve = 7892;
							#elif UNITY_IPHONE
								headObj.bReserve = 7891;
							#endif
							newByte = LoSocket.StructToBytes(headObj);

                        }
                        //计算出新的字节数组的长度     
                        int length = newByte.Length;

                        //向服务端异步发送这个字节数组    
						gameSocket.SendTimeout = 5000;
                        int sendLength = gameSocket.Send(newByte, 0, length, SocketFlags.None);

                        if (sendLength < length)
                        {
                            gameSocket.Close();
							Debug.LogError("Game sendLength<length !");
                            return;
                        }
                        sendPackages.Dequeue();
                    }
                    catch (Exception e)
                    {
                        OnSocketProblem();
						Debug.Log("Game send message error: " + e);
                        return;
                    }
                }else{
                    sendPackages.Dequeue();
                }
            }
        }
    }

    //向服务端发送数据包，也就是一个结构体对象     
    public void SendMessage(uint mainId ,uint assId , object obj=null)
    {
        //todo 这里要防止按钮的多次点击造成重新发送
        Sendpackage pkg = new Sendpackage();
        pkg.msgId = mainId;
        pkg.assId = assId;
        pkg.msgPackge = obj;
        lock (_syncSendObj)
        {
            //登录协议去重
            if (mainId == 100 && assId == 5)
            {
                Sendpackage[] packegeArr = sendPackages.ToArray();
                for (int i = 0; i < packegeArr.Length; i++)
                {
                    if (packegeArr[i].msgId == pkg.msgId && packegeArr[i].assId == pkg.assId)
                    {
                        sendPackages.Clear();
                    }
                }
            }
			else //由于登录会在重连线程里所以登录协议的loading 另行处理
			{
				if (Application.internetReachability == NetworkReachability.NotReachable)
				{
					EventMgr.ins.DispEvent("1000_02",null);//因为是主线程调用，所以可以正常使用
					return;
				}
			}
            sendPackages.Enqueue(pkg);
        }
 
    }

    private void sendCallback(IAsyncResult asyncSend)
    {
    }

    //向服务端发送数据包，也就是一个结构体对象     
    public void SendHeartbeat()
    {
        if (gameSocket == null || !gameSocket.Connected)
        {
            return;
        }
        try
        {
            NetMessageHead headObj = new NetMessageHead();
            headObj.bMainID = 1;
            headObj.uMessageSize = 20;
            headObj.bAssistantID = 1;
            headObj.bHandleCode = 0;
			//bReserve=7891 表示IOS发送，bReserve=7892表示安卓发送
			#if UNITY_ANDROID
				headObj.bReserve = 7892;
			#elif UNITY_IPHONE
				headObj.bReserve = 7891;
			#endif
            byte[] head = LoSocket.StructToBytes(headObj);
            //计算出新的字节数组的长度     
            int length = head.Length;
            //向服务端异步发送这个字节数组     
            IAsyncResult asyncSend = gameSocket.BeginSend(head, 0, length, SocketFlags.None, new AsyncCallback(sendCallback), gameSocket);
        }
        catch (Exception e)
        {
			Debug.Log("Game send message error: " + e);
        }
    }

    //生成包头结构体
    private object GenerateHead(Sendpackage pkg)
    {
        NetMessageHead head = new NetMessageHead();
        head.uMessageSize = (uint)(Marshal.SizeOf(pkg.msgPackge) +HEADSIZE);
        head.bMainID = pkg.msgId;
        head.bAssistantID = pkg.assId;
		//bReserve=7891 表示IOS发送，bReserve=7892表示安卓发送
		#if UNITY_ANDROID
			head.bReserve = 7892;
		#elif UNITY_IPHONE
			head.bReserve = 7891;
		#endif
        return head;
    }

    //Socket出现异常     
    public void OnSocketProblem()
    {
		if (!IsAppPaused && HasRequested) {
			CloseConnet ();	
			ReTryConnect ();
		}
    }

    void ReTryConnect()
    {
		if (!IsAppPaused && HasRequested && !isConnecting)
        {
			if (Global.GameTryReConnTimes < MAXRETRYTTIMES) //重试
            {
				Global.GameTryReConnTimes++;
                Connect();
            }
        }
    }

    public void ManualShutDown()
    {
		Debug.Log("Game clientSocket ManualShutDown");
        CloseConnet();
        ReSetConnectPara();
		HasRequested = false;
        isConnecting = false;
        runConnect = false;
    }

    void CloseConnet()
	{   
        lock (_syncSendObj)
        {
            sendPackages.Clear();
        }
		runSend = false;
		runReceive = false;

        if (gameSocket != null)
        {
            if (gameSocket.Connected)
                gameSocket.Shutdown(SocketShutdown.Both);
            gameSocket.Close();
        }
        gameSocket = null;
    }

   	public void ReSetConnectPara()
    {
		Global.GameTryReConnTimes = 0;
    }
		
    public void Pause()
    {
        IsAppPaused = true;
        CloseConnet();
        isConnecting = false;
        runConnect = false;
    }

    public void Resume()
    {
        IsAppPaused = false;
		if (ipEndpoint!=null&&HasRequested)
		{
			NetworkManager.Instance.ReLogin();
			Connect();
		}
    }

    public bool Isconnected()
    {
        return gameSocket.Connected;
    }
}