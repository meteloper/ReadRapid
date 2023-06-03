using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Diagnostics;
using UnityEngine.PlayerLoop;
using System.Linq;

namespace Metelab.ReadRapid
{
    public class MainPanel : MetePanel
    {
        public TMP_InputField Input_Text;
        public MeteButton Button_Convert;
        public TextMeshProUGUI Text_FontSize;
        [Range(0,10f)]
        public float ScrollSpeed = 0.5f;


        public override void Init()
        {
            base.Init();
            Button_Convert.Init(OnClickButton_Convert);

           
        }

        private void Update()
        {
            Text_FontSize.text = Input_Text.textComponent.fontSize.ToString("n0");
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                Metelab.Log("CTRL!");
                Input_Text.scrollSensitivity = 0;
                Input_Text.textComponent.fontSize = Mathf.Clamp(Input_Text.textComponent.fontSize + Input.mouseScrollDelta.y, 10, 100);


                Input_Text.verticalScrollbar.interactable = false;
                return;
            }

            Input_Text.verticalScrollbar.interactable = true;
            float scrollAmount = Input.mouseScrollDelta.y * ScrollSpeed;
            Input_Text.scrollSensitivity = ScrollSpeed;
        }
 

        private void OnClickButton_Convert()
        {
            Metelab.Log("Convert Button!");

            string rawText = Input_Text.text.Trim();
 
            //My mother has a good dog
            string[] rowTextArray = rawText.Split("\n");
            string outputText = string.Empty;

            for (int i = 0; i < rowTextArray.Length; i++)
            {
                string[] splitArray = rowTextArray[i].Split(" ");

                for (int j = 0; j < splitArray.Length; j++)
                {
                    if (splitArray[j].Contains("<b>"))
                        continue;

                    int letterCount = splitArray[j].Length;
                    int boldLetterCount = Mathf.CeilToInt(letterCount / 2);
                    outputText += $"<b>{splitArray[j].Substring(0, boldLetterCount)}</b>{splitArray[j].Substring(boldLetterCount)} ";
                    
                }

                outputText += "\n";
            }

            Input_Text.text = outputText;
        }

    }
}
