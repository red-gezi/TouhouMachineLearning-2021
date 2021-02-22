using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Info
{
    public class BookInfo : MonoBehaviour
    {
        //组件化
        [Header("书本模型")]

        public static bool isBookOpen;
        public static float angle = 0;
        [Header("组件")]
        public GameObject _cardListComponent;
        public GameObject _cardDeckListComponent;
        public GameObject _cardLibraryComponent;
        public GameObject _mapComponent;
        public static GameObject cardListComponent;
        public static GameObject cardDeckListComponent;
        public static GameObject cardLibraryComponent;
        public static GameObject mapComponent;
        // Start is called before the first frame update
        void Awake()
        {
            cardListComponent = _cardListComponent;
            cardDeckListComponent = _cardDeckListComponent;
            cardLibraryComponent = _cardLibraryComponent;
            mapComponent = _mapComponent;
        }
    }
}

