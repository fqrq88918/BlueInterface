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

        // Use this for initialization
        void Start () {
            registerBtn.onClick = register;
            loginBtn.onClick = login;

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
            Debug.Log("click login");
            GameObject.Find("Camera").GetComponent<InterfaceManager>().Login(1, "11223344556", "123456", "8888");
        }


        void OnLogin(object[] data)
        {
            Debug.Log("login result");
            Debug.Log(data[0]);
        }
    }
}
