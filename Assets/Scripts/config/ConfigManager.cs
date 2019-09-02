using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ConfigManager
{
	private volatile static ConfigManager instance = null;

	private static readonly object locker = new object();

	private LotteryConfigLoader lotteryCfgLoader = null;

	private EmailConfigLoader emailConfigLoader = null;

	private AddressConfigLoader addressConfigLoader = null;

	private MsgConfigLoader msgcfgLoader = null;

    private ConfigManager() {

    }

    public static ConfigManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                        instance = new ConfigManager();
                }
            }
            return instance;
        }
        set {
        }
    }

    public MsgConfigLoader GetMsgConfig()
    {
        if (msgcfgLoader == null)
        {
            TextAsset TXTFile = Resources.Load("Config/msgConfig") as TextAsset;
			msgcfgLoader = XmlHelper.XmlDeserialize<MsgConfigLoader>(TXTFile.text, System.Text.Encoding.UTF8);
        }
        return msgcfgLoader;
    }

    public LotteryConfigLoader GetLotteryCfgLoader()
    {
        if (lotteryCfgLoader == null)
        {

            TextAsset TXTFile = Resources.Load("Config/lotteryConfig") as TextAsset;
            lotteryCfgLoader = XmlHelper.XmlDeserialize<LotteryConfigLoader>(TXTFile.text, System.Text.Encoding.UTF8);
        }

        return lotteryCfgLoader;
    }

    public AddressConfigLoader GetAddressConfigLoader()
    {
        if (addressConfigLoader == null)
        {

            TextAsset TXTFile = Resources.Load("Config/AddressConfig") as TextAsset;
            addressConfigLoader = XmlHelper.XmlDeserialize<AddressConfigLoader>(TXTFile.text, System.Text.Encoding.UTF8);
        }

        return addressConfigLoader;
    }

	public EmailConfigLoader GetEmailConfigLoader()
	{
		if (emailConfigLoader == null)
		{
			TextAsset TXTFile = Resources.Load("Config/emailConfig") as TextAsset;
			emailConfigLoader = XmlHelper.XmlDeserialize<EmailConfigLoader>(TXTFile.text, System.Text.Encoding.UTF8);
		}
		return emailConfigLoader;
	}
}