
using System;
using System.Collections.Generic;
using TMPro;
using UnityCore.Audio;
using UnityCore.Menu;
using UnityEngine;
using UnityEngine.InputSystem;
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

            [SerializeField] private Transform[] hammerStartingPositions;
            [SerializeField] private TMP_Text m_WestTeamName, m_EastTeamName;
            [SerializeField] private Image m_WestTeamBanner, m_EastTeamBanner;

            public bool isRoundOver;


            #region Unity Functions

                private void Awake()
                {
                    if (!instance)
                    {
                        instance = this;
                        isRoundOver = false;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }

                    matchInfo = MatchController.instance.PackageMatchInfo();
                    m_PlayerInputs = PlayerJoinController.instance.playerList;
                    ConfigureRound();
                    SwitchAllPlayersActionMaps();
                    SetHammerPositions();
                    ActivateHammers();
                }

                private void Start()
                {
                    AudioController.instance.PlayAudio(AudioType.SFX_08);
                    AudioController.instance.PlayAudio(AudioType.ST_02,0.5f,2f);
                }


                private void OnDisable()
                {
                    CleanUpRound();
                }

                #endregion


            #region Private Functions

                private void ConfigureRound()
                {
                    m_WestTeamName.text = matchInfo.teamWest.teamInfo.city + " " + matchInfo.teamWest.teamInfo.name;
                    m_WestTeamBanner.sprite = matchInfo.teamWest.teamInfo.livery;
                    m_EastTeamName.text = matchInfo.teamEast.teamInfo.city + " " + matchInfo.teamEast.teamInfo.name;
                    m_EastTeamBanner.sprite = matchInfo.teamEast.teamInfo.livery;
                }

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
                
                private void Log(string _msg)
                {
                    Debug.Log("[GameRoundController]: "+_msg);
                }
                    
                private void LogWarning(string _msg)
                {
                    Debug.LogWarning("[GameRoundController]: "+_msg);
                }
                

            #endregion
        }
    }
}
