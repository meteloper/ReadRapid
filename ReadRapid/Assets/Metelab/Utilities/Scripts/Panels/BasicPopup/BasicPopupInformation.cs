using System;
using System.Collections;
using System.Collections.Generic;
using Metelab.CommonManagers;
using TMPro;
using UnityEngine;


namespace Metelab.Panels
{
    public class BasicPopupInformation : MeteMono
    {
        public MeteButton ButtonAccept;
        public TextMeshProUGUI Text_Message;
        public TextMeshProUGUI Text_ButtonAccept;

        private Action _OnClickAccept;

        public override void Init()
        {
            base.Init();

            ButtonAccept.Init(OnClickedButtonAccept);
        }

        public void Init(string message,string textButtonAccept, Action onClickAccept = null)
        {
            Init();
            Text_Message.text = message; 
            Text_ButtonAccept.text = textButtonAccept;
            _OnClickAccept = onClickAccept;
            gameObject.SetActive(true);
        }

        private void OnClickedButtonAccept()
        {
            _OnClickAccept?.Invoke();
 //           UIManager.Instance.BasicPopup.HidePanel();
            Destroy(gameObject);
        }

    }
}

