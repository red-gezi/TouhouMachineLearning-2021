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
    public class Card10006 : Card
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
                    if (!this[CardState.Seal])
                    {
                        List<Card> targetCardList= cardSet[Orientation.My][RegionTypes.Deck].CardList.Where(card=>card.CardId==10007||card.CardId==10008).ToList();
                        await GameSystem.TransSystem.SummonCard(new TriggerInfo(this,targetCardList));
                    }
                }
            };
            cardAbility[TriggerTime.When][TriggerType.FieldChange] = new List<Func<TriggerInfo, Task>>()
            {
#pragma warning disable CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
                async (triggerInfo) =>
#pragma warning restore CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
                {
                    EffectCommand.Bullet_Gain(triggerInfo);
                    EffectCommand.AudioEffectPlay(1);
                    //await Task.Delay(1000);
                    this[CardField.Vitality]=triggerInfo.point;
                    replaceDescribeValue=this[CardField.Vitality];
                }
            };
        }
    }
}
