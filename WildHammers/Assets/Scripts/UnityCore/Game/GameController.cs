
using UnityCore.Data;
using UnityCore.Session;
using UnityEngine;
using UnityEngine.Audio;

namespace UnityCore
{
    namespace Game
    {
        public class GameController : MonoBehaviour
        {
            public static GameController instance;
            
            public AudioMixer audioMixer;
            
            public bool isGamePaused;
            
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

            #endregion
        }
        
    }
}
