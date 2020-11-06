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
        public static void Run(Action RunAction) => TargetAction.Enqueue(RunAction);
        void Update()
        {
            if (TargetAction.Count > 0)
            {
                for (int i = 0; i < TargetAction.Count; i++)
                {
                    while (TargetAction.Any())
                    {
                        TargetAction.Dequeue()();
                    }
                }
            }
        }
    }

}
