using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

public class TestSocket : MonoBehaviour {

    LoSocket socket = null;
    // Use this for initialization
    void Start () {
        socket = LoSocket.GetInstance();

        MSG_GP_R_LogonResult test1 = new MSG_GP_R_LogonResult();
        //str = Marshal.SizeOf(test1).ToString();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    string stringToEnter = "";
    void OnGUI()
    {

        GUI.contentColor = Color.gray;// 输入窗口文字颜色为灰色
        
        stringToEnter = GUI.TextField(new Rect(0, 0, 100, 20), stringToEnter, 25);//确定输入窗口位置

        GUI.contentColor = Color.yellow; //两个按钮的文字的颜色
        if (Input.GetMouseButtonDown(0) && stringToEnter.Equals("Enter Code"))
        {//点击鼠标之后，输入窗口的内容消失
            stringToEnter = GUI.TextField(new Rect(0, 0, 100, 20), "", 25);
        }
        if (Input.GetKeyDown(KeyCode.Return) || GUI.Button(new Rect(0, 200, 500, 200), "确定"))
        {
            
            test2();
            //test();
            //TextAsset TXTFile = Resources.Load("Config/msgConfig") as TextAsset;
            //MsgConfigLoader xl = XmlHelper.XmlDeserialize<MsgConfigLoader>(TXTFile.text, System.Text.Encoding.UTF8);

            //socket.SendMessage(data);

            //MSG_GP_R_LogonResult data = new MSG_GP_R_LogonResult();

            //data.dwBank = 5471;
            //data.dwUserID = 1222;

            //int size = Marshal.SizeOf(data);
            //byte[] bytes = new byte[size];

            //IntPtr buffer = Marshal.AllocHGlobal(size);

            //Marshal.StructureToPtr(data, buffer, true);
            //Marshal.Copy(buffer, bytes, 0, size);


            //IntPtr bufferaccpt = Marshal.AllocHGlobal(size);

            //Marshal.Copy(bytes, 0, bufferaccpt, size);

            //MSG_GP_R_LogonResult test = new MSG_GP_R_LogonResult();
            //Marshal.PtrToStructure(bufferaccpt, test);

            //str = "dwbank: "+test.dwBank+" userid "+test.dwUserID;
            //Debug.Log(str);
            //PluginTool.add(5, 8);


            //TextAsset TXTFile = Resources.Load("Config/lotteryConfig") as TextAsset;
            //LotteryConfigLoader lCfgLoader = XmlHelper.XmlDeserialize<LotteryConfigLoader>(TXTFile.text, System.Text.Encoding.UTF8);

            //int i = 6;

            //string[] arr = Regex.Split("123gb213%213.213,234", @"^\d+");
            //for (int i = 0; i < arr.Length; ++i)
            //{
            //    str += arr[i] + "|||";
            //}


        }
        GUI.Label(new Rect(60, 60, 250, 50), str);

  
    }

    void Split(string ori, ref List<char[]> list, int SplitLen)
    {

        if (ori.Length > SplitLen)
        {
            int subLength = ori.Length - SplitLen;
            string subStr = ori.Substring(SplitLen, subLength);
            char[] subContent = subStr.ToCharArray();
            ori = ori.Substring(0, SplitLen);
            Split(subStr, ref list, SplitLen);

        }

        list.Add(ori.ToCharArray());
    }

    string str = "";
 

    void test()
    {
        MsgConfigLoader xl = new MsgConfigLoader();

        xl.msgConfigs = new List<MsgConfig>();

        for (int i = 0; i < 5; ++i)
        {
         
            MsgConfig scfg = new MsgConfig();
            scfg.mainId = 100;   
            scfg.name = "MDM_GP_LOGON";
     

            scfg.subcfgs = new List<MsgSubCfg>();
            for (int j = 0; j < 2; ++j)
            {
                MsgSubCfg sub = new MsgSubCfg();
                sub.assId = 1;
                sub.type = 0;
                sub.hasData = true;
                sub.rspType = "MSG_GP_R_LogonResult";
                sub.handler = "MDM_GP_LOGON_Handler";
                scfg.subcfgs.Add(sub);
            }
            xl.msgConfigs.Add(scfg);
        }
      
        XmlHelper.XmlSerializeToFile(xl, "G:/UnityProject/LotterySvn/trunk/Assets/test.xml", System.Text.Encoding.UTF8);
    }


    void test1()
    {
        LotteryConfigLoader xl = new LotteryConfigLoader();

        xl.lotteryConfigs = new List<LotteryConfig>();

        for (int i = 0; i < 2; ++i)
        {

            LotteryConfig scfg = new LotteryConfig();
            scfg.lotteryId = 100;
            scfg.name = "时时彩";


            scfg.modecfgs = new List<LotteryModeCfg>();
            for (int j = 0; j < 2; ++j)
            {
                LotteryModeCfg sub = new LotteryModeCfg();
                sub.modeId = 56;
                sub.name = "五星";
                sub.subModecfgs = new List<LotterySubModeCfg>();
                for (int k = 0; k < 2; ++k)
                {
                    LotterySubModeCfg submode = new LotterySubModeCfg();
                    submode.subModeId = 666;
                    submode.name = "五星直选";
               
                    submode.isShowTwo = false;

                    sub.subModecfgs.Add(submode);
                }

                scfg.modecfgs.Add(sub);
            }
            xl.lotteryConfigs.Add(scfg);
        }

        XmlHelper.XmlSerializeToFile(xl, "G:/UnityProject/LotterySvn/trunk/Assets/test.xml", System.Text.Encoding.UTF8);

    }

    void test2()
    {
        AddressConfigLoader xl = new AddressConfigLoader();

        xl.addConfigs = new List<AddressConfig>();

        for (int i = 0; i < 5; ++i)
        {

            AddressConfig scfg = new AddressConfig();
            scfg.Id = i;
            scfg.host = "www.baidu.com";
            scfg.port = 5555;

         
            xl.addConfigs.Add(scfg);
        }

        XmlHelper.XmlSerializeToFile(xl, "F:/UnityProject/lotterysvn/trunk/Assets/test.xml", System.Text.Encoding.UTF8);
    }
}


