
using System;
using System.Collections.Generic;
using TMPro;
using UnityCore.Audio;
using UnityCore.Data;
using UnityCore.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using AudioType = UnityCore.Audio.AudioType;

namespace WildHammers
{
    namespace UI
    {
        public class CycleSoundArrayButton : Button
        {
            public bool isForward;
            [SerializeField] private TMP_Text m_OptionDisplay;
            [SerializeField] private SettingMenu m_SettingsMenu;
            private int m_LookupTableIndex = 0;

            public override void OnSubmit(BaseEventData _eventData)
            {
                if (isForward)
                {
                    if (m_LookupTableIndex < m_SettingsMenu.audioLookupTable.Count-1) m_LookupTableIndex++;
                    else m_LookupTableIndex = 0;
                    GameController.instance.matchMusic = m_SettingsMenu.audioLookupTable[m_LookupTableIndex];
                    m_OptionDisplay.text = m_SettingsMenu.audioTitleLookupTable[m_LookupTableIndex];
                    AudioController.instance.PlayAudio(GameController.instance.matchMusic);
                }
                else
                {
                    if (m_LookupTableIndex > 0) m_LookupTableIndex--;
                    else m_LookupTableIndex = m_SettingsMenu.audioLookupTable.Count-1;
                    GameController.instance.matchMusic = m_SettingsMenu.audioLookupTable[m_LookupTableIndex];
                    m_OptionDisplay.text = m_SettingsMenu.audioTitleLookupTable[m_LookupTableIndex];
                    AudioController.instance.PlayAudio(GameController.instance.matchMusic);
                }

                DataController.instance.MatchMusic = m_LookupTableIndex;

                base.OnSelect(_eventData);
            }
        }
        
    }    
}

