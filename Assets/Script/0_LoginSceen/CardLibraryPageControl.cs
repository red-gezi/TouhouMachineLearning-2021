using Extension;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Info.CardInspector.CardLibraryInfo.LevelLibrary.SectarianCardLibrary.RankLibrary;
namespace Control
{
    public class CardLibraryPageControl : MonoBehaviour
    {
        [ShowInInspector]
        static List<CardModelInfo> multiModeCards;
        public GameObject cardLibraryContent;
        static GameObject _cardLibraryContent;

        public GameObject cardLibraryCardModel;
        static GameObject _cardLibraryCardModel;

        public GameObject cardDeckContent;
        public GameObject cardDeckCardModel;
        public GameObject cardDeckNameModel;

        static GameObject _cardDeckContent;
        static GameObject _cardDeckCardModel;
        static GameObject _cardDeckNameModel;

        static List<GameObject> deckCardModels = new List<GameObject>();
        static List<GameObject> libraryCardModels = new List<GameObject>();

        static Model.CardDeck tempDeck;
        static List<int> distinctCardIds => tempDeck.CardIds.Distinct().OrderBy(id => multiModeCards.First(info => info.cardId == id).cardRank).ToList();

        //static List<int> distinctCardIds => tempDeck.CardIds.Distinct().ToList();
        // Start is called before the first frame update
        void Awake()
        {
            _cardLibraryContent = cardLibraryContent;
            _cardLibraryCardModel = cardLibraryCardModel;
            _cardDeckContent = cardDeckContent;
            _cardDeckCardModel = cardDeckCardModel;
            _cardDeckNameModel = cardDeckNameModel;

            _cardLibraryCardModel.SetActive(false);
            _cardDeckCardModel.SetActive(false);
        }
        public static void InitCardLibrary()
        {
            multiModeCards = Command.CardInspector.CardLibraryCommand.GetLibraryInfo().multiModeCards;
            var hasCardLibrary = Info.AllPlayerInfo.UserInfo.cardLibrary;
            multiModeCards.ForEach(info =>
            {
                {
                    var newCardModel = Instantiate(_cardLibraryCardModel, _cardLibraryContent.transform);
                    libraryCardModels.Add(newCardModel);
                    string key = info.cardId.ToString();
                    int cardNum = 0;
                    if (hasCardLibrary.ContainsKey(key))
                    {
                        cardNum = hasCardLibrary[key];
                    }
                    newCardModel.transform.localScale = _cardLibraryCardModel.transform.localScale;
                    Sprite cardTex = Sprite.Create(info.icon, new Rect(0, 0, info.icon.width, info.icon.height), Vector2.zero);
                    newCardModel.GetComponent<Image>().sprite = cardTex;
                    newCardModel.transform.GetChild(1).GetComponent<Text>().text = info.cardName;
                    newCardModel.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "X" + cardNum;
                    newCardModel.GetComponent<Image>().color = new Color(1, 1, 1, cardNum == 0 ? 0.2f : 1);
                    newCardModel.GetComponent<CardLibraryCardControl>().id = key;
                    newCardModel.SetActive(true);
                }
            });
        }

        public static void InitCardDeck(bool isInitOptions = true)
        {
            if (tempDeck == null)
            {
                tempDeck = Info.AllPlayerInfo.UserInfo.UseDeck.Clone();
            }
            if (isInitOptions)
            {
                //初始化领袖栏
                Dropdown dropdown = _cardDeckNameModel.GetComponent<Dropdown>();
                dropdown.ClearOptions();
                dropdown.AddOptions(Info.AllPlayerInfo.UserInfo.decks.Select(deck => deck.DeckName).ToList());
            }
            //distinctCardIds = tempDeck.CardIds.Distinct().ToList();
            int deskCardNumber = distinctCardIds.Count();
            int deskModelNumber = deckCardModels.Count;
            deckCardModels.ForEach(model => model.SetActive(false));
            if (deskCardNumber > deskModelNumber)
            {
                for (int i = 0; i < deskCardNumber - deskModelNumber; i++)
                {
                    var newCardModel = Instantiate(_cardDeckCardModel, _cardDeckContent.transform);
                    deckCardModels.Add(newCardModel);
                }
            }
            //初始化卡牌栏
            for (int i = 0; i < distinctCardIds.Count(); i++)
            {
                int cardID = distinctCardIds[i];
                var newCardModel = deckCardModels[i];

                var info = multiModeCards.FirstOrDefault(cardInfo => cardInfo.cardId == cardID);
                newCardModel.transform.GetChild(0).GetComponent<Text>().text = info.cardName;
                Sprite cardTex = Sprite.Create(info.icon, new Rect(0, 0, info.icon.width, info.icon.height), Vector2.zero);
                newCardModel.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = cardTex;
                //设置数量
                Color RankColor = Color.white;
                switch (info.cardRank)
                {
                    case GameEnum.CardRank.Leader: RankColor = new Color(0.98f, 0.9f, 0.2f); break;
                    case GameEnum.CardRank.Gold: RankColor = new Color(0.98f, 0.9f, 0.2f); break;
                    case GameEnum.CardRank.Silver: RankColor = new Color(0.75f, 0.75f, 0.75f); break;
                    case GameEnum.CardRank.Copper: RankColor = new Color(0.55f, 0.3f, 0.1f); break;
                }
                newCardModel.transform.GetChild(2).GetComponent<Image>().color = RankColor;
                newCardModel.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = tempDeck.CardIds.Count(id => id == cardID).ToString();

                newCardModel.SetActive(true);
            }
            //tempDeck.CardIds.ForEach(cardID =>
            //{

            //});
        }
        public void OnSelectDeckChange()
        {
            Dropdown dropdown = _cardDeckNameModel.GetComponent<Dropdown>();
            Debug.Log("切换到deck" + dropdown.value);
            Info.AllPlayerInfo.UserInfo.useDeckNum = dropdown.value;
            tempDeck = Info.AllPlayerInfo.UserInfo.UseDeck.Clone();
            InitCardDeck(false);
        }
        public void CreatDeck()
        {
            Info.AllPlayerInfo.UserInfo.decks.Add(new Model.CardDeck("新卡组", 20002, new List<int> { 20002, 20001, 20001 }));
            Info.AllPlayerInfo.UserInfo.useDeckNum = Info.AllPlayerInfo.UserInfo.decks.Count;
            Debug.Log("切换到deck" + Info.AllPlayerInfo.UserInfo.useDeckNum);
            Command.Network.NetCommand.UpdateDecks(Info.AllPlayerInfo.UserInfo);
            InitCardDeck();
        }
        public void ChangeDeckName()
        {

        }
        public void DeleteDeck()
        {
            Debug.Log("删除卡组");
            Info.AllPlayerInfo.UserInfo.decks.Remove(Info.AllPlayerInfo.UserInfo.UseDeck);
            Info.AllPlayerInfo.UserInfo.useDeckNum = 0;
            Command.Network.NetCommand.UpdateDecks(Info.AllPlayerInfo.UserInfo);
            InitCardDeck();
        }
        public void SaveDeck()
        {
            Debug.Log("保存卡组");
            Info.AllPlayerInfo.UserInfo.UseDeck = tempDeck;
            Command.Network.NetCommand.UpdateDecks(Info.AllPlayerInfo.UserInfo);
            InitCardDeck();
        }
        public void CancelDeck()
        {
            Debug.Log("取消卡组修改");
            tempDeck = Info.AllPlayerInfo.UserInfo.UseDeck;
            InitCardDeck();
        }
        public void AddCardToDeck(GameObject clickCard)
        {
            //判断牌库是否有多余牌
            Debug.Log("添加卡牌");
            //判断卡组是否可添加
            int addCardId = multiModeCards[libraryCardModels.IndexOf(clickCard)].cardId;
            var cardInfo = Command.CardInspector.CardLibraryCommand.GetCardStandardInfo(addCardId);
            int sameCardOnDeck = tempDeck.CardIds.Count(id => id == addCardId);
            if (sameCardOnDeck<1||(sameCardOnDeck < 3&& cardInfo.cardRank== GameEnum.CardRank.Copper))
            {
                tempDeck.CardIds.Add(addCardId);
            }
            else
            {
                Debug.Log("已溢出");
            }
            InitCardDeck();
        }
        public void RemoveCardFromDeck(GameObject clickCard)
        {
            //distinctCardIds;
            Debug.Log("移除卡牌");
            int removeCardId = distinctCardIds[deckCardModels.IndexOf(clickCard)];
            tempDeck.CardIds.Remove(removeCardId);
            InitCardDeck();
        }
    }
}

