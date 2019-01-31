using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class NetworkManager : MonoBehaviour {
    IEnumerator Post(string url, string data, System.Action<JsonData> callback=null)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers["Content-Type"]= "application/x-www-form-urlencoded";
        if (UserData.instance.token != "")
        {
            headers["OSTOKEN"] = UserData.instance.token;
        }
        byte[] bs = System.Text.UTF8Encoding.UTF8.GetBytes(data);
        WWW www = new WWW(url, bs, headers);
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
                //if (int.Parse(jobject["status"].ToString()) == 1)
                //{
                //    callback(jobject["data"]);
                //}
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
    IEnumerator PostImage(string filePath,string fileName,string mimeType,System.Action<JsonData> callBack=null)
    {
        string url = "http://62.234.108.219/UserUpload/upLoadImg";

        WWWForm form = new WWWForm();
        form.AddField("OSTOKEN",UserData.instance.token);
      //  form.AddBinaryData("tempImg", FileContent(filePath), "1.jpg", "image/jpg");
        form.AddBinaryData("tempImg", FileContent(filePath), fileName, mimeType);

        WWW _www = new WWW(url, form);
        yield return _www;

        if (_www.error!=null)
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




    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="phone"></param>
    /// <param name="code"></param>
    /// <param name="password"></param>
    /// <param name="name"></param>
    /// <param name="invite"></param>
    public void Register(string phone,string code,string password,string name,string invite="")
    {
        string url = "http://62.234.108.219/User/register";


        string data = "phone="+"phone"+
            "&code="+code+"&password="+password+"&name="+name+"&invite="+invite;     
        
        StartCoroutine(Post(url,data, OnRegister));        
    }

    /// <summary>
    /// 注册结果
    /// </summary>
    /// <param name="result"></param>
    private void OnRegister(JsonData result)
    {
        result = result["data"];
        if (result == null)
            return;
        UserData.instance.id = result["id"].ToString();
        UserData.instance.name = result["name"].ToString();
        UserData.instance.phone = result["phone"].ToString();
        UserData.instance.role = result["role"].ToString();
        UserData.instance.area = result["area"].ToString();
        UserData.instance.avatar = result["avatar"].ToString();
        UserData.instance.coin = result["coin"].ToString();
        UserData.instance.point = result["point"].ToString();
        UserData.instance.token = result["token"].ToString();
        EventManager.instance.NotifyEvent(Event.Regist);     
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="type"></param>
    /// <param name="phone"></param>
    /// <param name="password"></param>
    /// <param name="code"></param>
    public void Login(int type, string phone, string password="", string code = "")
    {

        string url = "http://62.234.108.219/User/login";
        string data = "type=" + type + "&phone=" + phone + "&password=" + password + "&code=" + code;
        StartCoroutine(Post(url, data,OnLogin));
    }

    private void OnLogin(JsonData result)
    {
        result = result["data"];
        if (result == null)
            return;
        UserData.instance.id = result["id"].ToString();
        UserData.instance.name = result["name"].ToString();
        UserData.instance.phone = result["phone"].ToString();
        UserData.instance.role = result["role"].ToString();
        UserData.instance.area = result["area"].ToString();
        UserData.instance.avatar = result["avatar"].ToString();
        UserData.instance.coin = result["coin"].ToString();
        UserData.instance.point = result["point"].ToString();
        UserData.instance.token = result["token"].ToString();
        UserData.instance.token = result[" level"].ToString();
        UserData.instance.token = result[" totalSign"].ToString();

        EventManager.instance.NotifyEvent(Event.Login);
    }

    /// <summary>
    /// 获取帖子详情
    /// </summary>
    public void GetDetail(string forunid)
    {
        string url = "http://62.234.108.219/Forum/getDetail";
        string data = "forum_id="+forunid;
        StartCoroutine(Post(url, data, OnGetDetail));
    }

    private void OnGetDetail(JsonData result)
    {     
        EventManager.instance.NotifyEvent(Event.GetDetail);
    }

    /// <summary>
    /// 设置角色
    /// </summary>
    /// <param name="roleId"></param>
    public void SetRole(string roleId)
    {
        string url = "http://62.234.108.219/User/setRole";
        string data = "role_id="+ roleId;
        StartCoroutine(Post(url, data, OnSetRole));

    }

    /// <summary>
    /// 设置角色回调
    /// </summary>
    /// <param name="result"></param>
    private void OnSetRole(JsonData result)
    { }

    /// <summary>
    /// 设置区域
    /// </summary>
    /// <param name="areaid"></param>
    public void SetArea(string areaid)
    {
        string url = "http://62.234.108.219/User/setArea";
        string data = "area_id="+areaid;
        StartCoroutine(Post(url, data, OnSetArea));
    }

    /// <summary>
    /// 设置区域回调
    /// </summary>
    /// <param name="result"></param>
    private void OnSetArea(JsonData result)
    { }

    /// <summary>
    /// 发帖
    /// </summary>
    public void CreateForum(string catid,string title,string content,string[]imgArray)
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
