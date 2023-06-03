using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Metelab
{
    public class MeteSelectionList : MeteMono, IPointerClickHandler
    {
        public Action<List<MeteSelectionListItemData>> OnEditEnd;

        [Header(nameof(MeteSelectionList))]
        [SerializeField] private RectTransform Parent_ListItems;
        [SerializeField] private VerticalLayoutGroup VerticalLayoutGroup_Parent_ListItems;
        [SerializeField] private RectTransform TemplateItem;
        [SerializeField] private RectTransform Layer_List;
        [SerializeField] private MeteButton Button_SelectAll;
        [SerializeField] private MeteButton Button_UnselectAll;
        [SerializeField] private MeteButton Button_Outside;
        [SerializeField] private RectTransform Layer_Buttons;
        [SerializeField] private Sprite Sprite_Icon_ShowList;
        [SerializeField] private Sprite Sprite_Icon_HideList;
        [SerializeField] private Image Image_Icon;
        [SerializeField] private TextMeshProUGUI Text_Selected;

        [Header("Settings")]
        [SerializeField] private bool IsFixSize = true;
        [SerializeField] private int MaxItemCount = 5;
        [SerializeField] private bool IsClamp = true;
        [SerializeField] private int ListSpaceVerticle = 4;
        [SerializeField] private bool IsDown = true;
        [SerializeField] private string EmptyText = "NONE";
        [SerializeField] private string AllText = "ALL";
        [SerializeField] private string MixedText = "MIXED";

        /// <summary>
        /// This text will added after EmptyText, AllText, MixedText text
        /// </summary>
        public string ExtraText;

        public void SetExtraTextAndApply(string text)
        {
            ExtraText = text;
            UpdateSelectedText();
        }

        private float ItemHeight
        {
            get { return TemplateItem.rect.height; }
        }

        [Header("Debug")]
        private List<MeteSelectionListItemData> _List_ItemData = new List<MeteSelectionListItemData>();
        private List<MeteSelectionListItemData> _List_Selected_ItemData = new List<MeteSelectionListItemData>();

        private RectTransform _RectTransform_MainCanvas;
        public override void Init()
        {
            base.Init();
            Button_SelectAll.Init(OnClickedButton_SelectAll);
            Button_UnselectAll.Init(OnClickedButton_UnselectAll);
            Button_Outside.Init(OnClickedButton_Outside);
            _List_ItemData.Clear();
            UpdateLayers(false);
            _RectTransform_MainCanvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        }

        public List<MeteSelectionListItemData> GetSelectedItems()
        {
            return _List_ItemData.FindAll((x) => { return x.ToggleObject.isOn; });
        }

        public void AddOption( string optionText ,int value)
        {
            GameObject gameObj = Instantiate(TemplateItem, Parent_ListItems).gameObject;
            TextMeshProUGUI textObj = gameObj.GetComponentInChildren<TextMeshProUGUI>();
            Toggle toggleObj = gameObj.GetComponentInChildren<Toggle>();
            toggleObj.isOn = false;
            MeteSelectionListItemData itemData = new global::Metelab.MeteSelectionListItemData()
            {
                text = optionText,
                values = value,
                ToggleObject = toggleObj
            };

            textObj.text = optionText;
//            toggleObj.onValueChanged.AddListener(value => { OnToggleChanged(itemData, value); });
            gameObj.SetActive(true);

            _List_ItemData.Add(itemData);
        }

        /// <param name="options">(text,value,isOn)</param>
        public void AddOptions(List<Tuple<string, int>> options)
        {
            foreach (var item in options)
            {
                AddOption(item.Item1, item.Item2);
            }
        }

        public void ClearOptions()
        {
            _List_ItemData.Clear();
            Parent_ListItems.DestoryChildren();
        }

        /// <param name="options">(index, isOn)</param>
        public void SetOptions(List<Tuple<int, bool>> options)
        {
            foreach (var option in options)
                _List_ItemData[option.Item1].ToggleObject.isOn = option.Item2;

            _List_Selected_ItemData = GetSelectedItems();
            UpdateSelectedText();
        }

        public void SetOption(int index, bool isOn)
        {
            _List_ItemData[index].ToggleObject.isOn = isOn;

            _List_Selected_ItemData = GetSelectedItems();
            UpdateSelectedText();
        }

        private void UpdateListHeight()
        {
            if (IsFixSize)
            {
                Vector2 size = Layer_List.sizeDelta;
                float bordersHeight = VerticalLayoutGroup_Parent_ListItems.spacing * Mathf.Clamp(_List_ItemData.Count - 1, 0, MaxItemCount) +
                    VerticalLayoutGroup_Parent_ListItems.padding.top + VerticalLayoutGroup_Parent_ListItems.padding.bottom;

                size.y = ItemHeight * Mathf.Clamp(_List_ItemData.Count, 0, MaxItemCount) + bordersHeight;
                Layer_List.sizeDelta = size;
            }
        }

        private void UpdateListPosition()
        {
            if (IsClamp)
            {
                if (IsDown)
                {
                    Layer_List.anchorMax = new Vector2(1,0);
                    Layer_List.anchorMin = new Vector2(0,0);
                    Layer_List.pivot = new Vector2(0.5f,1);
                    Layer_List.anchoredPosition = new Vector2(0, -ListSpaceVerticle);
                }
                else
                {
                    Layer_List.anchorMax = new Vector2(1, 1);
                    Layer_List.anchorMin = new Vector2(0, 1);
                    Layer_List.pivot = new Vector2(0.5f, 0);
                    Layer_List.anchoredPosition = new Vector2(0, -ListSpaceVerticle);
                }
            }
        }

        public void UpdateSelectedText()
        {
            if (_List_Selected_ItemData.Count == 0)
                Text_Selected.text = EmptyText;
            else if (_List_Selected_ItemData.Count == 1)
                Text_Selected.text = _List_Selected_ItemData[0].text;
            else if(_List_Selected_ItemData.Count == _List_ItemData.Count)
                Text_Selected.text = AllText + ExtraText;
            else
                Text_Selected.text = MixedText + ExtraText;
        }

        private void UpdateLayers(bool isShowList)
        {
            Layer_List.gameObject.SetActive(isShowList);
            Layer_Buttons.gameObject.SetActive(isShowList);
            UpdateListHeight();
            UpdateListPosition();
            Button_Outside.gameObject.SetActive(isShowList);
            if (isShowList)
            {
                Button_Outside.RectTransfrom.SetParent(_RectTransform_MainCanvas);
                Layer_Buttons.SetParent(_RectTransform_MainCanvas);
                Layer_List.SetParent(_RectTransform_MainCanvas);
                Image_Icon.sprite = Sprite_Icon_HideList;
                Button_Outside.RectTransfrom.anchorMax = new Vector2(1, 1);
                Button_Outside.RectTransfrom.anchorMin = new Vector2(0, 0);
                Button_Outside.RectTransfrom.offsetMin = Vector2.zero;
                Button_Outside.RectTransfrom.offsetMax = Vector2.zero;
            }
            else
            {
                Layer_List.SetParent(transform);
                Layer_Buttons.SetParent(transform);
                Button_Outside.RectTransfrom.SetParent(transform);
                Image_Icon.sprite = Sprite_Icon_ShowList;
                Button_Outside.RectTransfrom.parent = transform;
            }

            _List_Selected_ItemData = GetSelectedItems();
            UpdateSelectedText();
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            UpdateLayers(!Layer_List.gameObject.activeSelf);

            if (!Layer_List.gameObject.activeSelf)
                OnEditEnd?.Invoke(_List_Selected_ItemData);
        }

        //private void OnToggleChanged(LudumSelectionListItemData itemData,bool value)
        //{
        //    Ludum.Log($"{itemData.text}, {itemData.values}, {value}");
        //}

        private void OnClickedButton_SelectAll()
        {
            foreach (var item in _List_ItemData)
            {
                item.ToggleObject.SetIsOnWithoutNotify(true);
            }
        }

        private void OnClickedButton_UnselectAll()
        {
            foreach (var item in _List_ItemData)
            {
                item.ToggleObject.SetIsOnWithoutNotify(false);
            }
        }

        private void OnClickedButton_Outside()
        {
            UpdateLayers(false);
            OnEditEnd?.Invoke(_List_Selected_ItemData);
        }
    }

    public class MeteSelectionListItemData
    {
        public string text;
        public int values;
        public Toggle ToggleObject;
    }
}
