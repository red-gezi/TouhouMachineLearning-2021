using CardModel;
using CardSpace;
using Command.Network;
using GameEnum;
using System.Threading.Tasks;
using UnityEngine;
using static Info.AgainstInfo;
namespace Control
{
    public class CardControl : MonoBehaviour
    {
        int gap_step = 0;
        Card thisCard => GetComponent<Card>();
        GameObject gap => transform.GetChild(1).gameObject;
        Material gapMaterial => gap.GetComponent<Renderer>().material;
        Material cardMaterial => GetComponent<Renderer>().material;
        private void OnMouseEnter()
        {
            PlayerFocusCard = thisCard;
            NetCommand.AsyncInfo(NetAcyncType.FocusCard);
        }
        private void OnMouseExit()
        {
            if (PlayerFocusCard == thisCard)
            {
                PlayerFocusCard = null;
                NetCommand.AsyncInfo(NetAcyncType.FocusCard);
            }
        }
        private void OnMouseDown()
        {
            if (thisCard.isPrepareToPlay)
            {
                PlayerPlayCard = thisCard;
            }
            //Command.EffectCommand.TheWorldPlay(GetComponent<Card>());
        }
        private void OnMouseUp()
        {
            if (PlayerPlayCard != null)
            {
                //if (PlayerFocusRegion != null)
                //{
                if (PlayerFocusRegion != null && PlayerFocusRegion.name == "下方_墓地")
                {
                    //print(name + "进入墓地");
                    _ = Command.CardCommand.DisCard(thisCard);
                }
                else if (PlayerFocusRegion != null && (PlayerFocusRegion.name == "下方_领袖" || PlayerFocusRegion.name == "下方_手牌"))
                {
                    PlayerPlayCard = null;
                }
                else
                {
                    Debug.Log("1打出一张牌" + PlayerPlayCard);
                    Task.Run(async () =>
                    {
                        await GameSystem.TransSystem.PlayCard(TriggerInfo.Build(PlayerPlayCard, PlayerPlayCard));
                        Debug.LogError("我的回合结束啦！");
                        IsCardEffectCompleted = true;
                    });

                }
            }
        }
        private void Update()
        {
            if (gap_step == 1)
            {
                gapMaterial.SetFloat("_gapWidth", Mathf.Lerp(gapMaterial.GetFloat("_gapWidth"), 1.5f, Time.deltaTime * 20));
            }
            else if (gap_step == 2)
            {
                gapMaterial.SetFloat("_gapWidth", Mathf.Lerp(gapMaterial.GetFloat("_gapWidth"), 10, Time.deltaTime * 2));
                cardMaterial.SetFloat("_gapWidth", Mathf.Lerp(cardMaterial.GetFloat("_gapWidth"), 10, Time.deltaTime * 2));
            }
        }
        public void CreatGap()
        {
            gap.SetActive(true);
            gap_step = 1;
        }
        public void FoldGap()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            gap_step = 2;
        }
        public void DestoryGap()
        {
            gap.SetActive(false);
            gap_step = 0;
            Destroy(gameObject);
        }
    }
}

