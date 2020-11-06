using UnityEngine;

namespace GameUI
{
    public class BoardCardControl : MonoBehaviour
    {
        public int Rank;
        public GameObject selectRect => transform.GetChild(1).gameObject;
        public void OnMouseClick()
        {
            if (Info.AgainstInfo.cardBoardMode== GameEnum.CardBoardMode.Select|| Info.AgainstInfo.cardBoardMode == GameEnum.CardBoardMode.ExchangeCard)
            {
                if (Info.AgainstInfo.selectBoardCardRanks.Contains(Rank))
                {
                    Info.AgainstInfo.selectBoardCardRanks.Remove(Rank);
                    selectRect.SetActive(false);
                    Debug.Log("取消选择"+Rank);
                    //如果是小局开局抽卡，则不同步选择卡牌数据消息，只同步换牌数据
                    //若是卡牌效果换牌，则同步换牌数据
                    if (!Info.AgainstInfo.isRoundStartExchange)
                    {
                        Command.Network.NetCommand.AsyncInfo(GameEnum.NetAcyncType.SelectBoardCard);
                    }
                }
                else
                {
                    Info.AgainstInfo.selectBoardCardRanks.Add(Rank);
                    selectRect.SetActive(true);
                    Debug.Log("选择" + Rank);
                    if (!Info.AgainstInfo.isRoundStartExchange)
                    {
                        Command.Network.NetCommand.AsyncInfo(GameEnum.NetAcyncType.SelectBoardCard);
                    }

                }
            }
        }
    }
}