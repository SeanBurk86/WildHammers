
using System;
using System.Collections.Generic;
using UnityCore.Game;
using UnityCore.Menu;
using UnityEngine;
using UnityEngine.InputSystem;
using WildHammers.Match;
using WildHammers.Player;

namespace WildHammers
{
    namespace Round
    {
        public class PracticeController : MonoBehaviour
        {
            public static PracticeController instance;
            
            private List<PlayerInput> m_PlayerInputs;

            #region Unity Functions

            private void Awake()
            {
                if(!instance) Configure();
                else Destroy(gameObject);
            }

            private void Start()
            {
                MatchController.instance.hasMatchStarted = true;
                PageController.instance.TurnOffAllPages();
                m_PlayerInputs = PlayerJoinController.instance.playerList;
                GameController.instance.SwitchAllPlayersActionMaps();
                ActivateHammers();
            }

            private void OnDisable()
            {
                CleanUpRound();
            }

            #endregion

            #region Private Functions

            private void Configure()
            {
                instance = this;
            }
            
            private void ActivateHammers()
            {
                foreach (PlayerInput _playerInput in m_PlayerInputs)
                {
                    GameObject _playerHammer = _playerInput.gameObject.GetComponentInChildren<HammerController>(true).transform.gameObject;
                    PlayerInfo _playerInfo = _playerInput.transform.GetComponent<PlayerInfo>();
                    SpriteRenderer[] _hammerSprites = _playerHammer.GetComponentsInChildren<SpriteRenderer>();
                    foreach (SpriteRenderer _spriteRenderer in _hammerSprites)
                    {
                        _spriteRenderer.color = _playerInfo.hammerColor;
                    }
                    _playerHammer.GetComponent<HammerController>().headTMPText.text = "---";
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
            }

            #endregion
        }
    }
}
