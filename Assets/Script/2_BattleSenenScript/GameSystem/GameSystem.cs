using CardModel;
using CardSpace;
using Control;
using GameEnum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static Info.AgainstInfo;
using static Control.CardEffectStackControl;
namespace GameSystem
{
    /// <summary>
    /// 改变卡牌点数的相关机制
    /// </summary>
    public class PointSystem
    {
        public static async Task Gain(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Gain]);
        public static async Task Hurt(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Hurt]);
        public static async Task Cure(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Cure]);
        public static async Task Reset(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Reset]);
        public static async Task Destory(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Destory]);
        public static async Task Strengthen(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Strengthen]);
        public static async Task Weak(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Weak]);

    }
    /// <summary>
    /// 转移卡牌位置、所属区域的相关机制
    /// </summary>
    public class TransSystem
    {
        public static async Task DrawCard(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Draw]);
        public static async Task PlayCard(TriggerInfo triggerInfo, bool isAnsy = true)
        {
            if (triggerInfo.targetCard!=null)
            {
                await Command.CardCommand.PlayCard(triggerInfo.targetCard, isAnsy);
                await TriggerLogic(triggerInfo[TriggerType.Play]);
            }
        }
        /// <summary>
        /// 回收卡牌
        /// </summary>
        /// <param name="triggerInfo"></param>
        /// <returns></returns>
        public static async Task RecycleCard(TriggerInfo triggerInfo)
        {

        }
        /// <summary>
        /// 复活卡牌
        /// </summary>
        /// <param name="triggerInfo"></param>
        /// <returns></returns>
        public static async Task ReviveCard(TriggerInfo triggerInfo)
        {
            await TriggerLogic(triggerInfo[TriggerType.Revive]);
        }
        public static async Task MoveToGrave(TriggerInfo triggerInfo)
        {
            await Task.Delay(500);
            await Command.CardCommand.MoveToGrave(triggerInfo.targetCard);
        }

        public static async Task DeployCard(TriggerInfo triggerInfo)
        {
            //部署效果特殊处理，先部署再触发部署效果
            if (triggerInfo.targetCards.Any() && SelectRegion != null)
            {
                await Command.CardCommand.DeployCard(triggerInfo.targetCard);
            }
            await TriggerLogic(triggerInfo[TriggerType.Deploy]);
        }
        public static async Task BanishCard(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Banish]);
        public static async Task Discard(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Discard]);
        public static async Task SummonCard(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Summon]);
    }
    public class StateSystem
    {
        public static async Task SealCard(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Seal]);
        public static async Task CloseCard(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Close]);
        public static async Task ScoutCard(TriggerInfo triggerInfo) => await TriggerLogic(triggerInfo[TriggerType.Scout]);
    }
    public class FieldSystem
    {
        //直接改变，不触发机制
#pragma warning disable CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        public static async Task Increase(TriggerInfo triggerInfo, CardField cardField)
#pragma warning restore CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        {
            foreach (var targetCard in triggerInfo.targetCards)
            {

                if (targetCard[cardField] != 0)
                {
                    targetCard[cardField]++;
                }
            }
        }
        //临时方案
        public static async Task Change(TriggerInfo triggerInfo)
        {
            foreach (var targetCard in triggerInfo.targetCards)
            {
                await TriggerLogic(triggerInfo[targetCard][TriggerType.FieldChange]);
                //if (targetCard[cardField] != 0)
                //{
                //    targetCard[cardField]=triggerInfo.point;
                //    targetCard.replaceDescribeValue = triggerInfo.point;
                //}
            }
        }
    }
    /// <summary>
    /// 选择单位、区域、场景属性的相关机制
    /// </summary>
    public class SelectSystem
    {
        public static async Task SelectUnite(Card card, List<Card> targetCards, int num, bool isAuto = false) => await Command.StateCommand.WaitForSelecUnit(card, targetCards, num, isAuto);
        // public static async Task SelectUnite(Card card, List<Card> targetCards, int num, bool isAuto = false) => await TriggerLogic(TriggerInfo.Build(card, card, 0, card, targetCards, num, false)[TriggerType.SelectUnite]);
        public static async Task SelectLocation(Card card) => await Command.StateCommand.WaitForSelectLocation(card);
        public static async Task SelectRegion(RegionTypes regionType= RegionTypes.Battle, Territory territory= Territory.All) => await Command.StateCommand.WaitForSelectRegion(regionType, territory);
        public static async Task SelectBoardCard(List<Card> cards, CardBoardMode Mode = CardBoardMode.Select, int num = 1)
        {
            if (Mode == GameEnum.CardBoardMode.Select)
            {
                await Command.StateCommand.WaitForSelectBoardCard(cards, isMyTurn ? CardBoardMode.Select : CardBoardMode.ShowOnly, num);
            }
            else
            {
                await Command.StateCommand.WaitForSelectBoardCard(cards, Mode, num);
            }
        }

        public static async Task SelectBoardCard(List<int> cardIDs) => await Command.StateCommand.WaitForSelectBoardCard(cardIDs);
    }
    //由系统触发的状态机制
    public class ProcessSystem
    {
        public static async Task WhenTurnStart() => await TriggerLogic(TriggerInfo.Build(null, targetCard: null)[TriggerType.TurnStart]);
        public static async Task WhenTurnEnd() => await TriggerAll(new TriggerInfo(null, targetCard: null)[TriggerTime.When][TriggerType.TurnEnd]);
        public static async Task WhenRoundStart() => await TriggerLogic(TriggerInfo.Build(null, cardSet[RegionTypes.Battle].CardList)[TriggerType.RoundStart]);
        public static async Task WhenRoundEnd() => await TriggerAll(new TriggerInfo(null, targetCard: null)[TriggerTime.When][TriggerType.RoundEnd]);
    }
}