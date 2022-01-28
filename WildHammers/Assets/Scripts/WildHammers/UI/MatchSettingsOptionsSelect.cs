
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace WildHammers
{
    namespace UI
    {
        public class MatchSettingsOptionsSelect : Button
        {
            public bool isForward;
            [SerializeField] private TMP_Text optionDisplay;
            public int incrementAmount = 1;
            
            public override void OnSubmit(BaseEventData _eventData)
            {
                base.OnSelect(_eventData);
                if (isForward)
                {
                    optionDisplay.text =  (Int32.Parse(optionDisplay.text) + incrementAmount).ToString();
                }
                else
                {
                    if(Int32.Parse(optionDisplay.text)>incrementAmount) 
                        optionDisplay.text =  (Int32.Parse(optionDisplay.text) - incrementAmount).ToString();
                }
                
            }
        }
        
    }
}

