using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class MsgConfig
{
    public uint mainId;

    public string name;

    [XmlElement]
    public List<MsgSubCfg> subcfgs { get; set; }

    public MsgSubCfg GetMsgSubCfg(uint assId)
    {
        if (subcfgs == null)
        {
            Debug.LogError("message sub configlist is null");
        }
        for (int i = 0; i < subcfgs.Count; ++i)
        {
            if (subcfgs[i].assId == assId)
            {
                return subcfgs[i];
            }
        }

        return null;
    }
}

public class MsgSubCfg
{
    public uint assId;
    public uint type;
    public bool hasData;
    public string rspType;
    public string handler;
}

public class MsgConfigLoader
{
    [XmlElement]
    public List<MsgConfig> msgConfigs { get; set; }

    public MsgConfig GetMsgConfig(uint mainId)
    {
        if (msgConfigs == null)
        {
            Debug.LogError("message configlist is null");
        }
        for (int i = 0; i < msgConfigs.Count; ++i)
        {
            if (msgConfigs[i].mainId == mainId)
            {
                return msgConfigs[i];
            }
        }

        return null;
    }
}