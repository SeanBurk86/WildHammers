
using UnityCore.Data;
using UnityCore.Menu;
using UnityCore.Session;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using WildHammers.Match;
using WildHammers.Player;
using AudioType = UnityCore.Audio.AudioType;

namespace UnityCore
{
    namespace Game
    {
        public class GameController : MonoBehaviour
        {
            public static GameController instance;
            
            public AudioMixer audioMixer;
            
            public bool isGamePaused, isInitialInputReceived, isHeadingToStart;

            public AudioType matchMusic;
            public AudioType victoryMusic;
            
            #region Unity Functions

            private void Awake()
            {
                if (!instance)
                {
                    Configure();
                }
                else
                {
                    Destroy(gameObject);
                }
                isInitialInputReceived = false;
                isHeadingToStart = true;
            }

            private void Start()
            {
                SessionController.instance.InitializeGame(instance);
                SetVolumeSettings();
            }

            #endregion

            #region Public Functions
            
            public void OnInit()
            {
                isGamePaused = false;
            }
            
            public void HandlePauseInput()
            {
                if (MatchController.instance.hasMatchStarted)
                {
                    if (!isGamePaused && !PageController.instance.PageIsOn(PageType.Victory))
                    {
                        PauseGame();
                    }
                    else
                    {
                        UnpauseGame();
                    }
                    
                }
            }
            
            public void SwitchAllPlayersActionMaps()
            {
                foreach (PlayerInput _playerInput in PlayerJoinController.instance.playerList)
                {
                    _playerInput.SwitchCurrentActionMap("Player");
                }
            }

            #endregion

            #region Private Functions

            private void Configure()
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            
            private void SetVolumeSettings()
            { 
                audioMixer.SetFloat("Volume", DataController.instance.MasterVolume);
                audioMixer.SetFloat("MusicVolume", DataController.instance.MusicVolume);
                audioMixer.SetFloat("SFXVolume",  DataController.instance.SfxVolume);
            }
            
            private void PauseGame()
            {
                Time.timeScale = 0;
                PageController.instance.TurnPageOn(PageType.PauseMenu);
                isGamePaused = true;
            }

            private void UnpauseGame()
            {
                PageController.instance.TurnPageOff(PageType.PauseMenu);
                Time.timeScale = 1;
                isGamePaused = false;
                if(MatchController.instance.hasMatchStarted) SwitchAllPlayersActionMaps();
            }

            #endregion
        }
        
    }
}
