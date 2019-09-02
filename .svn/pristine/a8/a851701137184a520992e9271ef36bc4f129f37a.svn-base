using UnityEngine;
using System.Collections;
using System;

public class TimeHelper {

    public static string SecondToHour(int sec)
    {
        string hstr = "";
        string mstr = "";
        string sstr = "";

        int hour = 0;
        int minute = 0;
        int second = 0;
        second = sec;

        if (second >= 60)
        {
            minute = second / 60;
            second = second % 60;
        }
        if (minute >= 60)
        {
            hour = minute / 60;
            minute = minute % 60;
        }

        if (hour < 10)
        {
            hstr = "0" ;
        }
        if (minute < 10)
        {
            mstr = "0";
        }
        if (second < 10)
        {
            sstr = "0";
        }
        return (hstr+hour + ":" +mstr+ minute + ":"
            + sstr+ second);
    }

	public static string yyyyMMddHHmmss(ulong sec)
	{
        DateTime dt = GetDateTimeBySeconds(sec);

        return dt.ToString ("yyyy-MM-dd HH:mm:ss");
	}

    public static ulong GetNowTime()
    {

        ulong t = (ulong)ConvertDateTimeInt(DateTime.Now);

        return t;
    }

    public static string GetTimeStrFromUlong(ulong time)
    {
        DateTime dt= GetDateTimeBySeconds(time);

        return dt.ToString("yyyy-MM-dd");
    }

	public static string GetNowTimeStr()
	{

		return DateTime.Now.ToString("yyyy-MM-dd");
	}

    public static ulong GetTimeFromStr(string timeStr) 
    {
        DateTime time = DateTime.ParseExact(timeStr, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
		ulong timelong = 0;
		try{
			timelong= (ulong)ConvertDateTimeInt (time);
		}catch(Exception e)
		{
			
		}

		return timelong;
    }

    static double ConvertDateTimeInt(System.DateTime time)
    {
        double intResult = 0;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        intResult = (time - startTime).TotalSeconds;
        return intResult;
    }


    static DateTime GetDateTimeBySeconds(ulong seconds)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        return startTime.AddSeconds(seconds);
    }
}