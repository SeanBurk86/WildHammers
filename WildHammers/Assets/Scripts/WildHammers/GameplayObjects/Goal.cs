
using TMPro;
using UnityCore.Audio;
using UnityEngine;
using UnityEngine.UI;
using WildHammers.Round;
using WildHammers.Team;
using AudioType = UnityCore.Audio.AudioType;

namespace WildHammers
{
    namespace GameplayObjects
    {
        public class Goal : MonoBehaviour
        {
            public GoalType goalType;

            private Sprite m_TeamLivery;
            [SerializeField] private TMP_Text m_TeamName;
            [SerializeField] private Transform m_ImageTransform;
            private TeamController.MatchTeam m_MatchTeam;


            private void Start()
            {
                if (goalType == GoalType.WEST)
                {
                    m_MatchTeam = GameRoundController.instance.matchInfo.teamEast;
                }
                else if (goalType == GoalType.EAST)
                {
                    m_MatchTeam = GameRoundController.instance.matchInfo.teamWest;
                }

                if (m_MatchTeam != null)
                {
                    m_TeamLivery = m_MatchTeam.teamInfo.livery;
                    m_TeamName.text = m_MatchTeam.teamInfo.name.ToString();
                }

                m_ImageTransform.GetComponent<Image>().sprite = m_TeamLivery;
            }
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

