using System;
using System.Collections;
using System.Collections.Generic;
using Metelab.CommonManagers;
using TMPro;
using UnityEngine;

namespace Metelab.Panels
{
    public class BasicPopupConfirmation : MeteMono
    {
        [Header("BasicPopupConfirmation")]
        public MeteButton ButtonAccept;
        public MeteButton ButtonRefuse;
        public TextMeshProUGUI Text_Message;
        public TextMeshProUGUI Text_ButtonAccept;
        public TextMeshProUGUI Text_ButtonRefuse;


        private Action _OnClickAccept;
        private Action _OnClickRefuse;

        public override void Init()
        {
            base.Init();

            ButtonAccept.Init(OnClickedButtonAccept);
            ButtonRefuse.Init(OnClickedButtonRefuse);

        }

        public void Init(string message, string acceptButtonText, string refuseButttonText, Action onClickAccept = null, Action onClickRefuse = null)
        {
            Init();
            Text_Message.text = message;
            _OnClickAccept = onClickAccept;
            _OnClickRefuse = onClickRefuse;
            gameObject.SetActive(true);
            Text_ButtonAccept.text = acceptButtonText;
            Text_ButtonRefuse.text = refuseButttonText;
        }

        private void OnClickedButtonAccept()
        {
            _OnClickAccept?.Invoke();
            UIManager.Instance.BasicPopup.HidePanel();
            Destroy(gameObject);
        }

        private void OnClickedButtonRefuse()
        {
            _OnClickRefuse?.Invoke();
            UIManager.Instance.BasicPopup.HidePanel();
            Destroy(gameObject);
        }
    }
}

