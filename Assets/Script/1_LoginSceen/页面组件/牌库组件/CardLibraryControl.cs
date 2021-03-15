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
    public class CardLibraryControl : MonoBehaviour
    {
        [ShowInInspector]
        static List<CardModelInfo> multiModeCards;
        public GameObject cardLibraryContent;
        static GameObject _cardLibraryContent;

        public GameObject cardLibraryCardModel;
        static GameObject _cardLibraryCardModel;

        static List<GameObject> libraryCardModels = new List<GameObject>();

        static Model.CardDeck tempDeck;
        public CardListControl cardDeckControl;
      
        void Awake()
        {
            _cardLibraryContent = cardLibraryContent;
            _cardLibraryCardModel = cardLibraryCardModel;
            _cardLibraryCardModel.SetActive(false);
        }
        public  void InitCardLibrary()
        {
            Command.CardListCommand.InitCardDeck();
            multiModeCards = Command.CardLibrary.CardLibraryCommand.GetLibraryInfo().multiModeCards;
            var hasCardLibrary = Info.AllPlayerInfo.UserInfo.cardLibrary;

            //牌库卡牌列表
            int libraryCardNumber = multiModeCards.Count();
            //已生成卡牌列表
            int libraryModelNumber = libraryCardModels.Count;
            libraryCardModels.ForEach(model => model.SetActive(false));
            if (libraryCardNumber > libraryModelNumber)
            {
                for (int i = 0; i < libraryCardNumber - libraryModelNumber; i++)
                {
                    var newCardModel = Instantiate(_cardLibraryCardModel, _cardLibraryContent.transform);
                    libraryCardModels.Add(newCardModel);
                }
            }
            for (int i = 0; i < libraryCardNumber; i++)
            {
                //卡牌信息集合
                var info = multiModeCards[i];
                //卡牌对应场景模型
                var newCardModel = libraryCardModels[i];
                //卡牌id
                string key = info.cardId.ToString();
                //该id的卡牌持有数量
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
                //newCardModel.GetComponent<CardLibraryCardControl>().id = key;
                newCardModel.SetActive(true);
            }
        }

        public void AddCardToDeck(GameObject clickCard)
        {
            //判断牌库是否有多余牌
            Debug.Log("添加卡牌");
            //判断卡组是否可添加
            int addCardId = multiModeCards[libraryCardModels.IndexOf(clickCard)].cardId;
            var cardInfo = Command.CardLibrary.CardLibraryCommand.GetCardStandardInfo(addCardId);
            int sameCardCountOnDeck = tempDeck.CardIds.Count(id => id == addCardId);
            if (sameCardCountOnDeck < 1 || (sameCardCountOnDeck < 3 && cardInfo.cardRank == GameEnum.CardRank.Copper))
            {
                tempDeck.CardIds.Add(addCardId);
            }
            else
            {
                Debug.Log("已溢出");
            }
            Command.CardListCommand.InitCardDeck();
        }
        public void FocusLibraryCardOnMenu(GameObject cardModel)
        {
            
            int cardID = multiModeCards[libraryCardModels.IndexOf(cardModel)].cardId;
            Control.GameUI.IntroductionControl.focusCardID = cardID;
        }
        public void LostFocusCardOnMenu()
        {
            Control.GameUI.IntroductionControl.focusCardID = 0;
        }
    }
}

