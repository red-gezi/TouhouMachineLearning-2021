using Extension;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Info.CardListInfo;
namespace Command
{
    public class CardListCommand : MonoBehaviour
    {
        //初始化牌组列表组件
        public static void InitCardDeck(bool isInitOptions = true)
        {
            tempDeck = Info.AllPlayerInfo.UserInfo.UseDeck.Clone();
            if (isInitOptions)
            {
                //初始化领袖栏
                Dropdown dropdown = cardDeckNameModel.GetComponent<Dropdown>();
                dropdown.ClearOptions();
                dropdown.AddOptions(Info.AllPlayerInfo.UserInfo.decks.Select(deck => deck.DeckName).ToList());
                dropdown.value = Info.AllPlayerInfo.UserInfo.useDeckNum;
                var cardTexture = Command.CardLibrary.CardLibraryCommand.GetCardStandardInfo(tempDeck.LeaderId).icon;
                deckCardModels.ForEach(model =>
                {
                    if (model!=null)
                    {
                        Destroy(model);
                    }
                });
                deckCardModels.Clear();
                //cardDeckNameModel.transform.GetChild(0).GetComponent<Image>().mainTexture. material.SetTexture("_Detail", cardTexture)  ;
            }
            int deskCardNumber = distinctCardIds.Count();
            int deskModelNumber = deckCardModels.Count;
            deckCardModels.ForEach(model => model.SetActive(false));
            Debug.Log("卡牌数量比" + deskCardNumber+"-"+ deskModelNumber);

            if (deskCardNumber > deskModelNumber)
            {
                for (int i = 0; i < deskCardNumber - deskModelNumber; i++)
                {
                    var newCardModel = Instantiate(cardDeckCardModel, cardDeckContent.transform);
                    deckCardModels.Add(newCardModel);
                    Debug.Log("新增卡牌");

                }
            }
            Debug.Log("去重数量为"+distinctCardIds.Count());
            //初始化卡牌栏
            for (int i = 0; i < distinctCardIds.Count(); i++)
            {
                int cardID = distinctCardIds[i];
                GameObject currentCardModel = deckCardModels[i];

                var info = multiModeCards.FirstOrDefault(cardInfo => cardInfo.cardId == cardID);
                currentCardModel.transform.GetChild(0).GetComponent<Text>().text = info.cardName;
                Sprite cardTex = Sprite.Create(info.icon, new Rect(0, 0, info.icon.width, info.icon.height), Vector2.zero);
                currentCardModel.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = cardTex;
                //设置数量
                Color RankColor = Color.white;
                switch (info.cardRank)
                {
                    case GameEnum.CardRank.Leader: RankColor = new Color(0.98f, 0.9f, 0.2f); break;
                    case GameEnum.CardRank.Gold: RankColor = new Color(0.98f, 0.9f, 0.2f); break;
                    case GameEnum.CardRank.Silver: RankColor = new Color(0.75f, 0.75f, 0.75f); break;
                    case GameEnum.CardRank.Copper: RankColor = new Color(0.55f, 0.3f, 0.1f); break;
                }
                //品质
                currentCardModel.transform.GetChild(2).GetComponent<Image>().color = RankColor;
                //点数
                int point = Command.CardLibrary.CardLibraryCommand.GetCardStandardInfo(cardID).point;
                currentCardModel.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = point == 0?" ": point+"";
                //数量
                currentCardModel.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "x"+tempDeck.CardIds.Count(id => id == cardID);

                currentCardModel.SetActive(true);
            }
        }
    }
}

