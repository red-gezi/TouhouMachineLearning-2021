using CardModel;
using CardSpace;
using GameEnum;
using Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Info
{
    /// <summary>
    /// 全局对战信息
    /// </summary>
    public static class AgainstInfo
    {
        //双方用户信息
        public static string userName;
        public static string opponentName;
        public static CardDeck userDeck;
        public static CardDeck opponentDeck;
        //网络同步信息
        public static bool isHostNetMode=true;//本地测试模式
        public static Card TargetCard;
        public static int RandomRank;

        

        public static bool IsSelectCardOver;
        public static int RoomID;
        //操作标志位
        public static int LanguageRank;
        public static bool IsCardEffectCompleted;
        public static List<GameObject> ArrowList = new List<GameObject>();

        //public static CardBoardMode CardBoardMode;
        public static List<int> Player1BlackCardList;
        public static List<int> Player2BlackCardList;

        public static Vector3 DragToPoint;
        public static Card PlayerFocusCard;
        public static Card OpponentFocusCard;
        public static Card PlayerPlayCard;
        //选择属性
        public static Region SelectProperty { get; set; }
        public static bool IsWaitForSelectProperty { get; set; }

        //选择的区域
        public static SingleRowInfo PlayerFocusRegion { get; set; }
        public static bool IsWaitForSelectRegion { get; set; }
        public static SingleRowInfo SelectRegion { get; set; }
        //选择的单位
        public static Card ArrowStartCard { get; set; }
        public static Card ArrowEndCard { get; set; }
        public static bool IsWaitForSelectUnits { get; set; }
        public static List<Card> SelectUnits = new List<Card>();//玩家选择的单位
        //选择的坐标
        public static Vector3 FocusPoint;
        public static bool IsWaitForSelectLocation;
        public static int SelectLocation = -1;
        //选择的卡牌面板卡片
        public static bool isRoundStartExchange=false;
        public static bool isPlayer1RoundStartExchangeOver = false;
        public static bool isPlayer2RoundStartExchangeOver = false;

        public static CardBoardMode cardBoardMode;
        public static List<int> cardBoardIDList;
        public static List<Card> cardBoardList;

        public static List<int> selectBoardCardRanks;
        public static List<Card> selectActualCards=> selectBoardCardRanks.Select(rank=> cardBoardList[rank]).ToList();
        public static List<int> selectVirualCardIds => selectBoardCardRanks.Select(rank => cardBoardIDList[rank]).ToList();

        public static bool IsFinishSelectBoardCard;
        public static int ExChangeableCardNum = 0;
        //判断是否1号玩家
        public static bool isPlayer1 = false;
        public static bool isMyTurn;
        public static bool isPVP = false;
        public static bool isPVE => !isPVP;
        public static bool isAIControl => isPVE && (!isMyTurn || Timer.isTimeout);

        /// <summary>
        /// 对局中卡牌的集合
        /// </summary>
        public static CardSet cardSet = new CardSet();

        public static List<Card> AllCardList => CardSet.globalCardList.SelectMany(x => x).ToList();
        public static (int P1Score, int P2Score) PlayerScore;
        public static (int MyScore, int OpScore) ShowScore => isPlayer1 ? (PlayerScore.P1Score, PlayerScore.P2Score) : (PlayerScore.P2Score, PlayerScore.P1Score);

        public static bool isUpPass = false;
        public static bool isDownPass = false;
        public static bool isCurrectPass => isMyTurn ? isDownPass : isUpPass;

        public static bool isBoothPass => isUpPass && isDownPass;
    };
}