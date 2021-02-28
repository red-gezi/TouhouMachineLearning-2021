using CardModel;
using Command;
using GameEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static Info.AgainstInfo;
namespace CardSpace
{
    public class Card10002 : Card
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
                    await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.My][RegionTypes.Battle][CardRank.Copper,CardRank.Silver][CardTag.Fairy].CardList,1);
                    await GameSystem.PointSystem.Cure(TriggerInfo.Build(this,SelectUnits));
                    if (SelectUnits.Any())
                    {
                        SelectRegion=Info.RowsInfo.GetSingleRowInfoById(SelectUnits[0].location.x);
                        SelectLocation=SelectUnits[0].location.y;
                    }
                    await GameSystem.TransSystem.DeployCard(TriggerInfo.Build(this,SelectUnits));
                }
            };
        }
    }
}
