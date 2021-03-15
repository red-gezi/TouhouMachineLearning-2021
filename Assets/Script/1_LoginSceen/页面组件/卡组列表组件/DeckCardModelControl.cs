using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
//对卡组列表卡牌模型中进行相关操作
public class DeckCardModelControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent onPointerEnter;
    public UnityEvent onPointerExit;
    public void OnPointerEnter(PointerEventData eventData) => onPointerEnter.Invoke();
    public void OnPointerExit(PointerEventData eventData) => onPointerExit.Invoke();
}
