using System.Threading.Tasks;
using UnityEngine;
using static Command.Dialogue.DialogueCommand;
using static Dialogue.DialgueInfo;

namespace Dialogue
{
    public class DialogueText : MonoBehaviour
    {
        public static DialogueText Instance;
        Chara lmA = Chara.灵梦A;
        Chara lmB = Chara.灵梦B;
        private void Awake()
        {
            Instance = this;
        }
        [Dial(1, 1)]
        public async Task Dia_1_1()
        {
            await Say("你好啊,灵梦", lmB, false);
            await Say("你好啊", lmA);
            await Say("你有钱吗", lmB, false);
            await Say("没有呢。。。", lmA, FaceNum: 1);
            await Say("...我也是", lmB, false, FaceNum: 1);
        }
        [Dial(1, 2)]
        public async Task Dia_1_2()
        {
            await Say("需要我教你规则吗", lmB, false);
            voice(2);
            await Say("好啊", lmA);
        }
    }
}