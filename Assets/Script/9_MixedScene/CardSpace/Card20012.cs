using CardModel;
using Command;
using GameEnum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Info.AgainstInfo;
namespace CardSpace
{
    public class Card20012 : Card
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
                    await GameSystem.TransSystem.DeployCard(TriggerInfo.Build(this,this));
                }
            };
            cardAbility[TriggerTime.When][TriggerType.Deploy] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.My][RegionTypes.Battle][CardRank.Copper].CardList,1);
                    await GameSystem.PointSystem.Cure(TriggerInfo.Build(this,SelectUnits));
                }
            };
        }
    }
}