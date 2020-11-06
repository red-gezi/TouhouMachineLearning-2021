using Dialogue;
using System.Threading.Tasks;
using UnityEngine;
using static Dialogue.DialgueInfo;
namespace Command
{
    namespace Dialogue
    {
        public class DialogueCommand
        {
            public static void Play(int v1, int v2)
            {
                foreach (var methond in typeof(DialogueText).GetMethods())
                {
                    foreach (Dial info in methond.GetCustomAttributes(typeof(Dial), false))
                    {
                        if (info.step == v1 && info.rank == v2)
                        {
                            methond.Invoke(DialogueText.Instance, new object[] { });
                        }
                    }
                }
            }
            public static void voice(int v)
            {
                Debug.Log($"n播放音乐{v}");
            }
            public static async Task Say(string message, Chara chara, bool IsLeft = true, int FaceNum = 0)
            {
                Debug.Log($"{message}                 角色为{chara}，立绘位置在{(IsLeft ? "左边" : "右边")}");
                DialgueInfos.Text.text = chara + ":" + message;
                if (IsLeft)
                {
                    //DialgueInfos.Left.GetComponent<Live2d>().FaceRank = FaceNum;
                    DialgueInfos.Left.gameObject.transform.localScale *= 1.1f;
                }
                else
                {
                    //DialgueInfos.Right.GetComponent<Live2d>().FaceRank = FaceNum;
                    DialgueInfos.Right.gameObject.transform.localScale *= 1.1f;
                }
                await Task.Run(() =>
                {
                    while (!DialgueInfos.IsNext)
                    {
                        Debug.Log("yaya");
                    }
                });
                DialgueInfos.IsNext = false;
                if (IsLeft)
                {
                    DialgueInfos.Left.gameObject.transform.localScale /= 1.1f;
                }
                else
                {
                    DialgueInfos.Right.gameObject.transform.localScale /= 1.1f;
                }
            }
        }
    }
}