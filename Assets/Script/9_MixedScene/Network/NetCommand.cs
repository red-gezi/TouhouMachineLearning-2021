using Extension;
using GameEnum;
using System.Collections.Generic;
using System.Linq;
using Thread;
using UnityEngine;
using static Network.NetInfoModel;
using WebSocketSharp;
using System.Threading.Tasks;
using CardModel;
using Model;

namespace Command
{

    namespace Network
    {
        public static class NetCommand
        {
            static string ip => Info.AgainstInfo.isHostNetMode ? "127.0.0.1:514" : "106.15.38.165:514";
            //static string ip = "106.15.38.165:514";
            static WebSocket AsyncConnect = new WebSocket($"ws://{ip}/AsyncInfo");
            public static void Init()
            {
                //var clients = new WebSocket($"ws://{ip}/Join");
                //clients.OnMessage += (sender, e) =>
                //{
                //    Debug.Log("收到回应: " + e.Data);
                //};
                //clients.Connect();
                //Debug.Log("加入房间");
            }
            public static void Dispose()
            {
                Debug.Log("释放网络资源");
                //clients.Close();
            }
            public static void Register(string name, string password)
            {
                Debug.Log("注册请求");
                var client = new WebSocket($"ws://{ip}/Register");
                client.OnMessage += (sender, e) =>
                {
                    Debug.Log("有回应了");
                    Debug.Log(e.Data);
                    string result = e.Data;
                    MainThread.Run(() =>
                    {
                        //打开弹窗
                        if (result == "1")
                        {
                            Debug.Log("注册成功");
                            _ = Command.GameUI.NoticeCommand.ShowAsync("注册成功", NotifyBoardMode.Ok);

                        }
                        if (result == "-1")
                        {
                            Debug.Log("账号已存在");
                            _ = Command.GameUI.NoticeCommand.ShowAsync("账号已存在", NotifyBoardMode.Ok);
                        }
                    });
                    Debug.Log("收到回应: " + e.Data);
                    client.Close();
                };
                client.Connect();
                client.Send(new GeneralCommand<string>(name, password).ToJson());

            }
            public static void Login(string name, string password)
            {
                //Debug.Log("登录请求");
                var client = new WebSocket($"ws://{ip}/Login");
                client.OnMessage += (sender, e) =>
                {
                    MainThread.Run(() =>
                    {
                        string result = e.Data.ToObject<GeneralCommand<string>>().Datas[0];
                        string playerInfo = e.Data.ToObject<GeneralCommand<string>>().Datas[1];
                        //Debug.Log("登录结果" + result);
                        Info.AllPlayerInfo.UserInfo = playerInfo.ToObject<PlayerInfo>();
                        //_ = Command.GameUI.NoticeCommand.ShowAsync(result);
                        switch (result)
                        {
                            case ("-1"):
                                _ = Command.GameUI.NoticeCommand.ShowAsync("账号或密码错误", NotifyBoardMode.Ok);
                                break;
                            case ("1"):
                                //成功登陆并初始化书本
                                Command.BookCommand.Init();
                                break;
                            default:
                                break;
                        }
                        //SceneManager.LoadSceneAsync(1);
                    });
                    //Debug.Log("收到回应: " + e.Data);
                    client.Close();
                };
                client.Connect();
                //Debug.Log("连接完成");
                client.Send(new GeneralCommand<string>(name, password).ToJson());
            }
            public static void UpdateDecks(PlayerInfo playerInfo)
            {
                //Debug.Log("更新请求");
                var client = new WebSocket($"ws://{ip}/UpdateDecks");
                client.OnMessage += (sender, e) =>
                {
                    MainThread.Run(() =>
                    {
                        //string result = e.Data.ToObject<GeneralCommand<string>>().Datas[0];
                        //string playerInfo = e.Data.ToObject<GeneralCommand<string>>().Datas[1];
                        Debug.Log("修改完毕");
                        //Info.AllPlayerInfo.UserInfo = playerInfo.ToObject<PlayerInfo>();
                        //SceneManager.LoadSceneAsync(1);
                    });
                    Debug.Log("收到回应: " + e.Data);
                    client.Close();
                };
                client.Connect();
                Debug.Log("连接完成");
                client.Send(playerInfo.ToJson());
            }
            public static void JoinRoom()
            {
                Debug.Log("登录请求");
                var client = new WebSocket($"ws://{ip}/Join");
                client.OnMessage += (sender, e) =>
                {
                    Debug.LogError("收到了来自服务器的初始信息" + e.Data);
                    object[] ReceiveInfo = e.Data.ToObject<GeneralCommand>().Datas;
                    Info.AgainstInfo.RoomID = int.Parse(ReceiveInfo[0].ToString());
                    Debug.Log("房间号为" + Info.AgainstInfo.RoomID);
                    Info.AgainstInfo.isPlayer1 = (bool)ReceiveInfo[1];
                    Debug.Log("是否玩家1？：" + Info.AgainstInfo.isPlayer1);
                    Info.AgainstInfo.isPVP = true;
                    //Info.AgainstInfo.isMyTurn = Info.AgainstInfo.isPlayer1;
                    PlayerInfo userInfo = ReceiveInfo[2].ToString().ToObject<PlayerInfo>();
                    PlayerInfo opponentInfo = ReceiveInfo[3].ToString().ToObject<PlayerInfo>();
                    Debug.Log("收到回应: " + e.Data);
                    //client.Close();
                    MainThread.Run(() =>
                    {
                        BattleConfigure.Init();
                        BattleConfigure.SetPlayerDeck(userInfo.UseDeck);
                        BattleConfigure.SetOpponentDeck(opponentInfo.UseDeck);
                        BattleConfigure.SetPvPMode(true);
                        BattleConfigure.SetTurnFirst(FirstTurn.Random);
                        BattleConfigure.Start();
                        //创建联机连接
                        InitAsyncConnection();
                    });
                };
                client.Connect();
                Debug.Log("连接完成");
                client.Send(Info.AllPlayerInfo.UserInfo.ToJson());
                Debug.Log(Info.AllPlayerInfo.UserInfo.ToJson());
                Debug.Log("发送完毕");
            }
            public static void LeaveRoom()
            {
                Debug.Log("登录请求");
                var client = new WebSocket($"ws://{ip}/Leave");
                client.OnMessage += (sender, e) =>
                {
                    Debug.LogError("已离开房间" + e.Data);
                };
                client.Connect();
                Debug.Log("连接完成");
                client.Send(Info.AgainstInfo.RoomID.ToJson());
                Debug.Log(Info.AgainstInfo.RoomID.ToJson());
                Debug.Log("发送完毕");
            }
            //初始化接收响应
            private static void InitAsyncConnection()
            {
                AsyncConnect = new WebSocket($"ws://{ip}/AsyncInfo");
                AsyncConnect.Connect();
                AsyncConnect.OnMessage += (sender, e) =>
                {
                    try
                    {
                        Debug.Log("收到信息" + e.Data);
                        object[] receiveInfo = e.Data.ToObject<GeneralCommand>().Datas;
                        NetAcyncType Type = (NetAcyncType)int.Parse(receiveInfo[0].ToString());
                        switch (Type)
                        {
                            case NetAcyncType.FocusCard:
                                {
                                    int X = int.Parse(receiveInfo[2].ToString());
                                    int Y = int.Parse(receiveInfo[3].ToString());
                                    Info.AgainstInfo.OpponentFocusCard = Info.RowsInfo.GetCard(X, Y);
                                    break;
                                }
                            case NetAcyncType.PlayCard:
                                {
                                    //Debug.Log("触发卡牌同步");
                                    int X = int.Parse(receiveInfo[2].ToString());
                                    int Y = int.Parse(receiveInfo[3].ToString());
                                    //Command.CardCommand.PlayCard(Info.RowsInfo.GetCard(X, Y), false).Wait();
                                    Card targetCard = Info.RowsInfo.GetCard(X, Y);
                                    Task.Run(async () =>
                                    {
                                        await GameSystem.TransSystem.PlayCard(new TriggerInfo(targetCard, targetCard), false);
                                        Info.AgainstInfo.IsCardEffectCompleted = true;
                                    });
                                    break;
                                }
                            case NetAcyncType.SelectRegion:
                                {
                                    //Debug.Log("触发区域同步");
                                    int X = int.Parse(receiveInfo[2].ToString());
                                    Info.AgainstInfo.SelectRegion = Info.RowsInfo.GetSingleRowInfoById(X);
                                    break;
                                }
                            case NetAcyncType.SelectUnites:
                                {
                                    //Debug.Log("收到同步单位信息为" + rawData);
                                    List<Location> Locations = receiveInfo[2].ToString().ToObject<List<Location>>();
                                    Info.AgainstInfo.SelectUnits.AddRange(Locations.Select(location => Info.RowsInfo.GetCard(location.x, location.y)));
                                    break;
                                }
                            case NetAcyncType.SelectLocation:
                                {
                                    Debug.Log("触发坐标同步");
                                    int X = int.Parse(receiveInfo[2].ToString());
                                    int Y = int.Parse(receiveInfo[3].ToString());
                                    //Info.RowsInfo.SingleRowInfos.First(infos => infos.ThisRowCard == Info.RowsInfo.GlobalCardList[X]);
                                    Info.AgainstInfo.SelectRegion = Info.RowsInfo.GetSingleRowInfoById(X);
                                    Info.AgainstInfo.SelectLocation = Y;
                                    Debug.Log($"坐标为：{X}:{Y}");
                                    Debug.Log($"信息为：{"gezi"}:{Info.AgainstInfo.SelectLocation}");
                                    break;
                                }
                            case NetAcyncType.Pass:
                                {
                                    GameUI.UiCommand.SetCurrentPass();
                                    break;
                                }
                            case NetAcyncType.Surrender:
                                {
                                    Task.Run(async () =>
                                    {
                                        Debug.Log("收到结束指令");
                                        await StateCommand.BattleEnd(true, true);
                                    });
                                    break;
                                }
                            case NetAcyncType.ExchangeCard:
                                {
                                    Debug.Log("交换卡牌信息");
                                    // Debug.Log("收到信息" + rawData);
                                    Location location = receiveInfo[2].ToString().ToObject<Location>();
                                    int randomRank = int.Parse(receiveInfo[3].ToString());
                                    _ = CardCommand.ExchangeCard(Info.RowsInfo.GetCard(location), false, RandomRank: randomRank);
                                    break;
                                }
                            case NetAcyncType.RoundStartExchangeOver:
                                if (Info.AgainstInfo.isPlayer1)
                                {
                                    Info.AgainstInfo.isPlayer2RoundStartExchangeOver = true;
                                }
                                else
                                {
                                    Info.AgainstInfo.isPlayer1RoundStartExchangeOver = true;
                                }
                                break;
                            case NetAcyncType.SelectProperty:
                                {
                                    Info.AgainstInfo.SelectProperty = (Region)int.Parse(receiveInfo[2].ToString());
                                    Debug.Log("通过网络同步当前属性为" + Info.AgainstInfo.SelectProperty);
                                    break;
                                }
                            case NetAcyncType.SelectBoardCard:
                                Info.AgainstInfo.selectBoardCardRanks = receiveInfo[2].ToString().ToObject<List<int>>(); ;
                                Info.AgainstInfo.IsFinishSelectBoardCard = (bool)receiveInfo[3];
                                break;
                            case NetAcyncType.Init:
                                break;

                            default:
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.Log(ex);
                        throw;
                    }

                };
                AsyncConnect.OnError += (sender, e) =>
                {
                    Debug.Log("连接失败" + e.Message);
                    Debug.Log("连接失败" + e.Exception);
                };
                Debug.LogError("初始化数据" + new GeneralCommand(NetAcyncType.Init, Info.AgainstInfo.RoomID, Info.AgainstInfo.isPlayer1).ToJson());
                AsyncConnect.Send(new GeneralCommand(NetAcyncType.Init, Info.AgainstInfo.RoomID, Info.AgainstInfo.isPlayer1).ToJson());

            }
            //数据同步类型
            public static void AsyncInfo(NetAcyncType AcyncType)
            {
                if (Info.AgainstInfo.isPVP && (Info.AgainstInfo.isMyTurn || AcyncType == NetAcyncType.FocusCard || AcyncType == NetAcyncType.ExchangeCard || AcyncType == NetAcyncType.RoundStartExchangeOver))
                {
                    switch (AcyncType)
                    {
                        case NetAcyncType.FocusCard:
                            {
                                Location TargetCardLocation = Info.AgainstInfo.PlayerFocusCard != null ? Info.AgainstInfo.PlayerFocusCard.location : new Location(-1, -1);
                                AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, (int)TargetCardLocation.x, (int)TargetCardLocation.y).ToJson());
                                break;
                            }
                        case NetAcyncType.PlayCard:
                            {
                                Debug.Log("发出同步");
                                Location TargetCardLocation = Info.AgainstInfo.PlayerPlayCard.location;
                                AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, (int)TargetCardLocation.x, (int)TargetCardLocation.y).ToJson());

                                break;
                            }
                        case NetAcyncType.SelectRegion:
                            {
                                int RowRank = Info.AgainstInfo.SelectRegion.RowRank;
                                Debug.Log("同步焦点区域为" + RowRank);
                                AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, (int)RowRank).ToJson());

                                break;
                            }
                        case NetAcyncType.SelectUnites:
                            {
                                List<Location> Locations = Info.AgainstInfo.SelectUnits.Select(unite => unite.location).ToList();
                                Debug.LogError("发出的指令为：" + new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, Locations).ToJson());
                                AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, Locations.ToJson()).ToJson());
                                Debug.LogError("选择单位完成");
                                break;
                            }
                        case NetAcyncType.SelectLocation:
                            {
                                int RowRank = Info.AgainstInfo.SelectRegion.RowRank;
                                int LocationRank = Info.AgainstInfo.SelectLocation;
                                Debug.Log("同步焦点坐标给对面：" + RowRank);
                                AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, RowRank, LocationRank).ToJson());
                                break;
                            }
                        case NetAcyncType.ExchangeCard:
                            {
                                Debug.Log("触发交换卡牌信息");
                                Location Locat = Info.AgainstInfo.TargetCard.location;
                                int RandomRank = Info.AgainstInfo.RandomRank;
                                AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, Locat.ToJson(), RandomRank).ToJson());
                                break;
                            }
                        case NetAcyncType.RoundStartExchangeOver:
                            Debug.Log("触发回合开始换牌完成信息");
                            AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID).ToJson());
                            break;
                        case NetAcyncType.Pass:
                            {
                                Debug.Log("pass");
                                AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID).ToJson());
                                break;
                            }
                        case NetAcyncType.Surrender:
                            {
                                Debug.Log("投降");
                                AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID).ToJson());
                                break;
                            }
                        case NetAcyncType.SelectProperty:
                            {
                                Debug.Log("选择场地属性");
                                AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, Info.AgainstInfo.SelectProperty).ToJson());
                                break;
                            }

                        case NetAcyncType.Init:
                            break;
                        case NetAcyncType.SelectBoardCard:
                            Debug.Log("同步面板卡牌数据选择");
                            AsyncConnect.Send(new GeneralCommand(AcyncType, Info.AgainstInfo.RoomID, Info.AgainstInfo.selectBoardCardRanks, Info.AgainstInfo.IsFinishSelectBoardCard).ToJson());



                            break;

                        default:
                            {
                                Debug.Log("异常同步指令");
                            }
                            break;
                    }
                }
            }
        }
    }
}