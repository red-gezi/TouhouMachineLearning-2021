using CardModel;
using Command;
using GameEnum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using static Info.AgainstInfo;
namespace CardSpace
{
    public class Card0 : Card
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
        }
    }
}