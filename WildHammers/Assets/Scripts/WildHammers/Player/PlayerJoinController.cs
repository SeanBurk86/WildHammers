
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityCore.Game;
using UnityCore.Menu;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using WildHammers.Match;
using WildHammers.ScreenFlow;
using WildHammers.UI;

namespace WildHammers
{
    namespace Player
    {
        public class PlayerJoinController : MonoBehaviour
        {
            public bool debug;
            
            public static PlayerJoinController instance;
            
            public List<PlayerInput> playerList = new List<PlayerInput>();
            public List<MultiplayerEventSystem> multiplayerEventSystems = new List<MultiplayerEventSystem>();

            public bool areAllPlayersEntered;
            [SerializeField] private InputAction joinAction, leaveAction;

            public event System.Action<PlayerInput> PlayerJoinedGame;
            public event System.Action<PlayerInput> PlayerLeftGame;

            public int maxPlayerCount;

            private int readyCount;

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

                areAllPlayersEntered = false;
                maxPlayerCount = 2;
            }

            private void Update()
            {
                if (!GameController.instance.isGamePaused)
                {
                    if (!MatchController.instance.hasMatchStarted)
                    {
                        if (readyCount >= maxPlayerCount && !areAllPlayersEntered)
                        {
                            areAllPlayersEntered = true;
                        }
                    }
                        
                }
            }

            #endregion

            #region Public Functions

            public void IncrementReadyCount()
            {
                readyCount += 1;
            }

            public void DecrementReadyCount()
            {
                readyCount -= 1; 
            }

            public void ResetJoinPanel()
            {
                readyCount = 0;
                areAllPlayersEntered = false;
            }

            public void AllSelectUIElement(GameObject _element)
            {
                foreach (PlayerInput _playerInput in playerList)
                {
                    _playerInput.SwitchCurrentActionMap("UI");
                }

                foreach (MultiplayerEventSystem _eventSystem in multiplayerEventSystems)
                {
                    _eventSystem.SetSelectedGameObject(_element);
                }
            }

            public void SetMaxPlayers(int _maxPlayers)
            {
                maxPlayerCount = _maxPlayers;
                if (_maxPlayers == 4)
                {
                    ActivateLastTwoPlayers();
                    ScreenFlowController.instance.Flow(ScreenPoseType.FourPlayerJoin);
                }
                else
                {
                    DeactivateLastTwoPlayers();
                    ScreenFlowController.instance.Flow(ScreenPoseType.TwoPlayerJoin);
                }
            }


            #endregion

            #region Private Functions
            private void DeactivateLastTwoPlayers()
            {
                if (playerList.Find(x => x.playerIndex == 2))
                {
                    PlayerInput _playerInput = playerList.Find(x => x.playerIndex == 2);
                    _playerInput.gameObject.GetComponent<PlayerInfo>().joinPanelUI.gameObject.SetActive(false);
                    _playerInput.DeactivateInput();
                } 
                if (playerList.Find(x => x.playerIndex == 3)) 
                {
                    PlayerInput _playerInput = playerList.Find(x => x.playerIndex == 3);
                    _playerInput.gameObject.GetComponent<PlayerInfo>().joinPanelUI.gameObject.SetActive(false);
                    _playerInput.DeactivateInput();
                }
            }

            private void ActivateLastTwoPlayers()
            {
                if (playerList.Find(x => x.playerIndex == 2)) 
                {
                    PlayerInput _playerInput = playerList.Find(x => x.playerIndex == 2);
                    _playerInput.gameObject.GetComponent<PlayerInfo>().joinPanelUI.gameObject.SetActive(true);
                    _playerInput.ActivateInput();
                }
                if (playerList.Find(x => x.playerIndex == 3)) 
                {
                    PlayerInput _playerInput = playerList.Find(x => x.playerIndex == 3);
                    _playerInput.gameObject.GetComponent<PlayerInfo>().joinPanelUI.gameObject.SetActive(true);
                    _playerInput.ActivateInput();
                }
            }

            public void Configure()
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                
                joinAction.Enable();
                joinAction.performed += context => JoinAction(context);
        
                leaveAction.Enable();
                leaveAction.performed += context => LeaveAction(context);

                readyCount = 0;
            }
            
            private void OnPlayerJoined(PlayerInput _playerInput)
            {
                _playerInput.transform.SetParent(transform);
                playerList.Add(_playerInput);
                multiplayerEventSystems.Add(_playerInput.transform.GetComponent<MultiplayerEventSystem>());

                if (ScreenFlowController.instance.currentPoseType == ScreenPoseType.SplashStart)
                {
                    StartCoroutine( WaitForFirstAnimation(_playerInput));
                }

                if (PlayerJoinedGame != null)
                {
                    PlayerJoinedGame(_playerInput);
                }
            }

            private IEnumerator WaitForFirstAnimation(PlayerInput _playerInput)
            {
                Transform _startMenuPage = PageController.instance.pages[4].transform;
                StartMenu _startMenu = _startMenuPage.GetComponentInChildren<StartMenu>(true);
                while (!_startMenu.gameObject.activeInHierarchy)
                {
                    yield return null;
                }
                _startMenu.OnInitialInput(_playerInput);
            }
    
            private void OnPlayerLeft(PlayerInput _playerInput)
            {
                UnregisterPlayer(_playerInput);
            }
    
            private void JoinAction(InputAction.CallbackContext context)
            {
                if (playerList.Count < maxPlayerCount)
                {
                    var _device = context.control.device;
                    if (PlayerInput.FindFirstPairedToDevice(_device) != null)
                        return;

                    PlayerInputManager.instance.JoinPlayer(playerIndex: playerList.Count, pairWithDevice: _device);
                }
            }
    
            private void LeaveAction(InputAction.CallbackContext context)
            {
                if (playerList.Count > 1)
                {
                    foreach (var player in playerList)
                    {
                        foreach (var device in player.devices)
                        {
                            if (device != null && context.control.device == device)
                            {
                                UnregisterPlayer(player);
                                return;
                            }
                        }
                    }
                }
            }
    
            private void UnregisterPlayer(PlayerInput _playerInput)
            {
                playerList.Remove(_playerInput);

                if (PlayerLeftGame != null)
                {
                    PlayerLeftGame(_playerInput);
                }
        
                Destroy(_playerInput.transform.parent.gameObject);
                
            }

            private void Log(string _msg)
            {
                if(debug) Debug.Log("[PlayerJoinController]: " + _msg);
            }

            #endregion
        }
        
    }
}
