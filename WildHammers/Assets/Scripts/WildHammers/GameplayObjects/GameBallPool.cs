
using System.Collections.Generic;
using UnityEngine;

namespace WildHammers
{
    namespace GameplayObjects
    {
        public class GameBallPool : MonoBehaviour
        {
            public static GameBallPool instance;

            [SerializeField] private GameBall prefab;
            [SerializeField] private Transform spawnPoint;

            private Queue<GameBall> ballsAvailable = new Queue<GameBall>();

            #region Unity Functions

                private void Awake()
                {
                    if (!instance)
                    {
                        instance = this;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            

            #endregion

            #region Public Functions

                public GameBall Get()
                {
                    if (ballsAvailable.Count == 0)
                    {
                        return AddBall();
                    }
                    
                    return ballsAvailable.Dequeue();
                }

                public void Return(GameBall _gameBall)
                {
                    _gameBall.transform.position = spawnPoint.position;
                    ballsAvailable.Enqueue(_gameBall);
                }


            #endregion

            #region Private Functions

                private GameBall AddBall()
                {
                    var _ball = Instantiate(prefab,spawnPoint.position,Quaternion.Euler(spawnPoint.eulerAngles));
                    return _ball;
                }
            

            #endregion
            
            
        }
        
    }
}
