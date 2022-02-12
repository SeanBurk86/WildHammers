
using System;
using UnityCore.Data;
using UnityCore.Game;
using UnityCore.Menu;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace WildHammers
{
    namespace UI
    {
        public class SettingMenu : MonoBehaviour
        {
            [SerializeField] private Slider m_MasterVolumeSlider;
            [SerializeField] private Slider m_MusicVolumeSlider;
            [SerializeField] private Slider m_SfxVolumeSlider;

            private void OnEnable()
            {
                SetVolumeSliders();
            }
            
            private void SetVolumeSliders()
            {
                m_MasterVolumeSlider.value = DataController.instance.MasterVolume;
                m_MusicVolumeSlider.value = DataController.instance.MusicVolume;
                m_SfxVolumeSlider.value = DataController.instance.SfxVolume;
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
