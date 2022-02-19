
using System.Collections.Generic;
using TMPro;
using UnityCore.Audio;
using UnityCore.Game;
using UnityCore.Menu;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using WildHammers.GameplayObjects;
using WildHammers.Match;
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

            public Dictionary<string, int> playerToGoalsScoredTable, safetyGoalsScoredTable, goalsForTeamScored;

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

                playerToGoalsScoredTable = new Dictionary<string, int>();
                safetyGoalsScoredTable = new Dictionary<string, int>();
                goalsForTeamScored = new Dictionary<string, int>();
            }

            private void Update()
            {
                if (!GameController.instance.isGamePaused)
                {
                    m_WestScoreText.text = westScore.ToString();
                    m_EastScoreText.text = eastScore.ToString();
                }
            }

            #endregion

            #region Public Functions

            public void IncrementScore(int _points, GoalType _goal, string _lastTouchedPlayer)
            {
                if (_goal == GoalType.WEST)
                {
                    westScore += _points;
                } 
                else if (_goal == GoalType.EAST)
                {
                    eastScore += _points;
                }
                
                if (_goal == GoalType.WEST)
                {
                    foreach (PlayerInfo _playerInfo in GameRoundController.instance.matchInfo.teamEast.teamRoster)
                    {
                        if (_lastTouchedPlayer == _playerInfo.GetID()) AddToSafetyGoalsTable(_lastTouchedPlayer);
                    }
                    foreach (PlayerInfo _playerInfo in GameRoundController.instance.matchInfo.teamWest.teamRoster)
                    {
                        if (_lastTouchedPlayer == _playerInfo.GetID()) AddToGoalsForTeamTable(_lastTouchedPlayer);
                    }
                }
                else
                {
                    foreach (PlayerInfo _playerInfo in GameRoundController.instance.matchInfo.teamWest.teamRoster)
                    {
                        if (_lastTouchedPlayer == _playerInfo.GetID()) AddToSafetyGoalsTable(_lastTouchedPlayer);
                    }
                    foreach (PlayerInfo _playerInfo in GameRoundController.instance.matchInfo.teamEast.teamRoster)
                    {
                        if (_lastTouchedPlayer == _playerInfo.GetID()) AddToGoalsForTeamTable(_lastTouchedPlayer);
                    }
                }
                
                AddToGoalsScoredTable(_lastTouchedPlayer);
            }

            #endregion

            #region Private Functions

            private void Configure()
            {
                instance = this;
                eastScore = 0;
                westScore = 0;
                m_WestScoreText = MatchController.instance.teamWestScoreBox;
                m_EastScoreText = MatchController.instance.teamEastScoreBox;
            }
            

            private void Log(string _msg)
            {
                Debug.Log("[ScoreController]: "+_msg);
            }
            
            private void LogWarning(string _msg)
            {
                Debug.LogWarning("[ScoreController]: "+_msg);
            }

            private void AddToGoalsScoredTable(string _lastTouchedPlayer)
            {
                if (playerToGoalsScoredTable.ContainsKey(_lastTouchedPlayer))
                {
                    playerToGoalsScoredTable[_lastTouchedPlayer] = playerToGoalsScoredTable[_lastTouchedPlayer] + 1;
                }
                else
                {
                    playerToGoalsScoredTable.Add(_lastTouchedPlayer, 1);
                }
            }
            
            private void AddToSafetyGoalsTable(string _lastTouchedPlayer)
            {
                if (safetyGoalsScoredTable.ContainsKey(_lastTouchedPlayer))
                {
                    safetyGoalsScoredTable[_lastTouchedPlayer] = safetyGoalsScoredTable[_lastTouchedPlayer] + 1;
                }
                else
                {
                    safetyGoalsScoredTable.Add(_lastTouchedPlayer, 1);
                }
            }
            
            private void AddToGoalsForTeamTable(string _lastTouchedPlayer)
            {
                if (goalsForTeamScored.ContainsKey(_lastTouchedPlayer))
                {
                    goalsForTeamScored[_lastTouchedPlayer] = goalsForTeamScored[_lastTouchedPlayer] + 1;
                }
                else
                {
                    goalsForTeamScored.Add(_lastTouchedPlayer, 1);
                }
            }

            #endregion
        }
        
    }
}
