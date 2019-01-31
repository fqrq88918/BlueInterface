using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class test : MonoBehaviour {
    public UIEventListener registerBtn;
    public UIEventListener loginBtn;

    // Use this for initialization
    void Start () {
        registerBtn.onClick = register;
        loginBtn.onClick = login;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void register(GameObject go, PointerEventData eventdata)
    {
        Debug.Log("click");    
        GameObject.Find("Camera").GetComponent<NetworkManager>().Register("11223344556","8888","123456","maple");
    }

    void login(GameObject go, PointerEventData eventdata)
    {
        Debug.Log("click");
        GameObject.Find("Camera").GetComponent<NetworkManager>().Login(1, "13000000014", "123456", "8888");
    }
}
