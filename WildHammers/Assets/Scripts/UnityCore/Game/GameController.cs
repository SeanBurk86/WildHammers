
using UnityCore.Data;
using UnityCore.Session;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using WildHammers.Match;
using WildHammers.Player;
using WildHammers.ScreenFlow;
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

            private ScreenPoseType m_PosePausedOn;
            
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
                    if (!isGamePaused && ScreenFlowController.instance.currentPoseType != ScreenPoseType.Victory)
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
                m_PosePausedOn = ScreenFlowController.instance.currentPoseType;
                ScreenFlowController.instance.Flow(ScreenPoseType.Pause, false);
                isGamePaused = true;
            }

            private void UnpauseGame()
            {
                ScreenFlowController.instance.Flow(m_PosePausedOn, false);
                Time.timeScale = 1;
                isGamePaused = false;
                if(MatchController.instance.hasMatchStarted) SwitchAllPlayersActionMaps();
            }

            #endregion
        }
        
    }
}
