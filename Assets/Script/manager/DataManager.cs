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

    public int orderList_lastTime = 0;


}
