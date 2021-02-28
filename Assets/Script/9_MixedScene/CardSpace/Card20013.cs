using CardModel;
using Command;
using GameEnum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Info.AgainstInfo;
namespace CardSpace
{
    public class Card20013 : Card
    {
        public override void Init()
        {
            base.Init();

            this[CardField.Vitality] = 1;
            replaceDescribeValue = this[CardField.Vitality];

            cardAbility[TriggerTime.When][TriggerType.Play] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.My][RegionTypes.Battle][CardRank.Copper,CardRank.Silver][CardTag.Fairy].CardList,1);
                    await GameSystem.TransSystem.MoveToGrave(new TriggerInfo(this,this));
                    await GameSystem.TransSystem.PlayCard(new TriggerInfo(this,SelectUnits));
                }
            };
        }
    }
}