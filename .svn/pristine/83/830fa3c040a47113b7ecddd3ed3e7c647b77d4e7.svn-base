using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class AssetManager
{
    bool useAssetBundle = false;
 
//不同平台下StreamingAssets的路径是不同的，这里需要注意一下。
	    public static readonly string PathURL =
#if UNITY_ANDROID && !UNITY_EDITOR
        "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE && !UNITY_EDITOR
		Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
    "file://" + Application.dataPath + "/StreamingAssets/";
#else
        string.Empty;
#endif

	private volatile static AssetManager instance = null;

	private static readonly object locker = new object();

	private AssetManager(){}

    public static AssetManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                        instance = new AssetManager();
                }
            }
            return instance;
        }
    }

    //此处后面会分目录
    public object LoadPrefab(string name )
    {
        if (useAssetBundle)
        {
            return LoadAssetBundle(name);
        }
        return Resources.Load("Prefabs/"+name);
    }

    object LoadAssetBundle(string name)
    {
        string url = PathURL + name; //+ ".assetbundle";

        object asset = null;
        try
        {
            AssetBundle ab = AssetBundle.LoadFromFile(url);
            if (ab != null)
            {
                asset = ab.mainAsset;
                ab.Unload(false);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Load assetbundle error " + e);
        }
        return asset;
    }
}