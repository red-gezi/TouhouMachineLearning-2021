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

        // Start is called before the first frame update
        void Awake()
        {
            _cardLibraryContent = cardLibraryContent;
            _cardLibraryCardModel = cardLibraryCardModel;
            _cardDeckContent = cardDeckContent;
            _cardDeckCardModel = cardDeckCardModel;
            _cardDeckNameModel = cardDeckNameModel;
        }
        public static void InitCardLibrary()
        {
            multiModeCards = Command.CardInspector.CardLibraryCommand.GetLibraryInfo().multiModeCards;
            var hasCardLibrary = Info.AllPlayerInfo.UserInfo.cardLibrary;
            multiModeCards.ForEach(info =>
            {
                {
                    var newCardModel = Instantiate(_cardLibraryCardModel, _cardLibraryContent.transform);
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
        public static void InitCardDeck()
        {
            var tempDeck = Info.AllPlayerInfo.UserInfo.UseDeck.Clone();
            Dropdown dropdown = _cardDeckNameModel.GetComponent<Dropdown>();
            dropdown.ClearOptions();
            dropdown.AddOptions(Info.AllPlayerInfo.UserInfo.decks.Select(deck => deck.DeckName).ToList());
            //cardDeckContent
            tempDeck.CardIds.ForEach(cardID =>
            {
                Debug.LogError(cardID);
                var newCardModel = Instantiate(_cardDeckCardModel, _cardDeckContent.transform);
                var info = multiModeCards.FirstOrDefault(cardInfo => cardInfo.cardId == cardID);
                newCardModel.transform.GetChild(0).GetComponent<Text>().text = info.cardName;
                //newCardModel.transform.GetChild(1).GetComponent<Image>().material.SetTexture("Detail",info.icon)  ;
                Sprite cardTex = Sprite.Create(info.icon, new Rect(0, 0, info.icon.width, info.icon.height), Vector2.zero);
                newCardModel.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite= cardTex;
                //newCardModel.transform.localScale = _cardDeckCardModel.transform.localScale;

                //
                //newCardModel.GetComponent<Image>().sprite = cardTex;
                //newCardModel.transform.GetChild(1).GetComponent<Text>().text = info.cardName;
                //newCardModel.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = "X" + cardNum;
                //newCardModel.GetComponent<Image>().color = new Color(1, 1, 1, cardNum == 0 ? 0.2f : 1);
                //newCardModel.GetComponent<CardLibraryCardControl>().id = key;
                newCardModel.SetActive(true);
            });
        }
        public void OnSelectDeckChange()
        {
            Dropdown dropdown = _cardDeckNameModel.GetComponent<Dropdown>();
            Debug.Log("切换到deck" + dropdown.value);
            Info.AllPlayerInfo.UserInfo.useDeckNum = dropdown.value;
            InitCardDeck();

            //Info.AllPlayerInfo.UserInfo.decks.Add(new Model.CardDeck("测试卡组", 20002, new List<int> { 20002, 20001, 20001 }));
            //Command.Network.NetCommand.UpdateDecks(Info.AllPlayerInfo.UserInfo);
        }
        public void AddDeck()
        {
            Dropdown dropdown = _cardDeckNameModel.GetComponent<Dropdown>();
            Debug.Log("切换到deck" + dropdown.value);
            Info.AllPlayerInfo.UserInfo.useDeckNum = dropdown.value;
            InitCardDeck();
            Info.AllPlayerInfo.UserInfo.decks.Add(new Model.CardDeck("测试卡组", 20002, new List<int> { 20002, 20001, 20001 }));
            Command.Network.NetCommand.UpdateDecks(Info.AllPlayerInfo.UserInfo);
        }
        public void DeleteDeck()
        {
            Debug.Log("杀除卡组");
        }
        public void SaveDeck()
        {
            Debug.Log("保存卡组");

        }
        public void CancelDeck()
        {

            Debug.Log("取消卡组修改");

        }
    }
}

