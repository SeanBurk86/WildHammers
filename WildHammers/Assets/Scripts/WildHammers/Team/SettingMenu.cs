
using UnityCore.Menu;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace WildHammers
{
    namespace UI
    {
        public class SettingMenu : MonoBehaviour
        {
            public AudioMixer audioMixer;
            
            [SerializeField] private GameObject startMenuButtons, startAMatchButton, startPrompt;

            public void SetMasterVolume(float _volume)
            {
                Debug.Log("Master _volume is "+_volume);
                audioMixer.SetFloat("Volume", _volume);
            }
            
            public void SetSFXVolume(float _volume)
            {
                Debug.Log("SFX _volume is "+_volume);
                audioMixer.SetFloat("SFXVolume", _volume);
            }
            
            public void SetMusicVolume(float _volume)
            {
                Debug.Log("Music _volume is "+_volume);
                audioMixer.SetFloat("MusicVolume", _volume);
            }

            public void OnBack(PlayerInput _playerInput)
            {
                PageController.instance.TurnPageOff(PageType.ConfigSettings, PageType.StartMenu);
                startPrompt.SetActive(false);
                startMenuButtons.SetActive(true);
                MultiplayerEventSystem _multiplayerEventSystem = _playerInput.transform.GetComponent<MultiplayerEventSystem>();
                _multiplayerEventSystem.SetSelectedGameObject(startAMatchButton);
            }


        }
        
    }
}
