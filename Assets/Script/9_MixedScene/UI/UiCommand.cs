using CardModel;
using GameEnum;
using GameUI;
using System.Linq;
using System.Threading.Tasks;
using Thread;
using UnityEngine;
using UnityEngine.UI;

namespace Command
{
    namespace GameUI
    {
        public class UiCommand : MonoBehaviour
        {
            static GameObject MyPass => Info.GameUI.UiInfo.Instance.DownPass;
            static GameObject OpPass => Info.GameUI.UiInfo.Instance.UpPass;
            public static void SetCardBoardShow()
            {
                MainThread.Run(() =>
                {
                    Info.GameUI.UiInfo.CardBoard.SetActive(true);
                    Info.GameUI.UiInfo.CardBoard.transform.GetChild(1).GetComponent<Text>().text = Info.GameUI.UiInfo.CardBoardTitle;
                });
            }
            public static void SetCardBoardHide()
            {
                MainThread.Run(() =>
                {
                    Info.GameUI.UiInfo.CardBoard.SetActive(false);
                });
            }
            public static void CardBoardReload()
            {
                Command.GameUI.CardBoardCommand.CreatBoardCardActual();
            }
            public static Sprite GetBoardCardImage(int Id)
            {
                if (!Info.GameUI.UiInfo.CardImage.ContainsKey(Id))
                {
                    var CardStandardInfo = Command.CardLibrary.CardLibraryCommand.GetCardStandardInfo(Id);
                    Texture2D texture = CardStandardInfo.icon;
                    Info.GameUI.UiInfo.CardImage.Add(Id, Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero));
                }
                return Info.GameUI.UiInfo.CardImage[Id];
            }
           
            public static void SetCardBoardTitle(string Title) => Info.GameUI.UiInfo.CardBoardTitle = Title;
            public static void SetNoticeBoardTitle(string Title) => Info.GameUI.UiInfo.NoticeBoardTitle = Title;
            public static void CreatFreeArrow()
            {
                MainThread.Run(() =>
                {
                    GameObject newArrow = Instantiate(Info.GameUI.UiInfo.Arrow);
                    newArrow.name = "Arrow-null";
                    newArrow.GetComponent<ArrowManager>().InitArrow(
                        Info.AgainstInfo.ArrowStartCard,
                        Info.GameUI.UiInfo.ArrowEndPoint
                        );
                    Info.AgainstInfo.ArrowList.Add(newArrow);
                });
            }
            public static void DestoryFreeArrow()
            {
                MainThread.Run(() =>
                {
                    GameObject targetArrow = Info.AgainstInfo.ArrowList.First(arrow => arrow.GetComponent<ArrowManager>().targetCard == null);
                    
                    Info.AgainstInfo.ArrowList.Remove(targetArrow);
                    Destroy(targetArrow);
                });
            }
            public static void CreatFixedArrow(Card card)
            {
                MainThread.Run(() =>
                {
                    GameObject newArrow = Instantiate(Info.GameUI.UiInfo.Arrow);
                    newArrow.name = "Arrow-" + card.name;
                    newArrow.GetComponent<ArrowManager>().InitArrow(
                        Info.AgainstInfo.ArrowStartCard,
                        Info.AgainstInfo.PlayerFocusCard
                        );
                    Info.AgainstInfo.ArrowList.Add(newArrow);
                });
            }
            public static void DestoryFixedArrow(Card card)
            {
                MainThread.Run(() =>
                {
                    //Arrow定位出错？
                    GameObject targetArrow = Info.AgainstInfo.ArrowList.First(arrow => arrow.GetComponent<ArrowManager>().targetCard == card);
                    
                    Debug.LogError("确实是" + targetArrow.GetComponent<ArrowManager>().targetCard);
                    Debug.LogError("确实是" + targetArrow);
                    Info.AgainstInfo.ArrowList.Remove(targetArrow);
                    Destroy(targetArrow);
                });
            }
            public static void DestoryAllArrow()
            {
                MainThread.Run(() =>
                {
                    Info.AgainstInfo.ArrowList.ForEach(Destroy);
                    Info.AgainstInfo.ArrowList.Clear();
                });
            }

            public static async Task NoticeBoardShow()
            {
                MainThread.Run(() =>
                {
                    Info.GameUI.UiInfo.NoticeBoard.transform.GetChild(0).GetComponent<Text>().text = Info.GameUI.UiInfo.NoticeBoardTitle;
                });
                Info.GameUI.UiInfo.isNoticeBoardShow = true;
                await Task.Delay(1000);
                Info.GameUI.UiInfo.isNoticeBoardShow = false;
            }
            public void CardBoardClose() => Info.AgainstInfo.IsSelectCardOver = true;
            //public static void SetCardBoardMode(CardBoardMode CardBoardMode) => Info.AgainstInfo.CardBoardMode = CardBoardMode;
            public static void SetCurrentPass()
            {
                MainThread.Run(() =>
                {
                    Info.GameUI.UiInfo.MyPass.SetActive(true);
                    switch (Info.AgainstInfo.isMyTurn)
                    {
                        case true: Info.AgainstInfo.isDownPass = true; break;
                        case false: Info.AgainstInfo.isUpPass = true; break;
                    }
                });
            }
            public static void ReSetPassState()
            {
                MainThread.Run(() =>
                {
                    Info.GameUI.UiInfo.MyPass.SetActive(false);
                    Info.GameUI.UiInfo.OpPass.SetActive(false);
                    Info.AgainstInfo.isUpPass = false;
                    Info.AgainstInfo.isDownPass = false;
                });
            }
           
        }
    }

}