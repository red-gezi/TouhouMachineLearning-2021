using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Info
{
    namespace GameUI
    {
        public class UiInfo : MonoBehaviour
        {
            public static UiInfo Instance;
            public GameObject DownPass;
            public GameObject UpPass;
            public static GameObject MyPass => AgainstInfo.isMyTurn ? Instance.DownPass : Instance.UpPass;
            public static GameObject OpPass => AgainstInfo.isMyTurn ? Instance.UpPass : Instance.DownPass;

            public Animator NoticeAnim;
            public GameObject Arrow_Model;
            public GameObject CardInstanceModel;
            public GameObject NoticeBoard_Model;
            public GameObject ArrowEndPoint_Model;
            public Transform ConstantInstance;
            public GameObject CardBoardInstance;
            public GameObject CardIntroductionModel;
           

            //卡牌面板进度
            [ShowInInspector]
            public  float cardBoardProcess => ConstantInstance.GetComponent<RectTransform>().rect.x;
            //public  RectOffset cardBoardProcess => ConstantInstance.GetComponent<HorizontalLayoutGroup>();
            //public static float targetCardBoardProcess=> cardBoardProcess;
            private void Awake() => Instance = this;
            private void Update()
            {
                if (true)
                {
                    //ConstantInstance.GetComponent<RectTransform>().rect.x=>
                }
            }
            public static string CardBoardTitle = "";
            public static string NoticeBoardTitle = "";
            [ShowInInspector]
            public static bool isNoticeBoardShow = false;

            public static List<GameObject> ShowCardLIstOnBoard = new List<GameObject>();
            public static Dictionary<int, Sprite> CardImage = new Dictionary<int, Sprite>();
            

            public static GameObject Arrow => Instance.Arrow_Model;
            public static GameObject ArrowEndPoint => Instance.ArrowEndPoint_Model;
            public static Transform Constant => Instance.ConstantInstance;
            public static GameObject CardModel => Instance.CardInstanceModel;
            public static GameObject CardBoard => Instance.CardBoardInstance;
            public static GameObject NoticeBoard => Instance.NoticeBoard_Model;
          

           
        }
    }
}