using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Loading : MonoBehaviour {

	// Use this for initialization
	void Start () {
       // StartCoroutine(Loaded());
		StartCoroutine(LoadAsset("Cube"));
	}
	
//    private IEnumerator Loaded()
//    {
//        WWW download = new WWW("http://localhost/HotfixAsset" + "/AssetBundles/assetBundle/assetBundle.manifest");
//        yield return download;
//      string text=  download.text;
//        string[] modelNames= text.Split(new string[] { "\n" }, StringSplitOptions.None);
//        Stack<string> prefabs = new Stack<string>();
//        for (int i = modelNames.Length-1; i>=0; i--)
//        {
//            modelNames[i] = modelNames[i].Trim();
//            if (modelNames[i].StartsWith("Name:"))
//            {
//                if (modelNames[i].EndsWith(".prefab"))
//                {
//                    prefabs.Push(modelNames[i].Substring(5).Trim());
//                }
//                else
//                {
//                    yield return StartCoroutine(LoadAsset(modelNames[i].Substring(5).Trim()));
//
//                }
//              
//            }
//        }
//        while (prefabs.Count > 0)
//        {
//            yield return StartCoroutine(LoadAsset(prefabs.Pop()));
//           
//        }
//
//    }

	public IEnumerator LoadAsset(string modelName, System.Action callBack = null)
	{
		string url = "http://192.168.0.107:8080/UnityToJavaWeb/DownloadMidi.do?Download=unity3d";
		WWW urlWWW = new WWW (url);
		yield return urlWWW;
		if (urlWWW.error != null)
		{
			Debug.Log (urlWWW.error);
		} else 
		{
			Debug.Log (urlWWW.text);
			WWW download = new WWW (urlWWW.text);
			yield return download;
			if (download.error != null) 
			{
				Debug.Log (download.error);
			} else
			{
				AssetBundle bundle = download.assetBundle;

//        //取到资源的名称
//        string[] strs = assetPath.Split('/');
//        string modelName = strs[strs.Length - 1];
//        string type = modelName.Split('.')[1];
//        modelName = modelName.Split('.')[0];
//       
//        if (type == "prefab")
//        {
				GameObject obj = bundle.LoadAsset<GameObject> (modelName);
				GameObject instanceObj = Instantiate (obj);
				instanceObj.name = obj.name;
				// instanceObj.transform.parent = transform;
//        }
			}

			if (callBack != null) {
				callBack ();
			}
		}
	}
}
