
using System;
using TMPro;
using UnityEngine;
using WildHammers.Match;

namespace WildHammers
{
    namespace UI
    {
        public class MatchSettingsPanel : MonoBehaviour
        {
            public MatchInfo matchSettings;

            [SerializeField] private TMP_Text m_NumberOfBallsInput, m_RoundTimeLimit, m_WinningScoreInput;

            public void AcceptSettings()
            {
                matchSettings = new MatchInfo(null,null,
                    AcceptNumberOfBallsInPlay(m_NumberOfBallsInput.text),
                    AcceptWinningScore(m_WinningScoreInput.text),
                    AcceptRoundTimeLimit(m_RoundTimeLimit.text));
            }

            private float AcceptRoundTimeLimit(string _timeLimit)
            {
                return float.Parse(_timeLimit);
            }

            private int AcceptNumberOfBallsInPlay(string _numberOfBalls)
            {
                return Int32.Parse(_numberOfBalls);
            }

            private int AcceptWinningScore(string _winningScore)
            {
                return Int32.Parse(_winningScore);
            }
        }
        
    }
}

