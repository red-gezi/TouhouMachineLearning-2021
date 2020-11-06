using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageArrowControl : MonoBehaviour
{
    public bool isNextPageArrow;
    public int targetIndex;
    public void Init(int targetIndex) => this.targetIndex = targetIndex;
    private void OnMouseDown()
    {
        Debug.Log("你怎么不说话呀");
        BookControl_废弃.OpenToPage(isNextPageArrow ? targetIndex + 1 : targetIndex - 0);
    }
}
