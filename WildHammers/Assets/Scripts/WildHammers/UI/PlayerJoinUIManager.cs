
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
                    DontDestroyOnLoad(gameObject);
                }

                private void PlayerJoinedGame(PlayerInput _playerInput)
                {
                    SetupJoinPlayerInputPanel(_playerInput);
                }

                private void SetupJoinPlayerInputPanel(PlayerInput _playerInput)
                {
                    playerJoinMenu.transform.GetChild(_playerInput.playerIndex).gameObject.SetActive(false);
                    Transform joinPlayerUI = _playerInput.transform.GetChild(0);
                    joinPlayerUI.SetParent(playerJoinMenu.transform, false);
                    if (_playerInput.playerIndex > 0)
                    {
                        float _panelXOffest = _playerInput.playerIndex * .25f;
                        joinPlayerUI.GetComponent<RectTransform>().anchorMin = new Vector2(_panelXOffest, 0);
                        joinPlayerUI.GetComponent<RectTransform>().anchorMax = new Vector2(_panelXOffest + .25f, 1);
                    }
                    _playerInput.transform.GetComponent<MultiplayerEventSystem>()
                        .SetSelectedGameObject(joinPlayerUI.GetComponent<JoinPanelUI>().firstSelectedInitialsButton); 
                }
            

            #endregion
            
        }
        
    }
}

