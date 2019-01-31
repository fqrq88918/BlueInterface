using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleLoad : MonoBehaviour {
	private AssetBundleManifest manifest = null;
	private GameObject go = null;
	private GameObject go1 = null;
	private string dir = "";
	// Use this for initialization
	void Start ()
	{
		dir = Application.dataPath + "/AssetBundles/";
		//LoadAssetBundleManifest ();
	//	LoadBundleAndDeps ();
		LoadAll2();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LoadAssetBundleManifest()
	{
		var bundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(dir, "AssetBundles"));
		manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

		//unload
		bundle.Unload(false);
		bundle = null;

	}

	void LoadBundleAndDeps()
	{
//		AssetBundle assetBundleManifest = AssetBundle.LoadFromFile(Application.dataPath + "/AssetBundles/AssetBundles");
//		if(assetBundleManifest != null)
//		    manifest = assetBundleManifest.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
//
//		string bundleName = "Cube";
//
//		string[] dependence = manifest.GetDirectDependencies(bundleName);
//		Debug.Log (dependence.Length);
//		for(int i=0; i<dependence.Length; ++i)
//		{
//			AssetBundle.LoadFromFile(System.IO.Path.Combine(dir, dependence[i]));
//		}

		var bundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(dir, "cube.unity3d"));
		var bundle1 = AssetBundle.LoadFromFile(System.IO.Path.Combine(dir, "sp.unity3d"));
		var asset = bundle.LoadAsset<GameObject>("Cube");
		var asset1 = bundle1.LoadAsset<GameObject>("Sphere");
		bundle.Unload(false);
		bundle = null;
		bundle1.Unload(false);
		bundle1 = null;
		go = GameObject.Instantiate<GameObject>(asset);
		go1 = GameObject.Instantiate<GameObject>(asset1);
	}

	void LoadAll()
	{
		Debug.Log ("000");
		AssetBundle assetBundleManifest = AssetBundle.LoadFromFile(Application.dataPath + "/AssetBundles/AssetBundles");
		if(assetBundleManifest != null)
			manifest = assetBundleManifest.LoadAsset<AssetBundleManifest>("AssetBundleManifest");


		string[] depends = manifest.GetAllDependencies("assetBundle");
		Debug.Log (depends.Length);
		AssetBundle[] dependsAssetbundle = new AssetBundle[depends.Length];
		for (int index = 0; index < depends.Length; ++index)
		{
			Debug.Log ("ffff");
			dependsAssetbundle[index] = AssetBundle.LoadFromFile(Application.dataPath + "/AssetBundles/" + depends[index]);
			Debug.Log ("1111");
			GameObject obj1 = dependsAssetbundle[index].LoadAsset<GameObject>("Sphere1");
			if (obj1 != null)
			{
				Debug.Log ("222");
				GameObject sphere = Instantiate(obj1);
				dependsAssetbundle [index].Unload (false);
				dependsAssetbundle [index] = null;
				//sphere.transform.SetParent(GameObject.Find("UIRoot").transform);
			}

		}
	}

	void LoadAll2()
	{
		var bundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(dir, "all.unity3d"));
		GameObject obj1 = bundle.LoadAsset<GameObject>("Cube1");
		GameObject sphere = Instantiate(obj1);
		bundle.Unload (false);
		bundle = null;
	}
}
