
using System;
using System.Collections.Generic;
using TMPro;
using UnityCore.Audio;
using UnityCore.Data;
using UnityCore.Game;
using UnityCore.Menu;
using UnityEngine;
using UnityEngine.InputSystem;
using WildHammers.GameplayObjects;
using WildHammers.Match;
using WildHammers.Player;
using AudioType = UnityCore.Audio.AudioType;


namespace WildHammers
{
    namespace Round
    {
        
        public class GameRoundController : MonoBehaviour
        {
            public static GameRoundController instance;
            
            public static readonly string DRAW = "Draw";

            public MatchInfo matchInfo;
            public string winningTeam = "";

            private GoalType m_WinningSide;
            private List<PlayerInput> m_PlayerInputs;
            private float m_RoundTimer;
            private TMP_Text m_RoundTimerUI;
            private bool m_IsCountingDown;
            private float m_CountdownTimer;

            [SerializeField] private TMP_Text m_CountdownText;
            [SerializeField] private Transform[] hammerStartingPositions;
            [SerializeField] private GameObject firstSelectedInVictoryMenu;

            public bool isRoundOver;


            #region Unity Functions

                private void Awake()
                {
                    if (!instance)
                    {
                        instance = this;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }

                    isRoundOver = false;
                    matchInfo = MatchController.instance.PackageMatchInfo();
                    m_PlayerInputs = PlayerJoinController.instance.playerList;
                    m_RoundTimerUI = MatchController.instance.roundTimer;
                    m_RoundTimer = matchInfo.roundTimeLength;

                    firstSelectedInVictoryMenu = PageController.instance.pages[1].transform.GetChild(2).gameObject;
                    GameController.instance.SwitchAllPlayersActionMaps();
                    SetHammerPositions();
                    ActivateHammers();
                    PageController.instance.TurnPageOn(PageType.ScoreBoard);
                }

                private void Start()
                {
                    AudioController.instance.PlayAudio(GameController.instance.matchMusic,0.5f);
                    AudioController.instance.PlayAudio(AudioType.SFX_08);
                }

                private void OnEnable()
                {
                    //Do the match countdown
                    m_IsCountingDown = true;
                    m_CountdownTimer = 5f;
                    m_CountdownText.gameObject.SetActive(true);
                }

                private void Update()
                {
                    if (!GameController.instance.isGamePaused && !m_IsCountingDown)
                    {
                        if (!isRoundOver)
                        {
                            m_RoundTimer -= Time.deltaTime;
                            int _RoundTimerInt = (int) m_RoundTimer;
                            if (_RoundTimerInt < 11 && _RoundTimerInt < Int32.Parse(m_RoundTimerUI.text))
                            {
                                AudioController.instance.PlayAudio(AudioType.SFX_11);
                            }
                            m_RoundTimerUI.text = _RoundTimerInt.ToString();
                        }
                        if (m_RoundTimer <= 0 && isRoundOver == false)
                        {
                            EndRound();
                        } 
                        else if ((ScoreController.instance.westScore >= matchInfo.winningScore 
                                    || ScoreController.instance.eastScore >= matchInfo.winningScore) 
                                   && !isRoundOver)
                        {
                            EndRound();
                        }
                        
                    }

                    if (m_IsCountingDown)
                    {
                        m_CountdownTimer -= Time.deltaTime;
                        if((int) m_CountdownTimer <  Int32.Parse(m_CountdownText.text)) AudioController.instance.PlayAudio(AudioType.SFX_11);
                        string _countDownString = ((int) m_CountdownTimer).ToString();
                        m_CountdownText.text = _countDownString;
                        if (m_CountdownTimer <= 0f)
                        {
                            m_IsCountingDown = false;
                            m_CountdownText.gameObject.SetActive(false);
                        }

                    }
                }


                private void OnDisable()
                {
                    CleanUpRound();
                }
                
            #endregion

            #region Public Functions
            


            #endregion


            #region Private Functions

            private void SetHammerPositions()
            {
                //TODO clean this up with for loops
                GameObject _teamWestPlayer1Hammer = matchInfo.teamWest.teamRoster[0].transform.GetChild(0).gameObject;
                _teamWestPlayer1Hammer.transform.position = hammerStartingPositions[0].position;
                _teamWestPlayer1Hammer.GetComponent<HammerController>().ResetChildrenPositionAndRotation();
                GameObject _teamEastPlayer1Hammer = matchInfo.teamEast.teamRoster[0].transform.GetChild(0).gameObject;
                _teamEastPlayer1Hammer.transform.position = hammerStartingPositions[1].position;
                _teamEastPlayer1Hammer.GetComponent<HammerController>().ResetChildrenPositionAndRotation();
                if (PlayerJoinController.instance.maxPlayerCount == 4)
                {
                    GameObject _teamWestPlayer2Hammer = matchInfo.teamWest.teamRoster[1].transform.GetChild(0).gameObject;
                    _teamWestPlayer2Hammer.transform.position = hammerStartingPositions[2].position;
                    _teamWestPlayer2Hammer.GetComponent<HammerController>().ResetChildrenPositionAndRotation();
                    GameObject _teamEastPlayer2Hammer = matchInfo.teamEast.teamRoster[1].transform.GetChild(0).gameObject;
                    _teamEastPlayer2Hammer.transform.position = hammerStartingPositions[3].position;
                    _teamEastPlayer2Hammer.GetComponent<HammerController>().ResetChildrenPositionAndRotation();
                }
            }

            private void ActivateHammers()
            {
                foreach (PlayerInput _playerInput in m_PlayerInputs)
                {
                    GameObject _playerHammer = _playerInput.transform.GetComponentInChildren<HammerController>(true).gameObject;
                    PlayerInfo _playerInfo = _playerInput.transform.GetComponent<PlayerInfo>();
                    SpriteRenderer[] _hammerSprites = _playerHammer.GetComponentsInChildren<SpriteRenderer>();
                    foreach (SpriteRenderer _spriteRenderer in _hammerSprites)
                    {
                        _spriteRenderer.color = _playerInfo.hammerColor;
                    }
                    _playerHammer.GetComponent<HammerController>().headTMPText.text = _playerInfo.playerInitials;
                    _playerHammer.SetActive(true);
                }
            }

            private void CleanUpRound()
            {
                foreach (PlayerInput _playerInput in m_PlayerInputs)
                {
                    GameObject _playerHammer = _playerInput.transform.GetComponentInChildren<HammerController>(true).transform.gameObject;
                    _playerHammer.SetActive(false);
                }
                PageController.instance.TurnPageOff(PageType.ScoreBoard);
            }
            
            private void EndRound()
            {
                isRoundOver = true;
                if (ScoreController.instance.westScore > ScoreController.instance.eastScore)
                {
                    winningTeam = matchInfo.teamWest.teamInfo.city + " " + matchInfo.teamWest.teamInfo.name;
                    m_WinningSide = GoalType.WEST;
                }
                else if (ScoreController.instance.westScore < ScoreController.instance.eastScore)
                {
                    winningTeam = matchInfo.teamEast.teamInfo.city + " " + matchInfo.teamEast.teamInfo.name;
                    m_WinningSide = GoalType.EAST;
                }
                else
                {
                    winningTeam = DRAW;
                    m_WinningSide = GoalType.None;
                }
                AddStatsToRecords();
                AudioController.instance.PlayAudio(AudioType.SFX_04);
                AudioController.instance.PlayAudio(AudioType.ST_03);
                PageController.instance.TurnPageOn(PageType.Victory);
            }

            private void Log(string _msg)
            {
                Debug.Log("[GameRoundController]: "+_msg);
            }

            private void LogWarning(string _msg)
            {
                Debug.LogWarning("[GameRoundController]: " + _msg);
            }

            private void AddStatsToRecords()
            {
                TallyGoalStats();
                TallyWinsAndLosses();
            }

            private void TallyGoalStats()
            {
                foreach (var _entry in ScoreController.instance.playerToGoalsScoredTable)
                {
                    DataController.instance.IncrementPlayerTotalGoals(_entry.Key, _entry.Value);
                }

                foreach (var _entry in ScoreController.instance.safetyGoalsScoredTable)
                {
                    DataController.instance.IncrementPlayerGoalsScoredOnSelf(_entry.Key, _entry.Value);
                }

                foreach (var _entry in ScoreController.instance.goalsForTeamScored)
                {
                    DataController.instance.IncrementPlayerGoalsScoredForTeam(_entry.Key, _entry.Value);
                }
            }

            private void TallyWinsAndLosses()
            {
                if (m_WinningSide == GoalType.WEST)
                {
                    foreach (PlayerInfo _playerInfo in matchInfo.teamWest.teamRoster)
                    {
                        DataController.instance.IncrementPlayerMatchesWon(_playerInfo.GetID());
                    }

                    foreach (PlayerInfo _playerInfo in matchInfo.teamEast.teamRoster)
                    {
                        DataController.instance.IncrementPlayerMatchesLost(_playerInfo.GetID());
                    }
                }
                else if (m_WinningSide == GoalType.EAST)
                {
                    foreach (PlayerInfo _playerInfo in matchInfo.teamEast.teamRoster)
                    {
                        DataController.instance.IncrementPlayerMatchesWon(_playerInfo.GetID());
                    }
                    foreach (PlayerInfo _playerInfo in matchInfo.teamWest.teamRoster)
                    {
                        DataController.instance.IncrementPlayerMatchesLost(_playerInfo.GetID());
                    }
                }
                else
                {
                    foreach (PlayerInfo _playerInfo in matchInfo.teamEast.teamRoster)
                    {
                        DataController.instance.IncrementPlayerMatchesDrawn(_playerInfo.GetID());
                    }
                    foreach (PlayerInfo _playerInfo in matchInfo.teamWest.teamRoster)
                    {
                        DataController.instance.IncrementPlayerMatchesDrawn(_playerInfo.GetID());
                    }
                }
            }

            #endregion
        }
    }
}
