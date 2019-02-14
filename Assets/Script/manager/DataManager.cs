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

    /// <summary>
    /// 上次获取官方发布列表的时间
    /// </summary>
    public int officialList_lastTime = 0;

    /// <summary>
    /// 上次获取banner列表的时间
    /// </summary>
    public int bannerList_lastTime = 0;

    /// <summary>
    /// 上次获取论坛帖子的时间
    /// </summary>
    public int forumList_lastTime = 0;

    /// <summary>
    /// 上次获取我的商品兑换列表的时间
    /// </summary>
    public int exchangeList_lastTime = 0;

    /// <summary>
    /// 上次获取商品列表的时间
    /// </summary>
    public int goodList_lastTime = 0;
   
    /// <summary>
    /// 系统的奖项列表
    /// </summary>
    public List<Award> systemAwardList = new List<Award>();

}
