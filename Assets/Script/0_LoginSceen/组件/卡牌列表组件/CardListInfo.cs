using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Info.CardInspector.CardLibraryInfo.LevelLibrary.SectarianCardLibrary.RankLibrary;
namespace Info
{
    public class CardListInfo : MonoBehaviour
    {
        //所有的多人模式卡牌
        [ShowInInspector]
        public static List<CardModelInfo> multiModeCards;

        public GameObject _cardDeckContent;
        public GameObject _cardDeckCardModel;
        public GameObject _cardDeckNameModel;

        public static GameObject cardDeckContent;
        public static GameObject cardDeckCardModel;
        public static GameObject cardDeckNameModel;

        public static List<GameObject> deckCardModels = new List<GameObject>();

        public static Model.CardDeck tempDeck;

        //获得指定卡组的去重并按品质排序后的列表
        public static List<int> distinctCardIds => tempDeck.CardIds
            .Distinct()
            .OrderBy(id => Command.CardInspector.CardLibraryCommand.GetCardStandardInfo(id).cardRank)
            .ThenByDescending(id => Command.CardInspector.CardLibraryCommand.GetCardStandardInfo(id).point)
            .ToList();

        private void Awake()
        {
            cardDeckContent = _cardDeckContent;
            cardDeckCardModel = _cardDeckCardModel;
            cardDeckNameModel = _cardDeckNameModel;
        }
    }
}
