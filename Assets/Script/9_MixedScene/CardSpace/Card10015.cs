using CardModel;
using Command;
using GameEnum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Info.AgainstInfo;
namespace CardSpace
{
    public class Card10015 : Card
    {
        public override void Init()
        {
            base.Init();

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
                    for (int i = 0; i < twoSideVitality+1; i++)
                    {
                        await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.Op][RegionTypes.Battle][CardRank.Copper,CardRank.Silver][CardFeature.Largest].CardList,1,true);
                        await GameSystem.PointSystem.Hurt(new TriggerInfo(this,SelectUnits,1));
                    }
                }
            };
        }
    }
}