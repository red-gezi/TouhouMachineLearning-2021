using CardModel;
using CardSpace;
using Extension;
using GameEnum;
using Info;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Command
{
    public static class RowCommand
    {
        public static async Task CreatTempCard(SingleRowInfo SingleInfo)
        {
            Card modelCard = AgainstInfo.cardSet[Orientation.My][RegionTypes.Uesd].CardList[0];
            SingleInfo.TempCard = await CardCommand.CreatCard(modelCard.CardId);
            SingleInfo.TempCard.isGray = true;
            SingleInfo.TempCard.SetCardSeeAble(true);
            SingleInfo.ThisRowCards.Insert(SingleInfo.Location, SingleInfo.TempCard);
            SingleInfo.TempCard.Init();
        }
        public static void DestoryTempCard(SingleRowInfo SingleInfo)
        {
            SingleInfo.ThisRowCards.Remove(SingleInfo.TempCard);
            GameObject.Destroy(SingleInfo.TempCard.gameObject);
            SingleInfo.TempCard = null;
        }
        public static void ChangeTempCard(SingleRowInfo SingleInfo)
        {
            SingleInfo.ThisRowCards.Remove(SingleInfo.TempCard);
            SingleInfo.ThisRowCards.Insert(SingleInfo.Location, SingleInfo.TempCard);
        }
        public static void RefreshHandCard(List<Card> cardList)
        {
            cardList.ForEach(card => card.isPrepareToPlay = (AgainstInfo.PlayerFocusCard != null && card == AgainstInfo.PlayerFocusCard && card.isFree));
        }
        public static void SetPlayCardMoveFree(bool isFree)
        {
            AgainstInfo.cardSet[Orientation.Down][RegionTypes.Leader, RegionTypes.Hand].CardList.ForEach(card => card.isFree = isFree);
        }
        public static void SetRegionSelectable(RegionTypes region, Territory territory = Territory.All)
        {
            if (region == RegionTypes.None)
            {
                AgainstInfo.cardSet[RegionTypes.Battle].singleRowInfos.ForEach(row => row.SetRegionSelectable(false));
            }
            else
            {
                AgainstInfo.cardSet[region][(Orientation)territory].singleRowInfos.ForEach(row => row.SetRegionSelectable(true));
            }
        }
    }
}


