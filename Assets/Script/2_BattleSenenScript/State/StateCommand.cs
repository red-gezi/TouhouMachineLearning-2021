using CardModel;
using CardSpace;
using Extension;
using GameEnum;
using Info;
using Model;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thread;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Command
{
    public static class StateCommand
    {
        public static async Task BattleStart()
        {
            AgainstInfo.isMyTurn = AgainstInfo.isPlayer1;
            AgainstInfo.cardSet = new CardSet();
            Info.StateInfo.TaskManager = new System.Threading.CancellationTokenSource();
            MainThread.Run(() =>
            {
                foreach (var item in GameObject.FindGameObjectsWithTag("SingleInfo"))
                {
                    SingleRowInfo singleRowInfo = item.GetComponent<SingleRowInfo>();
                    AgainstInfo.cardSet.singleRowInfos.Add(singleRowInfo);
                }
                AgainstInfo.cardSet.CardList = null;
            });
            await Task.Delay(500);
            if (AgainstInfo.isPVE)
            {
                AllPlayerInfo.UserInfo = new NetInfoModel.PlayerInfo(
                    "gezi", "yaya",
                    new List<CardDeck>
                    {
                        new CardDeck("gezi", 10001, new List<int>
                        {
                            //10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,
                             10015, 10016, 10012, 10013, 10014, 10015, 10016,10002, 10008, 10004, 10005, 10007, 10006,10008, 10004,  10012, 10010, 10011, 10012, 10014, 10007,10006, 10016,  10008, 10014
                            //10002, 10003, 10004, 10005, 10006, 10007, 10008, 10009, 10010, 10011, 10012, 10013, 10014, 10015, 10016, 10012, 10013, 10014, 10015, 10016, 10012, 10013, 10014, 10015, 10016
                            //1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004, 1004

                        })
                    });
                AllPlayerInfo.OpponentInfo = new NetInfoModel.PlayerInfo(
                    "gezi", "yaya",
                    new List<CardDeck>
                    {
                        new CardDeck("gezi", 10001, new List<int>
                        {
                            //10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,10002,
                             10002, 10003, 10004, 10005, 10006, 10007, 10008, 10009, 10010, 10011, 10012, 10013, 10014, 10015, 10016, 10012, 10013, 10014, 10015, 10016, 10012, 10013, 10014, 10015, 10016
                            //10002, 10003, 10004, 10005, 10006, 10007, 10008, 10009, 10010, 10011, 10012, 10013, 10014, 10015, 10016, 10012, 10013, 10014, 10015, 10016, 10012, 10013, 10014, 10015, 10016
                        })
                    });
            }
            RowCommand.SetRegionSelectable(RegionTypes.None);
            //Debug.LogError("初始双方信息");
            await Task.Run(async () =>
            {
                try
                {
                    //await Task.Delay(500);
                    Debug.Log("对战开始".TransUiText());
                    GameUI.UiCommand.SetNoticeBoardTitle("对战开始".TransUiText());
                    await GameUI.UiCommand.NoticeBoardShow();
                    //初始化我方领袖卡
                    Card MyLeaderCard = await CardCommand.CreatCard(AllPlayerInfo.UserInfo.UseDeck.LeaderId);
                    AgainstInfo.cardSet[Orientation.Down][RegionTypes.Leader].Add(MyLeaderCard);
                    MyLeaderCard.SetCardSeeAble(true);
                    //初始化敌方领袖卡
                    Card OpLeaderCard = await CardCommand.CreatCard(AllPlayerInfo.OpponentInfo.UseDeck.LeaderId);
                    AgainstInfo.cardSet[Orientation.Up][RegionTypes.Leader].Add(OpLeaderCard);
                    OpLeaderCard.SetCardSeeAble(true);
                    //初始双方化牌组
                    CardDeck Deck = AllPlayerInfo.UserInfo.UseDeck;
                    //Debug.LogError("初始双方化牌组");

                    for (int i = 0; i < Deck.CardIds.Count; i++)
                    {
                        Card NewCard = await CardCommand.CreatCard(Deck.CardIds[i]);
                        CardSet cardSet = AgainstInfo.cardSet[Orientation.Down][RegionTypes.Deck];
                        cardSet.Add(NewCard);
                    }
                    Deck = AllPlayerInfo.OpponentInfo.UseDeck;

                    for (int i = 0; i < Deck.CardIds.Count; i++)
                    {
                        Card NewCard = await CardCommand.CreatCard(Deck.CardIds[i]);
                        AgainstInfo.cardSet[Orientation.Up][RegionTypes.Deck].Add(NewCard);
                    }
                    await Task.Delay(000);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            });
        }
        public static async Task BattleEnd(bool IsSurrender = false, bool IsWin = true)
        {
            //Debug.Log("回合结束");
            await Task.Run(async () =>
            {
                GameUI.UiCommand.SetNoticeBoardTitle($"对战终止\n{AgainstInfo.ShowScore.MyScore}:{AgainstInfo.ShowScore.OpScore}");
                await GameUI.UiCommand.NoticeBoardShow();
                await Task.Delay(2000);
                MainThread.Run(() =>
                {
                    Debug.Log("释放线程资源");
                    StateInfo.TaskManager.Cancel();
                    SceneManager.LoadSceneAsync(1);
                });
            });
        }
        public static async Task RoundStart(int num)
        {
            await Task.Run(async () =>
            {
                GameUI.UiCommand.ReSetPassState();
                GameUI.UiCommand.SetNoticeBoardTitle($"第{num + 1}小局开始");
                await GameUI.UiCommand.NoticeBoardShow();
                //await Task.Delay(2000);
                switch (num)
                {
                    case (0):
                        {
                            Info.AgainstInfo.ExChangeableCardNum = 3;
                            Info.GameUI.UiInfo.CardBoardTitle = "剩余抽卡次数为".TransUiText() + Info.AgainstInfo.ExChangeableCardNum;
                            for (int i = 0; i < 10; i++)
                            {
                                await CardCommand.DrawCard(IsPlayerDraw: true, isOrder: false);
                            }
                            for (int i = 0; i < 10; i++)
                            {
                                await CardCommand.DrawCard(IsPlayerDraw: false, isOrder: false);
                            }
                            CardCommand.OrderCard();
                            break;
                        }
                    case (1):
                        {
                            Info.AgainstInfo.ExChangeableCardNum += 1;
                            Info.GameUI.UiInfo.CardBoardTitle = "剩余抽卡次数为" + Info.AgainstInfo.ExChangeableCardNum;
                            await CardCommand.DrawCard();
                            await CardCommand.DrawCard(false);
                            break;
                        }
                    case (2):
                        {
                            Info.AgainstInfo.ExChangeableCardNum += 1;
                            Info.GameUI.UiInfo.CardBoardTitle = "剩余抽卡次数为" + Info.AgainstInfo.ExChangeableCardNum;
                            await CardCommand.DrawCard();
                            await CardCommand.DrawCard(false);
                            break;
                        }
                    default:
                        break;
                }
                await Task.Delay(2500);
                AgainstInfo.isRoundStartExchange = true;
                await WaitForSelectBoardCard(AgainstInfo.cardSet[Orientation.Down][RegionTypes.Hand].CardList, CardBoardMode.ExchangeCard);
                AgainstInfo.isRoundStartExchange = false;
            });
        }
        public static async Task RoundEnd(int num)
        {
            await Task.Run(async () =>
            {
                GameUI.UiCommand.SetNoticeBoardTitle($"第{num + 1}小局结束\n{PointInfo.TotalDownPoint}:{PointInfo.TotalUpPoint}\n{((PointInfo.TotalDownPoint > PointInfo.TotalUpPoint) ? "Win" : "Lose")}");
                await GameUI.UiCommand.NoticeBoardShow();
                //await Task.Delay(2000);
                int result = 0;
                if (PointInfo.TotalPlayer1Point > PointInfo.TotalPlayer2Point)
                {
                    result = 1;
                }
                else if (PointInfo.TotalPlayer1Point < PointInfo.TotalPlayer2Point)
                {
                    result = 2;
                }
                AgainstInfo.PlayerScore.P1Score += result == 0 || result == 1 ? 1 : 0;
                AgainstInfo.PlayerScore.P2Score += result == 0 || result == 2 ? 1 : 0;
                await Task.Delay(3500);
                await GameSystem.ProcessSystem.WhenRoundEnd();
            });
        }
        public static async Task TurnStart()
        {
            await Task.Run(async () =>
            {
                GameUI.UiCommand.SetNoticeBoardTitle(AgainstInfo.isMyTurn ? "我方回合开始".TransUiText() : "对方回合开始".TransUiText());
                await GameUI.UiCommand.NoticeBoardShow();
                //await Task.Delay(000);
                AgainstInfo.IsCardEffectCompleted = false;
                RowCommand.SetPlayCardMoveFree(AgainstInfo.isMyTurn);
                await Task.Delay(000);
            });
        }
        public static async Task TurnEnd()
        {
            await Task.Run(async () =>
            {
                await GameSystem.ProcessSystem.WhenTurnEnd();
                GameUI.UiCommand.SetNoticeBoardTitle(AgainstInfo.isMyTurn ? "我方回合结束".TransUiText() : "对方回合结束".TransUiText());
                await GameUI.UiCommand.NoticeBoardShow();
                //await Task.Delay(000);
                RowCommand.SetPlayCardMoveFree(false);
                await Task.Delay(1000);
                AgainstInfo.isMyTurn = !AgainstInfo.isMyTurn;
            });
        }
        public static async Task WaitForPlayerOperation()
        {
            bool isFirstOperation = true;
            //Timer.SetIsTimerStart(60);
            Timer.SetIsTimerStart(6000);
            await Task.Run(async () =>
            {
                //Debug.Log("出牌");
                //当出牌,弃牌,pass时结束
                while (true)
                {
                    StateInfo.TaskManager.Token.ThrowIfCancellationRequested();
                    if (Timer.isTimeout)
                    {
                        Debug.Log("超时");
                    }
                    if (AgainstInfo.isAIControl && isFirstOperation)
                    {
                        isFirstOperation = false;
                        Debug.Log("自动出牌");
                        await AiCommand.TempOperationPlayCard();
                    }
                    if (AgainstInfo.IsCardEffectCompleted)
                    {
                        AgainstInfo.IsCardEffectCompleted = false;
                        break;
                    }
                    if (AgainstInfo.isCurrectPass)
                    {
                        AgainstInfo.IsCardEffectCompleted = false;
                        break;
                    }
                    await Task.Delay(1000);
                }
            });
            Timer.SetIsTimerClose();
        }
        public static async Task WaitForSelectProperty()
        {
            //放大硬币
            CoinControl.ScaleUp();
            await Task.Delay(1000);
            CoinControl.Unfold();
            AgainstInfo.IsWaitForSelectProperty = true;
            AgainstInfo.SelectProperty = Region.None;
            //暂时设为1秒，之后还原成10秒
            Timer.SetIsTimerStart(1);
            //AgainstInfo.SelectRegion = null;
            await Task.Run(async () =>
            {
                // Debug.Log("等待选择属性");
                while (AgainstInfo.SelectProperty == Region.None)
                {
                    StateInfo.TaskManager.Token.ThrowIfCancellationRequested();
                    if (AgainstInfo.isAIControl)
                    {
                        //Debug.Log("自动选择属性");
                        int rowRank = AiCommand.GetRandom(0, 4);
                        CoinControl.ChangeProperty((Region)rowRank);
                    }
                    await Task.Delay(1000);
                }
            });
            Command.Network.NetCommand.AsyncInfo(NetAcyncType.SelectProperty);
            Timer.SetIsTimerClose();
            AgainstInfo.IsWaitForSelectProperty = false;
            await Task.Delay(1000);
            CoinControl.ScaleDown();
        }
        public static async Task WaitForSelectRegion(RegionTypes regionTypes, Territory territory)
        {
            AgainstInfo.IsWaitForSelectRegion = true;
            AgainstInfo.SelectRegion = null;
            RowCommand.SetRegionSelectable(regionTypes, territory);
            await Task.Run(async () =>
            {
                while (Info.AgainstInfo.SelectRegion == null)
                {
                    StateInfo.TaskManager.Token.ThrowIfCancellationRequested();
                    if (AgainstInfo.isAIControl)
                    {
                        await Task.Delay(1000);
                        List<SingleRowInfo> rows = AgainstInfo.cardSet.singleRowInfos.Where(row => row.CanBeSelected).ToList();
                        int rowRank = AiCommand.GetRandom(0, rows.Count());
                        AgainstInfo.SelectRegion = rows[rowRank];//设置部署区域
                    }
                    await Task.Delay(1000);
                }
            });
            Network.NetCommand.AsyncInfo(NetAcyncType.SelectRegion);
            RowCommand.SetRegionSelectable(RegionTypes.None);
            AgainstInfo.IsWaitForSelectRegion = false;
        }
        public static async Task WaitForSelectLocation(Card card)
        {
            AgainstInfo.IsWaitForSelectLocation = true;
            RowCommand.SetRegionSelectable((RegionTypes)card.region, card.territory);
            AgainstInfo.SelectLocation = -1;
            await Task.Run(async () =>
            {
                while (AgainstInfo.SelectLocation < 0)
                {
                    StateInfo.TaskManager.Token.ThrowIfCancellationRequested();
                    if (AgainstInfo.isAIControl)
                    {
                        await Task.Delay(1000);
                        List<SingleRowInfo> rows = AgainstInfo.cardSet.singleRowInfos.Where(row => row.CanBeSelected).ToList();
                        int rowRank = AiCommand.GetRandom(0, rows.Count());
                        AgainstInfo.SelectRegion = rows[rowRank];//设置部署区域
                        AgainstInfo.SelectLocation = 0;//设置部署次序
                    }
                    await Task.Delay(1000);
                }
            });
            Debug.Log($"选择坐标{AgainstInfo.SelectLocation}");
            Network.NetCommand.AsyncInfo(NetAcyncType.SelectLocation);
            RowCommand.SetRegionSelectable(RegionTypes.None);
            AgainstInfo.IsWaitForSelectLocation = false;
        }
        public static async Task WaitForSelecUnit(Card OriginCard, List<Card> Cards, int num, bool isAuto)
        {
            //可选列表中移除自身
            Cards.Remove(OriginCard);
            AgainstInfo.ArrowStartCard = OriginCard;
            AgainstInfo.IsWaitForSelectUnits = true;
            AgainstInfo.AllCardList.ForEach(card => card.isGray = true);
            Cards.ForEach(card => card.isGray = false);
            AgainstInfo.SelectUnits.Clear();
            await Task.Run(async () =>
            {
                //await Task.Delay(500);
                if (Info.AgainstInfo.isMyTurn && !isAuto)
                {
                    GameUI.UiCommand.CreatFreeArrow();
                }
                int selectableNum = Math.Min(Cards.Count, num);
                while (AgainstInfo.SelectUnits.Count < selectableNum)
                {
                    StateInfo.TaskManager.Token.ThrowIfCancellationRequested();
                    //当pve模式或者pvp中的我方回合时，用自身随机决定，否则等待网络同步
                    if (AgainstInfo.isAIControl || (isAuto && AgainstInfo.isMyTurn))
                    {
                        Debug.Log("自动选择场上单位");
                        Cards = Cards.OrderBy(x => AiCommand.GetRandom(0, 514)).ToList();
                        AgainstInfo.SelectUnits = Cards.Take(selectableNum).ToList();
                    }
                    await Task.Delay(100);
                }
                Debug.Log("选择单位完毕" + Math.Min(Cards.Count, num));
                Network.NetCommand.AsyncInfo(NetAcyncType.SelectUnites);
                GameUI.UiCommand.DestoryAllArrow();
                await Task.Delay(250);
                Debug.Log("同步选择单位完毕");
            });
            AgainstInfo.AllCardList.ForEach(card => card.isGray = false);
            AgainstInfo.IsWaitForSelectUnits = false;
            Debug.Log("结束选择单位");
        }
        public static async Task WaitForSelectBoardCard<T>(List<T> CardIds, CardBoardMode Mode = CardBoardMode.Select, int num = 1)
        {
            AgainstInfo.selectBoardCardRanks = new List<int>();
            AgainstInfo.IsFinishSelectBoardCard = false;
            AgainstInfo.cardBoardMode = Mode;
            GameUI.UiCommand.SetCardBoardShow();
            if (typeof(T) == typeof(Card))
            {
                GameUI.CardBoardCommand.LoadBoardCardList(CardIds.Cast<Card>().ToList());
            }
            else
            {
                GameUI.CardBoardCommand.LoadBoardCardList(CardIds.Cast<int>().ToList());
            }
            //Debug.Log("进入选择模式");
            await Task.Run(async () =>
            {

                switch (Mode)
                {
                    case CardBoardMode.Select:
                        while (AgainstInfo.selectBoardCardRanks.Count < Mathf.Min(CardIds.Count, num) && !AgainstInfo.IsFinishSelectBoardCard) { }
                        GameUI.UiCommand.SetCardBoardHide();
                        break;
                    case CardBoardMode.ExchangeCard:
                        {
                            AiCommand.RoundStartExchange(false);
                            while (Info.AgainstInfo.ExChangeableCardNum != 0 && !Info.AgainstInfo.IsSelectCardOver)
                            {
                                StateInfo.TaskManager.Token.ThrowIfCancellationRequested();
                                if (Info.AgainstInfo.selectBoardCardRanks.Count > 0)
                                {
                                    //List<Card> CardLists = CardIds.Cast<Card>().ToList();
                                    List<Card> CardLists = AgainstInfo.cardSet[Orientation.Down][RegionTypes.Hand].CardList;
                                    await CardCommand.ExchangeCard(CardLists[AgainstInfo.selectBoardCardRanks[0]], isRoundStartExchange: true);
                                    Info.AgainstInfo.ExChangeableCardNum--;
                                    Info.AgainstInfo.selectBoardCardRanks.Clear();
                                    GameUI.UiCommand.SetCardBoardTitle("剩余抽卡次数为" + Info.AgainstInfo.ExChangeableCardNum);
                                }
                            }
                            if (AgainstInfo.isPlayer1)
                            {
                                AgainstInfo.isPlayer1RoundStartExchangeOver = true;
                            }
                            else
                            {
                                AgainstInfo.isPlayer2RoundStartExchangeOver = true;
                            }
                            GameUI.UiCommand.SetCardBoardHide();
                            Network.NetCommand.AsyncInfo(NetAcyncType.RoundStartExchangeOver);
                            while (!(AgainstInfo.isPlayer1RoundStartExchangeOver && AgainstInfo.isPlayer2RoundStartExchangeOver))
                            {
                                StateInfo.TaskManager.Token.ThrowIfCancellationRequested();
                                Debug.Log("等待双方选择完毕");
                                Debug.Log("等待双方选择完毕" + AgainstInfo.isPlayer1RoundStartExchangeOver + ":" + AgainstInfo.isPlayer2RoundStartExchangeOver);
                                await Task.Delay(1000);
                            }
                            AgainstInfo.isPlayer1RoundStartExchangeOver = false;
                            AgainstInfo.isPlayer2RoundStartExchangeOver = false;
                            AgainstInfo.IsSelectCardOver = false;
                            break;
                        }
                    case CardBoardMode.ShowOnly:
                        while (AgainstInfo.selectBoardCardRanks.Count < Mathf.Min(CardIds.Count, num) && !AgainstInfo.IsFinishSelectBoardCard) { }
                        GameUI.UiCommand.SetCardBoardHide();
                        break;
                    default:
                        break;

                }
            });

            AgainstInfo.cardBoardMode = CardBoardMode.None;
        }
        public static async Task Surrender()
        {
            Debug.Log("投降");
            await Task.Run(async () =>
            {
                Command.Network.NetCommand.AsyncInfo(NetAcyncType.Surrender);
                await BattleEnd(true, false);
            });
        }
    }
}

