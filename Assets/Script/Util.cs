using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util   {
    /// <summary>
    /// 日期转成时间戳，精确到秒
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    /// <param name="day">日</param>
    /// <param name="hour">时</param>
    /// <param name="mintue">分</param>
    /// <param name="second">秒</param>
    /// <returns></returns>
    public int DateTimeToTimeStamp(int year,int month,int day,int hour=0,int mintue=0,int second=0)
    {
        System.DateTime now = new System.DateTime(year, month, day, hour, mintue, second);
        return now.Ticks / 10000000;
    }
	
}
