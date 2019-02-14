using System.Collections;
using System.Collections.Generic;

public class Good  {
    /// <summary>
    /// 商品Id
    /// </summary>
    public int id;
    /// <summary>
    /// 标题
    /// </summary>
    public string title;
    /// <summary>
    /// 所需积分
    /// </summary>
    public int price;
    /// <summary>
    /// 市场参考价格
    /// </summary>
    public int referencePrice;
    /// <summary>
    /// 库存
    /// </summary>
    public int stock;
    /// <summary>
    /// 缩略图
    /// </summary>
    public string thumb;
    /// <summary>
    /// 详情图
    /// </summary>
    public string details;
    /// <summary>
    /// 所属分类Id
    /// </summary>
    public int catId;

    public bool isOpen = true;
	
}
