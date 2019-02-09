using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class InterfaceManager : MonoBehaviour
{
    IEnumerator Post(string url, string data, System.Action<JsonData> callback = null)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        WWW www;
        byte[] bs = System.Text.UTF8Encoding.UTF8.GetBytes(data);

        if (UserData.instance.token != "")
        {
            print("加入了token");
            headers["Content-Type"] = "application/x-www-form-urlencoded";
            headers["OSTOKEN"] = UserData.instance.token;
            www = new WWW(url, bs, headers);
        }
        else
        {
            www = new WWW(url, bs);
        }

        Debug.Log("url:" + url);
        Debug.Log("data:" + data);

        yield return www;
        if (www.error != null)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log(www.text);
            if (callback != null)
            {
                JsonData jobject = JsonMapper.ToObject(www.text);
                callback(jobject);
            }
        }
    }
    private byte[] FileContent(string filePath)
    {
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        try
        {
            byte[] buffur = new byte[fs.Length];
            fs.Read(buffur, 0, (int)fs.Length);

            return buffur;
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
            return null;
        }
        finally
        {
            if (fs != null)
            {
                //关闭资源  
                fs.Close();
            }
        }
    }

    /// <summary>
    /// 上传图片到服务器
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    IEnumerator PostImage(string filePath, string fileName, string mimeType, System.Action<JsonData> callBack = null)
    {
        string url = "http://62.234.108.219/UserUpload/upLoadImg";

        WWWForm form = new WWWForm();
        form.AddField("OSTOKEN", UserData.instance.token);
        //  form.AddBinaryData("tempImg", FileContent(filePath), "1.jpg", "image/jpg");
        form.AddBinaryData("tempImg", FileContent(filePath), fileName, mimeType);

        WWW _www = new WWW(url, form);
        yield return _www;

        if (_www.error != null)
        {
            Debug.LogError(_www.error);
            yield return _www.error;
        }
        else
        {
            print(_www.text);
            if (callBack != null)
            {
                JsonData jobject = JsonMapper.ToObject(_www.text);
                callBack(jobject);
            }
        }
    }


    #region 注册登录模块

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="phone"></param>
    /// <param name="code"></param>
    /// <param name="password"></param>
    /// <param name="name"></param>
    /// <param name="invite"></param>
    public void Register(string phone, string code, string password, string name, string invite = "")
    {
        string url = "http://62.234.108.219/User/register";


        string data = "phone=" + phone +
            "&code=" + code + "&password=" + password + "&name=" + name + "&invite=" + invite;

        StartCoroutine(Post(url, data, OnRegister));
    }

    /// <summary>
    /// 注册结果
    /// </summary>
    /// <param name="result"></param>
    private void OnRegister(JsonData result)
    {
        int status = int.Parse(result["status"].ToString());
        if (status != 1)
        {
            EventManager.instance.NotifyEvent(Event.Regist, false);
            Debug.LogError("OnRegister >>>>error status:" + status);
            return;
        }
        result = result["data"];
        if (result == null)
            return;
        UserData.instance.id = int.Parse(result["id"].ToString());
        UserData.instance.name = result["name"].ToString();
        UserData.instance.phone = result["phone"].ToString();
        UserData.instance.role = int.Parse(result["role"].ToString());
        UserData.instance.area = int.Parse(result["area"].ToString());
        UserData.instance.avatar = result["avatar"].ToString();
        UserData.instance.coin = int.Parse(result["coin"].ToString());
        UserData.instance.point = int.Parse(result["point"].ToString());
        UserData.instance.token = result["token"].ToString();
        EventManager.instance.NotifyEvent(Event.Regist, true);
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="type"></param>
    /// <param name="phone"></param>
    /// <param name="password"></param>
    /// <param name="code"></param>
    public void Login(int type, string phone, string password = "", string code = "")
    {
        string url = "http://62.234.108.219/User/login";
        string data = "type=" + type + "&phone=" + phone + "&password=" + password + "&code=" + code;
        StartCoroutine(Post(url, data, OnLogin));
    }

    private void OnLogin(JsonData result)
    {
        int status = int.Parse(result["status"].ToString());
        if (status != 1)
        {
            Debug.LogError("OnLogin >>>>error status:" + status);
            EventManager.instance.NotifyEvent(Event.Login, false);
            return;
        }
        result = result["data"];
        if (result == null)
            return;
        UserData.instance.id = int.Parse(result["id"].ToString());
        UserData.instance.name = result["name"].ToString();
        UserData.instance.phone = result["phone"].ToString();
        UserData.instance.role = int.Parse(result["role"].ToString());
        UserData.instance.area = int.Parse(result["area"].ToString());
        UserData.instance.avatar = result["avatar"].ToString();
        UserData.instance.coin = int.Parse(result["coin"].ToString());
        UserData.instance.point = int.Parse(result["point"].ToString());
        UserData.instance.token = result["token"].ToString();
        UserData.instance.level = int.Parse(result[" level"].ToString());
        UserData.instance.totalSign = int.Parse(result[" totalSign"].ToString());
        EventManager.instance.NotifyEvent(Event.Login, true);
    }

    /// <summary>
    /// 选择身份
    /// </summary>
    /// <param name="roleId">1为“经销商”，2为“水工”，3为“试压员”</param>
    public void SetRole(int roleId)
    {
        string url = "http://62.234.108.219/User/setRole";
        string data = "role_id=" + roleId;
        DataManager.instance.selectRoleID = roleId;
        StartCoroutine(Post(url, data, OnSetRole));
    }

    /// <summary>
    ///选择身份回调
    /// </summary>
    /// <param name="result"></param>
    private void OnSetRole(JsonData result)
    {
        int status = int.Parse(result["status"].ToString());
        if (status != 1)
        {
            EventManager.instance.NotifyEvent(Event.SetRole, false);
            Debug.LogError("OnSetRole >>>>error status:" + status);
            return;
        }

        UserData.instance.role = DataManager.instance.selectRoleID;
        EventManager.instance.NotifyEvent(Event.SetRole, true);
    }

    /// <summary>
    /// 选择区域
    /// </summary>
    /// <param name="areaid"></param>
    public void SetArea(int areaid)
    {
        DataManager.instance.selectAreaID = areaid;
        string url = "http://62.234.108.219/User/setArea";
        string data = "area_id=" + areaid;
        StartCoroutine(Post(url, data, OnSetArea));
    }

    /// <summary>
    /// 选择区域回调
    /// </summary>
    /// <param name="result"></param>
    private void OnSetArea(JsonData result)
    {
        int status = int.Parse(result["status"].ToString());
        if (status != 1)
        {
            Debug.LogError("OnSetArea >>>>error status:" + status);
            EventManager.instance.NotifyEvent(Event.SetArea, false);
            return;
        }
        UserData.instance.area = DataManager.instance.selectAreaID;
        EventManager.instance.NotifyEvent(Event.SetArea, true);
    }

    /// <summary>
    /// 获取短信验证码
    /// </summary>
    /// <param name="phone">手机号</param>
    /// <param name="type">类型，1-注册，2-登录 ，3改密码</param>
    public void getSmsCode(string phone, int type)
    {
        string url = "http://62.234.108.219/User/getSmsCode";
        string data = "phone=" + phone + "&type=" + type;
        StartCoroutine(Post(url, data, OnGetSmsCode));
    }

    /// <summary>
    /// 获取短信验证码结果
    /// </summary>
    /// <param name="result"></param>
    private void OnGetSmsCode(JsonData result)
    {
        int status = int.Parse(result["status"].ToString());
        if (status != 1)
        {
            Debug.LogError("OnGetSmsCode >>>>error status:" + status);
            EventManager.instance.NotifyEvent(Event.GetSmsCode, false);
            return;
        }
        EventManager.instance.NotifyEvent(Event.GetSmsCode, true);
    }

    #endregion

    #region 预约模块
    /// <summary>
    /// 发起预约
    /// </summary>
    /// <param name="areaId">区域ID</param>
    /// <param name="type">预约类型</param>
    /// <param name="community">小区</param>
    /// <param name="address">地址</param>
    /// <param name="name">业主姓名</param>
    /// <param name="phone">业主电话</param>
    /// <param name="time">预约上门时间</param>
    /// <param name="remark">备注</param>
    public void CreateAppoiment(string areaId, string type, string community, string address, string name, string phone, string time, string remark)
    {
        string url = "http://62.234.108.219/Appointment/create";
        string data = " area_id=" + areaId + "&type=" + type + "&community=" + community + "&address=" + address + "&name=" + name + "&phone=" + phone + "&time=" + time + "&remark=" + remark;
        StartCoroutine(Post(url, data, OnCreateAppointment));
    }

    /// <summary>
    /// 预约结果回調
    /// </summary>
    /// <param name="result"></param>
    public void OnCreateAppointment(JsonData result)
    {
        int status = int.Parse(result["status"].ToString());
        if (status != 1)
        {
            EventManager.instance.NotifyEvent(Event.CreateAppointment, false);
            Debug.LogError("OnCreateAppointment >>>>error status:" + status);
            return;
        }
        EventManager.instance.NotifyEvent(Event.CreateAppointment, true);
    }

    /// <summary>
    /// 获取我的订单列表
    /// </summary>
    /// <param name="type">1为我预约的订单，2为我安装的订单</param>
    /// <param name="status">0为全部，3为合格订单，4为不合格</param>
    /// <param name="page">分页，页数</param>
    /// <param name="pageCount">分页，每页条数，最大50</param>   
    /// <param name="searchOrderId">搜索条件，订单号，最小4位，最大15位</param>
    public void GetAppointmentOrderList(int type, int status, int page, int pageCount, string searchOrderId)
    {
        string url = "http://62.234.108.219/Appointment/getOrderList";
        string lastTime = "";
        //只有在首页的时候才会刷新上次请求的时间戳
        if (page == 0)
        {
            DataManager.instance.orderList_lastTime = int.Parse((System.DateTime.Now.Ticks / 10000000).ToString());
        }
        else
        {
            lastTime = DataManager.instance.orderList_lastTime.ToString();
        }


        string data = "order_type=" + type + "&order_status=" + status + "& page=" + page + "& page_count=" + pageCount + "&last_time=" + lastTime + "&search[order_id]=" + searchOrderId;
        StartCoroutine(Post(url, data, OnGetAppointmentOrderList));
    }

    /// <summary>
    /// 获取我的订单回调
    /// </summary>
    /// <param name="result"></param>
    private void OnGetAppointmentOrderList(JsonData result)
    {
        int status = int.Parse(result["status"].ToString());
        if (status != 1)
        {
            EventManager.instance.NotifyEvent(Event.GetAppointmentList, false);
            Debug.LogError("OnGetAppointmentOrderList >>>>error status:" + status);
            return;
        }

        EventManager.instance.NotifyEvent(Event.GetAppointmentList, true);
    }

    /// <summary>
    /// 获取预约订单详情
    /// </summary>
    /// <param name="id">订单Id，不是订单号</param>
    public void GetAppointmentDetail(string id)
    {
        string url = "http://62.234.108.219/Appointment/getDetail";
        string data = " order_id=" + id;
        StartCoroutine(Post(url, data, OnGetAppointmentDetail));
    }

    /// <summary>
    /// 获取预约订单详情回调
    /// </summary>
    /// <param name="result"></param>
    public void OnGetAppointmentDetail(JsonData result)
    { }


    #endregion

    /// <summary>
    /// 获取帖子详情
    /// </summary>
    public void GetDetail(string forunid)
    {
        string url = "http://62.234.108.219/Forum/getDetail";
        string data = "forum_id=" + forunid;
        StartCoroutine(Post(url, data, OnGetDetail));
    }

    private void OnGetDetail(JsonData result)
    {
        EventManager.instance.NotifyEvent(Event.GetDetail);
    }

    #region 我的模块
    /// <summary>
    /// 获取系统开发区域列表
    /// </summary>
    /// <param name="curPage">当前页数</param>
    /// <param name="pageCount">每页的条目数</param>
    public void GetAreaList(int curPage, int pageCount)
    {
        string url = "http://62.234.108.219/Area/getList";
        string data = "page=" + curPage + "& page_count=" + pageCount;
        StartCoroutine(Post(url, data, OnGetAreaList));
    }

    /// <summary>
    /// 获取系统开放区域列表结果
    /// </summary>
    private void OnGetAreaList(JsonData result)
    { }

    /// <summary>
    /// 获取收藏列表
    /// </summary>
    /// <param name="curPage">当前页数</param>
    /// <param name="pageCount">每页的条目数</param>
    public void GetCollectionList(int curPage, int pageCount)
    {
        string url = "http://62.234.108.219/Forum/getCollectionList";
        string data = "page=" + curPage + "& page_count=" + pageCount;
        StartCoroutine(Post(url, data, OnGetCollectionList));
    }

    /// <summary>
    /// 返回获取收藏列表
    /// </summary>
    /// <param name="result"></param>
    public void OnGetCollectionList(JsonData result)
    {

    }

    /// <summary>
    /// 【获取我的积分】针对经销商和水工
    /// </summary>
    public void GetUserPoint()
    {
        if (UserData.instance.role == 1 || UserData.instance.role == 2)
            return;

        string url = "http://62.234.108.219/User/getPoint";
        string data = "";
        StartCoroutine(Post(url, data, OnGetUserPoint));
    }

    /// <summary>
    /// 积分返回结果
    /// </summary>
    /// <param name="result"></param>
    private void OnGetUserPoint(JsonData result)
    {
    }

    /// <summary>
    /// 【获取我的邀请码】针对经销商和水工
    /// </summary>
    public void GetInviteCode()
    {
        if (UserData.instance.role == 1 || UserData.instance.role == 2)
            return;

        string url = "http://62.234.108.219/User/getInviteCode";
        string data = "";
        StartCoroutine(Post(url, data, OnGetInviteCode));
    }

    /// <summary>
    /// 邀请码返回结果
    /// </summary>
    /// <param name="result"></param>
    private void OnGetInviteCode(JsonData result)
    {
        if (UserData.instance.role == 1 || UserData.instance.role == 2)
            return;

        string url = "http://62.234.108.219/User/getInviteCode";
        string data = "";
        StartCoroutine(Post(url, data, OnGetInviteCode));
    }

    /// <summary>
    /// 【获取我的等级】针对经销商和水工
    /// </summary>
    public void GetLevel()
    {
        if (UserData.instance.role == 1 || UserData.instance.role == 2)
            return;

        string url = "http://62.234.108.219/User/getLevel";
        string data = "";
        StartCoroutine(Post(url, data, OnGetLevel));
    }

    /// <summary>
    /// 返回等级
    /// </summary>
    /// <param name="result"></param>
    private void OnGetLevel(JsonData result)
    { }

    /// <summary>
    /// 获取我的蓝圈列表】 我的回帖，将显示“我回复的帖子”所属主贴，系统自动去重
    /// </summary>
    /// <param name="type"></param>
    /// <param name="curPage"></param>
    /// <param name="pageCount"></param>
    public void GetCircleList(int type, int curPage, int pageCount)
    {
        string url = "http://62.234.108.219/Forum/getCircleList";
        string data = "type=" + type + "&page=" + curPage + "&page_count=" + pageCount;
        StartCoroutine(Post(url, data, OnGetCircleList));
    }

    private void OnGetCircleList(JsonData result)
    { }


    #endregion


    /// <summary>
    /// 发帖
    /// </summary>
    public void CreateForum(string catid, string title, string content, string[] imgArray)
    {
        string url = "http://62.234.108.219/Forum/create";
        string data = "cat_id=" + catid + "&title=" + title + "&content=" + content;
        for (int i = 0; i < imgArray.Length; i++)
        {
            data += "&upload_images[]=" + imgArray[i];
        }
        StartCoroutine(Post(url, data, OnCreateForum));
    }

    /// <summary>
    /// 发帖回调
    /// </summary>
    /// <param name="result"></param>
    private void OnCreateForum(JsonData result)
    {
    }
}
