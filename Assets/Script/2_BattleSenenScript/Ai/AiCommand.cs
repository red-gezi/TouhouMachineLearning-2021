using CardModel;
using Extension;
using GameEnum;
using Info;
using System;
using System.Threading.Tasks;

namespace Command
{
    public class AiCommand
    {
        static Random rand = new Random("gezi".GetHashCode());
        public static void Init() => rand = new Random("gezi".GetHashCode());
        public static int GetRandom(int Min, int Max) => rand.Next(Min, Max);
        public static async Task RoundStartExchange(bool isControlPlayer)
        {
            if (isControlPlayer)//超时自动操纵玩家时
            {
                AgainstInfo.IsSelectCardOver = true;
                if (AgainstInfo.isPlayer1)
                {
                    AgainstInfo.isPlayer1RoundStartExchangeOver = true;
                }
                else
                {
                    AgainstInfo.isPlayer2RoundStartExchangeOver = true;
                }
            }
            else//操纵对面Ai
            {
                if (AgainstInfo.isPVE)
                {
                    UnityEngine.Debug.Log("ai选择了交换卡牌"+ AgainstInfo.isPlayer1);
                    if (AgainstInfo.isPlayer1)
                    {
                        AgainstInfo.isPlayer2RoundStartExchangeOver = true;
                    }
                    else
                    {
                        AgainstInfo.isPlayer1RoundStartExchangeOver = true;
                    }
                }
            }
            
            
        }
        public static async Task TempOperationPlayCard()
        {
            if ((Info.AgainstInfo.isDownPass && Info.PointInfo.TotalDownPoint < Info.PointInfo.TotalUpPoint) ||
                Info.AgainstInfo.cardSet[Orientation.My][RegionTypes.Hand].CardList.Count == 0)
            {
                GameUI.UiCommand.SetCurrentPass();
            }
            else
            {

                Card targetCard = Info.AgainstInfo.cardSet[Orientation.My][RegionTypes.Hand].CardList[0];
                await GameSystem.TransSystem.PlayCard(TriggerInfo.Build(targetCard, targetCard));
                Info.AgainstInfo.IsCardEffectCompleted = true;
                //await CardCommand.PlayCard(targetCard);
            }
        }
        //临时的ai操作
        public static async Task TempOperationDiscard()
        {
            if (Info.AgainstInfo.isDownPass || Info.AgainstInfo.isUpPass)
            {
                Command.GameUI.UiCommand.SetCurrentPass();
            }
            else
            {
                await CardCommand.DisCard(Info.AgainstInfo.cardSet[Orientation.My][RegionTypes.Hand].CardList[0]);
            }
        }
    }
}

