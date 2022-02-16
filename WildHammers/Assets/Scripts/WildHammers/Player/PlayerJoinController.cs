
using System.Collections.Generic;
using UnityCore.Game;
using UnityCore.Menu;
using UnityEngine;
using UnityEngine.InputSystem;
using WildHammers.Match;
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
                        if (readyCount >= maxPlayerCount && !PageController.instance.PageIsOn(PageType.TeamSelectRosterPanel))
                        {
                            areAllPlayersEntered = true;
                            PageController.instance.TurnPageOff(PageType.PlayerJoin);
                            PageController.instance.TurnPageOn(PageType.TeamSelectRosterPanel);
                        } 
                        else if (!PageController.instance.PageIsOn(PageType.PlayerJoin) 
                                 && !PageController.instance.PageIsOn(PageType.StartMenu)
                                 && !PageController.instance.PageIsOn(PageType.ConfigSettings)
                                 && !PageController.instance.PageIsOn(PageType.TeamSelectRosterPanel)
                                 && !PageController.instance.PageIsOn(PageType.TeamSelect))
                        {
                            if (maxPlayerCount == 2)
                            {
                                if (playerList.Find(x => x.playerIndex == 3))
                                    UnregisterPlayer(playerList.Find(x => x.playerIndex == 3));
                                if (playerList.Find(x => x.playerIndex == 4)) 
                                    UnregisterPlayer(playerList.Find(x => x.playerIndex == 4));
                            }
                            PageController.instance.TurnPageOn(PageType.PlayerJoin);
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

                if (PageController.instance.PageIsOn(PageType.StartMenu))
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
                if(debug)
                    Debug.Log("[PlayerJoinController]: " + _msg);
            }

            #endregion
        }
        
    }
}
