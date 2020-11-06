#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RestSharp;
using System.IO;
using Extension;

public class Danmu : OdinEditorWindow
{
    [HideLabel]
    [HorizontalGroup("danmu")]
    public string text;
    static RestClient client = new RestClient("http://api.live.bilibili.com/msg/send");
    static RestRequest request = new RestRequest(Method.POST);
    [MenuItem("Tools/弹幕姬")]
    private static void OpenWindow()
    {
        Danmu window = GetWindow<Danmu>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
        //Init();
    }
    public static void Init()
    {
        string cookiedata = File.ReadAllText(@"Assets\Script\-1_Other\cookie.txt");
        List<Cookie> cookies = cookiedata.ToObject<List<Cookie>>();
        foreach (var cookie in cookies)
        {
            request.AddOrUpdateParameter(cookie.Name, cookie.Value, cookie.ContentType, cookie.Type);
        }
        request.AddOrUpdateParameter("color", 1);
        request.AddOrUpdateParameter("fontsize", 1);
        request.AddOrUpdateParameter("rnd", 1);
        request.AddOrUpdateParameter("roomid", 13517);
        request.AddOrUpdateParameter("bubble", 1);
        request.AddOrUpdateParameter("csrf_token", File.ReadAllLines(@"Assets\Script\-1_Other\info.txt")[1]);
        request.AddOrUpdateParameter("csrf", File.ReadAllLines(@"Assets\Script\-1_Other\info.txt")[1]);
    }
    [HorizontalGroup("danmu", 120, LabelWidth = 70)]
    [Button("发送")]
    public void Send()
    {
        Init();
        request.AddOrUpdateParameter("msg", text);
        IRestResponse response = client.Execute(request);
        text = "";
    }
    public class Cookie
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public ParameterType Type { get; set; }
        public int DataFormat { get; set; }
        public string ContentType { get; set; }
        public Cookie(Parameter cookie)
        {
            this.Name = cookie.Name;
            this.Value = cookie.Value.ToString();
            this.Type = cookie.Type;
            this.ContentType = cookie.ContentType;
        }
        public Cookie()
        {
        }
    }
}
#endif