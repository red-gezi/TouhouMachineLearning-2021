using CardModel;
using CardSpace;
using Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;

namespace Control
{

    public class CardEffectStackControl : MonoBehaviour
    {
        //[ShowInInspector]
        //public static bool IsRuning;
        //[ShowInInspector]
        //public static int taskCount = 0;
        //[ShowInInspector]
        //public Dictionary<string, int> dict;
        //[ShowInInspector]
        //public static (Func<TriggerInfo, Task>, TriggerInfo) taskinfo;
        //[ShowInInspector]
        //public static Stack<(Func<TriggerInfo, Task>, TriggerInfo)> runTaskStack = new Stack<(Func<TriggerInfo, Task>, TriggerInfo)>();
        //[ShowInInspector]
        //public static Stack<(Func<TriggerInfo, Task>, TriggerInfo)> tempTaskStack = new Stack<(Func<TriggerInfo, Task>, TriggerInfo)>();
        //public static async Task Run()
        //{
        //    Debug.LogError("执行状态" + IsRuning);
        //    if (!IsRuning)
        //    {
        //        IsRuning = true;
        //        while (runTaskStack.Count != 0)
        //        {
        //            Debug.LogError("开始执行任务，当前数量为" + runTaskStack.Count);
        //            taskinfo = runTaskStack.Pop();
        //            Thread.MainThread.Run(() =>
        //            {
        //                Debug.LogError($"{taskinfo.Item2.triggerTime} {taskinfo.Item2.triggerCard.name}触发{taskinfo.Item2.targetCard.name}的{taskinfo.Item2.triggerType}效果");

        //            });
        //            await taskinfo.Item1(taskinfo.Item2);
        //        }
        //        IsRuning = false;
        //    }
        //}
        //public static async Task Run(TriggerInfo triggerInfo)
        //{
        //    var tasks = triggerInfo.targetCard.cardEffect[triggerInfo.triggerTime][triggerInfo.triggerType];
        //    Thread.MainThread.Run(() =>
        //    {
        //        for (int i = 0; i < tasks.Count; i++)
        //        {
        //            Debug.LogWarning($"效果栈登记{triggerInfo.triggerTime} {triggerInfo.triggerCard.name}触发{triggerInfo.targetCard.name}的{triggerInfo.triggerType}{tasks.Count - i}效果");
        //        }
        //    });
        //    if (!IsRuning)
        //    {
        //        for (int i = tasks.Count - 1; i >= 0; i--)
        //        {
        //            runTaskStack.Push((tasks[i], triggerInfo));
        //        }
        //        IsRuning = true;
        //        while (runTaskStack.Count != 0)
        //        {
        //            Debug.LogError("开始执行任务，当前数量为" + runTaskStack.Count);
        //            taskinfo = runTaskStack.Pop();
        //            Thread.MainThread.Run(() =>
        //            {
        //                Debug.LogError($"{taskinfo.Item2.triggerTime} {taskinfo.Item2.triggerCard.name}触发{taskinfo.Item2.targetCard.name}的{taskinfo.Item2.triggerType}效果");

        //            });
        //            await taskinfo.Item1(taskinfo.Item2);
        //            while (tempTaskStack.Count>0)
        //            {
        //                runTaskStack.Push(tempTaskStack.Pop());
        //            }
        //        }
        //        IsRuning = false;
        //    }
        //    else
        //    {
        //        for (int i = tasks.Count - 1; i >= 0; i--)
        //        {
        //            tempTaskStack.Push((tasks[i], triggerInfo));
        //        }
        //    }
        //}
        public static async Task TriggerLogic(TriggerInfo triggerInfo)
        {
            var a = triggerInfo[TriggerTime.When];
            var b = triggerInfo[TriggerType.Banish];

            try
            {
                foreach (var card in triggerInfo.targetCards)
                {
                    //AddEffectStactTask();
                    //await TriggerBoradcast(triggerInfo[card][TriggerTime.Before]);
                    //Debug.LogError(triggerInfo.targetCards + "!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    await Trigger(triggerInfo[card][TriggerTime.When]);
                    //await TriggerBoradcast(triggerInfo[card][TriggerTime.After]);
                    //RemoveEffectStactTask();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Debug.Log(e);
            }

        }
        public static async Task Trigger(TriggerInfo triggerInfo)
        {
            List<Func<TriggerInfo, Task>> tasks = triggerInfo.targetCard.cardAbility[triggerInfo.triggerTime][triggerInfo.triggerType];
            foreach (var task in tasks)
            {
                await task(triggerInfo);
            }
        }
        //public static async Task TriggerMulti(TriggerInfo triggerInfo)
        //{
        //    var tasks = triggerInfo.targetCard.cardEffect[triggerInfo.triggerTime][triggerInfo.triggerType];
        //    tasks.Reverse();
        //    tasks.ForEach(task => TaskStack.Push((task, triggerInfo)));
        //    await Run();
        //}
        public static async Task TriggerAll(TriggerInfo triggerInfo)
        {
            try
            {
                foreach (var card in AgainstInfo.cardSet.CardList)
                {
                    List<Func<TriggerInfo, Task>> tasks = card.cardAbility[triggerInfo.triggerTime][triggerInfo.triggerType];
                    foreach (var task in tasks)
                    {
                        await task(triggerInfo);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Debug.Log(e);
            }
        }
        /// <summary>
        /// 多个卡牌同时触发效果
        /// </summary>
        /// <param name="triggerInfo"></param>
        /// <returns></returns>
        public static async Task TriggerMeanwhile(TriggerInfo triggerInfo)
        {
            foreach (var card in AgainstInfo.cardSet.CardList)
            {
                List<Func<TriggerInfo, Task>> tasks = card.cardAbility[triggerInfo.triggerTime][triggerInfo.triggerType];
                foreach (var task in tasks)
                {
                    await task(triggerInfo);
                }
            }
        }
        
        //public static void AddEffectStactTask() => taskCount++;
        //public static void RemoveEffectStactTask()
        //{
        //    taskCount--;
        //    if (taskCount == 0)
        //    {
        //        Info.AgainstInfo.IsCardEffectCompleted = true;
        //        Debug.LogWarning("当前效果栈清空");
        //    }
        //}

        //public static async Task TriggerBoradcast(TriggerInfo triggerInfo)
        //{
        //    triggerInfo.targetCards = Info.AgainstInfo.cardSet.BroastCardList(triggerInfo.triggerCard);
        //    foreach (var card in triggerInfo.targetCards)
        //    {
        //        triggerInfo.targetCards = new List<Card>() { card };
        //        await Trigger(triggerInfo);
        //    }
        //}

        //Thread.MainThread.Run(() =>
        //{
        //    for (int i = 0; i < tasks.Count; i++)
        //    {
        //        Debug.LogWarning($"效果栈登记{triggerInfo.triggerTime} {triggerInfo.triggerCard.name}触发{triggerInfo.targetCard.name}的{triggerInfo.triggerType}{tasks.Count - i}效果");
        //    }
        //});
        //await Task.Delay(100);

        ////Debug.LogError($"尝试触发效果");
        //Thread.MainThread.Run(() =>
        //{
        //    Debug.LogError($"尝试触发效果{triggerInfo.triggerTime} {triggerInfo.triggerCard.name}触发{triggerInfo.targetCard.name}的{triggerInfo.triggerType}效果");
        //});
        //await Task.Delay(100);
        //Debug.LogError($"尝试触发效果{triggerInfo.triggerTime} {triggerInfo.triggerCard.name}触发{triggerInfo.targetCard.name}的{triggerInfo.triggerType}");

        //Thread.MainThread.Run(() =>
        //{
        //    Debug.LogError($"触发效果完成:{triggerInfo.triggerTime} {triggerInfo.triggerCard.name}触发{triggerInfo.targetCard.name}的{triggerInfo.triggerType}效果");
        //});
        //await Task.Delay(100);

        //tasks.ForEach(async task =>await task(triggerInfo));
        //var tasks = triggerInfo.targetCard.cardEffect[triggerInfo.triggerTime][triggerInfo.triggerType];
        //for (int i = tasks.Count - 1; i >= 0; i--)
        //{
        //    TaskStack.Push((tasks[i], triggerInfo));
        //}
        //Thread.MainThread.Run(() =>
        //{
        //    for (int i = 0; i < tasks.Count; i++)
        //    {
        //        Debug.LogWarning($"效果栈登记{triggerInfo.triggerTime} {triggerInfo.triggerCard.name}触发{triggerInfo.targetCard.name}的{triggerInfo.triggerType}{tasks.Count - i}效果");
        //    }
        //});
        //await Task.Delay(10);
        //await Run(triggerInfo);
        //public static async Task TriggerMulti(TriggerInfo triggerInfo)
        //{
        //    var tasks = triggerInfo.targetCard.cardEffect[triggerInfo.triggerTime][triggerInfo.triggerType];
        //    tasks.Reverse();
        //    tasks.ForEach(task => TaskStack.Push((task, triggerInfo)));
        //    await Run();
        //}
    }
}