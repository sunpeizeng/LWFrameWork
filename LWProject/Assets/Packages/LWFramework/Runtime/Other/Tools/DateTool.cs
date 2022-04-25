using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateTool
{
    public const string dataFormat = "yyyy-MM-dd HH:mm:ss";
    public static string Date2Str(DateTime dateTime,string format = dataFormat) {
        return dateTime.ToString(format);
    }
    public static DateTime Str2Date(string dateString, string format = dataFormat) { 
        return DateTime.ParseExact(dateString, dataFormat, System.Globalization.CultureInfo.CurrentCulture);
    }

    public static string GetNowStr(string format = dataFormat) {
        return Date2Str(DateTime.Now, format);
    }
   
    /// <summary>
    /// 比较日期
    /// </summary>
    /// <param name="dateStr1">日期1</param>
    /// <param name="dateStr2"></param>
    /// <returns>大于0 1大   小于0 2大</returns>
    public static int CompanyDate(DateTime t1, DateTime t2)
    {
    
        //通过DateTIme.Compare()进行比较（）
        int compNum = DateTime.Compare(t1, t2);

        return compNum;
    }
    /// <summary>
    /// 比较日期
    /// </summary>
    /// <param name="dateStr1"></param>
    /// <param name="dateStr2"></param>
    /// <returns>大于0 1大   小于0 2大</returns>
    public static int CompanyDate(string str1, string str2)
    {        
        //通过DateTIme.Compare()进行比较（）
        int compNum = DateTime.Compare(Str2Date(str1), Str2Date(str2));

        return compNum;
    }
    /// <summary>
    /// 将秒数转化为时分秒
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static string Sec2HMS(long duration)
    {
        TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(duration));
        string str = "";
        if (ts.Hours > 0)
        {
            str = string.Format("{0:00}", ts.Hours) + ":" + string.Format("{0:00}", ts.Minutes) + ":" + string.Format("{0:00}", ts.Seconds);
        }
        if (ts.Hours == 0 && ts.Minutes > 0)
        {
            str = "00:" + string.Format("{0:00}", ts.Minutes) + ":" + string.Format("{0:00}", ts.Seconds);
        }
        if (ts.Hours == 0 && ts.Minutes == 0)
        {
            str = "00:00:" + string.Format("{0:00}", ts.Seconds);
        }
        return str;
    }
    /// <summary>
    /// 获取星期
    /// </summary>
    /// <returns></returns>
    public static string GetWeek() {
        return "星期" + DateTime.Now.DayOfWeek.ToString(("d"));
    }
}
