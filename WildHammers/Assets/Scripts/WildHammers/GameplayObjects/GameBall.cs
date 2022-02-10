
using System;
using UnityCore.Audio;
using UnityEngine;
using AudioType = UnityCore.Audio.AudioType;

namespace WildHammers
{
    namespace GameplayObjects
    {
        public class GameBall : MonoBehaviour
        {

            private Rigidbody2D m_RigidBody;
            [SerializeField] private float m_GhostSpeed = 5f;

            private void Awake()
            {
                m_RigidBody = GetComponent<Rigidbody2D>();
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

            private void Update()
            {
                // if the ball reaches a certain velocity
                if (m_RigidBody.velocity.x >= m_GhostSpeed || m_RigidBody.velocity.y >= m_GhostSpeed)
                {
                    // turn on the ghost after image sprite
                    BallGhostSpritePool.instance.Spawn(this);
                }
            }
        }
        
    }
}

