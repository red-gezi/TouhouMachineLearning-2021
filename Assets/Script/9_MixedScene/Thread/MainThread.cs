using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Thread
{
    public class MainThread : MonoBehaviour
    {
        static Queue<Action> TargetAction = new Queue<Action>();
        static Queue<Action> UiAction = new Queue<Action>();
        public static void Run(Action RunAction) => TargetAction.Enqueue(RunAction);
        public static void UiRun(Action RunAction) => UiAction.Enqueue(RunAction);
        public static void Init()
        {
            TargetAction.Clear();
            UiAction.Clear();
        }

        void Update()
        {
            //if (TargetAction.Count > 0)
            //{
            //    for (int i = 0; i < TargetAction.Count; i++)
            //    {
            //        while (TargetAction.Any())
            //        {
            //            TargetAction.Dequeue()();
            //        }
            //    }
            //}
            while (TargetAction.Any())
            {
                TargetAction.Dequeue()();
            }
            while (UiAction.Any())
            {
                UiAction.Dequeue()();
            }
        }
    }

}
