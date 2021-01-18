//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//public class PageControl_废弃 : MonoBehaviour
//{
//    public GameObject tag_Model;
//    public GameObject frontTextModel;
//    public GameObject frontPictureModel;
//    public GameObject frontCardListModel;
//    public PageArrowControl frontPageArrow;

//    public GameObject backTextModel;
//    public GameObject backPictureModel;
//    public GameObject backCardListModel;
//    public PageArrowControl backPageArrow;


//    public bool isOpen;
//    public float angle;
//    public int pageIndex;//所属标签
//    public string tagText;//所属标签

//    public void SetTag(int tagRank, string tagText)
//    {

//        if (tag_Model != null)
//        {
//            tag_Model.SetActive(true);
//            this.tagText = tagText;
//            tag_Model.transform.localPosition -= tagRank * new Vector3(0, 0, 0.2f);
//        }
//    }
//    public void Init(int pageIndex)
//    {
//        this.pageIndex = pageIndex;
//        tag_Model.GetComponent<PageTagControl>().Init(pageIndex, tagText);
//        frontPageArrow.Init(pageIndex);
//        backPageArrow.Init(pageIndex);
//    }

//    Vector3 pos => pageIndex == 0 ? new Vector3(0.5f, 0.068f, 0) : new Vector3(0.45f, 0.034f, 0);
//    //暂时注释
//    public void RefreshPos(Transform axis)
//    {
//        //int pageRank = pages.IndexOf(this);
//        this.transform.eulerAngles = Vector3.zero;
//        float length = (pos - axis.position).magnitude;

//        this.transform.localPosition = new Vector3(0, 0.08f, 0) + new Vector3(length * Mathf.Cos(Mathf.PI / 180 * angle), length * Mathf.Sin(Mathf.PI / 180 * angle));
//        this.transform.eulerAngles = new Vector3(0, 0, angle);
//        //如果该书页在翻开页数之前
//        if (BookControl_废弃.openPage > pageIndex)
//        {
//            //如果是封面
//            if (pageIndex == 0)
//            {
//                //Debug.LogError("open");
//                angle = Mathf.Lerp(angle, isOpen ? 180 : 0, Time.deltaTime * 3);
//            }
//            else
//            {
//                //如果是前一张
//                if (BookControl_废弃.openPage == pageIndex + 1)
//                {
//                    angle = Mathf.Lerp(angle, isOpen ? 180 : 0, Time.deltaTime * 3);
//                }
//                else
//                {
//                    angle = Mathf.Lerp(angle, isOpen ? 181 : 0, Time.deltaTime * 3);
//                }
//            }
//        }
//        else
//        {
//            //如果是封面
//            if (pageIndex == 0)
//            {
//                //Debug.LogError("close");
//                angle = Mathf.Lerp(angle, isOpen ? 180 : 0, Time.deltaTime * 3);
//            }
//            else
//            {
//                //如果是当前页
//                if (BookControl_废弃.openPage == pageIndex)
//                {
//                    angle = Mathf.Lerp(angle, isOpen ? 180 : 1, Time.deltaTime * 3);
//                }
//                else
//                {
//                    angle = Mathf.Lerp(angle, isOpen ? 180 : 0, Time.deltaTime * 3);
//                }
//            }
//        }
//        //RotateRound(pos, axis.position, Vector3.forward, angle);
//        //model.transform.RotateAround(axis.position, Vector3.forward, angle);
//    }



//    internal void SetFrontPage(PageType pageType)
//    {
//        switch (pageType)
//        {
//            case PageType.CardList:
//                frontCardListModel.SetActive(true);
//                break;
//            case PageType.Image:
//                frontPictureModel.SetActive(true);
//                break;
//            case PageType.Text:
//                //frontTextModel.SetActive(true);
//                break;
//            default:
//                break;
//        }
//    }
//    internal void SetBackPage(PageType pageType)
//    {
//        switch (pageType)
//        {
//            case PageType.CardList:
//                backCardListModel.SetActive(true);
//                break;
//            case PageType.Image:
//                backPictureModel.SetActive(true);
//                break;
//            case PageType.Text:
//                backTextModel.SetActive(true);
//                break;
//            default:
//                break;
//        }
//    }
//}
