
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using WildHammers.Player;
using WildHammers.ScreenFlow;

namespace WildHammers
{
    namespace UI
    {
        public class PlayerJoinUIManager : MonoBehaviour
        {

            public static PlayerJoinUIManager instance = null;
            
            public GameObject playerJoinMenu;
            public GameObject confirmPlayersPanel, confirmPlayersButton;


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

                private void OnEnable()
                {
                    foreach (PlayerInput _playerInput in PlayerJoinController.instance.playerList)
                    {
                        if(_playerInput.inputIsActive) SetupJoinPlayerInputPanel(_playerInput);
                    }
                }

                private void Update()
                {
                    if (PlayerJoinController.instance.areAllPlayersEntered && !confirmPlayersPanel.activeInHierarchy)
                    {
                        confirmPlayersPanel.SetActive(true);
                        PlayerJoinController.instance.AllSelectUIElement(confirmPlayersButton);
                    }
                }

                private void OnDestroy()
                {
                    PlayerJoinController.instance.PlayerJoinedGame -= PlayerJoinedGame;
                }
            

            #endregion

            #region Public Functions

            public void AcceptPlayers()
            {
                confirmPlayersPanel.SetActive(false);
                ScreenFlowController.instance.Flow(ScreenPoseType.TeamSelect, false);
            }

            public void ResetPlayers()
            {
                confirmPlayersPanel.SetActive(false);
                PlayerJoinController.instance.ResetJoinPanel();
            }

            public void ChangePlayers()
            {
                confirmPlayersPanel.SetActive(false);
                PlayerJoinController.instance.ResetJoinPanel();
                foreach (PlayerInput _playerInput in PlayerJoinController.instance.playerList)
                {
                    if(_playerInput.inputIsActive) SetupJoinPlayerInputPanel(_playerInput);
                }
            }

            #endregion

            #region Private Functions

                private void Configure()
                {
                    instance = this;
                    PlayerJoinController.instance.PlayerJoinedGame += PlayerJoinedGame;
                }

                private void PlayerJoinedGame(PlayerInput _playerInput)
                {
                    SetupJoinPlayerInputPanel(_playerInput);
                }

                private void SetupJoinPlayerInputPanel(PlayerInput _playerInput)
                {
                    playerJoinMenu.transform.GetChild(_playerInput.playerIndex).gameObject.SetActive(false);
                    MultiplayerEventSystem _eventSystem = _playerInput.transform.GetComponent<MultiplayerEventSystem>();
                    float _joinPanelwidth = .25f;
                    if (_playerInput.transform.GetComponentInChildren<JoinPanelUI>() != null)
                    {
                        InitialJoinPanelSetup(_playerInput, _eventSystem, _joinPanelwidth);
                    }
                    else
                    {
                        ReturningJoinPanelSetup(_playerInput, _eventSystem);
                    }
                }

                private void ReturningJoinPanelSetup(PlayerInput _playerInput, MultiplayerEventSystem _eventSystem)
                {
                    GameObject _joinPlayerUIGameObject = transform.GetChild(_playerInput.playerIndex + 7).gameObject;
                    _joinPlayerUIGameObject.SetActive(true);
                    JoinPanelUI _joinPanelUI = _joinPlayerUIGameObject.GetComponent<JoinPanelUI>();
                    _joinPanelUI.ClearPlayerInfo();
                    _joinPlayerUIGameObject.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
                    _joinPlayerUIGameObject.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);

                    _eventSystem.SetSelectedGameObject(null);
                    _eventSystem.SetSelectedGameObject(_joinPanelUI.firstSelectedInitialsButton);
                    
                }

                private void InitialJoinPanelSetup(PlayerInput _playerInput, MultiplayerEventSystem _eventSystem, float _joinPanelwidth)
                {
                    Transform _joinPlayerUI = _playerInput.transform.GetComponentInChildren<JoinPanelUI>().transform;
                    _joinPlayerUI.SetParent(playerJoinMenu.transform, false);
                    if (_playerInput.playerIndex > 0)
                    {
                        float _panelXOffest = _playerInput.playerIndex * _joinPanelwidth;
                        _joinPlayerUI.GetComponent<RectTransform>().anchorMin = new Vector2(_panelXOffest, 0);
                        _joinPlayerUI.GetComponent<RectTransform>().anchorMax = new Vector2(_panelXOffest + _joinPanelwidth, 1);
                    }

                    _eventSystem.SetSelectedGameObject(null);
                    _eventSystem.SetSelectedGameObject(_joinPlayerUI.GetComponent<JoinPanelUI>().firstSelectedInitialsButton);
                }

                #endregion
            
        }
        
    }
}

