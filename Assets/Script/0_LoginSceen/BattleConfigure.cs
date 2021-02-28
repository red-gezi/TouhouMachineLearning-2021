using static Network.NetInfoModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Model;

public partial class BattleConfigure
{
    // Start is called before the first frame update
    static CardDeck defaultDeck => new CardDeck("gezi", 10001, new List<int>
                        {
                             10015, 10016, 10012,10013,
                             10014, 10015, 10016,10002,10008,10004,
                             10007, 10006,10008,
                             10004, 10012, 10010,
                             10011,10012, 10014,
                             10007,10006, 10016,
                             10008, 10014,10005,
                        });
    public static void Init()
    {
        Info.AgainstInfo.userDeck = null;
        Info.AgainstInfo.opponentDeck = null;
    }
    public static void Start()
    {

        if (Info.AgainstInfo.userDeck == null)
        {
            SetPlayerDeck(defaultDeck);
        }
        if (Info.AgainstInfo.opponentDeck == null)
        {
            SetOpponentDeck(defaultDeck);
        }
        SceneManager.LoadSceneAsync(1);
    }
    //设置我方卡组
    public static void SetPlayerDeck(CardDeck playerDeck) => Info.AgainstInfo.userDeck = playerDeck;
    //设置对方卡组
    public static void SetOpponentDeck(CardDeck opponentDeck) => Info.AgainstInfo.opponentDeck = opponentDeck;
    //设置先手方
    public static void SetTurnFirst(FirstTurn firstTurn)
    {
        switch (firstTurn)
        {
            case FirstTurn.PlayerFirst: Info.AgainstInfo.isMyTurn = true; break;
            case FirstTurn.OpponentFirst: Info.AgainstInfo.isMyTurn = false; break;
            case FirstTurn.Random: Info.AgainstInfo.isMyTurn = Info.AgainstInfo.isPlayer1; break;
            default: break;
        }
    }
    //设置对局类型
    public static void SetPvPMode(bool isPvPMode)
    {
        Info.AgainstInfo.isPVP = isPvPMode;
    }
    //设置初始胜利小局数
    public static void SetScore(int Score1, int Score2)
    {
        //Info.AgainstInfo.isPVP = isPvPMode;
    }


}
