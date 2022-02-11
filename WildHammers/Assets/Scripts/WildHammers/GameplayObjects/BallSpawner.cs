using System;
using System.Collections;
using UnityCore.Audio;
using UnityCore.Game;
using UnityEngine;
using WildHammers.GameplayObjects;
using WildHammers.Round;
using AudioType = UnityCore.Audio.AudioType;

namespace WildHammers
{
    namespace GameplayObject
    {
        public class BallSpawner : MonoBehaviour
        {
            public static BallSpawner instance;

            [SerializeField] private float m_SpawnDelay = 3f;
            [SerializeField] private float m_DelayBetweenSpawns = 3f;
            [SerializeField] private float m_SpawnPushForce = 300f;
            [SerializeField] private int m_RequiredNumberOfBalls;
            private int m_BallsInPlay;
            private int m_BallsWaitingToSpawn;
            private float m_DelayBetweenSpawnsTimer = 0f;

            private void Awake()
            {
                if (instance == null)
                {
                    instance = this;
                } 
                else if (instance != null)
                {
                    Destroy(gameObject);
                }

                
            }

            private void Start()
            {
                m_RequiredNumberOfBalls = GameRoundController.instance.matchInfo.numberOfBalls;
                
                Debug.Log("ReqnumBalls: "+m_RequiredNumberOfBalls);
            }

            private void Update()
            {
                if (!GameController.instance.isGamePaused)
                {
                    m_DelayBetweenSpawnsTimer += Time.deltaTime;
                    
                    // update the balls in play reflecting the number of active child objects
                    int _numberOfActiveChildren = 0;
                    foreach (Transform _childTransform in gameObject.transform)
                    {
                        if (_childTransform.gameObject.activeInHierarchy)
                        {
                            _numberOfActiveChildren++;
                        }
                    }

                    m_BallsInPlay = _numberOfActiveChildren;
                    
                    //check if balls in play + balls waiting to spawn is less than the match setting
                    if (((m_BallsInPlay + m_BallsWaitingToSpawn) < m_RequiredNumberOfBalls)
                        && (m_DelayBetweenSpawnsTimer>=m_DelayBetweenSpawns))
                    {
                        //if we're short "spawn" a ball
                        SpawnBall();
                        m_DelayBetweenSpawnsTimer = 0f;
                    }
                        
                }
            }

            public void SpawnBall()
            {
                // increment the amount of balls waiting to spawn
                m_BallsWaitingToSpawn++;
                StartCoroutine(SpawnBallDelay());
            }

            IEnumerator SpawnBallDelay()
            {
                yield return new WaitForSeconds(m_SpawnDelay);
                m_BallsWaitingToSpawn--;
                var _gameBall = GameBallPool.instance.Get();
                _gameBall.gameObject.SetActive(true);
                _gameBall.transform.parent = transform;
                Rigidbody2D _gameBallRB = _gameBall.gameObject.GetComponent<Rigidbody2D>();
                _gameBallRB.AddForce(Vector2.up*m_SpawnPushForce,ForceMode2D.Impulse);
                AudioController.instance.PlayAudio(AudioType.SFX_07);
            }
            
        }
        
    }
}
