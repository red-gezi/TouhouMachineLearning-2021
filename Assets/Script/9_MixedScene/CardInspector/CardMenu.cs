#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Info.CardInspector;
using static Info.CardInspector.CardLibraryInfo.LevelLibrary;
using static Info.CardInspector.CardLibraryInfo.LevelLibrary.SectarianCardLibrary;
using static Info.CardInspector.CardLibraryInfo.LevelLibrary.SectarianCardLibrary.RankLibrary;
using System.Linq;
using Command.CardInspector;

namespace CardInspector
{
    public class CardMenu : OdinMenuEditorWindow
    {
        static CardMenu instance;
        [MenuItem("Tools/卡组编辑器")]
        private static void OpenWindow()
        {
            CardMenu window = GetWindow<CardMenu>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
        }
        //public static void UpdateInspector() { }
        public static void UpdateInspector() => instance.ForceMenuTreeRebuild();
        protected override OdinMenuTree BuildMenuTree()
        {
            CardLibraryInfo cardLibraryInfo = CardLibraryCommand.GetLibraryInfo();
            var tree = new OdinMenuTree(true);
            tree.DefaultMenuStyle.Height = 60;
            tree.DefaultMenuStyle.IconSize = 48.00f;
            tree.Config.DrawSearchToolbar = true;
            CardLibraryCommand.Init();

            tree.Add("单人模式牌库", cardLibraryInfo);
            foreach (var levelLibrary in cardLibraryInfo.levelLibries.Where(library => library.isSingleMode))
            {
                tree.Add($"单人模式牌库/{levelLibrary.level}", levelLibrary);
                foreach (var sectarianCardLibrary in levelLibrary.sectarianCardLibraries)
                {
                    tree.Add($"单人模式牌库/{levelLibrary.level}/{sectarianCardLibrary.sectarian}", sectarianCardLibrary);
                    foreach (var rankLibrary in sectarianCardLibrary.rankLibraries)
                    {
                        tree.Add($"单人模式牌库/{levelLibrary.level}/{sectarianCardLibrary.sectarian}/{rankLibrary.rank}", rankLibrary);
                        foreach (var cardModel in rankLibrary.cardModelInfos)
                        {
                            tree.Add($"单人模式牌库/{levelLibrary.level}/{sectarianCardLibrary.sectarian}/{rankLibrary.rank}/{cardModel.cardName}", cardModel);
                        }
                    }
                }
            }

            tree.Add("多人模式牌库", cardLibraryInfo);
            foreach (var levelLibrary in cardLibraryInfo.levelLibries.Where(library => !library.isSingleMode))
            {
                foreach (var sectarianCardLibrary in levelLibrary.sectarianCardLibraries)
                {
                    tree.Add($"多人模式牌库/{sectarianCardLibrary.sectarian}", sectarianCardLibrary);
                    foreach (var rankLibrary in sectarianCardLibrary.rankLibraries)
                    {
                        tree.Add($"多人模式牌库/{sectarianCardLibrary.sectarian}/{rankLibrary.rank}", rankLibrary);
                        foreach (var cardModel in rankLibrary.cardModelInfos)
                        {
                            tree.Add($"多人模式牌库/{sectarianCardLibrary.sectarian}/{rankLibrary.rank}/{cardModel.cardName}", cardModel);
                        }
                    }
                }
            }
            //tree.EnumerateTree().AddIcons<CardLibraryInfo>(x => x.singleIcon);
            tree.EnumerateTree().AddIcons<SectarianCardLibrary>(x => x.icon);
            tree.EnumerateTree().AddIcons<RankLibrary>(x => x.icon);
            tree.EnumerateTree().AddIcons<CardModelInfo>(x => x.icon);
            instance = this;
            return tree;
        }
    }
}
#endif