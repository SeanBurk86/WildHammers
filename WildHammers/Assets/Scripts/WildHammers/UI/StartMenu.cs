
using System;
using UnityCore.Audio;
using UnityCore.Menu;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using WildHammers.Player;
using AudioType = UnityCore.Audio.AudioType;

namespace WildHammers
{
    namespace UI
    {
        public class StartMenu : MonoBehaviour
        {
            [SerializeField] private GameObject startPrompt;
            [SerializeField] private GameObject startMenuButtons;
            [SerializeField] private GameObject matchChoiceButtons;
            [SerializeField] private GameObject startAMatchButton;

            private bool isInitialInputReceived;

            private void Awake()
            {
                isInitialInputReceived = false;
            }

            private void Start()
            {
                AudioController.instance.PlayAudio(AudioType.SFX_08);
                AudioController.instance.PlayAudio(AudioType.ST_01,1f,0.5f);
            }


            public void OnInitialInput(PlayerInput _playerInput)
            {
                if (!isInitialInputReceived)
                {
                    isInitialInputReceived = true;
                    startPrompt.SetActive(false);
                    startMenuButtons.SetActive(true);
                    MultiplayerEventSystem _multiplayerEventSystem = _playerInput.transform.GetComponent<MultiplayerEventSystem>();
                    _multiplayerEventSystem.SetSelectedGameObject(startAMatchButton);
                }
            }

            public void SetMaxPlayers(int _maxPlayers)
            {
                PlayerJoinController.instance.maxPlayerCount = _maxPlayers;
                PageController.instance.TurnPageOff(PageType.StartMenu, PageType.PlayerJoin);
                matchChoiceButtons.SetActive(false);
                startMenuButtons.SetActive(true);
            }
        }
    }
}
