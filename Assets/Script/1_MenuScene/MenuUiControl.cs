using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUiControl : MonoBehaviour
{
    public GameObject gear;
    [ShowInInspector]
    public List<string> titles;
    public List<Sprite> textures;
    int modeCount => textures.Count;
    [ShowInInspector]
    public float process => Mathf.Abs(i) % 1;
    public GameObject Tex1;
    public GameObject Tex2;
    public float i = 100000;
    //float rank => i > 0 ? i % textures.Count : (textures.Count - i) % textures.Count;
    bool isReset;
    RectTransform rectTransform;
    float heigh => Tex1.GetComponent<RectTransform>().rect.height;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = gear.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Tex1.GetComponent<Image>().sprite = textures[(int)i % modeCount];
        Tex1.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = titles[(int)i % modeCount];
        Tex2.GetComponent<Image>().sprite = textures[((int)i + 1) % modeCount];
        Tex2.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = titles[((int)i + 1) % modeCount];

        Tex1.GetComponent<RectTransform>().localPosition = Vector3.up * heigh/2 * Mathf.Sin(Mathf.PI / 2 * process);
        Tex1.GetComponent<RectTransform>().eulerAngles = Vector3.Lerp(Vector3.zero, Vector3.right * -90, process);
        Tex2.GetComponent<RectTransform>().localPosition = Vector3.down * heigh/2 * Mathf.Sin(Mathf.PI / 2 * (1 - process));
        Tex2.GetComponent<RectTransform>().eulerAngles = Vector3.Lerp(Vector3.zero, Vector3.right * 90, 1 - process);
        rectTransform.eulerAngles = Quaternion.Slerp(Quaternion.Euler(rectTransform.eulerAngles), Quaternion.Euler(0, 0, -i * 72 + 52), Time.deltaTime * 5).eulerAngles;
        if (isReset)
        {
            i = Mathf.Lerp(i, Mathf.Round(i), Time.deltaTime * 5);
        }
    }
    public void OnGearMouseDrag()
    {
        isReset = false;
        i -= Input.GetAxis("Mouse Y") * 0.1f;
    }
    public void OnGearMouseUp()
    {
        isReset = true;
        //i = Mathf.Round(i);
    }
    public void ModeSelect(int rank)
    {
        Debug.Log((int)i % modeCount);
    }
}
