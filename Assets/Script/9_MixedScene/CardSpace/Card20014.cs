using CardModel;
using Command;
using GameEnum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Info.AgainstInfo;
namespace CardSpace
{
    public class Card20014 : Card
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
                    await GameSystem.SelectSystem.SelectLocation(this);
                    await GameSystem.TransSystem.DeployCard(new TriggerInfo(this,this));
                }
            };
            cardAbility[TriggerTime.When][TriggerType.Deploy] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.My][RegionTypes.Deck][CardRank.Copper][CardFeature.Lowest][CardType.Unite][CardTag.Fairy].CardList,1,true);
                    await GameSystem.TransSystem.PlayCard(new TriggerInfo(this,SelectUnits,1));
                }
            };
        }
    }
}