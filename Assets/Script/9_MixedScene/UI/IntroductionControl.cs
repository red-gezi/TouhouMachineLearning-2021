using UnityEngine;
namespace Control
{
    namespace GameUI
    {
        public class IntroductionControl : MonoBehaviour
        {
            float Cd;
            public Vector3 Bias;
            public Vector3 ViewportPoint => Camera.main.ScreenToViewportPoint(Input.mousePosition);
            public bool IsRight => ViewportPoint.x < 0.5;
            public bool IsDown => ViewportPoint.y < 0.5;
            void Update()
            {
                Bias = new Vector3(IsRight ? 0.1f : -0.1f, IsDown ? 0.1f : -0.1f);
                transform.position = Camera.main.ViewportToScreenPoint(ViewportPoint + Bias);
                if (Info.AgainstInfo.PlayerFocusCard != null && Info.AgainstInfo.PlayerFocusCard.isCanSee)
                {
                    Cd = Mathf.Min(0.25f, Cd + Time.deltaTime);
                }
                else
                {
                    Cd = 0;
                }
                if (Cd == 0.25f)
                {
                    Command.GameUI.UiCommand.ChangeIntroduction(Info.AgainstInfo.PlayerFocusCard);
                    transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }
}
