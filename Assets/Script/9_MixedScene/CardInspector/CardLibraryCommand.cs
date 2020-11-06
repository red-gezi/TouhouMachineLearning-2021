using CardInspector;
using Extension;
using GameEnum;
using Info.CardInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using static Info.CardInspector.CardLibraryInfo;
using static Info.CardInspector.CardLibraryInfo.LevelLibrary;
using static Info.CardInspector.CardLibraryInfo.LevelLibrary.SectarianCardLibrary;
using static Info.CardInspector.CardLibraryInfo.LevelLibrary.SectarianCardLibrary.RankLibrary;

namespace Command
{
    namespace CardInspector
    {
        public static class CardLibraryCommand
        {
            public static CardLibraryInfo GetLibraryInfo() => Resources.Load<CardLibraryInfo>("CardData\\SaveData");
            public static CardModelInfo GetCardStandardInfo(int id) => new List<CardModelInfo>()
                    .Union(GetLibraryInfo().singleModeCards)
                    .Union(GetLibraryInfo().multiModeCards)
                    .First(info => info.cardId == id);

            static string[] CsvData;
            //初始化每个牌库的每个关卡所包含的卡牌
            public static void Init()
            {
                CardLibraryInfo cardLibraryInfo = GetLibraryInfo();
                cardLibraryInfo.levelLibries = new List<LevelLibrary>();
                cardLibraryInfo.includeLevel.ForEach(level => cardLibraryInfo.levelLibries.Add(new LevelLibrary(cardLibraryInfo.singleModeCards, level)));
                foreach (var levelLibrart in cardLibraryInfo.levelLibries.Where(library => library.isSingleMode))
                {
                    levelLibrart.sectarianCardLibraries = new List<SectarianCardLibrary>();
                    foreach (var sectarian in levelLibrart.includeSectarian)
                    {
                        levelLibrart.sectarianCardLibraries.Add(new SectarianCardLibrary(levelLibrart.cardModelInfos, sectarian));

                        foreach (var sectarianLibrary in levelLibrart.sectarianCardLibraries)
                        {
                            foreach (var rank in sectarianLibrary.includeRank)
                            {
                                sectarianLibrary.rankLibraries.Add(new RankLibrary(sectarianLibrary.cardModelInfos, rank));
                            }
                        }
                    }
                }
                cardLibraryInfo.levelLibries.Add(new LevelLibrary(cardLibraryInfo.multiModeCards, "多人"));
                foreach (var levelLibrart in cardLibraryInfo.levelLibries.Where(library => !library.isSingleMode))
                {
                    levelLibrart.sectarianCardLibraries = new List<SectarianCardLibrary>();
                    foreach (var sectarian in levelLibrart.includeSectarian)
                    {
                        levelLibrart.sectarianCardLibraries.Add(new SectarianCardLibrary(levelLibrart.cardModelInfos, sectarian));

                        foreach (var sectarianLibrary in levelLibrart.sectarianCardLibraries)
                        {
                            foreach (var rank in sectarianLibrary.includeRank)
                            {
                                sectarianLibrary.rankLibraries.Add(new RankLibrary(sectarianLibrary.cardModelInfos, rank));
                            }
                        }
                    }
                }
            }
            public static void LoadFromCsv()
            {
                //加载单人模式卡牌信息
                //CsvData = File.ReadAllLines("Assets\\Resources\\CardData\\CardData-Single.csv", Encoding.GetEncoding("gb2312"));
                CsvData = File.ReadAllLines("Assets\\Resources\\CardData\\CardData-Single.csv", Encoding.UTF8);
                GetLibraryInfo().singleModeCards = new List<CardModelInfo>();
                for (int i = 1; i < CsvData.Length; i++)
                {
                    Texture2D tex = Resources.Load<Texture2D>("CardTex\\" + GetCsvData<string>(i, "ImageUrl"));
                    GetLibraryInfo().singleModeCards.Add(
                        new CardModelInfo(
                            GetCsvData<int>(i, "Id") + 10000,
                            GetCsvData<string>(i, "Level"),
                            GetCsvData<string>(i, "Name-" + useLanguage),
                            GetCsvData<string>(i, "Describe-" + useLanguage),
                            GetCsvData<string>(i, "Ability-" + useLanguage),
                            GetCsvData<string>(i, "Tag"),
                            GetCsvData<CardType>(i, "Type"),
                            GetCsvData<Sectarian>(i, "Camp"),
                            GetCsvData<CardRank>(i, "Rank"),
                            GetCsvData<Region>(i, "Region"),
                            GetCsvData<Territory>(i, "Territory"),
                            GetCsvData<int>(i, "Point"),
                            GetCsvData<int>(i, "RamificationRank"),
                            tex
                        ));
                }
                //加载多人模式卡牌信息
                //CsvData = File.ReadAllLines("Assets\\Resources\\CardData\\CardData-Multi.csv", Encoding.GetEncoding("gb2312"));
                CsvData = File.ReadAllLines("Assets\\Resources\\CardData\\CardData-Multi.csv", Encoding.UTF8);
                GetLibraryInfo().multiModeCards = new List<CardModelInfo>();
                for (int i = 1; i < CsvData.Length; i++)
                {
                    Texture2D tex = Resources.Load<Texture2D>("CardTex\\" + GetCsvData<string>(i, "ImageUrl"));
                    GetLibraryInfo().multiModeCards.Add(
                        new CardModelInfo(
                            GetCsvData<int>(i, "Id") + 20000,
                            "多人",
                            GetCsvData<string>(i, "Name-" + useLanguage),
                            GetCsvData<string>(i, "Describe-" + useLanguage),
                            GetCsvData<string>(i, "Ability-" + useLanguage),
                            GetCsvData<string>(i, "Tag"),
                            GetCsvData<CardType>(i, "Type"),
                            GetCsvData<Sectarian>(i, "Camp"),
                            GetCsvData<CardRank>(i, "Rank"),
                            GetCsvData<Region>(i, "Region"),
                            GetCsvData<Territory>(i, "Territory"),
                            GetCsvData<int>(i, "Point"),
                            GetCsvData<int>(i, "RamificationRank"),
                            tex
                        ));
                }
                Init();
                Refresh();
            }

            public static void Refresh()
            {
                #if UNITY_EDITOR    
                CardMenu.UpdateInspector();
                #endif
            }

            private static T GetCsvData<T>(int i, string item)
            {
                try
                {
                    //Debug.Log(CsvData[0]);
                    int rank = CsvData[0].Split(',').ToList().IndexOf(item);
                    return (T)Convert.ChangeType(CsvData[i].Split(',')[rank], typeof(T).IsEnum ? typeof(int) : typeof(T));
                }
                catch (Exception e)
                {
                    Debug.Log(item+i+"出错");
                    Debug.Log(e.ToString());
                    return default;
                }

            }
            public static void SaveToCsv()
            {


            }
            public static void ClearCsvData()
            {
                GetLibraryInfo().multiModeCards.Clear();
                GetLibraryInfo().singleModeCards.Clear();
                foreach (var cardLibrarie in GetLibraryInfo().levelLibries)
                {
                    foreach (var sIngleSectarianLibrary in cardLibrarie.sectarianCardLibraries)
                    {
                        sIngleSectarianLibrary.cardModelInfos.Clear();
                    }
                }
#if UNITY_EDITOR
                CardMenu.UpdateInspector();
#endif
            }

            public static void CreatScript(int cardId)
            {
                string targetPath = Application.dataPath + $@"\Script\9_MixedScene\CardSpace\Card{cardId}.cs";

                if (!File.Exists(targetPath))
                {
                    string OriginPath = Application.dataPath + @"\Script\9_MixedScene\CardSpace\Card0.cs";
                    string ScriptText = File.ReadAllText(OriginPath).Replace("Card0", "Card" + cardId);
                    File.Create(targetPath).Close();
                    File.WriteAllText(targetPath, ScriptText);
#if UNITY_EDITOR
                    AssetDatabase.Refresh();
#endif
                }
            }
        }
    }
}