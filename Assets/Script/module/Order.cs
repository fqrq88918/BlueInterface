using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order  {
    public string id;
    /// <summary>
    /// 订单号
    /// </summary>
    public string orderId;
    /// <summary>
    ///区域ID
    /// </summary>
    public string areaId;
    /// <summary>
    /// 发布者ID
    /// </summary>
    public string userId;

    /// <summary>
    /// 发布者姓名
    /// </summary>
    public string userName;

    /// <summary>
    /// 发布者手机号
    /// </summary>
    public string userPhone;

    /// <summary>
    /// 预约类型ID
    /// </summary>
    public string type;
    /// <summary>
    /// 小区
    /// </summary>
    public string community;
    /// <summary>
    /// 地址
    /// </summary>
    public string address;
    /// <summary>
    /// 业主姓名
    /// </summary>
    public string name;
    /// <summary>
    /// 业主电话
    /// </summary>
    public string phone;
    /// <summary>
    /// 预约上门的时间
    /// </summary>
    public string time;
    /// <summary>
    /// 备注
    /// </summary>
    public string remark;
    /// <summary>
    /// 操作分配的管理员Id
    /// </summary>
    public string adminId;
    /// <summary>
    /// 分配的试压员的Id
    /// </summary>
    public string testerId;
    /// <summary>
    /// 管理员分配订单的时间
    /// </summary>
    public string adminCreateTime;
    /// <summary>
    /// 进场照片
    /// </summary>
    public string entrancePic;
    /// <summary>
    /// 试压照片
    /// </summary>
    public string testPic;
    /// <summary>
    /// 客厅照片
    /// </summary>
    public string roomPic;
    /// <summary>
    /// 卧室照片
    /// </summary>
    public string bedroomPic;
    /// <summary>
    /// 卫生间照片
    /// </summary>
    public string toiletPic;
    /// <summary>
    /// 厨房照片
    /// </summary>
    public string kitchenPic;
    /// <summary>
    /// 阳台照片
    /// </summary>
    public string balconyPic;
    /// <summary>
    /// 走廊照片
    /// </summary>
    public string corridorPic;
    /// <summary>
    /// 安装者ID（非必）
    /// </summary>
    public string buildId;
    /// <summary>
    /// 安装者姓名
    /// </summary>
    public string buildName;
    /// <summary>
    /// 安装者电话
    /// </summary>
    public string buildPhone;

    /// <summary>
    /// 购买地点
    /// </summary>
    public string testBuyPlace;
    /// <summary>
    /// 房屋类型 1别墅，2商品房，3安置房，4车库
    /// </summary>
    public string testHouseType;
    /// <summary>
    /// 房屋户型厨房
    /// </summary>
    public string testKitchenType;
    /// <summary>
    /// 房屋户型卫生间
    /// </summary>
    public string testToiletType;
    /// <summary>
    /// 开发商姓名
    /// </summary>
    public string testDeveloper;
    /// <summary>
    /// 家装公司
    /// </summary>
    public string testDecoration;
    /// <summary>
    /// 暖通公司
    /// </summary>
    public string testHvac;
    /// <summary>
    /// 空调公司
    /// </summary>
    public string testAir;
    /// <summary>
    /// 产品类型 1纯净蓝，2 3米白色精品家装管，3普通咖喱色PPR,4.Z-HOME PPR 5无忧抗冻管 6 SW康居管 7 其他
    /// </summary>
    public string testProductType;
    /// <summary>
    /// 安装长度
    /// </summary>
    public string testLength;
    /// <summary>
    /// 管道铺设方案 管道铺设方案 1：屋顶；2：地面，3：墙壁
    /// </summary>
    public string testLayingType;
    /// <summary>
    /// 现场管道保护：000，每一位分别表示：现场干净、管道固定、管道无压埋
    /// </summary>
    public string testPipeline;
    /// <summary>
    /// 备注
    /// </summary>
    public string testRemark;
    /// <summary>
    /// 安装规范评定：000，每一位分别表示：水表后安装户内总阀、管道无混装、管材表面无划伤
    /// </summary>
    public string testAssess;
    /// <summary>
    /// 加压1.0兆帕接口渗漏情况：00000，每一位分别表示：热水器、厨房水龙头、淋浴、面盆、坐便器
    /// </summary>
    public string testCompress;
    /// <summary>
    /// 焊接接头点敲击检查渗漏情况：00000，每一位分别表示：内丝弯、直接、90度弯、三通、过桥弯
    /// </summary>
    public string testWeld;
    /// <summary>
    /// 焊接接头翻边现象检查：00000，每一位分别表示：内丝弯、直接、90度弯、三通、过桥弯
    /// </summary>
    public string testWeldCheck;
    /// <summary>
    /// 保压开始时间
    /// </summary>
    public string testKeepStart;
    /// <summary>
    /// 保压结束时间
    /// </summary>
    public string testKeepEnd;
    /// <summary>
    /// 管路运行压力：管路运行压力：1，0.3兆帕，2，0.4兆帕，3，0.5兆帕，
    /// </summary>
    public string testOperatePressure;
    /// <summary>
    /// 管路检测压力：1，0.8兆帕，2，1.0兆帕，3，1.2兆帕
    /// </summary>
    public string testCheckPressure;
    /// <summary>
    /// 试压员id
    /// </summary>
    public string testUserId;
    /// <summary>
    /// 试压员姓名
    /// </summary>
    public string testUserName;
    /// <summary>
    /// 试压员手机号
    /// </summary>
    public string testUserPhone;
    /// <summary>
    /// 管道使用注意事项告知：0000，每一位分别表示：防水与油漆、热源连接、保温与防冻、管道保护
    /// </summary>
    public string testNotice;
    /// <summary>
    ///提交试压结果时间
    /// </summary>
    public string testCreateTime;
    /// <summary>
    /// 日期简写，用于搜索，格式：180312
    /// </summary>
    public string testSimpleTime;
    /// <summary>
    /// 1：预约成功，待分配；2：分配成功，待试压；3：试压合格；4：试压不合格；
    /// </summary>
    public string orderStatus;
    /// <summary>
    /// 下单时间
    /// </summary>
    public string createTime;

}
