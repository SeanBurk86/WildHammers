
using TMPro;
using UnityCore.Audio;
using UnityCore.Menu;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using WildHammers.GameplayObjects;
using WildHammers.Player;
using AudioType = UnityCore.Audio.AudioType;

namespace WildHammers
{
    namespace Round
    {
        public class ScoreController : MonoBehaviour
        {
            public static ScoreController instance;

            public int westScore, eastScore, winningScore;

            [SerializeField] private TMP_Text m_WestScoreText, m_EastScoreText;
            
            [SerializeField] private GameObject firstSelectedInVictoryMenu;

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

            private void Update()
            {
                m_WestScoreText.text = westScore.ToString();
                m_EastScoreText.text = eastScore.ToString();
            }

            #endregion

            #region Public Functions

            public void IncrementScore(int _points, GoalType _goal)
            {
                if (_goal == GoalType.WEST)
                {
                    westScore += _points;
                } 
                else if (_goal == GoalType.EAST)
                {
                    eastScore += _points;
                }

                if ((westScore >= winningScore || eastScore >= winningScore) && !GameRoundController.instance.isRoundOver)
                {
                    GameRoundController.instance.isRoundOver = true;
                    AudioController.instance.PlayAudio(AudioType.SFX_05);
                    PageController.instance.TurnPageOn(PageType.Victory);
                    SelectIntoVictoryPanel();
                }
            }

            #endregion

            #region Private Functions

            private void Configure()
            {
                instance = this;
                eastScore = 0;
                westScore = 0;
                firstSelectedInVictoryMenu = PageController.instance.pages[1].transform.GetChild(2).gameObject;
            }
            
            private void SelectIntoVictoryPanel()
            {
                foreach (PlayerInput _playerInput in PlayerJoinController.instance.playerList)
                {
                    _playerInput.SwitchCurrentActionMap("UI");
                    MultiplayerEventSystem _eventSystem = _playerInput.transform.GetComponent<MultiplayerEventSystem>();
                    _eventSystem.SetSelectedGameObject(firstSelectedInVictoryMenu);
                }
            }

            private void Log(string _msg)
            {
                Debug.Log("[ScoreController]: "+_msg);
            }
            
            private void LogWarning(string _msg)
            {
                Debug.LogWarning("[ScoreController]: "+_msg);
            }

            #endregion
        }
        
    }
}
