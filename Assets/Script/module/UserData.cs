using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : Singleton<UserData> {
    /// <summary>
    /// id
    /// </summary>
    public int id;
    /// <summary>
    /// 姓名
    /// </summary>
    public string name;
    /// <summary>
    /// 手机号
    /// </summary>
    public string phone;
    /// <summary>
    /// 身份:1为“经销商”，2为“水工”，3为“试压员”
    /// </summary>
    public int role;
    /// <summary>
    /// 区域
    /// </summary>
    public int area;
    /// <summary>
    /// 头像
    /// </summary>
    public string avatar;
    /// <summary>
    /// 蓝币，试压员无此项
    /// </summary>
    public int coin;
    /// <summary>
    /// 积分，试压员无此项
    /// </summary>
    public int point;
    /// <summary>
    /// 唯一ID
    /// </summary>
    public string token = "";

   /// public string token= "yxDRVobKtMYzKN7q";
    /// <summary>
    /// 等级，试压员无此项
    /// </summary>
    public int level = 0;
    /// <summary>
    /// 签到天数，试压员无此项
    /// </summary>
    public int totalSign = 0;

    /// <summary>
    /// 【近7日签到情况】针对经销商和水工,试压员无此项
    /// </summary>
    public List<int> lastWeekSign = new List<int>();
}
