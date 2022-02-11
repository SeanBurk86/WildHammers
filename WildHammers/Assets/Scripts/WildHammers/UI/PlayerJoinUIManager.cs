
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using WildHammers.Player;

namespace WildHammers
{
    namespace UI
    {
        public class PlayerJoinUIManager : MonoBehaviour
        {

            public static PlayerJoinUIManager instance = null;
            
            public GameObject playerJoinMenu;


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

                private void Start()
                {
                    PlayerJoinController.instance.PlayerJoinedGame += PlayerJoinedGame;
                }

                private void OnEnable()
                {
                    foreach (PlayerInput _playerInput in PlayerJoinController.instance.playerList)
                    {
                        SetupJoinPlayerInputPanel(_playerInput);
                    }
                }

                private void OnDisable()
                {
                    PlayerJoinController.instance.PlayerJoinedGame -= PlayerJoinedGame;
                }
            

            #endregion

            #region Private Functions

                private void Configure()
                {
                    instance = this;
                }

                private void PlayerJoinedGame(PlayerInput _playerInput)
                {
                    SetupJoinPlayerInputPanel(_playerInput);
                }

                private void SetupJoinPlayerInputPanel(PlayerInput _playerInput)
                {
                    playerJoinMenu.transform.GetChild(_playerInput.playerIndex).gameObject.SetActive(false);
                    // This is a little ugly but the idea is
                    // the JoinPanelUI will be a child to the Player object the first time through
                    // and a child of the Player Join Page afterwards
                    if (_playerInput.transform.GetComponentInChildren<JoinPanelUI>() != null)
                    {
                        Transform _joinPlayerUI = _playerInput.transform.GetChild(0);
                        _joinPlayerUI.SetParent(playerJoinMenu.transform, false);
                        if (_playerInput.playerIndex > 0)
                        {
                            float _panelXOffest = _playerInput.playerIndex * .25f;
                            _joinPlayerUI.GetComponent<RectTransform>().anchorMin = new Vector2(_panelXOffest, 0);
                            _joinPlayerUI.GetComponent<RectTransform>().anchorMax = new Vector2(_panelXOffest + .25f, 1);
                        }
                        _playerInput.transform.GetComponent<MultiplayerEventSystem>()
                            .SetSelectedGameObject(_joinPlayerUI.GetComponent<JoinPanelUI>().firstSelectedInitialsButton); 
                    }
                    else
                    {
                        GameObject _joinPlayerUIGameObject = transform.GetChild(_playerInput.playerIndex + 4).gameObject;
                        _joinPlayerUIGameObject.SetActive(true);
                        JoinPanelUI _joinPanelUI = _joinPlayerUIGameObject.GetComponent<JoinPanelUI>();
                        _joinPanelUI.playerNameInput = "";
                        _joinPlayerUIGameObject.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
                        _joinPlayerUIGameObject.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                        _playerInput.transform.GetComponent<MultiplayerEventSystem>()
                            .SetSelectedGameObject(_joinPanelUI.firstSelectedInitialsButton);
                    }
                }
            

            #endregion
            
        }
        
    }
}

