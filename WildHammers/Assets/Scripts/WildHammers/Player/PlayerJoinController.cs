
using System.Collections.Generic;
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
            public static PlayerJoinController instance;
            
            public List<PlayerInput> playerList = new List<PlayerInput>();
            [SerializeField] private InputAction joinAction, leaveAction;

            public event System.Action<PlayerInput> PlayerJoinedGame;
            public event System.Action<PlayerInput> PlayerLeftGame;

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
                
            }

            private void Update()
            {
                if (readyCount >= PlayerInputManager.instance.maxPlayerCount && !MatchController.instance.hasMatchStarted)
                {
                    PageController.instance.TurnPageOff(PageType.PlayerJoin,PageType.TeamSelect);
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
    
            private void OnPlayerLeft(PlayerInput playerInput)
            {
            }
    
            private void JoinAction(InputAction.CallbackContext context)
            {
                if (playerList.Count < PlayerInputManager.instance.maxPlayerCount)
                {
                    PlayerInputManager.instance.JoinPlayerFromActionIfNotAlreadyJoined(context);
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

            #endregion
        }
        
    }
}