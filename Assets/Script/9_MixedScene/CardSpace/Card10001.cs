using CardModel;
using Command;
using GameEnum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Info.AgainstInfo;
namespace CardSpace
{
    public class Card10001 : Card
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
                    int targetCount=cardSet[Orientation.My][RegionTypes.Battle][CardTag.Fairy].count;
                    Debug.Log("场上妖精数量为"+targetCount);
                    for (int i = 0; i < targetCount; i++)
                    {
                        if (basePoint>1)
                        {
                            await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.Op][RegionTypes.Battle][CardRank.Silver,CardRank.Copper].CardList,1,isAuto:true);
                            await GameSystem.PointSystem.Hurt(TriggerInfo.Build(this,SelectUnits,1));
                            await GameSystem.PointSystem.Weak(TriggerInfo.Build(this,this,1));
                        }
                    }
                }
            };
        }
    }
}