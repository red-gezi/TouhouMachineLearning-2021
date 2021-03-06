﻿using Extension;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thread;
using UnityEngine;
using UnityEngine.UI;
using static Info.BookInfo;
//书本控制器，需要大幅度修改
namespace Control
{
    partial class BookControl : MonoBehaviour
    {
        [LabelText("多人模式牌组板控制器")]
        CardDeckBoardControl cardDeckBoardControl;
        [LabelText("牌库列表控制器")]
        CardLibraryControl cardLibraryControl;

        [Header("书本模型")]

        public GameObject cover_model;
        public GameObject axis_model;

        public static BookControl instance;
        public static GameObject cover;
        public static GameObject axis;
        
        private void Awake()
        {
            instance = this;
            cover = cover_model;
            axis = axis_model;
            singlePage = _singlePage;
            multiplayerPage = _multiplayerPage;
            cardLibraryPage = _cardLibraryPage;
            shrinePage = _shrinePage;
            collectPage = _collectPage;
            configPage = _configPage;
        }
        public void Update()
        {
            cover.transform.eulerAngles = Vector3.zero;
            float length = (cover.transform.position - axis.transform.position).magnitude;
            angle = Mathf.Lerp(angle, isBookOpen ? 180 : 0, Time.deltaTime * 3);
            cover.transform.localPosition = new Vector3(0, 0.08f, 0) + new Vector3(length * Mathf.Cos(Mathf.PI / 180 * angle), length * Mathf.Sin(Mathf.PI / 180 * angle));
            cover.transform.eulerAngles = new Vector3(0, 0, angle);
        }
        [Button]
        public static void SetCoverOpen(bool isOpen) => isBookOpen = isOpen;
        [Header("页面")]
        public GameObject _singlePage;
        public GameObject _multiplayerPage;
        public GameObject _cardLibraryPage;
        public GameObject _shrinePage;
        public GameObject _collectPage;
        public GameObject _configPage;

        public static GameObject singlePage;
        public static GameObject multiplayerPage;
        public static GameObject cardLibraryPage;
        public static GameObject shrinePage;
        public static GameObject collectPage;
        public static GameObject configPage;
        [System.Obsolete("废弃")]
        public  void OpenToPage(PageMode pageMode)
        {
            Task.Run(async () =>
            {
                MainThread.Run(() =>
                {
                    singlePage.SetActive(false);
                    multiplayerPage.SetActive(false);
                    cardLibraryPage.SetActive(false);
                    shrinePage.SetActive(false);
                    collectPage.SetActive(false);
                    configPage.SetActive(false);
                });
                await Task.Delay(1000);
                MainThread.Run(() =>
                {
                    switch (pageMode)
                    {
                        case PageMode.single:
                            
                            singlePage.SetActive(true);
                            break;
                        case PageMode.multiplayer:
                            multiplayerPage.SetActive(true);
                            Control.CardDeckBoardControl.instance.InitDeck();
                            break;
                        case PageMode.cardLibrary:
                            cardLibraryPage.SetActive(true);
                            cardLibraryControl.InitCardLibrary();
                            break;
                        case PageMode.shrine:
                            shrinePage.SetActive(true);
                            break;
                        case PageMode.collect:
                            collectPage.SetActive(true);
                            break;
                        case PageMode.config:
                            configPage.SetActive(true);
                            break;
                        case PageMode.none:
                            singlePage.SetActive(false);
                            multiplayerPage.SetActive(false);
                            cardLibraryPage.SetActive(false);
                            shrinePage.SetActive(false);
                            collectPage.SetActive(false);
                            configPage.SetActive(false);
                            break;
                        default:
                            break;
                    }
                });
            });
           
        }
    }
}
