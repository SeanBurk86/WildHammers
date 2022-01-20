
using UnityCore.Audio;
using UnityEngine;
using AudioType = UnityCore.Audio.AudioType;

namespace WildHammers
{
    namespace GameplayObjects
    {
        public class GameBall : MonoBehaviour
        { 
            void OnCollisionEnter2D(Collision2D other)
            {
                if (other.transform.gameObject.CompareTag("ArenaWall"))
                {
                    AudioController.instance.PlayAudio(AudioType.SFX_02);
                }
            }
            
        }
        
    }
}

