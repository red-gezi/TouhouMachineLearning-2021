using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchPanelControl : MonoBehaviour
{
    public bool isMatchPanelOpen = false;
    //匹配面板相关
    public GameObject matchPanel;
    public GameObject loadingBar;
    public GameObject loadingCancelButton;
    public GameObject loadingCancelButtonText;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float prcoess = matchPanel.GetComponent<Image>().material.GetFloat("_Process");
        float matchPanelAlpha = matchPanel.GetComponent<Image>().material.GetFloat("_Alpha");
        Color buttonColor = loadingCancelButton.GetComponent<Image>().color;
        Color buttonTextColor = loadingCancelButtonText.GetComponent<Text>().color;
        if (isMatchPanelOpen)
        {
            prcoess = Mathf.Lerp(prcoess, 0.02f, Time.deltaTime * 5);
            matchPanelAlpha = Mathf.Lerp(matchPanelAlpha, 0.95f, Time.deltaTime * 5);
        }
        else
        {
            prcoess = Mathf.Lerp(prcoess, 0.93f, Time.deltaTime * 5);
            matchPanelAlpha = Mathf.Lerp(matchPanelAlpha, 0.00f, Time.deltaTime * 5);

        }
        matchPanel.GetComponent<Image>().material.SetFloat("_Process", prcoess);
        matchPanel.GetComponent<Image>().material.SetFloat("_Alpha", matchPanelAlpha);
        loadingBar.GetComponent<Image>().material.SetFloat("_Alpha", matchPanelAlpha);
        loadingCancelButton.GetComponent<Image>().color = new Color(buttonColor.r, buttonColor.g, buttonColor.b, matchPanelAlpha);
        loadingCancelButtonText.GetComponent<Text>().color = new Color(buttonTextColor.r, buttonTextColor.g, buttonTextColor.b, matchPanelAlpha);
    }
    /////////////////////////匹配面板相关操作//////////////////////////////

    public void MatchPanelOpen()
    {
        //matchPanel.GetComponent<Image>().material.SetFloat("_Strength", 0.93f);
        isMatchPanelOpen = true;
        matchPanel.GetComponent<Image>().raycastTarget = true;
        loadingBar.SetActive(true);
        loadingCancelButton.SetActive(true);

    }
    public void MatchPanelClose()
    {
        isMatchPanelOpen = false;
        matchPanel.GetComponent<Image>().raycastTarget = false;
        loadingBar.SetActive(false);
        loadingCancelButton.SetActive(false);
    }
}
