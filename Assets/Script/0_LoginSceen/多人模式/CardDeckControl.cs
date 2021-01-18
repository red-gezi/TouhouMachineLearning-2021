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
    public class CardDeckControl : MonoBehaviour
    {

        [ShowInInspector]
        static List<CardModelInfo> multiModeCards;


        public GameObject cardDeckContent;
        public GameObject cardDeckCardModel;
        public GameObject cardDeckNameModel;

        static List<GameObject> deckCardModels = new List<GameObject>();

        static Model.CardDeck tempDeck;
        static List<int> distinctCardIds => tempDeck.CardIds.Distinct().OrderBy(id => Command.CardInspector.CardLibraryCommand.GetCardStandardInfo(id).cardRank).ToList();
        void Awake()
        {
            cardDeckCardModel.SetActive(false);
            multiModeCards = Command.CardInspector.CardLibraryCommand.GetLibraryInfo().multiModeCards;
        }

        public void InitCardDeck(bool isInitOptions = true)
        {
            tempDeck = Info.AllPlayerInfo.UserInfo.UseDeck.Clone();
            if (isInitOptions)
            {
                //初始化领袖栏
                Dropdown dropdown = cardDeckNameModel.GetComponent<Dropdown>();
                dropdown.ClearOptions();
                dropdown.AddOptions(Info.AllPlayerInfo.UserInfo.decks.Select(deck => deck.DeckName).ToList());
                dropdown.value = Info.AllPlayerInfo.UserInfo.useDeckNum;
                var cardTexture = Command.CardInspector.CardLibraryCommand.GetCardStandardInfo(tempDeck.LeaderId).icon;
                //cardDeckNameModel.transform.GetChild(0).GetComponent<Image>().mainTexture. material.SetTexture("_Detail", cardTexture)  ;
            }
            int deskCardNumber = distinctCardIds.Count();
            int deskModelNumber = deckCardModels.Count;
            deckCardModels.ForEach(model => model.SetActive(false));
            if (deskCardNumber > deskModelNumber)
            {
                for (int i = 0; i < deskCardNumber - deskModelNumber; i++)
                {
                    var newCardModel = Instantiate(cardDeckCardModel, cardDeckContent.transform);
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
        }
        public void refreshDeckName()
        {
            //cardDeckNameModel
        }

        public void OnSelectDeckChange()
        {
            Dropdown dropdown = cardDeckNameModel.GetComponent<Dropdown>();
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
        public void FocusDeckCardOnMenu(GameObject cardModel)
        {
            int v = deckCardModels.IndexOf(cardModel);
            Debug.Log(v);
            int cardID = distinctCardIds[v];
            Control.GameUI.IntroductionControl.focusCardID = cardID;
        }
        public void LostFocusCardOnMenu()
        {
            Control.GameUI.IntroductionControl.focusCardID = 0;
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

