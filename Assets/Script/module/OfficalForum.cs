using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficalForum  {
    /// <summary>
    /// 官方帖子ID
    /// </summary>
    public int id;
    /// <summary>
    /// 帖子标题
    /// </summary>
    public string title;
    /// <summary>
    /// 帖子内容
    /// </summary>
    public string content;
    /// <summary>
    /// 帖子缩略图
    /// </summary>
    public string thumb;
    /// <summary>
    /// 是否置顶
    /// </summary>
    public bool isTop;
    /// <summary>
    /// 评论数量
    /// </summary>
    public int comment;
    /// <summary>
    /// 浏览数量
    /// </summary>
    public int view;
    /// <summary>
    /// 发布时间
    /// </summary>
    public string createTime;

    /// <summary>
    /// 评论列表
    /// </summary>
    public List<Comment> commentList = new List<Comment>();
}
