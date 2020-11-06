using CardModel;
using System.Collections.Generic;
using System.Linq;
//using UnityEngine;

public class TriggerInfo
{
    public TriggerTime triggerTime;
    public TriggerType triggerType;
    public Card triggerCard;
    public List<Card> targetCards;
    public object[] param;
    public Card targetCard => targetCards[0];
    public int point;

    //public TriggerInfo(params object[] param)
    //{
    //    this.param = param;
    //}
    //var height = Mathf.Sin(Time.time) + Mathf.PerlinNoise(0.5f, Time.deltaTime);

    public TriggerInfo this[TriggerTime triggerTime] => Clone(triggerTime: triggerTime);
    public TriggerInfo this[TriggerType triggerType] => Clone(triggerType: triggerType);
    //public TriggerInfo this[List<Card> targetCards] => Clone(targetCards: targetCards);
    public TriggerInfo this[Card targetCard] => Clone(targetCards: new List<Card> { targetCard });

    private TriggerInfo Clone(TriggerTime? triggerTime = null, TriggerType? triggerType = null, List<Card> targetCards = null)
    {
        TriggerInfo triggerInfo = new TriggerInfo();
        triggerInfo.triggerTime = triggerTime ?? this.triggerTime;
        triggerInfo.triggerType = triggerType ?? this.triggerType;
        triggerInfo.triggerCard = triggerCard;
        triggerInfo.targetCards = targetCards ?? this.targetCards;
        triggerInfo.point = point;
        triggerInfo.param = param;
        return triggerInfo;
    }
    public TriggerInfo()
    {

    }
    public TriggerInfo(Card triggerCard, List<Card> targetCards, int point = 0, params object[] param)
    {
        this.triggerCard = triggerCard;
        this.targetCards = targetCards.ToList();
        this.point = point;
        this.param = param;
    }
    public TriggerInfo(Card triggerCard, Card targetCard, int point = 0, params object[] param)
    {
        this.triggerCard = triggerCard;
        this.targetCards = new List<Card>() { targetCard };
        this.point = point;
        this.param = param;
    }
    //舍弃
    public static TriggerInfo Build(Card triggerCard, List<Card> targetCards, int point = 0, params object[] param)
    {
        TriggerInfo triggerInfo = new TriggerInfo();
        triggerInfo.triggerCard = triggerCard;
        triggerInfo.targetCards = targetCards.ToList();
        triggerInfo.point = point;
        triggerInfo.param = param;
        return triggerInfo;
    }
    //舍弃
    public static TriggerInfo Build(Card triggerCard, Card targetCard, int point = 0, params object[] param)
    {
        TriggerInfo triggerInfo = new TriggerInfo();
        triggerInfo.triggerCard = triggerCard;
        triggerInfo.targetCards = new List<Card>() { targetCard };
        triggerInfo.point = point;
        triggerInfo.param = param;
        return triggerInfo;
    }
}
