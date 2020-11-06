using Extension;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
namespace Control
{
    namespace GameUI
    {
        public class NoticeControl : MonoBehaviour
        {
            public float Aplha = 0;
            public Color a;
            Color targetBackColor = new Color(0, 0, 0, 0);
            Color targetWordColor = new Color(0, 0, 0, 0);
            private Image image;
            private Text text;
            Vector3 targetAugel = new Vector3(0, 0, 0);
            Vector3 currentAugel => Info.GameUI.UiInfo.NoticeBoard.transform.eulerAngles;
            private void Start()
            {
                image = Info.GameUI.UiInfo.NoticeBoard.GetComponent<Image>();
                text = Info.GameUI.UiInfo.NoticeBoard.transform.GetChild(0).GetComponent<Text>();
            }
            void Update()
            {
                if (Info.GameUI.UiInfo.isNoticeBoardShow)
                {
                    targetBackColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                    targetWordColor = new Color(0, 0, 0, 1f);
                    targetAugel = new Vector3(0, 0, 0);

                }
                else
                {
                    targetBackColor = new Color(0, 0, 0, 0);
                    targetWordColor = new Color(0, 0, 0, 0);
                    targetAugel = new Vector3(90, 0, 0);
                }
                image.color = Color.Lerp(image.color, targetBackColor, Time.deltaTime * 5);
                text.color = Color.Lerp(text.color, targetWordColor, Time.deltaTime * 5);
                Info.GameUI.UiInfo.NoticeBoard.transform.eulerAngles = Vector3.Lerp(currentAugel, targetAugel, Time.deltaTime * 5);
            }
        }
    }
}