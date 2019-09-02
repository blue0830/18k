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

//目前存在的问题2：切到后台时还会处理网络连接但是没有心跳包发送

public class LoSocket
{
    public const short HEADSIZE = 20;
    public const int MAXRETRYTTIMES = 3;
	public const int SOCKET_CONNOUT_TIMES = 3000;

    //Socket客户端对象     
    private Socket clientSocket;
	private bool isConnecting = false;
	private byte[] buffer = null;  //作为缓存只供接收线程使用
	//单例模式     
	private static LoSocket instance;

    //在与服务器交互的时候会传递这个结构体     
    //当客户端接到到服务器返回的数据包时，我把结构体add存在链表中。     
    public Queue<Lopackage> rcevPackages = new Queue<Lopackage>();
    public Queue<Sendpackage> sendPackages=new Queue<Sendpackage>();

    public readonly object syncObj = new object();
    public readonly object syncSendObj = new object();
    public readonly object syncSocket = new object();

	List<IPEndPoint> ips = new List<IPEndPoint>();
	Thread connectThread;
	bool runConnect = true;
	Thread receiveThread;
	Thread sendThread;
	bool runReceive = true;
	bool runSend = true;
	bool IsAppPaused = false;
	bool HasRequested = false; //是否发过请求
	public Signal<MsgPara> msgSigal = new Signal<MsgPara>();
	public Signal connectSignal = new Signal();

    public static LoSocket GetInstance()
    {
        if (instance == null)
        {
            instance = new LoSocket();
        }
        return instance;
    }

    //单例的构造函数     
    LoSocket()
    {
        Init();
    }

    void Connect()
    {
        if (!isConnecting)
        {
			Debug.Log("DoConnect ReTryTimes:" + Global.AppTryReConnTimes + " AppConnIpsIndex:" + Global.AppConnIpsIndex);
            //连接服务器
            isConnecting = true;
            connectThread = new Thread(new ThreadStart(DoConnect));
            connectThread.IsBackground = true;
            runConnect = true;
            connectThread.Start();
        }
    }

    void DoConnect()
    {
        //创建Socket对象， 这里我的连接类型是TCP     
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		IPEndPoint ipEndpoint = ips[Global.AppConnIpsIndex];
        //这是一个异步的建立连接，当连接建立成功时调用connectCallback方法     
        IAsyncResult result = clientSocket.BeginConnect(ipEndpoint, new AsyncCallback(connectCallback), clientSocket);
        //这里做一个超时的监测，当连接超过5秒还没成功表示超时     
		bool success = result.AsyncWaitHandle.WaitOne(SOCKET_CONNOUT_TIMES, true);  //这里会造成线程阻塞
        if (!success)
        {
            //超时  
            Debug.Log("connect Time Out");
            isConnecting = false;
            if(runConnect)
                OnSocketProblem();  
        }
        isConnecting = false;
    }

    void Init()
    {
        AddressConfigLoader loader = ConfigManager.Instance.GetAddressConfigLoader();
        for(int i=0; i<loader.addConfigs.Count;++i)
        {
            AddressConfig ac = loader.addConfigs[i];
            ips.AddRange(Hostname2ip(ac.host, ac.port));
        }
    }

    List<IPEndPoint>  Hostname2ip(string hostname,int port)
    {
        List<IPEndPoint> ipList = new List<IPEndPoint>();
        try
        {
            IPAddress ip;
			if (IPAddress.TryParse(hostname, out ip)){
                ipList.Add(new IPEndPoint(ip,port));
			}else{
                for (int i = 0; i < Dns.GetHostEntry(hostname).AddressList.Length; i++)
                {
                    ipList.Add(new IPEndPoint(Dns.GetHostEntry(hostname).AddressList[i], port));
                }

            }
            return ipList;
        }
        catch (Exception)
        {
            throw new Exception("IP Address Error");
        }
    }

    private void connectCallback(IAsyncResult asyncConnect)
    {
        try
        {
            clientSocket.EndConnect(asyncConnect);
        }
        catch (SocketException e)
        {
            Debug.Log("clientSocket EndConnect error." + e);
            isConnecting = false;
            OnSocketProblem();
        }

        if (clientSocket.Connected)
        {
            Debug.Log("connectSuccess");
			connectSignal.Dispatch ();
            //ReSetConnectPara();//因为刘哥说要保证优先重连当前服务器，所以注释掉
            //与socket建立连接成功，开启线程接受服务端数据。           
            receiveThread = new Thread(new ThreadStart(ReceiveSorket));
            receiveThread.IsBackground = true;
            runReceive = true;
            receiveThread.Start();
            //建立发送线程
            sendThread = new Thread(new ThreadStart(SendPkgQueue));
            sendThread.IsBackground = true;
            runSend = true;
            sendThread.Start();
        }
    }

    private void ReceiveSorket()
    {
        //在这个线程中接受服务器返回的数据     
        while (runReceive)
        {

            if (clientSocket==null||!clientSocket.Connected)
            {
                //与服务器断开连接跳出循环     
                Debug.Log("Failed to clientSocket server.");
                //OnSocketProblem();
                break;
            }
            try
            {
                //接受数据保存至bytes当中     服务器最大包大小加上包头为 3192
                byte[] bytesRcv = new byte[4096];
                //Receive方法中会一直等待服务端回发消息     
                //如果没有回发会一直在这里等着。     
                int size = clientSocket.Receive(bytesRcv);
                if (size <= 0)
                {
                    clientSocket.Close();
                    break;
                }
                //Debug.Log("接收到数据： " + size);
                byte[] bytes = new byte[size];
                Array.Copy(bytesRcv, 0, bytes, 0, bytes.Length);
                SplitPackage(bytes);
            }
            catch (Exception e)
            {
                Debug.Log("Failed to clientSocket error." + e);
                OnSocketProblem();
                break;
            }
        }
    }

    private void SplitPackage(byte[] bytespara)
    {
        //在这里进行拆包，因为一次返回的数据包的数量是不定的     
        byte[] bytes;
        if (buffer != null && buffer.Length > 0)
        {
            bytes = new byte[buffer.Length + bytespara.Length];
            Array.Copy(buffer, 0, bytes, 0, buffer.Length);
            Array.Copy(bytespara, 0, bytes, buffer.Length, bytespara.Length);
            buffer = null;
        }
        else
        {
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
            headobj = (NetMessageHead)BytesToStruct(head, headobj.GetType());
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
            lock (syncObj)
            rcevPackages.Enqueue(package);
        }
    }

    //按顺序发送数据包
    private void SendPkgQueue()
    {
        while (runSend)
        {
            if (clientSocket == null || !clientSocket.Connected)
            {
                Debug.Log("SendPkgQueue " + clientSocket.Connected);
                OnSocketProblem();
                break;
            }
            Thread.Sleep(200);
            if (sendPackages.Count > 0) 
            {
                lock (syncSendObj)
                {
                    Sendpackage sendpkg = sendPackages.Peek();

                    if (sendpkg != null)
                    {
                        try
                        {
                            //Debug.Log("start send packege :" + sendpkg.msgId);
                            byte[] newByte=null;
                            if (sendpkg.msgPackge != null)  //有包体 包头
                            {
                                byte[] head = StructToBytes(GenerateHead(sendpkg));
                                //把结构体对象转换成数据包，也就是字节数组    
                                object sendobj = sendpkg.msgPackge;
                                byte[] data = StructToBytes(sendobj);
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
                                newByte = StructToBytes(headObj);
                            }
                            //计算出新的字节数组的长度     
                            int length = newByte.Length;
                            //向服务端异步发送这个字节数组    
							clientSocket.SendTimeout = 5000;
                            int sendLength = clientSocket.Send(newByte, 0, length, SocketFlags.None);
                            if (sendLength < length)
                            {
                                clientSocket.Close();
                                Debug.LogError("sendLength<length !");
                                break;
                            }
                            sendPackages.Dequeue();
                        }
                        catch (Exception e)
                        {
                            OnSocketProblem();
                            Debug.Log("send message error: " + e);
                            break;
                        }
                    }
                    else
                    {
                        sendPackages.Dequeue();
                    }
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
        lock (syncSendObj)
        {
            //登录协议去重
            if (mainId == 100 && assId == 1)
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
                    msgSigal.Dispatch(new MsgPara("无法连接，请检查您的网络", 2));
                    return;
                }
            }
            sendPackages.Enqueue(pkg);
        }
    }

    private void sendCallback(IAsyncResult asyncSend)
    {
    }

    //结构体转字节数组     
    public static byte[] StructToBytes(object structObj)
    {
        int size = Marshal.SizeOf(structObj);
        IntPtr buffer = Marshal.AllocHGlobal(size);
        try
        {
            Marshal.StructureToPtr(structObj, buffer, true);
            byte[] bytes = new byte[size];
            Marshal.Copy(buffer, bytes, 0, size);
            return bytes;
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    //字节数组转结构体     
    public static object BytesToStruct(byte[] bytes, Type strcutType)
    {

		if (strcutType.IsArray) {
			Type eleType=strcutType.GetElementType ();
			int eleSize = Marshal.SizeOf(eleType);
			if (bytes.Length % eleSize != 0) {
				Debug.Log("buffer is Array and Length is not correct");
				return null;
			}
			int arraySize = bytes.Length / eleSize;
			Array newArr=Array.CreateInstance (eleType, arraySize);

			for (int i = 0; i < arraySize; ++i) {
				IntPtr buffer = Marshal.AllocHGlobal(eleSize);
				try
				{
					Marshal.Copy(bytes, i*eleSize, buffer, eleSize);
					if (buffer == null)
					{
						Debug.Log("buffer is null");
					}
					newArr.SetValue(Marshal.PtrToStructure(buffer, eleType),i);
				}
				catch (Exception e)
				{
					Debug.LogError("Exception: strcutType "+ eleType.ToString() + " eleSize ：" + eleSize + " message ：" + e.Message +" stack "+e.StackTrace);
					return null;
				}
				finally
				{
					Marshal.FreeHGlobal(buffer);
				}
			}

			return newArr;

		} else {
			int size = Marshal.SizeOf(strcutType);
			IntPtr buffer = Marshal.AllocHGlobal(size);
			try
			{
				Marshal.Copy(bytes, 0, buffer, size);
				if (buffer == null)
				{
					Debug.Log("buffer is null");
				}
				return Marshal.PtrToStructure(buffer, strcutType);
			}
			catch (Exception e)
			{
				Debug.LogError("Exception: strcutType "+ strcutType.ToString() + " Size ：" + size + " message ：" + e.Message +" stack "+e.StackTrace);
				return null;
			}
			finally
			{
				Marshal.FreeHGlobal(buffer);
			}

		}

      
    }

    //向服务端发送数据包，也就是一个结构体对象     
    public void Heartbeat()
    {
        if (clientSocket == null || !clientSocket.Connected)
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
            byte[] head = StructToBytes(headObj);
            //计算出新的字节数组的长度     
            int length = head.Length;
            //向服务端异步发送这个字节数组     
            IAsyncResult asyncSend = clientSocket.BeginSend(head, 0, length, SocketFlags.None, new AsyncCallback(sendCallback), clientSocket);
        }
        catch (Exception e)
        {
            //OnSocketProblem();
            Debug.Log("send message error: " + e);
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
			Global.IsLoginApp = false;
			CloseConnet ();	
			ReTryConnect ();//原先是在if外面的
		}
    }

    void ReTryConnect()
    {
        if (!IsAppPaused && HasRequested&& !isConnecting)
        {
			if (Global.AppTryReConnTimes < MAXRETRYTTIMES) //重试
            {
				if (Global.AppConnIpsIndex == ips.Count - 1) {
					Global.AppTryReConnTimes++;
					Global.AppConnIpsIndex = 0;
				} else {
					Global.AppConnIpsIndex++;
				}
                NetworkManager.Instance.ReLogin();
                Connect();
            }
            else
            {
                Debug.Log("clientSocket CLosed and will stop retry");
                msgSigal.Dispatch(new MsgPara("连接服务器超时，是否重试？", 1,() => {ManualConnect(); NetworkManager.Instance.ReLogin(); }));
            }
        }
    }

    public void ManualShutDown()
    {
        CloseConnet();
        ReSetConnectPara();
        HasRequested = false;
        isConnecting = false;
        runConnect = false;
    }

    void CloseConnet()
    {
        lock (syncSendObj)
        {
            sendPackages.Clear();
        }
        runSend = false;
        runReceive = false;

        if (clientSocket != null)
        {
            if (clientSocket.Connected)
                clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
        clientSocket = null;
    }

    public void ManualConnect()
    {
        HasRequested = true;
        ReSetConnectPara();
        Connect();
    }

	public void ReSetConnectPara()
    {
		Global.AppTryReConnTimes = 0;
		System.Random random = new System.Random();
		Global.AppConnIpsIndex = random.Next(ips.Count);
		Global.AppConnIpsIndex = PlayerPrefs.GetInt("ipListIndex", Global.AppConnIpsIndex);//查询是否保存最近连接服务器的index
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
        if (HasRequested)
        {
            NetworkManager.Instance.ReLogin();
            Connect();
        }
    }
		
    public bool Isconnected()
    {
		if (clientSocket != null)
			return clientSocket.Connected;
		else
			return false;
    }
}