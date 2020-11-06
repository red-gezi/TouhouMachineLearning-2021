using Command;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
using UnityEngine;
namespace Control
{
    public class StateControl : MonoBehaviour
    {
        Task currentTask;
        void Start()
        {
            Translate.Load();
            currentTask = BattleProcess();
        }

        private void OnApplicationQuit() => Info.StateInfo.TaskManager.Cancel();
        [Button("打印线程异常")]//没卵用
        public void ShowMessage()
        {
            try
            {
                currentTask.Wait();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
        public async Task BattleProcess()
        {
            //Debug.LogError("对战开始");
            await StateCommand.BattleStart();
            for (int i = 0; i < 3; i++)
            {
                //Debug.LogError("小局开始");
                await StateCommand.RoundStart(i);
                //await StateCommand.WaitForSelectProperty();
                while (true)
                {
                    Debug.LogError("回合开始");
                    await StateCommand.TurnStart();
                    await StateCommand.WaitForPlayerOperation();
                    if (Info.AgainstInfo.isBoothPass) { break; }
                    await StateCommand.TurnEnd();
                }
                await StateCommand.RoundEnd(i);
            }
            await StateCommand.BattleEnd();
            Debug.Log("结束对局");
        }
    }
}
