using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager> {
    /// <summary>
    /// 注册时选的角色ID
    /// </summary>
    public int selectRoleID=0;
    /// <summary>
    /// 注册时选的区域ID
    /// </summary>
    public int selectAreaID = 0;

    /// <summary>
    /// 上次获取订单的时间
    /// </summary>
    public int orderList_lastTime = 0;

    /// <summary>
    /// 上次获取系统开放区域的时间
    /// </summary>
    public int areaList_lastTime = 0;

    /// <summary>
    /// 上次获取收藏列表的时间
    /// </summary>
    public int collectionList_lastTime = 0;

    /// <summary>
    /// 上次获取我的蓝圈列表的时间
    /// </summary>
    public int circleList_lastTime = 0;

}
