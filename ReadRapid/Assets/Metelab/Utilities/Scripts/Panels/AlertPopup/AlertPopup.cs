using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

namespace Metelab.Panels
{
    public enum AlertTypes
    {
        None = -1,
        WordAdded = 0,
        NotEnoughWord = 1,
        PleaseFill = 2,
        Copyed = 3,
        ExistsProfile = 4,
    }

    public class AlertPopup : MetePanel
    {
        [Header(nameof(AlertPopup))]
        public TextMeshProUGUI TemplateTextMessage;
        public GameObject TemplateObject;

        private const float TOTAL_DURATION_SEC = 1.3f;
        private const float MOVE_DURATION_SEC = 0.4f;
        private const float HIDE_DURATINON = 0.2f;

        private readonly string[] AlertMessage = {
            "\"{0}\" has been added",
            "There are not enough words in the train list!",
            "Please fill word ({0})",
            "\"{0}\" has been copyed",
            "This profile already exists",
        };

        public override void Init()
        {
            base.Init();
            TemplateObject.SetActive(false);
            TemplateObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            TemplateObject.transform.localScale = new Vector3(1, 0, 1);
        }

        public void CreatePopup(AlertTypes alertType, params string[] indexMessages)
        {
            TemplateTextMessage.text = string.Format(AlertMessage[(int)alertType], indexMessages);
            GameObject instance = Instantiate(TemplateObject, PanelSafeArea);
            instance.SetActive(true);
            float endValueY = TemplateObject.transform.position.y - (Screen.height / 3);
            instance.transform.DOScaleY(1, HIDE_DURATINON).SetEase(Ease.OutExpo);
            instance.transform.DOMoveY(endValueY, MOVE_DURATION_SEC).SetEase(Ease.OutQuad);
            instance.transform.DOScaleY(0, HIDE_DURATINON).SetEase(Ease.OutExpo).SetDelay(TOTAL_DURATION_SEC - HIDE_DURATINON);
            Destroy(instance, TOTAL_DURATION_SEC);
            ShowPanel();
        }
    }
}
