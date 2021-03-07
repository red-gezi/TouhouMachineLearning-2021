using CardModel;
using Command;
using GameEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Info.AgainstInfo;
namespace CardSpace
{
    public class Card20008 : Card
    {
        public override void Init()
        {
            base.Init();

            this[CardField.Vitality] = 2;
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
                    List<Card> targetCardList= cardSet[Orientation.My][RegionTypes.Deck].CardList.Where(card=>card.CardId==20006||card.CardId==20007).ToList();
                    await GameSystem.TransSystem.SummonCard(new TriggerInfo(this,targetCardList));
                }
            };
            cardAbility[TriggerTime.When][TriggerType.FieldChange] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    EffectCommand.Bullet_Gain(triggerInfo);
                    EffectCommand.AudioEffectPlay(1);
                    await Task.Delay(1000);
                    this[CardField.Vitality]=triggerInfo.point;
                    replaceDescribeValue=this[CardField.Vitality];
                }
            };
        }

    }
}