using Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Thread;
using UnityEngine;
using UnityEngine.UI;
namespace Command
{
    namespace GameUI
    {
        public class NoticeCommand : MonoBehaviour
        {
            string language = "Ch";
            static Func<Task> okAction;
            static Func<Task> cancelAction;
            static Image image = Info.GameUI.UiInfo.Notice.transform.GetComponent<Image>();
            static Transform noticeTransform = Info.GameUI.UiInfo.Notice.transform;
            static Text noticeText = noticeTransform.GetChild(0).GetComponent<Text>();
            static Transform okButton = noticeTransform.GetChild(1);
            static Transform cancelButton = noticeTransform.GetChild(2);
            static bool isShowOver = true;
            public static async Task OkAsync()
            {
                await CloseAsync();
                await Task.Delay(1000);
                if (okAction != null)
                {
                    await okAction();
                }
                await Task.Delay(500);
                isShowOver = true;
            }

            public static async Task CancaelAsync()
            {
                await CloseAsync();
                await Task.Delay(1000);
                if (cancelAction != null)
                {
                    await cancelAction();
                }
                await Task.Delay(500);
                isShowOver = true;
            }

            public static async Task ShowAsync(string text, NotifyBoardMode notifyBoardMode = NotifyBoardMode.Ok_Cancel, Func<Task> okAction = null, Func<Task> cancelAction = null)
            {
                isShowOver = false;
                NoticeCommand.okAction = okAction;
                NoticeCommand.cancelAction = cancelAction;
                Color color = image.color;
                MainThread.Run(() =>
                {
                    noticeText.text = text;
                    Info.GameUI.UiInfo.Notice.SetActive(true);
                    switch (notifyBoardMode)
                    {
                        case NotifyBoardMode.Ok:
                            okButton.gameObject.SetActive(true);
                            cancelButton.gameObject.SetActive(false);
                            okButton.localPosition = new Vector3(0, -100, 0);
                            //cancelButton.localPosition=new Vector3(130,-100,0);
                            break;
                        case NotifyBoardMode.Ok_Cancel:
                            okButton.gameObject.SetActive(true);
                            cancelButton.gameObject.SetActive(true);
                            okButton.localPosition = new Vector3(-130, -100, 0);
                            //cancelButton.localPosition = new Vector3(130, -100, 0);
                            break;
                        case NotifyBoardMode.Cancel:
                            okButton.gameObject.SetActive(false);
                            cancelButton.gameObject.SetActive(true);
                            cancelButton.localPosition = new Vector3(-130, -100, 0);
                            break;
                        default:
                            break;
                    }
                    //image.color = color.SetA(0.5f);
                });
                await Task.Run(async () =>
                {
                    for (float i = 0.5f; i < 1; i += 0.03f)
                    {
                        MainThread.Run(() =>
                        {
                            noticeTransform.localScale = new Vector3(1, i, 1);
                            image.color = color.SetA(i);
                        });
                        await Task.Delay(10);
                    }
                });
                while (!isShowOver)
                {
                    await Task.Delay(100);
                }
            }
            public static async Task CloseAsync()
            {
                Color color = image.color;
                await Task.Run(async () =>
                {
                    for (float i = 1; i >= 0.5f; i -= 0.03f)
                    {
                        MainThread.Run(() =>
                        {
                            noticeTransform.localScale = new Vector3(1, i, 1);
                            image.color = color.SetA(i);
                        });
                        await Task.Delay(10);
                    }
                    MainThread.Run(() =>
                    {
                        Info.GameUI.UiInfo.Notice.SetActive(false);
                    });
                });

            }
        }
    }


}
