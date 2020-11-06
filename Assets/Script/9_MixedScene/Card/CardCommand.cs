using CardModel;
using CardSpace;
using Command.CardInspector;
using Control;
using Extension;
using GameEnum;
using Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thread;
using UnityEngine;

namespace Command
{
    //具体实现，还需进一步简化
    public static class CardCommand
    {
        public static void OrderCard() => AgainstInfo.cardSet[RegionTypes.Hand].Order();
        public static void RemoveCard(Card card) => card.belongCardList.Remove(card);
        public static async Task<Card> CreatCard(int id)
        {
            Card NewCardScript = null;
            MainThread.Run(() =>
            {
                GameObject NewCard = GameObject.Instantiate(Info.CardInfo.cardModel);
                NewCard.transform.SetParent(GameObject.FindGameObjectWithTag("Card").transform);
                NewCard.name = "Card" + Info.CardInfo.CreatCardRank++;
                var CardStandardInfo = CardLibraryCommand.GetCardStandardInfo(id);
                NewCard.AddComponent(Type.GetType("CardSpace.Card" + id));
                Card card = NewCard.GetComponent<Card>();
                card.CardId = CardStandardInfo.cardId;
                card.basePoint = CardStandardInfo.point;
                card.icon = CardStandardInfo.icon;
                card.region = CardStandardInfo.cardProperty;
                card.territory = CardStandardInfo.cardTerritory;
                card.cardTag = CardStandardInfo.cardTag;
                card.cardRank = CardStandardInfo.cardRank;
                card.cardType = CardStandardInfo.cardType;
                card.GetComponent<Renderer>().material.SetTexture("_Front", card.icon);
                switch (card.cardRank)
                {
                    case CardRank.Leader: card.GetComponent<Renderer>().material.SetColor("_side", new Color(0.43f, 0.6f, 1f)); break;
                    case CardRank.Gold: card.GetComponent<Renderer>().material.SetColor("_side", new Color(0.8f, 0.8f, 0f)); break;
                    case CardRank.Silver: card.GetComponent<Renderer>().material.SetColor("_side", new Color(0.75f, 0.75f, 0.75f)); break;
                    case CardRank.Copper: card.GetComponent<Renderer>().material.SetColor("_side", new Color(1, 0.42f, 0.37f)); break;
                    default: break;
                }
                //if (card.cardType== CardType.Special)
                //{
                //    card.transform.GetChild(0).GetChild(0).get
                //}
                card.Init();
                NewCardScript = card;
            });
            await Task.Run(() => { while (NewCardScript == null) { } });
            return NewCardScript;
        }
        public static async Task BanishCard(Card card)
        {
            MainThread.Run(() => { card.GetComponent<CardControl>().CreatGap(); });
            await Task.Delay(800);
            MainThread.Run(() => { card.GetComponent<CardControl>().FoldGap(); });
            await Task.Delay(800);
            MainThread.Run(() => { card.GetComponent<CardControl>().DestoryGap(); });
            RemoveCard(card);
        }
        public static async Task SummonCard(Card targetCard)
        {
            //await Task.Delay(1000);
            RemoveCard(targetCard);
            List<Card> TargetRow = AgainstInfo
                .cardSet[(RegionTypes)targetCard.region][(Orientation)targetCard.territory]
                .singleRowInfos.First().ThisRowCards;
            TargetRow.Add(targetCard);
            targetCard.isCanSee = true;
            //targetCard.moveSpeed = 0.1f;
            targetCard.isMoveStepOver = false;
            await Task.Delay(1000);
            targetCard.isMoveStepOver = true;
            //targetCard.moveSpeed = 0.1f;
            EffectCommand.AudioEffectPlay(1);
        }

        public static async Task DeployCard(Card targetCard)
        {
            List<Card> TargetRow = AgainstInfo.SelectRegion.ThisRowCards;
            RemoveCard(targetCard);
            TargetRow.Insert(AgainstInfo.SelectLocation, targetCard);
            //targetCard.moveSpeed = 0.1f;
            targetCard.isMoveStepOver = false;
            await Task.Delay(1000);
            targetCard.isMoveStepOver = true;
            //targetCard.moveSpeed = 0.1f;
            EffectCommand.AudioEffectPlay(1);
        }
        public static async Task ExchangeCard(Card targetCard, bool IsPlayerExchange = true,bool isRoundStartExchange=false, int RandomRank = 0)
        {
            //Debug.Log("交换卡牌");
            await WashCard(targetCard, IsPlayerExchange, RandomRank);
            await DrawCard(IsPlayerExchange, true);
            if (IsPlayerExchange)
            {
                GameUI.CardBoardCommand.LoadBoardCardList(AgainstInfo.cardSet[isRoundStartExchange? Orientation.Down:Orientation.My][RegionTypes.Hand].CardList);
            }
        }
        internal static Task RebackCard()
        {
            throw new NotImplementedException();
        }
        public static async Task DrawCard(bool IsPlayerDraw = true, bool ActiveBlackList = false, bool isOrder = true)
        {
            //Debug.Log("抽卡");
            EffectCommand.AudioEffectPlay(0);
            Card TargetCard = AgainstInfo.cardSet[IsPlayerDraw ? Orientation.Down : Orientation.Up][RegionTypes.Deck].CardList[0];
            TargetCard.SetCardSeeAble(IsPlayerDraw);
            CardSet TargetCardtemp = AgainstInfo.cardSet[IsPlayerDraw ? Orientation.Down : Orientation.Up][RegionTypes.Deck];

            AgainstInfo.cardSet[IsPlayerDraw ? Orientation.Down : Orientation.Up][RegionTypes.Deck].Remove(TargetCard);
            AgainstInfo.cardSet[IsPlayerDraw ? Orientation.Down : Orientation.Up][RegionTypes.Hand].Add(TargetCard);
            if (isOrder)
            {
                OrderCard();
            }
            await Task.Delay(100);
        }
        public static async Task WashCard(Card TargetCard, bool IsPlayerWash = true, int InsertRank = 0)
        {
            Debug.Log("洗回卡牌");
            if (IsPlayerWash)
            {
                AgainstInfo.TargetCard = TargetCard;
                int MaxCardRank = AgainstInfo.cardSet[Orientation.Down][RegionTypes.Deck].CardList.Count;
                AgainstInfo.RandomRank = AiCommand.GetRandom(0, MaxCardRank);
                Network.NetCommand.AsyncInfo(NetAcyncType.ExchangeCard);
                AgainstInfo.cardSet[Orientation.Down][RegionTypes.Hand].Remove(TargetCard);
                AgainstInfo.cardSet[Orientation.Down][RegionTypes.Deck].Add(TargetCard, AgainstInfo.RandomRank);
                TargetCard.SetCardSeeAble(false);
            }
            else
            {
                AgainstInfo.cardSet[Orientation.Up][RegionTypes.Hand].Remove(TargetCard);
                AgainstInfo.cardSet[Orientation.Up][RegionTypes.Deck].Add(TargetCard, InsertRank);
            }
            await Task.Delay(500);
        }
        public static async Task PlayCard(Card targetCard, bool IsAnsy = true)
        {

            AgainstInfo.PlayerPlayCard = targetCard;
            EffectCommand.AudioEffectPlay(0);
            RowCommand.SetPlayCardMoveFree(false);
            targetCard.isPrepareToPlay = false;
            if (IsAnsy)
            {
                Network.NetCommand.AsyncInfo(NetAcyncType.PlayCard);
            }
            targetCard.SetCardSeeAble(true);
            RemoveCard(targetCard);
            AgainstInfo.cardSet[Orientation.My][RegionTypes.Uesd].Add(targetCard);
            AgainstInfo.PlayerPlayCard = null;
        }
        public static async Task DisCard(Card card)
        {
            card.isPrepareToPlay = false;
            card.SetCardSeeAble(false);
            RemoveCard(card);
            AgainstInfo.cardSet[Orientation.My][RegionTypes.Grave].Add(card);
            AgainstInfo.PlayerPlayCard = null;
        }

        public static async Task ReviveCard(TriggerInfo triggerInfo)
        {
            Card card = triggerInfo.targetCard;
            EffectCommand.AudioEffectPlay(0);
            card.SetCardSeeAble(true);
            RemoveCard(card);
            AgainstInfo.cardSet[Orientation.My][RegionTypes.Uesd].Add(card);
            await card.cardAbility[TriggerTime.When][TriggerType.Play][0](triggerInfo);
        }

        public static async Task SealCard(Card card)
        {
            Debug.Log("锁定卡牌！");
            if (card.cardStates.ContainsKey(CardState.Seal) && card.cardStates[CardState.Seal])
            {
                card.cardStates[CardState.Seal] = false;
                MainThread.Run(() =>
                {
                    card.transform.GetChild(2).gameObject.SetActive(false);
                });
            }
            else
            {
                card.cardStates[CardState.Seal] = true;
                MainThread.Run(() =>
                {
                    card.transform.GetChild(2).gameObject.SetActive(true);
                });
            }
        }

        public static async Task Gain(TriggerInfo triggerInfo)
        {
            EffectCommand.Bullet_Gain(triggerInfo);
            EffectCommand.AudioEffectPlay(1);
            await Task.Delay(1000);
            triggerInfo.targetCard.changePoint += triggerInfo.point;
            await Task.Delay(1000);
        }
        public static async Task Hurt(TriggerInfo triggerInfo)
        {
            EffectCommand.Bullet_Hurt(triggerInfo);
            EffectCommand.AudioEffectPlay(1);
            await Task.Delay(1000);
            triggerInfo.targetCard.changePoint -= triggerInfo.point;
            await Task.Delay(1000);
        }
        public static async Task MoveToGrave(Card card, int Index = 0)
        {
            Orientation orientation = card.belong == Territory.My ? Orientation.Down : Orientation.Up;
            RemoveCard(card);
            AgainstInfo.cardSet[orientation][RegionTypes.Grave].singleRowInfos[0].ThisRowCards.Insert(Index, card);
            card.SetCardSeeAble(false);
            card.changePoint = 0;
            card.isMoveStepOver = false;
            await Task.Delay(100);
            card.isMoveStepOver = true;
            EffectCommand.AudioEffectPlay(1);
        }
    }
}