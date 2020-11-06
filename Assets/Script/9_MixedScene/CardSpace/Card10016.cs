using CardModel;
using Command;
using GameEnum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Info.AgainstInfo;
namespace CardSpace
{
    public class Card10016 : Card
    {
        public override void Init()
        {
            base.Init();

            cardAbility[TriggerTime.When][TriggerType.Play] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    await Task.Delay(1000);
                    await  GameSystem.SelectSystem.SelectBoardCard(cardSet[Orientation.My][RegionTypes.Grave][CardRank.Copper][CardTag.Fairy][CardType.Unite].CardList);
                    await  GameSystem.TransSystem.MoveToGrave(new TriggerInfo(this,this));
                    await  GameSystem.TransSystem.ReviveCard(new TriggerInfo(this,selectActualCards));
                }
            };
        }
    }
}