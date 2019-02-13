using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XMWorkspace
{
    public class Util
    {
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
        public static int DateTimeToTimeStamp(int year, int month, int day, int hour = 0, int mintue = 0, int second = 0)
        {
            System.DateTime now = new System.DateTime(year, month, day, hour, mintue, second);
            return int.Parse((now.Ticks / 10000000).ToString());
        }

        /// <summary>
        /// 日期转成时间戳，精确到秒
        /// </summary>
        /// <param name="timeString">格式为yyyy-mm-dd hh:mm:ss</param>
        /// <returns></returns>
        public static int DateTimeToTimeStamp(string timeString)
        {
            int year = int.Parse(timeString.Split(' ')[0].Split('-')[0]);
            int month = int.Parse(timeString.Split(' ')[0].Split('-')[1]);
            int day = int.Parse(timeString.Split(' ')[0].Split('-')[2]);
            int hour = int.Parse(timeString.Split(' ')[1].Split('-')[0]);
            int min = int.Parse(timeString.Split(' ')[1].Split('-')[1]);
            int sec = int.Parse(timeString.Split(' ')[1].Split('-')[2]);

            return DateTimeToTimeStamp(year, month, day, hour, min, sec);
        }

        /// <summary>
        /// 获取当前时间的时间戳，精确到秒
        /// </summary>
        /// <returns></returns>
        public static int GetTimeStamp()
        {
            return int.Parse((System.DateTime.Now.Ticks / 10000000).ToString());
        }

    }
}
