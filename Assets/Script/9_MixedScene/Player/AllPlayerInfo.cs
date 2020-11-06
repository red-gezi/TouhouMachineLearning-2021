using Sirenix.OdinInspector;
namespace Info
{
    /// <summary>
    /// 双方信息
    /// </summary>
    public class AllPlayerInfo : SerializedMonoBehaviour
    {
        //玩家的用户信息
        [ShowInInspector]
        public static Network.NetInfoModel.PlayerInfo UserInfo;
        //对手的用户信息
        [ShowInInspector]
        public static Network.NetInfoModel.PlayerInfo OpponentInfo;
    }
}

