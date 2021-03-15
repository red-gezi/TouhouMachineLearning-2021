using Extension;
using System.Threading.Tasks;
using Thread;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Network.NetInfoModel;
namespace Control
{
    public class UserLoginControl : MonoBehaviour
    {
        public Text UserName;
        public Text Password;
        void Start()
        {
            Command.Network.NetCommand.Init();//Command.Network.NetCommand.Login("", "");
            UserLogin();//自动登录
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //BookControl.ClosePageAll();
                _ = Command.GameUI.NoticeCommand.ShowAsync("退出登录",
                    okAction: async () =>
                     {
                         MainThread.Run(() =>
                         {
                             BookControl.SetCoverOpen(false);
                             CameraViewControl.MoveToInitView();
                             //BookControl.instance.OpenToPage(PageMode.none);
                             Command.BookCommand.OpenToPage(PageMode.none);
                             Info.GameUI.UiInfo.loginCanvas.SetActive(true);
                         });
                     }
                    );

                //
            }
        }
        public void UserRegister() => Command.Network.NetCommand.Register(UserName.text, Password.text);
        public void UserLogin() => Command.Network.NetCommand.Login(UserName.text, Password.text);
        public void UserServerSelect() => Info.AgainstInfo.isHostNetMode = !Info.AgainstInfo.isHostNetMode;

        private void OnApplicationQuit() => Command.Network.NetCommand.Dispose();
    }
}