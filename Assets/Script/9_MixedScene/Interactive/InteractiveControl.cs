using CardModel;
using GameEnum;
using Info;
using UnityEngine;

namespace Control
{
    public class InteractiveControl : MonoBehaviour
    {
        public float height;
        Ray ray;
        public float PassPressTime;
        void Update()
        {
            GetFocusTarget();
            MouseEvent();
            KeyBoardEvent();
        }
        private void GetFocusTarget()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] Infos = Physics.RaycastAll(ray);
            if (Infos.Length > 0)
            {
                for (int i = 0; i < Infos.Length; i++)
                {
                    if (Infos[i].transform.GetComponent<SingleRowInfo>() != null)
                    {
                        AgainstInfo.PlayerFocusRegion = Infos[i].transform.GetComponent<SingleRowInfo>();
                        AgainstInfo.FocusPoint = Infos[i].point;
                        break;
                    }
                    AgainstInfo.PlayerFocusRegion = null;
                }
            }
            float distance = (height - ray.origin.y) / ray.direction.y;
            AgainstInfo.DragToPoint = ray.GetPoint(distance);
            Debug.DrawLine(ray.origin, AgainstInfo.DragToPoint, Color.red);
            Debug.DrawRay(ray.origin, ray.direction, Color.white);
        }
        private void KeyBoardEvent()
        {

            if (Input.GetKey(KeyCode.Space) && Info.AgainstInfo.isMyTurn)
            {
                PassPressTime += Time.deltaTime;
                if (PassPressTime > 2)
                {
                    Command.Network.NetCommand.AsyncInfo(NetAcyncType.Pass);
                    Command.GameUI.UiCommand.SetCurrentPass();
                    PassPressTime = 0;
                }
            }
            if (Input.GetKeyUp(KeyCode.Space) && Info.AgainstInfo.isMyTurn)
            {
                PassPressTime = 0;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _ = Command.GameUI.NoticeCommand.ShowAsync("确认投降？", okAction: Command.StateCommand.Surrender);
            }
        }
        private void MouseEvent()
        {
            if (Input.GetMouseButtonDown(0) && Info.AgainstInfo.isMyTurn)
            {
                if (AgainstInfo.IsWaitForSelectRegion)
                {
                    AgainstInfo.SelectRegion = AgainstInfo.PlayerFocusRegion;
                }
                //处理选择单位的箭头
                if (AgainstInfo.IsWaitForSelectUnits && AgainstInfo.PlayerFocusCard != null && !AgainstInfo.PlayerFocusCard.isGray)
                {
                    Card playerFocusCard = AgainstInfo.PlayerFocusCard;
                    if (!AgainstInfo.SelectUnits.Contains(playerFocusCard))
                    {
                        //Debug.LogError("add" + playerFocusCard);
                        AgainstInfo.SelectUnits.Add(playerFocusCard);
                        Command.GameUI.UiCommand.CreatFixedArrow(playerFocusCard);
                    }
                    else
                    {
                        //Debug.LogError("remove" + playerFocusCard);
                        AgainstInfo.SelectUnits.Remove(playerFocusCard);
                        Command.GameUI.UiCommand.DestoryFixedArrow(playerFocusCard);
                    }
                }
                if (AgainstInfo.IsWaitForSelectLocation)
                {
                    if (AgainstInfo.PlayerFocusRegion != null && AgainstInfo.PlayerFocusRegion.CanBeSelected)
                    {
                        AgainstInfo.SelectRegion = AgainstInfo.PlayerFocusRegion;
                        AgainstInfo.SelectLocation = AgainstInfo.PlayerFocusRegion.Location;
                    }
                }
            }
        }
    }
}