using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;


namespace XMWorkspace
{
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
                Util.ShowErrorMessage(status);
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
                Util.ShowErrorMessage(status);
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
            UserData.instance.level = int.Parse(result["level"].ToString());
            UserData.instance.totalSign = int.Parse(result["totalSign"].ToString());
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
                Util.ShowErrorMessage(status);
                Debug.LogError("OnSetRole >>>>error status:" + status);
                return;
            }

            UserData.instance.role = DataManager.instance.selectRoleID;
            EventManager.instance.NotifyEvent(Event.SetRole, true, UserData.instance.role);
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
                Util.ShowErrorMessage(status);
                EventManager.instance.NotifyEvent(Event.SetArea, false);
                return;
            }
            UserData.instance.area = DataManager.instance.selectAreaID;
            EventManager.instance.NotifyEvent(Event.SetArea, true, UserData.instance.area);
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
                Util.ShowErrorMessage(status);
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
        public void CreateAppoiment(int areaId, int type, string community, string address, string name, string phone, int time, string remark)
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
                Util.ShowErrorMessage(status);
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
            string data = "";
            //只有在首页的时候才会刷新上次请求的时间戳
            if (page == 0)
            {
                DataManager.instance.orderList_lastTime = Util.GetTimeStamp();
                data = "order_type=" + type + "&order_status=" + status + "& page=" + page + "& page_count=" + pageCount + "&search[order_id]=" + searchOrderId;
            }
            else
            {
                lastTime = DataManager.instance.orderList_lastTime.ToString();
                data = "order_type=" + type + "&order_status=" + status + "& page=" + page + "& page_count=" + pageCount + "&last_time=" + lastTime + "&search[order_id]=" + searchOrderId;
            }
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
                Util.ShowErrorMessage(status);
                Debug.LogError("OnGetAppointmentOrderList >>>>error status:" + status);
                return;
            }
            result = result["data"];
            if (result == null)
                return;

            int total = int.Parse(result["total"].ToString());
            int page = int.Parse(result["page"].ToString());
            int pageCount = int.Parse(result["page_count"].ToString());
            JsonData data = result["list"];
            List<Order> resultList = new List<Order>();
            for (int i = 0; i < data.Count; i++)
            {
                Order order = new Order();
                order.id = data[i]["id"].ToString();
                order.orderId = data[i]["order_id"].ToString();
                order.areaId = data[i]["area_id"].ToString();
                order.userId = data[i]["user_id"].ToString();
                //order.userName = data[i]["user_name"].ToString();
                //order.userPhone = data[i]["user_phone"].ToString();
                order.userName = "";
                order.userPhone = "";
                order.type = data[i]["a_type"].ToString();
                order.community = data[i]["a_community"].ToString();
                order.address = data[i]["a_address"].ToString();
                order.name = data[i]["a_name"].ToString();
                order.phone = data[i]["a_phone"].ToString();
                order.time = data[i]["a_time"].ToString();
                order.remark = data[i]["a_remark"].ToString();
                order.adminId = data[i]["b_admin_id"].ToString();
                order.testerId = data[i]["b_tester_id"].ToString();
                order.adminCreateTime = data[i]["b_create_time"].ToString();
                order.entrancePic = data[i]["c_pic_entrance"].ToString();
                order.testPic = data[i]["c_pic_test"].ToString();
                order.roomPic = data[i]["c_pic_room"].ToString();
                order.bedroomPic = data[i]["c_pic_bedroom"].ToString();
                order.toiletPic = data[i]["c_pic_toilet"].ToString();
                order.kitchenPic = data[i]["c_pic_kitchen"].ToString();
                order.balconyPic = data[i]["c_pic_balcony"].ToString();
                order.corridorPic = data[i]["c_pic_corridor"].ToString();
                order.buildId = data[i]["c_build_id"].ToString();
                order.buildName = data[i]["c_build_name"].ToString();
                order.buildPhone = data[i]["c_build_phone"].ToString();
                order.testBuyPlace = data[i]["c_test_buy_place"].ToString();
                order.testHouseType = data[i]["c_test_house_type"].ToString();
                order.testKitchenType = data[i]["c_test_kitchen_type"].ToString();
                order.testToiletType = data[i]["c_test_toilet_type"].ToString();
                order.testDeveloper = data[i]["c_test_developer"].ToString();
                order.testDecoration = data[i]["c_test_decoration"].ToString();
                order.testHvac = data[i]["c_test_hvac"].ToString();
                order.testAir = data[i]["c_test_air"].ToString();
                order.testProductType = data[i]["c_test_product_type"].ToString();
                order.testLength = data[i]["c_test_length"].ToString();
                order.testLayingType = data[i]["c_test_laying_type"].ToString();
                order.testPipeline = data[i]["c_test_pipeline"].ToString();
                order.testRemark = data[i]["c_test_remark"].ToString();
                order.testAssess = data[i]["c_test_assess"].ToString();
                order.testCompress = data[i]["c_test_compress"].ToString();
                order.testWeld = data[i]["c_test_weld"].ToString();
                order.testWeldCheck = data[i]["c_test_weld_check"].ToString();
                order.testKeepStart = data[i]["c_test_keep_start"].ToString();
                order.testKeepEnd = data[i]["c_test_keep_end"].ToString();
                order.testOperatePressure = data[i]["c_test_operate_pressure"].ToString();
                order.testCheckPressure = data[i]["c_test_check_pressure"].ToString();
                order.testUserId = data[i]["c_test_user_id"].ToString();
                order.testUserName = data[i]["c_test_user_name"].ToString();
                order.testUserPhone = data[i]["c_test_user_phone"].ToString();
                order.testNotice = data[i]["c_test_notice"].ToString();
                order.testCreateTime = data[i]["c_create_time"].ToString();
                order.testSimpleTime = data[i]["c_test_simple_time"].ToString();
                order.orderStatus = data[i]["order_status"].ToString();
                order.createTime = data[i]["create_time"].ToString();

                resultList.Add(order);
            }

            EventManager.instance.NotifyEvent(Event.GetAppointmentList, true, resultList, total, page, pageCount);
        }

        /// <summary>
        /// 获取预约订单详情
        /// </summary>
        /// <param name="id">订单Id，不是订单号</param>
        public void GetAppointmentDetail(int id)
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
        {
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                EventManager.instance.NotifyEvent(Event.GetAppointmentGetList, false);
                Util.ShowErrorMessage(status);
                Debug.LogError("OnGetAppointmentDetail >>>>error status:" + status);
                return;
            }
            result = result["data"];
            if (result == null)
                return;
            Order order = new Order();
            order.id = result["id"].ToString();
            order.orderId = result["order_id"].ToString();
            order.areaId = result["area_id"].ToString();
            order.userId = result["user_id"].ToString();
            order.userName = result["user_name"].ToString();
            order.userPhone = result["user_phone"].ToString();
            order.type = result["a_type"].ToString();
            order.community = result["a_community"].ToString();
            order.address = result["a_address"].ToString();
            order.name = result["a_name"].ToString();
            order.phone = result["a_phone"].ToString();
            order.time = result["a_time"].ToString();
            order.remark = result["a_remark"].ToString();
            order.adminId = result["b_admin_id"].ToString();
            order.testerId = result["b_tester_id"].ToString();
            order.adminCreateTime = result["b_create_time"].ToString();
            order.entrancePic = result["c_pic_entrance"].ToString();
            order.testPic = result["c_pic_test"].ToString();
            order.roomPic = result["c_pic_room"].ToString();
            order.bedroomPic = result["c_pic_bedroom"].ToString();
            order.toiletPic = result["c_pic_toilet"].ToString();
            order.kitchenPic = result["c_pic_kitchen"].ToString();
            order.balconyPic = result["c_pic_balcony"].ToString();
            order.corridorPic = result["c_pic_corridor"].ToString();
            order.buildId = result["c_build_id"].ToString();
            order.buildName = result["c_build_name"].ToString();
            order.buildPhone = result["c_build_phone"].ToString();
            order.testBuyPlace = result["c_test_buy_place"].ToString();
            order.testHouseType = result["c_test_house_type"].ToString();
            order.testKitchenType = result["c_test_kitchen_type"].ToString();
            order.testToiletType = result["c_test_toilet_type"].ToString();
            order.testDeveloper = result["c_test_developer"].ToString();
            order.testDecoration = result["c_test_decoration"].ToString();
            order.testHvac = result["c_test_hvac"].ToString();
            order.testAir = result["c_test_air"].ToString();
            order.testProductType = result["c_test_product_type"].ToString();
            order.testLength = result["c_test_length"].ToString();
            order.testLayingType = result["c_test_laying_type"].ToString();
            order.testPipeline = result["c_test_pipeline"].ToString();
            order.testRemark = result["c_test_remark"].ToString();
            order.testAssess = result["c_test_assess"].ToString();
            order.testCompress = result["c_test_compress"].ToString();
            order.testWeld = result["c_test_weld"].ToString();
            order.testWeldCheck = result["c_test_weld_check"].ToString();
            order.testKeepStart = result["c_test_kepp_start"].ToString();
            order.testKeepEnd = result["c_test_kepp_end"].ToString();
            order.testOperatePressure = result["c_test_operate_pressure"].ToString();
            order.testCheckPressure = result["c_test_check_pressure"].ToString();
            order.testUserId = result["c_test_user_id"].ToString();
            order.testUserName = result["c_test_user_name"].ToString();
            order.testUserPhone = result["c_test_user_phone"].ToString();
            order.testNotice = result["c_test_notice"].ToString();
            order.testCreateTime = result["c_create_time"].ToString();
            order.testSimpleTime = result["c_test_simple_time"].ToString();
            order.orderStatus = result["order_status"].ToString();
            order.createTime = result["create_time"].ToString();

            EventManager.instance.NotifyEvent(Event.GetAppointmentGetList, true, order);
        }


        #endregion       

        #region 我的模块
        /// <summary>
        /// 获取系统开发区域列表
        /// </summary>
        /// <param name="curPage">当前页数</param>
        /// <param name="pageCount">每页的条目数</param>
        public void GetAreaList(int curPage, int pageCount)
        {
            string url = "http://62.234.108.219/Area/getList";

            string lastTime = "";
            string data = "";
            //只有在首页的时候才会刷新上次请求的时间戳
            if (curPage == 0)
            {
                DataManager.instance.areaList_lastTime = Util.GetTimeStamp();
                data = "page=" + curPage + "&page_count=" + pageCount;
            }
            else
            {
                lastTime = DataManager.instance.areaList_lastTime.ToString();
                data = "page=" + curPage + "&page_count=" + pageCount + "&last_time=" + lastTime;
            }


            StartCoroutine(Post(url, data, OnGetAreaList));
        }

        /// <summary>
        /// 获取系统开放区域列表结果
        /// </summary>
        private void OnGetAreaList(JsonData result)
        {
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                EventManager.instance.NotifyEvent(Event.GetAreaList, false);
                Util.ShowErrorMessage(status);
                Debug.LogError("OnGetAreaList >>>>error status:" + status);
                return;
            }
            result = result["data"];
            if (result == null)
                return;

            JsonData data = result["list"];
            List<Area> dataList = new List<Area>();
            for (int i = 0; i < data.Count; i++)
            {
                Area area = new Area();
                area.id = int.Parse(data[i]["id"].ToString());
                area.title = data[i]["title"].ToString();
                area.createTime = data[i]["create_time"].ToString();
                dataList.Add(area);
            }
            EventManager.instance.NotifyEvent(Event.GetAreaList, true, dataList);

        }

        /// <summary>
        /// 获取收藏列表
        /// </summary>
        /// <param name="curPage">当前页数</param>
        /// <param name="pageCount">每页的条目数</param>
        public void GetCollectionList(int curPage, int pageCount)
        {
            string url = "http://62.234.108.219/Forum/getCollectionList";
            string lastTime = "";
            string data = "";
            //只有在首页的时候才会刷新上次请求的时间戳
            if (curPage == 0)
            {
                DataManager.instance.collectionList_lastTime = Util.GetTimeStamp();
                data = "page=" + curPage + "&page_count=" + pageCount;
            }
            else
            {
                lastTime = DataManager.instance.collectionList_lastTime.ToString();
                data = "page=" + curPage + "&page_count=" + pageCount + "&last_time=" + lastTime;
            }
            StartCoroutine(Post(url, data, OnGetCollectionList));
        }

        /// <summary>
        /// 返回获取收藏列表
        /// </summary>
        /// <param name="result"></param>
        public void OnGetCollectionList(JsonData result)
        {
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                EventManager.instance.NotifyEvent(Event.GetCollectionList, false);
                Util.ShowErrorMessage(status);
                Debug.LogError("OnGetCollectionList >>>>error status:" + status);
                return;
            }
            result = result["data"];
            if (result == null)
                return;

            JsonData data = result["list"];
            int total = int.Parse(result["total"].ToString());
            int page = int.Parse(result["page"].ToString());
            int pageCount = int.Parse(result["page_count"].ToString());

            List<Forum> nodeList = new List<Forum>();
            for (int i = 0; i < data.Count; i++)
            {
                Forum node = new Forum();
                node.id = int.Parse(data[i]["id"].ToString());
                node.title = data[i]["title"].ToString();
                node.catId = int.Parse(data[i]["cat_id"].ToString());
                node.userId = int.Parse(data[i]["user_id"].ToString());
                node.userName = data[i]["user_name"].ToString();
                node.userAvatar = data[i]["user_avatar"].ToString();
                node.view = int.Parse(data[i]["view"].ToString());
                node.comment = int.Parse(data[i]["comment"].ToString());
                node.create_time = data[i]["create_time"].ToString();
                nodeList.Add(node);
            }
            EventManager.instance.NotifyEvent(Event.GetCollectionList, true, nodeList, total, page, pageCount);

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
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                EventManager.instance.NotifyEvent(Event.GetUserPoint, false);
                Util.ShowErrorMessage(status);
                Debug.LogError("OnGetUserPoint >>>>error status:" + status);
                return;
            }
            result = result["data"];
            if (result == null)
                return;

            UserData.instance.point = int.Parse(result.ToString());
            EventManager.instance.NotifyEvent(Event.GetUserPoint, true, UserData.instance.point);
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
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                EventManager.instance.NotifyEvent(Event.GetInviteCode, false);
                Util.ShowErrorMessage(status);
                Debug.LogError("OnGetInviteCode >>>>error status:" + status);
                return;
            }
            result = result["data"];
            if (result == null)
                return;

            string code = result.ToString();
            EventManager.instance.NotifyEvent(Event.GetInviteCode, true, code);
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
        {
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                EventManager.instance.NotifyEvent(Event.GetLevel, false);
                Util.ShowErrorMessage(status);
                Debug.LogError("OnGetLevel >>>>error status:" + status);
                return;
            }
            result = result["data"];
            if (result == null)
                return;

            UserData.instance.level = int.Parse(result["level"].ToString());
            int orderNum = int.Parse(result["order_num"].ToString());
            EventManager.instance.NotifyEvent(Event.GetLevel, true, UserData.instance.level, orderNum);
        }

        /// <summary>
        /// 获取我的蓝圈列表】 我的回帖，将显示“我回复的帖子”所属主贴，系统自动去重
        /// </summary>
        /// <param name="type"></param>
        /// <param name="curPage"></param>
        /// <param name="pageCount"></param>
        public void GetCircleList(int type, int curPage, int pageCount)
        {
            string url = "http://62.234.108.219/Forum/getCircleList";
            string lastTime = "";
            string data = "";
            //只有在首页的时候才会刷新上次请求的时间戳
            if (curPage == 0)
            {
                DataManager.instance.circleList_lastTime = Util.GetTimeStamp();
                data = "type=" + type + "&page=" + curPage + "&page_count=" + pageCount;
            }
            else
            {
                lastTime = DataManager.instance.circleList_lastTime.ToString();
                data = "type=" + type + "&page=" + curPage + "&page_count=" + pageCount + "&last_time=" + lastTime;
            }
            StartCoroutine(Post(url, data, OnGetCircleList));
        }

        /// <summary>
        /// 获取篮圈列表回调
        /// </summary>
        /// <param name="result"></param>
        private void OnGetCircleList(JsonData result)
        {
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                EventManager.instance.NotifyEvent(Event.GetCircleList, false);
                Util.ShowErrorMessage(status);
                Debug.LogError("OnGetCircleList >>>>error status:" + status);
                return;
            }
            result = result["data"];
            if (result == null)
                return;

            int total = int.Parse(result["total"].ToString());
            int page = int.Parse(result["page"].ToString());
            int page_count = int.Parse(result["page_count"].ToString());

            JsonData data = result["list"];
            List<Forum> nodeList = new List<Forum>();
            for (int i = 0; i < data.Count; i++)
            {
                Forum node = new Forum();
                node.id = int.Parse(data[i]["id"].ToString());
                node.title = data[i]["title"].ToString();
                node.content = data[i]["content"].ToString();
                node.catId = int.Parse(data[i]["cat_id"].ToString());
                node.userId = int.Parse(data[i]["user_id"].ToString());
                node.userName = data[i]["user_name"].ToString();
                node.userAvatar = data[i]["user_avatar"].ToString();
                node.uploadImages = JsonMapper.ToObject<List<string>>(data[i]["upload_images"].ToString());

                node.view = int.Parse(data[i]["view"].ToString());
                node.comment = int.Parse(data[i]["comment"].ToString());
                node.create_time = data[i]["create_time"].ToString();
                nodeList.Add(node);
            }
            EventManager.instance.NotifyEvent(Event.GetCircleList, true, nodeList, total, page, page_count);

        }

        /// <summary>
        /// 【获取我的等级】针对经销商和水工
        /// </summary>
        public void GetLastWeekSign()
        {
            if (UserData.instance.role == 3)
                return;

            string url = "http://62.234.108.219/User/getLastWeekSign";
            string data = "";
            StartCoroutine(Post(url, data, OnGetLastWeekSign));
        }

        /// <summary>
        /// 返回等级
        /// </summary>
        /// <param name="result"></param>
        private void OnGetLastWeekSign(JsonData result)
        {
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                EventManager.instance.NotifyEvent(Event.GetLastWeekSign, false);
                Util.ShowErrorMessage(status);
                Debug.LogError("OnGetLastWeekSign >>>>error status:" + status);
                return;
            }
            result = result["data"];
            if (result == null)
                return;

            UserData.instance.lastWeekSign = JsonMapper.ToObject<List<int>>(result["level"].ToString());
            EventManager.instance.NotifyEvent(Event.GetLevel, true, UserData.instance.lastWeekSign);
        }

        /// <summary>
        /// 设置头像
        /// </summary>
        /// <param name="url">新的头像地址</param>
        public void SetAvatar(string avatarUrl)
        {
            string url = "http://62.234.108.219/User/setAvatar";
            string data = "avatar_url="+avatarUrl;
            StartCoroutine(Post(url, data, OnGetLastWeekSign));
        }

        /// <summary>
        /// 设置头像结果
        /// </summary>
        /// <param name="result"></param>
        public void OnSetAvatar(JsonData result)
        {
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                EventManager.instance.NotifyEvent(Event.SetAvatar, false);
                Util.ShowErrorMessage(status);
                Debug.LogError("OnSetAvatar >>>>error status:" + status);
                return;
            }
            EventManager.instance.NotifyEvent(Event.SetAvatar, true);
           
        }

       /// <summary>
       /// 修改密码
       /// </summary>
       /// <param name="phone">手机号</param>
       /// <param name="code">短信验证码</param>
       /// <param name="password">密码</param>
        public void ModifyPassword(string phone, string code ,string password)
        {
            string url = "http://62.234.108.219/User/modifyPwd";
            string data = "phone=" + phone + "&sms_code=" + code + "&new_pwd=" + password;
            StartCoroutine(Post(url, data, OnModifyPassword));
        }

        /// <summary>
        /// 修改密码返回结果
        /// </summary>
        /// <param name="result"></param>
        private void OnModifyPassword(JsonData result)
        {
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                Debug.LogError("OnModifyPassword >>>>error status:" + status);
                Util.ShowErrorMessage(status);
                EventManager.instance.NotifyEvent(Event.ModifyPassword, false);
                return;
            }
            EventManager.instance.NotifyEvent(Event.ModifyPassword, true);
        }

        /// <summary>
        /// 【获取我的基本信息】 用户已登录用户再次打开APP
        /// </summary>
        public void GetBaseInfo()
        {
            string url = "http://62.234.108.219/User/getBaseInfo";
            string data ="" ;
            StartCoroutine(Post(url, data, OnGetBaseInfo));
        }

        /// <summary>
        /// 获取用户基本信息结果
        /// </summary>
        /// <param name="result"></param>
        public void OnGetBaseInfo(JsonData result)
        {
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                Debug.LogError("OnGetBaseInfo >>>>error status:" + status);
                Util.ShowErrorMessage(status);
                EventManager.instance.NotifyEvent(Event.GetBaseInfo, false);
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
            UserData.instance.level = int.Parse(result["level"].ToString());
            UserData.instance.totalSign = int.Parse(result["totalSign"].ToString());
            EventManager.instance.NotifyEvent(Event.GetBaseInfo, true);
        }
        #endregion

        #region 论坛模块
        /// <summary>
        /// 获取纯净小蓝官方发布列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageCount"></param>
        public void GetOfficialList(int curPage,int pageCount)
        {
            string url = "http://62.234.108.219/Official/getList";
            string lastTime = "";
            string data = "";
            if (curPage == 0)
            {
                DataManager.instance.officialList_lastTime = Util.GetTimeStamp();
                data ="page=" + curPage + "&page_count=" + pageCount;
            }
            else
            {
                lastTime = DataManager.instance.officialList_lastTime.ToString();
                data ="page=" + curPage + "&page_count=" + pageCount + "&last_time=" + lastTime;
            }
            StartCoroutine(Post(url, data, OnGetOfficialList));
        }

        /// <summary>
        /// 获取官方发布列表结果
        /// </summary>
        /// <param name="result"></param>
        private void OnGetOfficialList(JsonData result)
        {
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                Debug.LogError("OnGetOfficialList >>>>error status:" + status);
                Util.ShowErrorMessage(status);
                EventManager.instance.NotifyEvent(Event.GetOfficialList, false);
                return;
            }
            result = result["data"];
            if (result == null)
                return;
            int total = int.Parse(result["total"].ToString());
            int page = int.Parse(result["page"].ToString());
            int pageCount = int.Parse(result["page_count"].ToString());
            JsonData data = result["list"];
            List<OfficalForum> officialForumList = new List<OfficalForum>();
            for (int i = 0; i < data.Count; i++)
            {
                OfficalForum tmp = new OfficalForum();
                tmp.id = int.Parse(data[i]["id"].ToString());
                tmp.title = data[i]["title"].ToString();
                tmp.content = data[i]["content"].ToString();
                tmp.thumb = data[i]["thumb"].ToString();
                tmp.isTop = int.Parse(data[i]["is_top"].ToString()) == 1;
                tmp.comment = int.Parse(data[i]["comment"].ToString());
                tmp.view = int.Parse(data[i]["view"].ToString());
                officialForumList.Add(tmp);
            }
            EventManager.instance.NotifyEvent(Event.GetOfficialList, true,officialForumList,total,page,pageCount);
        }

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
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                Debug.LogError("OnCreateForum >>>>error status:" + status);
                Util.ShowErrorMessage(status);
                EventManager.instance.NotifyEvent(Event.CreateForum, false);
                return;
            }
            EventManager.instance.NotifyEvent(Event.CreateForum, true);
        }

        /// <summary>
        /// 回帖
        /// </summary>
        /// <param name="forumId">主贴ID</param>
        /// <param name="content">回复内容,最多140字</param>
        public void ReplyForum(int forumId,string content)
        {
            string url = "http://62.234.108.219/Forum/reply";
            string data = "forum_id=" + forumId + "&content=" + content ;
            StartCoroutine(Post(url, data, OnReplyForum));
        }

        /// <summary>
        /// 回帖结果
        /// </summary>
        /// <param name="result"></param>
        private void OnReplyForum(JsonData result)
        {
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                Debug.LogError("OnReplyForum >>>>error status:" + status);
                Util.ShowErrorMessage(status);
                EventManager.instance.NotifyEvent(Event.ReplyForum, false);
                return;
            }
            EventManager.instance.NotifyEvent(Event.ReplyForum, true);
        }

        /// <summary>
        /// 收藏帖子
        /// </summary>
        /// <param name="forumId"></param>
        public void CollectForum(int forumId)
        {
            string url = "http://62.234.108.219/Forum/collect";
            string data = "user_id=" + UserData.instance.id + "&forum_id=" + forumId;
            StartCoroutine(Post(url, data, OnCollectForum));
        }

        /// <summary>
        /// 收藏帖子回调
        /// </summary>
        /// <param name="result"></param>
        public void OnCollectForum(JsonData result)
        {
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                Debug.LogError("OnCollectForum >>>>error status:" + status);
                Util.ShowErrorMessage(status);
                EventManager.instance.NotifyEvent(Event.CollectForum, false);
                return;
            }
            EventManager.instance.NotifyEvent(Event.CollectForum, true);
        }

        /// <summary>
        /// 获取官方帖子详情
        /// </summary>
        /// <param name="id"></param>
        public void GetOfficialDetail(int id)
        {
            string url = "http://62.234.108.219/Official/getDetail";
            string data = "id=" + id;
            StartCoroutine(Post(url, data, OnGetOfficialDetail));
        }

        /// <summary>
        /// 获取官方帖子详情结果
        /// </summary>
        /// <param name="result"></param>
        private void OnGetOfficialDetail(JsonData result)
        {
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                Debug.LogError("OnGetOfficialDetail >>>>error status:" + status);
                Util.ShowErrorMessage(status);
                EventManager.instance.NotifyEvent(Event.GetOfficialForumDetail, false);
                return;
            }
            result = result["data"];
            if (result == null)
                return;
            OfficalForum officalForum = new OfficalForum();
            officalForum.id = int.Parse(result["id"].ToString());
            officalForum.title = result["title"].ToString();
            officalForum.content = result["content"].ToString();
            officalForum.thumb = result["thumb"].ToString();
            officalForum.isTop = int.Parse(result["is_top"].ToString()) == 1;
            officalForum.view = int.Parse(result["view"].ToString());
            officalForum.comment = int.Parse(result["comment"].ToString());
            officalForum.createTime = result["createTime"].ToString();
            EventManager.instance.NotifyEvent(Event.GetOfficialForumDetail, true);
        }

        /// <summary>
        /// 获取banner列表
        /// </summary>
        /// <param name="curPage"></param>
        /// <param name="pageCount"></param>
        public void GetBannerList(int curPage, int pageCount)
        {
            string url = "http://62.234.108.219/Banner/getList";
            string lastTime = "";
            string data = "";
            if (curPage == 0)
            {
                DataManager.instance.bannerList_lastTime = Util.GetTimeStamp();
                data = "page=" + curPage + "&page_count=" + pageCount;
            }
            else
            {
                lastTime = DataManager.instance.bannerList_lastTime.ToString();
                data = "page=" + curPage + "&page_count=" + pageCount + "&last_time=" + lastTime;
            }
            StartCoroutine(Post(url, data, OnGetBannerList));
        }

        /// <summary>
        /// 获取banner回调
        /// </summary>
        /// <param name="result"></param>
        private void OnGetBannerList(JsonData result)
        {
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                Debug.LogError("OnGetBannerList >>>>error status:" + status);
                Util.ShowErrorMessage(status);
                EventManager.instance.NotifyEvent(Event.GetBannerList, false);
                return;
            }
            result = result["data"];
            if (result == null)
                return;
            int total = int.Parse(result["total"].ToString());
            int page = int.Parse(result["page"].ToString());
            int pageCount = int.Parse(result["page_count"].ToString());
            JsonData data = result["list"];
            List<Banner> bannerList = new List<Banner>();
            for (int i = 0; i < data.Count; i++)
            {
                Banner tmp = new Banner();
                tmp.id = int.Parse(data[i]["id"].ToString());
                tmp.title = data[i]["title"].ToString();
                tmp.thumb = data[i]["thumb"].ToString();
                tmp.createTime = data[i]["create_time"].ToString();
                bannerList.Add(tmp);
            }
            EventManager.instance.NotifyEvent(Event.GetBannerList, true, bannerList, total,page,pageCount);
        }

        /// <summary>
        /// 获取帖子详情及回帖，回帖按时间倒序
        /// </summary>
        public void GetDetail(int forunid)
        {
            string url = "http://62.234.108.219/Forum/getDetail";
            string data = "forum_id=" + forunid;
            StartCoroutine(Post(url, data, OnGetDetail));
        }
        /// <summary>
        /// 获取帖子详情结果返回
        /// </summary>
        /// <param name="result"></param>
        private void OnGetDetail(JsonData result)
        {
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                Debug.LogError("OnGetDetail >>>>error status:" + status);
                Util.ShowErrorMessage(status);
                EventManager.instance.NotifyEvent(Event.GetDetail, false);
                return;
            }
            result = result["data"];
            if (result == null)
                return;

            Forum forum = new Forum();
            JsonData mainForum = result["mainForum"];
            forum.id = int.Parse(mainForum["id"].ToString());
            forum.catId = int.Parse(mainForum["cat_id"].ToString());
            forum.userId = int.Parse(mainForum["user_id"].ToString());
            forum.view = int.Parse(mainForum["view"].ToString());
            forum.comment = int.Parse(mainForum["comment"].ToString());
            forum.title = mainForum["title"].ToString();
            forum.content=mainForum["content"].ToString();
            forum.userAvatar = mainForum["user_avatar"].ToString();
            forum.uploadImages = JsonMapper.ToObject<List<string>>(mainForum["upload_images"].ToString());
            forum.create_time = mainForum["create_time"].ToString();
            forum.userName = mainForum["user_name"].ToString();

            JsonData subForum = result["subForum"];
            for (var i = 0; i < subForum.Count; i++)
            {
                Comment tmp = new Comment();
                tmp.id = int.Parse(subForum[i]["id"].ToString());
                tmp.content = subForum[i]["content"].ToString();
                tmp.userId = int.Parse(subForum[i]["user_id"].ToString());
                tmp.userName = subForum[i]["user_name"].ToString();
                tmp.userAvatar = subForum[i]["user_avatar"].ToString();
                tmp.createTime = subForum[i]["create_time"].ToString();
                forum.commentList.Add(tmp);
            }

            EventManager.instance.NotifyEvent(Event.GetDetail,true,forum);
        }

        /// <summary>
        /// 【获取论坛帖子（主贴）列表】含搜索
        /// </summary>
        /// <param name="cat_id">获取的指定版块id，0为全部，1为动态分享，2为工艺展示，3为寻找工友，4为问题咨询</param>
        /// <param name="page">页数</param>
        /// <param name="pageCount">每页条数</param>
        /// <param name="title">搜索标题数组</param>
        public void GetForumList(int catId, int page, int pageCount, string[] title)
        {
            string url = "http://62.234.108.219/Forum/getForumList";
            string data = "cat_id="+ catId+"&page="+page+"&page_count="+pageCount;
            for (var i = 0; i < title.Length; i++)
            {
                data += "&search[]=" + title[i];
            }
            if (page == 0)            
                DataManager.instance.forumList_lastTime = Util.GetTimeStamp();
            
            else            
                data += "&last_time=" + DataManager.instance.forumList_lastTime.ToString();
                      
            StartCoroutine(Post(url, data, OnGetForumList));
        }

        /// <summary>
        /// 获取论坛帖子结果
        /// </summary>
        /// <param name="result"></param>
        private void OnGetForumList(JsonData result)
        {
            int status = int.Parse(result["status"].ToString());
            if (status != 1)
            {
                Debug.LogError("OnGetForumList >>>>error status:" + status);
                Util.ShowErrorMessage(status);
                EventManager.instance.NotifyEvent(Event.GetForumList, false);
                return;
            }
            result = result["data"];
            if (result == null)
                return;
            int total = int.Parse(result["total"].ToString());
            int page = int.Parse(result["page"].ToString());
            int pageCount = int.Parse(result["page_count"].ToString());

            JsonData data = result["list"];
            List<Forum> forumList = new List<Forum>();
            for (var i = 0; i < data.Count; i++)
            {
                Forum tmp = new Forum();
                tmp.id = int.Parse(data[i]["id"].ToString());
                tmp.userName = data[i]["user_name"].ToString();
                tmp.userAvatar = data[i]["user_avatar"].ToString();
                tmp.create_time = data[i]["create_time"].ToString();
                tmp.title = data[i]["title"].ToString();
                tmp.view = int.Parse(data[i]["view"].ToString());
                tmp.comment = int.Parse(data[i]["comment"].ToString());
                tmp.catId = int.Parse(data[i]["cat_id"].ToString());
                forumList.Add(tmp);
            }
            EventManager.instance.NotifyEvent(Event.GetForumList, true,forumList,total,page,pageCount);
        }
        #endregion

        #region 抽奖模块

        #endregion
    }
}
