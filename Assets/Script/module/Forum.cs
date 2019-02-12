using System.Collections;
using System.Collections.Generic;

public class Forum  {
    /// <summary>
    /// 帖子Id
    /// </summary>
    public int id;
    /// <summary>
    /// 标题
    /// </summary>
    public string title;
    /// <summary>
    /// 内容
    /// </summary>
    public string content="";
    /// <summary>
    /// 帖子所属分类Id
    /// </summary>
    public int catId;
    /// <summary>
    /// 发布者id
    /// </summary>
    public int userId;
    /// <summary>
    /// 发布者姓名
    /// </summary>
    public string userName;
    /// <summary>
    /// 发布者头像
    /// </summary>
    public string userAvatar;
    /// <summary>
    /// 主贴图片的序列化
    /// </summary>
    public List<string> uploadImages = new List<string>();
    /// <summary>
    /// 浏览量
    /// </summary>
    public int view;
    /// <summary>
    /// 评论数量
    /// </summary>
    public int comment;
    /// <summary>
    /// 发布时间
    /// </summary>
    public string create_time;

    /// <summary>
    /// 评论列表
    /// </summary>
    public List<Comment> commentList = new List<Comment>();
}

public class Comment
{
    /// <summary>
    /// 评论ID
    /// </summary>
    public int id;

    /// <summary>
    /// 回帖内容
    /// </summary>
    public string content;

    /// <summary>
    /// 回复者ID
    /// </summary>
    public int userId;

    /// <summary>
    /// 回复者姓名
    /// </summary>
    public string userName;

    /// <summary>
    /// 回复者头像
    /// </summary>
    public string userAvatar;

    /// <summary>
    /// 回复时间
    /// </summary>
    public string createTime;

}

