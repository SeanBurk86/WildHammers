
using System;
using System.Collections.Generic;
using TMPro;
using UnityCore.Audio;
using UnityCore.Menu;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
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

            public MatchInfo matchInfo;


            private List<PlayerInput> m_PlayerInputs;
            private float m_RoundTimer;
            private TMP_Text m_RoundTimerUI;

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
                    SwitchAllPlayersActionMaps();
                    SetHammerPositions();
                    ActivateHammers();
                    PageController.instance.TurnPageOn(PageType.ScoreBoard);
                }

                private void Start()
                {
                    AudioController.instance.PlayAudio(AudioType.ST_02,0.5f);
                    AudioController.instance.PlayAudio(AudioType.SFX_08);
                }

                private void Update()
                {
                    if (!isRoundOver)
                    {
                        m_RoundTimer -= Time.deltaTime;
                        m_RoundTimerUI.text = ((int) m_RoundTimer).ToString();
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

                


                private void OnDisable()
                {
                    CleanUpRound();
                }

                #endregion


            #region Private Functions

            private void SwitchAllPlayersActionMaps()
                {
                    foreach (PlayerInput _playerInput in m_PlayerInputs)
                    {
                        _playerInput.SwitchCurrentActionMap("Player");
                    }
                }

                private void SetHammerPositions()
                {
                    //TODO clean this up with for loops
                    GameObject _teamWestPlayer1Hammer = matchInfo.teamWest.teamRoster[0].transform.GetChild(0).gameObject;
                    GameObject _teamWestPlayer2Hammer = matchInfo.teamWest.teamRoster[1].transform.GetChild(0).gameObject;
                    GameObject _teamEastPlayer1Hammer = matchInfo.teamEast.teamRoster[0].transform.GetChild(0).gameObject;
                    GameObject _teamEastPlayer2Hammer = matchInfo.teamEast.teamRoster[1].transform.GetChild(0).gameObject;
                    _teamWestPlayer1Hammer.transform.position = hammerStartingPositions[0].position;
                    _teamWestPlayer1Hammer.GetComponent<HammerController>().ResetChildrenPositionAndRotation();
                    _teamWestPlayer2Hammer.transform.position = hammerStartingPositions[2].position;
                    _teamWestPlayer2Hammer.GetComponent<HammerController>().ResetChildrenPositionAndRotation();
                    _teamEastPlayer1Hammer.transform.position = hammerStartingPositions[1].position;
                    _teamEastPlayer1Hammer.GetComponent<HammerController>().ResetChildrenPositionAndRotation();
                    _teamEastPlayer2Hammer.transform.position = hammerStartingPositions[3].position;
                    _teamEastPlayer2Hammer.GetComponent<HammerController>().ResetChildrenPositionAndRotation();
                }

                private void ActivateHammers()
                {
                    foreach (PlayerInput _playerInput in m_PlayerInputs)
                    {
                        GameObject _playerHammer = _playerInput.transform.GetChild(0).gameObject;
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
                        GameObject _playerHammer = _playerInput.transform.GetChild(0).gameObject;
                        _playerHammer.SetActive(false);
                    }
                    PageController.instance.TurnOffAllPages();
                }
                
                private void EndRound()
                {
                    isRoundOver = true;
                    AudioController.instance.PlayAudio(AudioType.SFX_04);
                    AudioController.instance.PlayAudio(AudioType.ST_03);
                    PageController.instance.TurnPageOn(PageType.Victory);
                    SelectIntoVictoryPanel();
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
                    Debug.Log("[GameRoundController]: "+_msg);
                }

                private void LogWarning(string _msg)
                {
                    Debug.LogWarning("[GameRoundController]: " + _msg);
                }


                #endregion
        }
    }
}
