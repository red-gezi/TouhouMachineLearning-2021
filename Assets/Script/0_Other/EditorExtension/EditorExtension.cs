#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class EditorExtension : MonoBehaviour
{
    [MenuItem("Tools/发布新版本", false, 1)]
    static void PublicClient()
    {
        Process.Start(@"E:\东方格致录\TouHouCardServer\更新器\客户端上传器\bin\Debug\客户端上传器.exe");
    }
    [MenuItem("Tools/打开服务端", false, 1)]
    static void StartServer()
    {
        Process.Start(@"E:\东方格致录\TouHouCardServer\WebSocketServer\bin\Debug\netcoreapp3.0\WebSocketServer.exe");
    }
    [MenuItem("Tools/打开客户端", false, 1)]
    static void StartClient()
    {
        Process.Start(@"G:\UnityProject\Pc\TouhouMachineLearning2021.exe");
    }
    [MenuItem("Tools/更新数据表格", false, 1)]
    static void UpdateXls()
    {
        Process.Start(@"E:\东方格致录\TouHouCardServer\更新器\客户端上传器\bin\Debug\客户端上传器.exe");
    }
}
#endif