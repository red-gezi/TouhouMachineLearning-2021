using Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Control
{
    //用于展示单人多人页面中的卡组面板
    public class CardDeckBoardControl : MonoBehaviour
    {
        public Transform desksBoard;
        public float fre = 100;
        public float bias = 100;
        public float show = 100;
        public bool isDragMode;//是否处于手动拖拽模式
        public bool isCardClick;
        public List<float> values;
        public GameObject deckModel;
        public GameObject addDeckModel;

        public List<GameObject> deckModels;
        public int seleceDeckRank = 0;

        public static CardDeckBoardControl instance;
        public CardListControl cardDeckControl;
        Vector3 start => desksBoard.GetComponent<RectTransform>().position;

        private void Awake() => instance = this;
        public void InitDeck()
        {
            seleceDeckRank = Info.AllPlayerInfo.UserInfo.useDeckNum;
            Command.CardListCommand.InitCardDeck();

            deckModel.SetActive(false);
            var decks = Info.AllPlayerInfo.UserInfo.decks;
            deckModels.ForEach(model => model.SetActive(false));

            Debug.LogWarning(deckModels.Count + "-" + decks.Count);
            if (decks.Count > deckModels.Count - 1)
            {
                int num = decks.Count - (deckModels.Count - 1);
                for (int i = 0; i < num; i++)
                {
                    deckModels.Insert(deckModels.Count - 1, Instantiate(deckModel, deckModel.transform.parent));
                }
            }
            Debug.LogWarning(deckModels.Count + "-" + decks.Count);
            for (int i = (deckModels.Count - 1) - decks.Count; i < deckModels.Count - 1; i++)
            {
                deckModels[i].SetActive(true);
                //修正卡组
                deckModels[i].transform.GetChild(1).GetComponent<Text>().text = decks[i].DeckName;
                var cardInfo = Command.CardLibrary.CardLibraryCommand.GetCardStandardInfo(decks[i].LeaderId);
                Sprite cardTex = Sprite.Create(cardInfo.icon, new Rect(0, 0, cardInfo.icon.width, cardInfo.icon.height), Vector2.zero);
                deckModels[i].transform.GetComponent<Image>().sprite = cardTex;
            }
            values.Clear();
            for (int i = 0; i < deckModels.Count; i++)
            {
                values.Add(bias + i * fre);
            }
            addDeckModel.SetActive(true);
            addDeckModel.transform.SetAsLastSibling();
        }
        void Update()
        {
            show = start.x;
            for (int i = 0; i < values.Count; i++)
            {
                values[i] = bias + i * fre;
            }
            if (Input.GetMouseButtonDown(0))
            {
                isDragMode = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (!isCardClick)
                {
                    int selectRank = values.IndexOf(values.OrderBy(value => Mathf.Abs(value - start.x)).First());
                    if (seleceDeckRank != selectRank)
                    {
                        seleceDeckRank = selectRank;
                        Info.AllPlayerInfo.UserInfo.useDeckNum = seleceDeckRank;
                        Command.Network.NetCommand.UpdateDecks(Info.AllPlayerInfo.UserInfo);
                        Command.CardListCommand.InitCardDeck();
                        Debug.LogWarning("拖拽修改为" + seleceDeckRank);
                    }

                }
                else
                {
                    isCardClick = false;
                }
                isDragMode = false;
            }
            if (!isDragMode)
            {

                Vector3 end = new Vector3(values[seleceDeckRank], start.y, start.z);
                desksBoard.GetComponent<RectTransform>().position = Vector3.Lerp(start, end, Time.deltaTime * 3);
            }
        }
        public void OnDeckClick(GameObject deck)
        {
            int selectRank = deckModels.IndexOf(deck);
            if (seleceDeckRank != selectRank)
            {
                seleceDeckRank = selectRank;
                isCardClick = true;
                Info.AllPlayerInfo.UserInfo.useDeckNum = seleceDeckRank;
                Command.Network.NetCommand.UpdateDecks(Info.AllPlayerInfo.UserInfo);
                Command.CardListCommand.InitCardDeck();
                Debug.LogWarning("点击修改为" + seleceDeckRank);
            }
        }
        public void OnAddDeckClick()
        {

        }
        public void StartBattle()
        {
            if (true)
            {
                _ = Command.GameUI.NoticeCommand.ShowAsync("等待匹配");
                Command.Network.NetCommand.JoinRoom();
            }
            else
            {
                BattleConfigure.Init();
                BattleConfigure.SetPvPMode(false);
                BattleConfigure.SetTurnFirst(FirstTurn.PlayerFirst);
                BattleConfigure.SetPlayerDeck(
                    new CardDeck("gezi", 10001, new List<int>
                        {
                             10001,10001,10001,10001,
                             10001,10001,10001,10001,10001,10001,
                             10001,10001,10001,
                             10001,10001,10001,
                             10001,10001,10001,
                             10001,10001,10001,
                             10001,10001,10001,
                        })
                    );
                BattleConfigure.SetOpponentDeck(
                   new CardDeck("npc", 10002, new List<int>
                       {
                             10001,10001,10001,10001,
                             10001,10001,10001,10001,10001,10001,
                             10001,10001,10001,
                             10001,10001,10001,
                             10001,10001,10001,
                             10001,10001,10001,
                             10001,10001,10001,
                       })
                   );
                BattleConfigure.Start();
            }
           
        }
    }
}

