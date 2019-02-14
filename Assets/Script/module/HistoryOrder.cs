using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 历史商品订单
/// </summary>
public class HistoryOrder  {
    /// <summary>
    /// Id
    /// </summary>
    public int id;
    /// <summary>
    /// 商品Id
    /// </summary>
    public int goodsId;
    /// <summary>
    /// 商品标题
    /// </summary>
    public string goodsTitle;
    /// <summary>
    /// 商品缩略图
    /// </summary>
    public string thumb;
    /// <summary>
    /// 商品所属Id
    /// </summary>
    public int catId;
    /// <summary>
    /// 商品所需积分
    /// </summary>
    public int price;
    /// <summary>
    /// 订单姓名
    /// </summary>
    public string name;
    /// <summary>
    /// 订单电话
    /// </summary>
    public string phone;
    /// <summary>
    /// 订单地址
    /// </summary>
    public string address;
    /// <summary>
    /// 订单兑换时间
    /// </summary>
    public string createTime;
    /// <summary>
    /// 订单备注
    /// </summary>
    public string remark;
	
}
