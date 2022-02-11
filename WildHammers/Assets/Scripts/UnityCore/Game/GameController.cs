
using System;
using UnityCore.Session;
using UnityEngine;
using WildHammers.Match;
using WildHammers.Team;

namespace UnityCore
{
    namespace Game
    {
        public class GameController : MonoBehaviour
        {
            public static GameController instance;
            
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
                SessionController.instance.InitializeGame(instance);
            }
            
            

            #endregion
        }
        
    }
}
