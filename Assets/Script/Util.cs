﻿using System.Collections;
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
            System.TimeSpan ts = now.ToUniversalTime() - new System.DateTime(1970, 1, 1);
            return (int)ts.TotalSeconds;
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
            System.TimeSpan ts = System.DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1);
            return (int)ts.TotalSeconds; ;
        }

        /// <summary>
        /// 错误消息提示
        /// </summary>
        /// <param name="messageId"></param>
        public static void ShowErrorMessage(int messageId)       
        {
            string msg = "";
            switch (messageId)
            {
                case -1:msg = "失败";break;
                case -2: msg = "参数错误"; break;
                case -3: msg = "验证码错误"; break;
                case -4: msg = "TOKEN校验失败"; break;
                //case 1: msg = "成功"; break;
                case 2: msg = "内容为空"; break;                
                default:msg = "未知错误";break;
            }
            EventManager.instance.NotifyEvent(Event.ShowMessage, msg);
        }       
    }
}
