using CardModel;
using CardSpace;
using Extension;
using GameEnum;
using Network;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

namespace Info
{
    public class RowsInfo : SerializedMonoBehaviour
    {
        public static List<Card> GetCardList(Card targetCard) => CardSet.globalCardList.First(list => list.Contains(targetCard));
        //public static List<Card> GetRegion(Card targetCard) =>RowsInfo. CardSet.globalCardList.First(list => list.Contains(targetCard));
        public static NetInfoModel.Location GetLocation(Card TargetCard)
        {
            int RankX = -1;
            int RankY = -1;
            for (int i = 0; i < CardSet.globalCardList.Count; i++)
            {
                if (CardSet.globalCardList[i].Contains(TargetCard))
                {
                    RankX = i;
                    RankY = CardSet.globalCardList[i].IndexOf(TargetCard);
                }
            }
            return new NetInfoModel.Location(RankX, RankY);
        }
        public static Card GetCard(int x, int y) => x == -1 ? null : CardSet.globalCardList[x][y];
        public static Card GetCard(NetInfoModel.Location Locat) => Locat.x == -1 ? null : CardSet.globalCardList[Locat.x][Locat.y];
        public static SingleRowInfo GetSingleRowInfoById(int Id) => AgainstInfo.cardSet.singleRowInfos.First(infos => infos.ThisRowCards == CardSet.globalCardList[Id]);
    }
}