using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Thread;
using UnityEngine;
namespace Command
{
    public class BookCommand : MonoBehaviour
    {
        public static void Init()
        {
            Control.BookControl.SetCoverOpen(true);
            Control.CameraViewControl.MoveToBookView();
            //Control.BookControl.instance.OpenToPage(PageMode.single);
            OpenToPage(PageMode.single);
            Info.GameUI.UiInfo.loginCanvas.SetActive(false);
        }
        public static  void InitDeck()
        {
          //  seleceDeckRank = Info.AllPlayerInfo.UserInfo.useDeckNum;
          //Control.cardDeckControl.InitCardDeck();

          //  deckModel.SetActive(false);
          //  var decks = Info.AllPlayerInfo.UserInfo.decks;
          //  deckModels.ForEach(model => model.SetActive(false));

          //  Debug.LogWarning(deckModels.Count + "-" + decks.Count);
          //  if (decks.Count > deckModels.Count - 1)
          //  {
          //      int num = decks.Count - (deckModels.Count - 1);
          //      for (int i = 0; i < num; i++)
          //      {
          //          deckModels.Insert(deckModels.Count - 1, Instantiate(deckModel, deckModel.transform.parent));
          //      }
          //  }
          //  Debug.LogWarning(deckModels.Count + "-" + decks.Count);
          //  for (int i = (deckModels.Count - 1) - decks.Count; i < deckModels.Count - 1; i++)
          //  {
          //      deckModels[i].SetActive(true);
          //      //修正卡组
          //      deckModels[i].transform.GetChild(1).GetComponent<Text>().text = decks[i].DeckName;
          //      var cardInfo = Command.CardInspector.CardLibraryCommand.GetCardStandardInfo(decks[i].LeaderId);
          //      Sprite cardTex = Sprite.Create(cardInfo.icon, new Rect(0, 0, cardInfo.icon.width, cardInfo.icon.height), Vector2.zero);
          //      deckModels[i].transform.GetComponent<Image>().sprite = cardTex;
          //  }
          //  values.Clear();
          //  for (int i = 0; i < deckModels.Count; i++)
          //  {
          //      values.Add(bias + i * fre);
          //  }
          //  addDeckModel.SetActive(true);
          //  addDeckModel.transform.SetAsLastSibling();
        }
        public static void OpenToPage(PageMode pageMode)
        {
            Task.Run(async () =>
            {
                MainThread.Run(() =>
                {
                    Info.BookInfo.cardListComponent.SetActive(false);
                    Info.BookInfo.cardDeckListComponent.SetActive(false);
                    Info.BookInfo.mapComponent.SetActive(false);
                });
                await Task.Delay(1000);
                MainThread.Run(() =>
                {
                    switch (pageMode)
                    {
                        case PageMode.single:
                            Info.BookInfo.cardListComponent.SetActive(true);
                            Info.BookInfo.mapComponent.SetActive(true);
                            Command.CardListCommand.InitCardDeck();
                            break;
                        case PageMode.multiplayer:
                            Info.BookInfo.cardListComponent.SetActive(true);
                            Info.BookInfo.cardDeckListComponent.SetActive(true);
                            Control.CardDeckBoardControl.instance.InitDeck();
                            break;
                        case PageMode.cardLibrary:
                            //cardLibraryPage.SetActive(true);
                            //cardLibraryControl.InitCardLibrary();
                            break;
                        case PageMode.shrine:
                            //shrinePage.SetActive(true);
                            break;
                        case PageMode.collect:
                            //collectPage.SetActive(true);
                            break;
                        case PageMode.config:
                            //configPage.SetActive(true);
                            break;
                        case PageMode.none:
                            //singlePage.SetActive(false);
                            //multiplayerPage.SetActive(false);
                            //cardLibraryPage.SetActive(false);
                            //shrinePage.SetActive(false);
                            //collectPage.SetActive(false);
                            //configPage.SetActive(false);
                            break;
                        default:
                            break;
                    }
                });
            });

        }
    }
}

