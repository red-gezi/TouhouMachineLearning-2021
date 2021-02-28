using Extension;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Info.CardListInfo;

namespace Control
{
    public class CardListControl : MonoBehaviour
    {

        void Awake()
        {
            cardDeckCardModel.SetActive(false);
            multiModeCards = Command.CardLibrary.CardLibraryCommand.GetLibraryInfo().multiModeCards;
        }

        public void refreshDeckName()
        {
        }

        public void OnSelectDeckChange()
        {
            Dropdown dropdown = cardDeckNameModel.GetComponent<Dropdown>();
            Debug.Log("切换到deck" + dropdown.value);
            Info.AllPlayerInfo.UserInfo.useDeckNum = dropdown.value;
            tempDeck = Info.AllPlayerInfo.UserInfo.UseDeck.Clone();
            Command.CardListCommand.InitCardDeck(false);
        }
        public void CreatDeck()
        {
            Info.AllPlayerInfo.UserInfo.decks.Add(new Model.CardDeck("新卡组", 20002, new List<int> { 20002, 20001, 20001 }));
            Info.AllPlayerInfo.UserInfo.useDeckNum = Info.AllPlayerInfo.UserInfo.decks.Count;
            Debug.Log("切换到deck" + Info.AllPlayerInfo.UserInfo.useDeckNum);
            Command.Network.NetCommand.UpdateDecks(Info.AllPlayerInfo.UserInfo);
            Command.CardListCommand.InitCardDeck();
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
            Command.CardListCommand.InitCardDeck();
        }
        public void SaveDeck()
        {
            Debug.Log("保存卡组");
            Info.AllPlayerInfo.UserInfo.UseDeck = tempDeck;
            Command.Network.NetCommand.UpdateDecks(Info.AllPlayerInfo.UserInfo);
            Command.CardListCommand.InitCardDeck();

        }
        public void CancelDeck()
        {
            Debug.Log("取消卡组修改");
            tempDeck = Info.AllPlayerInfo.UserInfo.UseDeck;
            Command.CardListCommand.InitCardDeck();

        }
        public void FocusDeckCardOnMenu(GameObject cardModel)
        {
            int v = deckCardModels.IndexOf(cardModel);
            int cardID = distinctCardIds[v];
            Control.GameUI.IntroductionControl.focusCardID = cardID;
        }
        public void LostFocusCardOnMenu()
        {
            Control.GameUI.IntroductionControl.focusCardID = 0;
        }
        public void RemoveCardFromDeck(GameObject clickCard)
        {
            Debug.Log("移除卡牌");
            int removeCardId = distinctCardIds[deckCardModels.IndexOf(clickCard)];
            tempDeck.CardIds.Remove(removeCardId);
            Command.CardListCommand.InitCardDeck();
        }
    }
}

