using Command.Dialogue;
using UnityEngine;
namespace Dialogue
{
    public class DialogueControl : MonoBehaviour
    {
        public GameObject DialogueUI;
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!DialogueUI.activeSelf)
                {
                    DialogueUI.SetActive(true);
                    DialogueCommand.Play(1, 1);
                }
                else
                {
                    DialgueInfo.DialgueInfos.IsNext = true;
                }
            }
        }
    }
}