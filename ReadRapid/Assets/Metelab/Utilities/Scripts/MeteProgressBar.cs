using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

namespace Metelab
{
    public class MeteProgressBar : MeteMono
    {
        [Header("LudumProgressBar")]
        [SerializeField] private Image Image_Progress;
        [SerializeField] private TextMeshProUGUI Text_Percentage;
        [SerializeField] private TextMeshProUGUI Text_Counts;
        [SerializeField] private bool IsActive_PercentageText;
        [SerializeField] private bool IsActive_CountsText;
        [Range(0,5)]
        [SerializeField] private int PercentageDigit;
        [SerializeField] private float Percent;
 
        private void Start()
        {
            Text_Percentage.gameObject.SetActive(IsActive_PercentageText);
            Text_Counts.gameObject.SetActive(IsActive_CountsText);
        }

        public void SetProgress(float current, float max)
        {
            Percent = max == 0 ? 0 : Mathf.Clamp01(current / max);
            Image_Progress.fillAmount = Percent;

            if (IsActive_PercentageText)
                Text_Percentage.text = Percent.ToString($"P{PercentageDigit}");

            if (IsActive_CountsText)
                Text_Counts.text = $"{current}/{max}";
        }
    }
}