
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace WildHammers
{
    namespace UI
    {
        public class StartMenu : MonoBehaviour
        {
            [SerializeField] private GameObject startPrompt;
            [SerializeField] private GameObject startMenuButtons;
            [SerializeField] private GameObject startAMatchButton;

            private bool isInitialInputReceived;

            private void Awake()
            {
                isInitialInputReceived = false;
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
        }
    }
}
