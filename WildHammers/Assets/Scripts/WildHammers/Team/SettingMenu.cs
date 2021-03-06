
using System;
using System.Collections.Generic;
using TMPro;
using UnityCore.Data;
using UnityCore.Game;
using UnityCore.Menu;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using AudioType = UnityCore.Audio.AudioType;

namespace WildHammers
{
    namespace UI
    {
        public class SettingMenu : MonoBehaviour
        {
            [SerializeField] private Slider m_MasterVolumeSlider;
            [SerializeField] private Slider m_MusicVolumeSlider;
            [SerializeField] private Slider m_SfxVolumeSlider;
            [SerializeField] private TMP_Text m_MatchMusicDisplayText;
            public List<AudioType> audioLookupTable;
            public List<string> audioTitleLookupTable;

            private void OnEnable()
            {
                SetVolumeSliders();
                SetMusicChoices();
            }
            
            private void SetVolumeSliders()
            {
                m_MasterVolumeSlider.value = DataController.instance.MasterVolume;
                m_MusicVolumeSlider.value = DataController.instance.MusicVolume;
                m_SfxVolumeSlider.value = DataController.instance.SfxVolume;
            }

            private void SetMusicChoices()
            {
                m_MatchMusicDisplayText.text = audioTitleLookupTable[DataController.instance.MatchMusic];
            }

            public void SetMasterVolume(float _volume)
            {
                GameController.instance.audioMixer.SetFloat("Volume", _volume);
                DataController.instance.MasterVolume = (int) _volume;
            }

            public void SetMusicVolume(float _volume)
            {
                GameController.instance.audioMixer.SetFloat("MusicVolume", _volume);
                DataController.instance.MusicVolume = (int) _volume;
            }

            public void SetSfxVolume(float _volume)
            {
                GameController.instance.audioMixer.SetFloat("SFXVolume", _volume);
                DataController.instance.SfxVolume = (int) _volume;
            }

        }
        
    }
}
