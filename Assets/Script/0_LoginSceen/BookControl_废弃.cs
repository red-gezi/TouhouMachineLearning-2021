//using Extension;
//using Sirenix.OdinInspector;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using UnityEngine;
//using UnityEngine.UI;
//using static BookControl_废弃.PageTree;

//public class BookControl_废弃 : MonoBehaviour
//{

//    public Transform axis;
//    public GameObject cover;
//    static GameObject cover_model;

//    public GameObject page;
//    //static GameObject page_model;

//    //public GameObject page_Cardlist;

//    [ShowInInspector]
//    public static int openPage = 0;
//    public static bool isOpenOver = true;

//    PageTree pageTree;
//    static Dictionary<int, PageTree.PageNode> pageNodeIndex = new Dictionary<int, PageTree.PageNode>();

//    public class PageTree
//    {
//        public int index;
//        public List<PageNode> pageNodes;
//        public void SetIndex(List<PageNode> nodes)
//        {
//            nodes.ForEach(node =>
//            {
//                if (index != 0)
//                {
//                    node.PageControl.Init(index);
//                }
//                node.pageModel.name += index;
//                pageNodeIndex[index] = node;
//                index++;
//                SetIndex(node.PageNodes);
//            });
//        }
//        public class PageNode
//        {
//            public GameObject pageModel;
//            public PageControl_废弃 PageControl;
//            public List<PageNode> PageNodes = new List<PageNode>();
//            public PageNode(GameObject pageMode, bool isCover = false, bool hasTag = false, int tagRank = 0, string tag = "")
//            {
//                //GameObject pageModel = isCover ? cover_model : 

//                //pageModel = isCover ? cover_model:Instantiate(page_model);
//                pageModel = pageMode;
//                //if (!isCover)
//                //{
//                //    pageModel.transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
//                //    pageModel.transform.GetChild(0).name = "gezi";
//                //}
//                //pageModel.transform.parent=tra
//                PageControl = pageModel.GetComponent<PageControl_废弃>();
//                if (hasTag)
//                {
//                    PageControl.SetTag(tagRank, tag);
//                }
//                pageModel.name = tag;
//            }

//            public PageNode SetFrontPage(PageType pageType)
//            {
//                PageControl.SetFrontPage(pageType);
//                return this;
//            }
//            public PageNode SetBackPage(PageType pageType)
//            {
//                PageControl.SetBackPage(pageType);
//                return this;
//            }
//            public PageNode AddPage(PageNode pageNode)
//            {
//                PageNodes.Add(pageNode);
//                return this;
//            }
//        }
//    }
//    void Start()
//    {
//        cover_model = cover;
//        //page_model = page;
//        //pageTree = new PageTree();
//        //pageTree.pageNodes = new List<PageNode>()
//        //{
//        //    new PageNode(cover,isCover:true,tag:"封面"),
//        //    new PageNode(page, hasTag:true,tag:"单人",tagRank:0)
//        //    .SetFrontPage(PageType.Text)
//        //    .SetBackPage(PageType.Text)
//        //    .AddPage(new PageNode(page,tag:"教程模式"))
//        //    .AddPage(new PageNode(page,tag:"剧情模式")),
//        //    new PageNode(page,hasTag:true,tag:"多人",tagRank:1)
//        //    .SetFrontPage(PageType.Text)
//        //    .SetBackPage(PageType.Text)
//        //    .AddPage(new PageNode(page,tag:"休闲模式"))
//        //    .AddPage(new PageNode(page,tag:"天梯模式")),
//        //    new PageNode(page,hasTag:true,tag:"收藏",tagRank:2)
//        //    .SetFrontPage(PageType.Text)
//        //    .SetBackPage(PageType.Text)
//        //    .AddPage(new PageNode(page_Cardlist,tag:"卡牌"))
//        //        .SetFrontPage(PageType.Text)
//        //        .SetBackPage(PageType.Text)
//        //        .AddPage(new PageNode(page))
//        //        //.SetFrontPage(PageType.CardList)
//        //        //.SetBackPage(PageType.CardList))
//        //    .AddPage(new PageNode(page,tag:"成就一览")
//        //        .SetFrontPage(PageType.Text)
//        //        .SetBackPage(PageType.Text))
//        //    .AddPage(new PageNode(page,tag:"对战记录")
//        //        .SetFrontPage(PageType.Text)
//        //        .SetBackPage(PageType.Text)
//        //    ),
//        //    new PageNode(page)
//        //};
//        //pageTree.SetIndex(pageTree.pageNodes);
//        // transform.GetChild(0).GetComponent<Canvas>().
//    }
//    void Update()
//    {
//        pageNodeIndex.Values.ForEach(node => node.PageControl.RefreshPos(axis));
//    }
//    [Button]
//    [System.Obsolete("废弃")]
//    public static void OpenToPage(int page)
//    {
//        if (isOpenOver)
//        {
//            isOpenOver = false;

//            int targetRank = Mathf.Min(pageNodeIndex.Count, page);
//            Task.Run(async () =>
//            {

//                if (openPage < targetRank)
//                {
//                    for (int i = openPage; i < targetRank; i++)
//                    {
//                        pageNodeIndex[i].PageControl.isOpen = true;
//                        await Task.Delay(200);
//                    }
//                }
//                else
//                {
//                    for (int i = openPage - 1; i >= targetRank; i--)
//                    {
//                        pageNodeIndex[i].PageControl.isOpen = false;
//                        await Task.Delay(200);
//                    }
//                }
//                openPage = targetRank;
//                isOpenOver = true;
//            });
//        }
//    }
//    public static void SetCover(bool isOpen)
//    {
//        //cover_model.transform.eulerAngles = Vector3.zero;
//        //float length = (cover_model.transform.position - axis.position).magnitude;

//        //float angle = Mathf.Lerp(angle, isOpen ? 180 : 0, Time.deltaTime * 3);

//        //cover_model.transform.localPosition = new Vector3(0, 0.08f, 0) + new Vector3(length * Mathf.Cos(Mathf.PI / 180 * angle), length * Mathf.Sin(Mathf.PI / 180 * angle));
//        //cover_model.transform.eulerAngles = new Vector3(0, 0, angle);
//    }
//}
