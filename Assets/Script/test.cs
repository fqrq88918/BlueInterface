using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace XMWorkspace
{
    public class test : MonoBehaviour {
        public UIEventListener registerBtn;
        public UIEventListener loginBtn;
        public UIEventListener getBtn;


        // Use this for initialization
        void Start () {
           
         //   Debug.Log((System.DateTime.Now.Ticks / 10000000).ToString());
            registerBtn.onClick = register;
            loginBtn.onClick = login;
            getBtn.onClick = getForumList;
            EventManager.instance.RegisterEvent(Event.Login, OnLogin);
        }
	
	    // Update is called once per frame
	    void Update () {
		
	    }

        void register(GameObject go, PointerEventData eventdata)
        {
            Debug.Log("click regist");    
            GameObject.Find("Camera").GetComponent<InterfaceManager>().Register("11223344556","8888","123456","maple");
        }

        void login(GameObject go, PointerEventData eventdata)
        {
            //System.DateTime start = new System.DateTime(1970, 1, 1, 0, 0, 0);
            //Debug.Log(Util.GetTimeStamp());
            
            //return;
            Debug.Log("click login");
            GameObject.Find("Camera").GetComponent<InterfaceManager>().Login(1, "11223344556", "123456", "8888");
        }

        void getForemanList(GameObject go,PointerEventData data)
        {
            string[] tmp = new string[1];
            tmp[0] = "mmm";
            GameObject.Find("Camera").GetComponent<InterfaceManager>().GetForemanList(tmp);
        }

        void getForumList(GameObject go, PointerEventData data)
        {
            string[] tmp = new string[2];
            tmp[0] = "啥是佩奇？";
            tmp[1] = "今天天气真好";
            GameObject.Find("Camera").GetComponent<InterfaceManager>().GetForumList(0,0,10,tmp);
        }

        void OnLogin(object[] data)
        {
            Debug.Log("login result");
            Debug.Log(data[0]);
        }
    }
}
