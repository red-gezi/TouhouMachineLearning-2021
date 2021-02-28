using CardModel;
using Command;
using GameEnum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Info.AgainstInfo;
namespace CardSpace
{
    public class Card20004 : Card
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
                    await GameSystem.SelectSystem.SelectRegion( RegionTypes.Battle, Territory.Op);
                    List<Card> targetCardList= SelectRegion.ThisRowCards;
                    int hurtMaxValue=twoSideVitality+1;
                    for (int i = 0; i < Math.Min(targetCardList.Count,hurtMaxValue); i++)
                    {
                        await GameSystem.PointSystem.Hurt(new TriggerInfo(this,targetCardList[i],hurtMaxValue-i));
                    }
                }
            };
        }
    }
}