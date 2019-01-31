using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System;
using System.Linq;

public class InterfaceManager : MonoBehaviour {
	//private string url = "http://192.168.0.106:8080/UnityToJavaWeb/StringContentServlet";
	// Use this for initialization
	public Texture2D postImage;
	void Start () 
	{
		//StartCoroutine (PostUnityWeb ());
		StartCoroutine(PostImage());
	}
	
	// Update is called once per frame
	void Update () 
	{
		
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
		catch (Exception ex)
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

//	IEnumerator PostLogin()
//	{
//		//表单
//		WWWForm form = new WWWForm();
////		form.AddField("phone", "13813829757");
////		form.AddField("sms_code", "1234");
////		form.AddField("password", "88888888");
////		form.AddField("name", "lm");
//
//		form.AddField("UserName","test1");
//		form.AddField ("PassWord", "777");
//
//		WWW www = new WWW(url, form);
//
//		yield return www;
//
//		if (www.error != null)
//		{
//			print("php请求错误: 代码为" + www.error);
//		}
//		else
//		{
//			print("php请求成功" + www.text);
//		}
//	}

	IEnumerator PostUnityWeb()
	{
		Debug.Log ("111");
		//表单
		string url = "http://62.234.108.219/User/register";
		WWWForm form = new WWWForm();
		form.AddField("phone", "13813855555");
		form.AddField("code", "8888");
		form.AddField("password", "88888888");
		form.AddField("name", "lm222");

//		form.AddField("UserName","李梦");
//		form.AddField ("PassWord", "777");

		UnityWebRequest www = UnityWebRequest.Post (url, form);

		yield return www.Send ();

		if (www.isError)
		{

			yield return  www.error;
		}
		else
		{
			

				print(www.downloadHandler.text);

		}


	}

	IEnumerator SetRole()
	{
		string url = "http://62.234.108.219/User/setRole";

		Dictionary<string,string> header = new Dictionary<string, string>();
		header.Add ("OSTOKEN", "yxDRVobKtMYzKN7q");

		//form.AddField ("role_id", "1");
		string data = "role_id=1";
		byte[] bs = System.Text.UTF8Encoding.UTF8.GetBytes(data);
		WWW _www = new WWW(url, bs,header);
		//UnityWebRequest www = UnityWebRequest.Post(url, form,header);

		yield return _www;

		if (!_www.isDone)
		{

			yield return  _www.error;
		}
		else
		{


			print(_www.text);

		}
	}

	IEnumerator CreateForum()
	{
		string url = "http://62.234.108.219/Forum/create";
		Dictionary<string,string> header = new Dictionary<string, string>();
		header.Add ("OSTOKEN", "yxDRVobKtMYzKN7q");
		string data = "cat_id=1&title=测试&content=内容&upload_images="+postImage.EncodeToJPG().ToString();
		byte[] bs = System.Text.UTF8Encoding.UTF8.GetBytes(data);
//		WWWForm form = new WWWForm();
	
		WWW _www = new WWW(url, bs, header);
		yield return _www;

		if (!_www.isDone)
		{

			yield return  _www.error;
		}
		else
		{


			print(_www.text);

		}
	}

	IEnumerator PostImage()
	{
		string url = "http://62.234.108.219/UserUpload/upLoadImg";
		string filePath = Application.dataPath + "/1.jpg";
		Dictionary<string,string> header = new Dictionary<string, string>();
		header.Add ("OSTOKEN", "yxDRVobKtMYzKN7q");
		string data = "tempImg=";
		//Debug.Log (data);
		byte[] bs1 = System.Text.UTF8Encoding.UTF8.GetBytes(data);
		byte[] bs2 = FileContent (filePath);
		byte[] bsAll = bs1.Concat (bs2).ToArray ();
		//WWWForm form = new WWWForm();
		//form.AddBinaryData("tempImg",FileContent(filePath),"1.jpg");
		WWW _www = new WWW(url, bsAll,header);
		yield return _www;

		if (!_www.isDone)
		{

			yield return  _www.error;
		}
		else
		{


			print(_www.text);

		}
	}
}
