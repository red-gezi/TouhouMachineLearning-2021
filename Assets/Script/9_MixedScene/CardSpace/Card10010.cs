using CardModel;
using Command;
using GameEnum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Info.AgainstInfo;
namespace CardSpace
{
    public class Card10010 : Card
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
                    //await GameSystem.SelectSystem.SelectUnite(this,cardSet[Orientation.My][CardField.Vitality].CardList,1,false);
                    foreach (var unite in cardSet[Orientation.My][CardField.Vitality].CardList)
                    {
                        await  GameSystem.FieldSystem.Change(new TriggerInfo(this,unite,unite[CardField.Vitality]+1));
                    }

                }
            };
        }
    }
}