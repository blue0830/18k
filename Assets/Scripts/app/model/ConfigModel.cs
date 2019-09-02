using UnityEngine;
using System.Collections;

public class ConfigModel : IConfigModel
{

    public LotteryConfigLoader GetLotteryCfg()
    {

        return ConfigManager.Instance.GetLotteryCfgLoader();
    }
}
