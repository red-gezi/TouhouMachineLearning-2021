using CardModel;
using CardSpace;
using Extension;
using GameEnum;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Thread;
using UnityEngine;
namespace Info
{
    public class SingleRowInfo : MonoBehaviour
    {
        public Color color;
        public Card TempCard;
        public Orientation orientation;
        public RegionTypes region;
        public bool CanBeSelected;
        [ShowInInspector]
        public int rank
        {
            get
            {
                int regionId = (int)region;
                if (region > RegionTypes.Battle)
                {
                    regionId -= 2;
                }
                return regionId + (AgainstInfo.isPlayer1 ^ (orientation == Orientation.Down) ? 9 : 0);
            }
        }

        private void Awake() => AgainstInfo.cardSet.singleRowInfos.Add(this);
        public int Location => this.JudgeRank(AgainstInfo.FocusPoint);
        public int RowRank => CardSet.globalCardList.IndexOf(ThisRowCards);
        public Material CardMaterial => transform.GetComponent<Renderer>().material;
        public List<Card> ThisRowCards
        {
            get => AgainstInfo.cardSet[rank];
            set => AgainstInfo.cardSet[rank] = value;
        }
        public void SetRegionSelectable(bool CanBeSelected)
        {
            this.CanBeSelected = CanBeSelected;
            MainThread.Run(() => { CardMaterial.SetColor("_GlossColor", CanBeSelected ? color : Color.black); });
        }
    }
}
