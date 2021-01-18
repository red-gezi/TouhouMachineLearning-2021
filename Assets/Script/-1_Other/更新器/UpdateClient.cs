using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class UpdateClient : MonoBehaviour
{
    [MenuItem("Tools/发布新版本", false, 1)]
    static void PublicClient()
    {
        Process.Start(@"E:\东方格致录\TouHouCardServer\更新器\客户端上传器\bin\Debug\客户端上传器.exe");
    }
}
