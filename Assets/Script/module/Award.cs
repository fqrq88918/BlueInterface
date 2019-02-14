using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Award  {
    /// <summary>
    /// 奖项ID
    /// </summary>
    public int id;
    /// <summary>
    /// 奖品类型，1：蓝币；2：积分；3：谢谢参与；
    /// </summary>
    public int type;
    /// <summary>
    /// 奖品标题
    /// </summary>
    public string title;
    /// <summary>
    /// 所获奖金，type为3，谢谢参与时为0；
    /// </summary>
    public int price;
	
}
