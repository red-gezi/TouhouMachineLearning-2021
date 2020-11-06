using UnityEngine;
using UnityEngine.SceneManagement;
namespace Control
{
    public class UserModeControl : MonoBehaviour
    {
        public MatchPanelControl matchPanelControl=>GetComponent<MatchPanelControl>();
        //private void Start() => JoinPvpRoom();

        public void JoinPvpRoom()
        {
            matchPanelControl.MatchPanelOpen();
            //Command.GameUI.UiCommand.MatchPanelOpen();
            //Command.Network.NetCommand.JoinRoom();
        }

        public static void CreatSingleRoom() => SceneManager.LoadSceneAsync(2);
        private void OnApplicationQuit() => Command.Network.NetCommand.Dispose();
    }
}