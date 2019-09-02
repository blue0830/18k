using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class AddressConfig
{
    public int Id;
    public string host;
    public int port;
}

public class AddressConfigLoader
{
    [XmlElement]
    public List<AddressConfig> addConfigs { get; set; }
}