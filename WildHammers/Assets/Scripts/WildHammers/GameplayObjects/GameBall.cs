
using System;
using UnityCore.Audio;
using UnityCore.Game;
using UnityEngine;
using AudioType = UnityCore.Audio.AudioType;

namespace WildHammers
{
    namespace GameplayObjects
    {
        public class GameBall : MonoBehaviour
        {

            private Rigidbody2D m_RigidBody;
            private bool m_IsInGhostMode;

            private void Awake()
            {
                m_RigidBody = GetComponent<Rigidbody2D>();
                m_IsInGhostMode = false;
            }

            void OnCollisionEnter2D(Collision2D other)
            {
                if (other.transform.gameObject.CompareTag("ArenaWall"))
                {
                    AudioController.instance.PlayAudio(AudioType.SFX_01);
                }
                else if (other.transform.gameObject.CompareTag("GameBall"))
                {
                    AudioController.instance.PlayAudio(AudioType.SFX_03);
                }
            }

            public void ActivateTrail()
            {
                m_IsInGhostMode = true;
            }

            private void Update()
            {
                if (!GameController.instance.isGamePaused)
                {
                    if (Mathf.Abs(m_RigidBody.velocity.x) <= 5f 
                             && Mathf.Abs(m_RigidBody.velocity.y) <= 5f ) 
                        m_IsInGhostMode = false;
                    
                    if (m_IsInGhostMode) BallGhostSpritePool.instance.Spawn(this);
                }
            }
        }
        
    }
}

