using CardModel;
using Command;
using GameEnum;
using Info;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Extension;
using static Network.NetInfoModel;
using System.Linq;

public class test : MonoBehaviour
{
    [ShowInInspector]
    public CardSet cardSet => AgainstInfo.cardSet;
    [ShowInInspector]
    public CardSet FiltercardSet;
    private void Start()
    {

    }
    public string text;
    [Button]
    public void test0()
    {
        //Debug.Log("?");
        //List<object> s = new List<object> { 5, "asd",new int[]{5,6,7}};
        //string str = Newtonsoft. Json. JsonConvert.SerializeObject(s) ;
        //s = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>> (str);
        //Debug.Log(s.ToJson().ToObject<>());
        //Debug.Log("?");

        //var s = new List<object> { 5, "ddd", new int[] { 5, 6, 7 } };
        //Debug.Log(s.ToJson());
        //Debug.Log(s.ToJson().ToObject<List<object>>()[2]);

        //Debug.Log(JsonUtility.FromJson<object[]>(s.ToJson()));


        //s = new object[] { 5, "ddd", new List<int>() { 5, 6, 7 } };
        //Debug.Log(s.ToJson());
        //Debug.Log(s.ToJson().ToObject<object[]>()[0]);

        object[] receiveInfo = "{\"Datas\":[2,0,5,0]}".ToObject<Network.NetInfoModel.GeneralCommand>().Datas;
        Debug.Log("解析为" + receiveInfo[2]);
        receiveInfo = "{\"Datas\":[7,0,[0,0],false,-262.5]}".ToObject<Network.NetInfoModel.GeneralCommand>().Datas;
        Debug.Log("解析为" + receiveInfo[2]);
        Debug.Log("解析为" + receiveInfo[3]);
        //Console.WriteLine(receiveInfo[0]);
        //Console.WriteLine(receiveInfo[1]);
        //Console.WriteLine(receiveInfo[3]);
        // var s= Resources.Load<TextAsset>("CardData/Tag").text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);


    }
    [Button]
    public void useLanguage(GameEnum.Language language)
    {
        Translate.currentLanguage = language.ToString();
    }
    [Button("翻译标签")]
    public void ShowText(GameEnum.CardTag tag)
    {
        text = tag.TransTag();
    }
    [Button("查找集合")]

    public void filterCardSet(List<GameEnum.CardTag> tags)
    {
        FiltercardSet = cardSet[tags.ToArray()];
    }

}
