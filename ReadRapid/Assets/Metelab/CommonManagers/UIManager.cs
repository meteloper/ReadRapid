using System.Collections.Generic;
using Metelab.Panels;
using UnityEngine;


namespace Metelab.CommonManagers
{
    public enum PanelTypes: int
    {
        None = -1,
        MainMenu = 0,
        WordAdd = 1,
        WordLanguageSelection = 2,
        WordList = 3,
        WordTrainingLauncher = 4,
        BasicPopup = 5,
        WordTraining = 6,
        NewWordListAddPopup = 7,
        Settings = 8,
        WordLearningPopup = 9,
        AlertPopup=10,
    }


    public class UIManager : MeteSingleton<UIManager>
    {
        [SerializeField] private PanelTypes StarterPanel;
        [SerializeField] private List<MetePanel> PanelList = new List<MetePanel>();
        [SerializeField] private List<PanelTypes> ActivePanelList = new List<PanelTypes>();
        [SerializeField] private PanelTypes CurrentActivePanel;
        [SerializeField] public BasicPopup BasicPopup { get { return (BasicPopup)PanelList[(int)PanelTypes.BasicPopup]; } }
        [SerializeField] public AlertPopup AlertPopup { get { return (AlertPopup)PanelList[(int)PanelTypes.AlertPopup]; } }

        public override void EarlyInit()
        {
            base.EarlyInit();

            foreach (var panel in PanelList)
            {
                panel.EarlyInit();
                panel.Deactive();
            }
        }

        public override void Init()
        {
            base.Init();

            foreach (var panel in PanelList)
                panel.Init();

            ShowPanel(StarterPanel);
        }

        public MetePanel ShowPanel(PanelTypes panel)
        {
            MetePanel targetPanel = PanelList[(int)panel];
            if (targetPanel.PanelData.IsFullScreenPanel)
            {
                if (CurrentActivePanel != PanelTypes.None && CurrentActivePanel != panel)
                    HidePanel(CurrentActivePanel);
                
                CurrentActivePanel = panel;
            }

            if (!ActivePanelList.Contains(panel))
                ActivePanelList.Add(panel);

            targetPanel.ShowPanel();
            return targetPanel;
        }

        public T ShowPanel<T>(PanelTypes panel) where T:MetePanel
        {
            return (T)ShowPanel(panel);
        }

        public  void HidePanel(PanelTypes panel)
        {
            PanelList[(int)panel].HidePanel();

            if (ActivePanelList.Contains(panel))
                ActivePanelList.Remove(panel);

            if (CurrentActivePanel == panel)
                CurrentActivePanel = PanelTypes.None;
        }

        public MetePanel GetPanel(PanelTypes panel)
        {
            if (panel != PanelTypes.None)
                return PanelList[(int)panel];
            else
                return null;
        }

        public T GetPanel<T>(PanelTypes panel) where T:MetePanel
        {
            return (T)GetPanel(panel);
        }
    }
}

