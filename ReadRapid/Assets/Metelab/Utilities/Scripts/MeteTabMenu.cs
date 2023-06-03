using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Metelab
{
    public class MeteTabMenu : MeteMono
    {
        [Header(nameof(MeteTabMenu))]
        [SerializeField] private List<MeteButton> Buttons_Tab;
        [SerializeField] private List<GameObject> Panels_Tab;

        [SerializeField] private int CurrentPanelIndex = 0;
        public override void Init()
        {
            base.Init();

            //Initializing buttons and panels
            for (int i = 0; i < Buttons_Tab.Count; i++)
            {
                Panels_Tab[i].SetActive(false);

                int index = i;
                Buttons_Tab[index].Init(() => { ShowPanel(index); });
                Buttons_Tab[index].image.color = Buttons_Tab[index].colors.disabledColor;
            }

            ShowPanel(CurrentPanelIndex);
        }

        public void ShowPanel(int panelIndex)
        {
            Panels_Tab[CurrentPanelIndex].SetActive(false);
            Buttons_Tab[CurrentPanelIndex].image.color = Buttons_Tab[CurrentPanelIndex].colors.disabledColor;
            Panels_Tab[panelIndex].SetActive(true);
            Buttons_Tab[panelIndex].image.color = Color.white;
            CurrentPanelIndex = panelIndex;
        }

        public void SetButtonText(int index,string text)
        {
            Buttons_Tab[index].GetComponentInChildren<TextMeshProUGUI>().text = text;
        }
    }
}
