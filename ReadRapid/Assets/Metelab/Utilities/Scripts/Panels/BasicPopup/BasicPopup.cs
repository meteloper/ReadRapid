using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Metelab.CommonManagers;

namespace Metelab.Panels
{

    public enum ConfirmationPopupType
    {
        ListRemove = 0,
        WordRemove = 1
    }

    public enum InformationPopupType
    {
    }


    public class BasicPopup : MetePanel
    {
        [Header("BasicPopup")]
        public BasicPopupConfirmation TemplateConfirmation;
        public BasicPopupInformation TemplateInformation;


        private readonly string[] ConfirmationMessages = {
            "Are you sure you want to delete {0} list?\nThere {1} {2} words in it.",
            "Are you sure you want to delete <b><color=red>\"{0}\"</color></b> word?"
        };

        private readonly string[] InformationMessages = {
        };

        private readonly string[][] ConfirmationButtonText = {
            new string[]{ "Okey","Cancel" },
            new string[]{ "Okey","Cancel" }
        };

        private readonly string[] InformationButtonText = {
            
        };

        /// <param name="indexMessages">
        /// ListRemove [0] => List name, [1] => List count
        /// WordRemove [0] => Word
        /// </param>
        public void CreatePopup(ConfirmationPopupType type,Action onClickAccept = null, Action onClickRefuse = null, params string[] indexMessages)
        {
            string message = string.Empty;
            int typeIndex = (int)type;
            switch (type)
            {
                case ConfirmationPopupType.ListRemove:
                    int wordCount = 0;
                    int.TryParse(indexMessages[1], out wordCount);
                    message = string.Format(ConfirmationMessages[typeIndex], indexMessages[0], wordCount > 1? "are": "is" , indexMessages[1]);
                    break;
                case ConfirmationPopupType.WordRemove:
                    message = string.Format(ConfirmationMessages[typeIndex], indexMessages[0]);
                    break;
                default:
                    message = string.Format(ConfirmationMessages[typeIndex], indexMessages);
                    break;
            }

            Instantiate(TemplateConfirmation, PanelSafeArea).Init(message, ConfirmationButtonText[typeIndex][0], ConfirmationButtonText[typeIndex][1], onClickAccept, onClickRefuse);
            ShowPanel();
        }

        public void CreatePopup(InformationPopupType type, Action onClickAccept = null,params string[] indexMessages)
        {
            int typeIndex = (int)type;
            string message = string.Format(InformationMessages[typeIndex], indexMessages);
            Instantiate(TemplateInformation, PanelSafeArea).Init(message, InformationButtonText[typeIndex], onClickAccept);
            ShowPanel();
        }
    }
}


