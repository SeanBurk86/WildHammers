
using UnityCore.Audio;
using UnityCore.Game;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using WildHammers.Player;
using WildHammers.ScreenFlow;
using AudioType = UnityCore.Audio.AudioType;

namespace WildHammers
{
    namespace UI
    {
        public class StartMenu : MonoBehaviour
        {
            [SerializeField] private GameObject startPrompt;
            [SerializeField] private GameObject startMenuButtons;
            [SerializeField] private GameObject startAMatchButton;


            private void Start()
            {
                AudioController.instance.PlayAudio(AudioType.SFX_08);
                AudioController.instance.PlayAudio(AudioType.ST_05, 1f);
            }
            


            public void OnInitialInput(PlayerInput _playerInput)
            {
                if (!GameController.instance.isInitialInputReceived)
                {
                    GameController.instance.isInitialInputReceived = true;
                    startPrompt.SetActive(false);
                    startMenuButtons.SetActive(true);
                    MultiplayerEventSystem _multiplayerEventSystem = _playerInput.transform.GetComponent<MultiplayerEventSystem>();
                    _multiplayerEventSystem.SetSelectedGameObject(startAMatchButton);
                }
            }

            public void SetMaxPlayers(int _maxPlayers)
            {
                PlayerJoinController.instance.maxPlayerCount = _maxPlayers;
                if(_maxPlayers == 4) ScreenFlowController.instance.Flow(ScreenPoseType.FourPlayerJoin);
                else ScreenFlowController.instance.Flow(ScreenPoseType.TwoPlayerJoin);
            }
            
            public void QuitGame()
            {
                Application.Quit();
            }
        }
    }
}
