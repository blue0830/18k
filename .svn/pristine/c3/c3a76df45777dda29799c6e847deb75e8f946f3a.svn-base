using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeManager : MonoBehaviour
{
	private  Dictionary<string, Timer> timerDic = new Dictionary<string, Timer>();

	private List<string> toremove = new List<string>();

	static TimeManager instance;

    public static TimeManager Instance()
    {
        if (instance == null)  // 如果没有找到

        {
            GameObject go = new GameObject("_TimeManager"); // 创建一个新的GameObject
            DontDestroyOnLoad(go);  // 防止被销毁
            instance = go.AddComponent<TimeManager>(); // 将实例挂载到GameObject上
        }
        return instance;
    }
		
    public delegate void TimerDelegate(int count, float time);

    
    /// <summary>
    /// 注册定时器
    /// </summary>
    /// <param name="name"></param>
    /// <param name="count">循环次数  -1 表示无限</param>
    /// <param name="interval">循环间隔，单位是毫秒</param>
    /// <param name="startDelay">延迟开始，单位是毫秒</param>
	/// <param name="callback">回调函数参数为次数和运行时间(单位是秒 运行时间包括延迟开始的时间)</param>
    public  void Register(string name, int count, int interval, int startDelay, TimerDelegate callback)
    {
        if (timerDic.ContainsKey(name))
        {
            Debug.LogError("duplicate name used in timer !!");
        }
        else
        {
            Timer timer = new Timer((float)interval / 1000, count, (float)startDelay/1000, callback);
            timerDic.Add(name, timer);
        }
    }

    public void UnRegister(string name)
    {
        if (timerDic.ContainsKey(name))
        {
            timerDic[name].Stop();
            timerDic.Remove(name);
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        toremove.Clear();
        foreach (KeyValuePair<string, Timer> pair in timerDic)
        {
            pair.Value.Update(Time.deltaTime);

            if (pair.Value.IsStop())
            {
                toremove.Add(pair.Key);
            }
        }

        foreach (string str in toremove)
        {
            timerDic.Remove(str);
        }
    }
}