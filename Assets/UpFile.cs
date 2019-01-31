using System;
using System.IO;
using UnityEngine;
using System.Collections;

public class UpFile : MonoBehaviour
{
    //持有三个状态面板的对象
   // public GameObject upFileing;
   // public GameObject successPanel;
   // public GameObject failPanel;

    //定义访问JSP登录表单的post方式访问路径
    //private string Url = "http://192.168.31.39:8080/toUpData/UpFile.do";
   // private string Url = "http://192.168.1.100/MyUnityToJSPTest/ByteFileContentServlet.do";
	private string Url = "http://192.168.0.107:8080/UnityToJavaWeb/ByteFileContentServlet.do";

	private void Start()
	{
		StartCoroutine(UpFileToJSP(Url, Application.dataPath + "/midi.mid"));
	}

    //点击上传按钮
    public void OnUpFileButtonClick()
    {
        //设置上传文件中面板为显示状态
       // upFileing.SetActive(true);
        //上传本地文件
		StartCoroutine(UpFileToJSP(Url, Application.dataPath + "/midi.mid"));
    }



    //访问JSP服务器
    private IEnumerator UpFileToJSP(string url, string filePath)
    {
        WWWForm form=new WWWForm();
		form.AddBinaryData("midiFile",FileContent(filePath),"midi.mid");
       
        WWW upLoad=new WWW(url,form);
		Debug.Log (filePath);
        yield return upLoad;
        //如果失败
        if (!string.IsNullOrEmpty(upLoad.error)||upLoad.text.Equals("false"))
        {
            //在控制台输出错误信息
            print(upLoad.error);
            //将失败面板显示  上传中不显示
           // upFileing.SetActive(false);
           // failPanel.SetActive(true);
        }
        else
        {
            //如果成功
            print("Finished Uploading");
			print (upLoad.text);
            //将成功面板显示  上传中不显示
           // upFileing.SetActive(false);
           // successPanel.SetActive(true);
        }

    }



    //将文件转换为字节流
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
}
