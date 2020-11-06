using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearControl : MonoBehaviour
{
    //public int modeCount;
    public float i;
    [ShowInInspector]
    //public int rank;
    RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //rank = (int)Mathf.PingPong((int)i, modeCount);
    }
    
}
