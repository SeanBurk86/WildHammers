using System;
using System.Collections;
using System.Collections.Generic;
using UnityCore.Menu;
using UnityEngine;
using UnityEngine.Pool;


namespace WildHammers
{
    namespace GameplayObjects
    {
        public class BallSparksSpawner : MonoBehaviour
        {
            public static BallSparksSpawner instance = null;
            
            [SerializeField] private ParticleSystem m_BallSparks;
            private ObjectPool<ParticleSystem> m_ParticlePool;

            #region Unity Functions

            private void Awake()
            {
                if (!instance)
                {
                    instance = this;
                    ConfigurePool();
                }
                else
                {
                    Destroy(this);
                }
            }
            

            #endregion

            #region Public Functions

            public void Spawn(Vector3 _pos)
            {
                ParticleSystem _sparks =  m_ParticlePool.Get();
                _sparks.transform.position = _pos;
            }

            #endregion

            #region Private Functions

            private void ConfigurePool()
            {
                m_ParticlePool = new ObjectPool<ParticleSystem>(() => { return Instantiate(m_BallSparks); },
                    _sparks => { _sparks.gameObject.SetActive(true); }, _sparks => { _sparks.gameObject.SetActive(false); },
                    _sparks => { Destroy(_sparks.gameObject); }, false, 10, 20);
            }
            

            #endregion

        }
        
    }
}
