
using System;
using UnityCore.Session;
using UnityEngine;

namespace UnityCore
{
    namespace Game
    {
        public class GameController : MonoBehaviour
        {
            #region Unity Functions

            private void Awake()
            {
                SessionController.instance.InitializeGame(this);
            }

            #endregion

            #region Public Functions
            
            public void OnInit()
            {
                throw new NotImplementedException();
            }

            #endregion

            #region Private Functions

            

            #endregion
        }
        
    }
}
