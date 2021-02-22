using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace Control
{
    public class NoticeControl : MonoBehaviour
    {
        public void Ok() => Command.GameUI.NoticeCommand.OkAsync();
        public void Cancel() => Command.GameUI.NoticeCommand.CancaelAsync();
    }
}