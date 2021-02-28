using GameEnum;
using Info;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thread;
using UnityEngine;
using UnityEngine.UI;
namespace CardModel
{


    public class Card : MonoBehaviour
    {
        public int CardId;

        public int basePoint;
        public int changePoint;
        public int showPoint => Mathf.Max(0, basePoint + changePoint);

        public Texture2D icon;
        public float moveSpeed = 0.1f;
        public Region region;
        public Territory territory;
        public string cardTag;
        public CardRank cardRank;
        public CardType cardType;


        [ShowInInspector]
        public Dictionary<CardField, int> cardFields = new Dictionary<CardField, int>();
        public int this[CardField cardField]
        {
            get => cardFields.ContainsKey(cardField) ? cardFields[cardField] : 0;
            set => cardFields[cardField] = value;
        }
        [ShowInInspector]
        public Dictionary<CardState, bool> cardStates = new Dictionary<CardState, bool>();
        public bool this[CardState cardState]
        {
            get => cardStates.ContainsKey(cardState) ? cardStates[cardState] : false;
            set => cardStates[cardState] = value;
        }
        public Territory belong => Info.AgainstInfo.cardSet[GameEnum.Orientation.Down].CardList.Contains(this) ? Territory.My : Territory.Op;
        public Vector3 targetPosition;
        public Quaternion targetQuaternion;

        //状态相关参数
        public bool isInit = false;
        public bool isGray = false;
        public bool isCover = false;
        public bool isLocked = false;
        public bool isFree = false;
        public bool isCanSee = false;
        public void SetCardSeeAble(bool isCanSee) => this.isCanSee = isCanSee;
        public bool isMoveStepOver = true;
        public bool isPrepareToPlay = false;
        public bool IsAutoMove => this != AgainstInfo.PlayerPlayCard;

        public List<Card> belongCardList => RowsInfo.GetCardList(this);
        public Network.NetInfoModel.Location location => RowsInfo.GetLocation(this);
        //[ShowInInspector]
        public Card LeftCard => location.y > 0 ? belongCardList[location.y - 1] : null;
        //[ShowInInspector]
        public Card RightCard => location.y < belongCardList.Count - 1 ? belongCardList[location.y + 1] : null;
        [ShowInInspector]
        public int twoSideVitality => (LeftCard == null|| LeftCard[CardState.Seal] ? 0 : LeftCard[CardField.Vitality]) + (RightCard == null || RightCard[CardState.Seal] ? 0 : RightCard[CardField.Vitality]);

        public Text PointText => transform.GetChild(0).GetChild(0).GetComponent<Text>();
        public string CardName => Command.CardLibrary.CardLibraryCommand.GetCardStandardInfo(CardId).cardName;
        //卡牌描述中可能会发生变化的值
        public int replaceDescribeValue = 0;
        [ShowInInspector]
        public string CardIntroduction => Command.CardLibrary.CardLibraryCommand.GetCardStandardInfo(CardId).ability.Replace("{x}", replaceDescribeValue.ToString());


        public Dictionary<TriggerTime, Dictionary<TriggerType, List<Func<TriggerInfo, Task>>>> cardAbility = new Dictionary<TriggerTime, Dictionary<TriggerType, List<Func<TriggerInfo, Task>>>>();

        private void Update() => RefreshCardUi();

        public void RefreshCardUi()
        {
            PointText.text = cardType == CardType.Unite ? showPoint.ToString() : "";
            if (changePoint > 0)
            {
                PointText.color = Color.green;
            }
            else if (changePoint < 0)
            {
                PointText.color = Color.red;
            }
            else
            {
                PointText.color = Color.black;
            }
        }
        public virtual void Init()
        {
            isInit = true;

            foreach (TriggerTime tirggerTime in Enum.GetValues(typeof(TriggerTime)))
            {
                cardAbility[tirggerTime] = new Dictionary<TriggerType, List<Func<TriggerInfo, Task>>>();
                foreach (TriggerType triggerType in Enum.GetValues(typeof(TriggerType)))
                {
                    cardAbility[tirggerTime][triggerType] = new List<Func<TriggerInfo, Task>>();
                }
            }
            cardAbility[TriggerTime.When][TriggerType.Gain] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    await Command.CardCommand.Gain(triggerInfo);
                }
            };
            cardAbility[TriggerTime.When][TriggerType.Hurt] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    await Command.CardCommand.Hurt(triggerInfo);
                }
            };
            cardAbility[TriggerTime.When][TriggerType.Destory] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    triggerInfo.point=showPoint;
                    await Command.CardCommand.Hurt(triggerInfo);
                }
            };
            cardAbility[TriggerTime.When][TriggerType.Cure] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    triggerInfo.point=-Math.Min(0, triggerInfo.targetCard.changePoint);
                     await Command.CardCommand.Gain(triggerInfo);
                }
            };
            cardAbility[TriggerTime.When][TriggerType.SelectUnite] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    Card card=  (Card)triggerInfo.param[0];
                    List<Card> targetCards =  (List<Card>)triggerInfo.param[1];
                    int num = (int) triggerInfo.param[2];
                    bool isAuto = (bool) triggerInfo.param[3];
                    await Command.StateCommand.WaitForSelecUnit(card, targetCards, num, isAuto);
                }
            };
            //cardEffect[TriggerTime.Before][TriggerType.Deploy] = new List<Func<TriggerInfo, Task>>()
            //{
            //    async (triggerInfo) =>
            //    {
            //        if (triggerInfo.triggerCard==this)
            //        {

            //     }
            //    }
            //};
            cardAbility[TriggerTime.When][TriggerType.Banish] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                  await Command.CardCommand.BanishCard(this);
                }
            };
            cardAbility[TriggerTime.When][TriggerType.Summon] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                  await Command.CardCommand.SummonCard(this);
                }
            };
            cardAbility[TriggerTime.When][TriggerType.Revive] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                  await Command.CardCommand.ReviveCard(triggerInfo);
                  //await Command.CardCommand.PlayCard(this);
                }
            };
            //卡牌状态变化时效果
            cardAbility[TriggerTime.When][TriggerType.Seal] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    await Command.CardCommand.SealCard(this);
                }
            };
            //登记卡牌回合状态变化时效果
            cardAbility[TriggerTime.When][TriggerType.TurnEnd] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    //我死啦
                    if (showPoint==0&&AgainstInfo.cardSet[RegionTypes.Battle].CardList.Contains(this))
                    {
                        await Command.CardCommand.MoveToGrave(this);
                    }
                }
            };
            cardAbility[TriggerTime.When][TriggerType.RoundEnd] = new List<Func<TriggerInfo, Task>>()
            {
                async (triggerInfo) =>
                {
                    if (AgainstInfo.cardSet[RegionTypes.Battle].CardList.Contains(this))
                    {
                        Debug.Log("移除啦");
                        await Command.CardCommand.MoveToGrave(this);
                    }
                }
            };
        }
        public void SetMoveTarget(Vector3 TargetPosition, Vector3 TargetEulers)
        {
            targetPosition = TargetPosition;
            targetQuaternion = Quaternion.Euler(TargetEulers + new Vector3(0, 0, isCanSee ? 0 : 180));
            if (isInit)
            {
                transform.position = targetPosition;
                transform.rotation = targetQuaternion;
                isInit = false;
            }
        }

        public void RefreshState()
        {
            Material material = GetComponent<Renderer>().material;
            if (AgainstInfo.PlayerFocusCard == this)
            {
                material.SetFloat("_IsFocus", 1);
                material.SetFloat("_IsRed", 0);
            }
            else if (AgainstInfo.OpponentFocusCard == this)
            {
                material.SetFloat("_IsFocus", 1);
                material.SetFloat("_IsRed", 1);
            }
            else
            {
                material.SetFloat("_IsFocus", 0);
            }
            material.SetFloat("_IsTemp", isGray ? 0 : 1);
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, Time.deltaTime * 10);
            PointText.text = cardType == CardType.Unite ? showPoint.ToString() : "";
        }
    }
}
