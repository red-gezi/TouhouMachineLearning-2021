using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageTagControl : MonoBehaviour
{
    int targetIndex;
    public Text ForntTagText;
    public Text BackTagText;
    public PageMode pageMode;
    public void Init(int targetIndex, string tagText)
    {
        ForntTagText.text = string.Join("\n", tagText.ToCharArray());
        BackTagText.text = string.Join("\n", tagText.ToCharArray());
        this.targetIndex = targetIndex;
    }
    private void OnMouseDown() => Control.BookModelControl.OpenToPage(pageMode);
}
