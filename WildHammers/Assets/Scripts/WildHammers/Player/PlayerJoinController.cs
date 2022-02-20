
using System.Collections.Generic;
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
                        if (maxPlayerCount == 2)
                        {
                            if (playerList.Find(x => x.playerIndex == 3))
                                UnregisterPlayer(playerList.Find(x => x.playerIndex == 3));
                            if (playerList.Find(x => x.playerIndex == 4)) 
                                UnregisterPlayer(playerList.Find(x => x.playerIndex == 4));
                        }
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

            #endregion

            #region Private Functions

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
                    Transform _startMenuPage = PageController.instance.pages[4].transform;
                    StartMenu _startMenu = _startMenuPage.GetComponentInChildren<StartMenu>();
                    _startMenu.OnInitialInput(_playerInput);
                }

                if (PlayerJoinedGame != null)
                {
                    PlayerJoinedGame(_playerInput);
                }
            }
    
            private void OnPlayerLeft(PlayerInput _playerInput)
            {
                UnregisterPlayer(_playerInput);
            }
    
            private void JoinAction(InputAction.CallbackContext context)
            {
                if (playerList.Count < maxPlayerCount)
                {
                    var device = context.control.device;
                    if (PlayerInput.FindFirstPairedToDevice(device) != null)
                        return;

                    Log("Joining player with index: "+playerList.Count);
                    PlayerInputManager.instance.JoinPlayer(playerIndex: playerList.Count, pairWithDevice: device);
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
    
            private void UnregisterPlayer(PlayerInput playerInput)
            {
                playerList.Remove(playerInput);

                if (PlayerLeftGame != null)
                {
                    PlayerLeftGame(playerInput);
                }
        
                Destroy(playerInput.transform.parent.gameObject);
            }

            private void Log(string _msg)
            {
                if(!debug)
                    Debug.Log("[PlayerJoinController]: " + _msg);
            }

            #endregion
        }
        
    }
}
