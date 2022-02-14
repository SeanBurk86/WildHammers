
using UnityCore.Audio;
using UnityCore.Data;
using UnityEngine;
using WildHammers.Round;
using AudioType = UnityCore.Audio.AudioType;

namespace WildHammers
{
    namespace GameplayObjects
    {
        public class Goal : MonoBehaviour
        {
            public GoalType goalType;
            private void OnTriggerEnter2D(Collider2D _other)
            {
                if (GameRoundController.instance != null)
                {
                    if (_other.gameObject.CompareTag("GameBall"))
                    {
                        //Deactivate ball
                        _other.gameObject.SetActive(false);
                        //Return to ball pool
                        GameBall _gameBall = _other.gameObject.GetComponent<GameBall>();
                        GameBallPool.instance.Return(_gameBall);
                        //Increment add points to team score
                        ScoreController.instance.IncrementScore(1,goalType,_gameBall.lastPlayerTouched);
                        AudioController.instance.PlayAudio(AudioType.SFX_05);
                        
                    }
                }
            }
        }
        
    }
}

