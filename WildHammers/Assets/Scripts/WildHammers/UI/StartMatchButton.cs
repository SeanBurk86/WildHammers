
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace WildHammers
{
    namespace UI
    {
        public class StartMatchButton : Button
        {
            [SerializeField] private GameObject menuButtons, matchChoiceButtons, twoOnTwoButton;
            public override void OnSubmit(BaseEventData eventData)
            {
                base.OnSubmit(eventData);
                BaseInput _playerInput = eventData.currentInputModule.input;
                menuButtons.SetActive(false);
                matchChoiceButtons.SetActive(true);
                MultiplayerEventSystem _multiplayerEventSystem = _playerInput.transform.GetComponent<MultiplayerEventSystem>();
                _multiplayerEventSystem.SetSelectedGameObject(twoOnTwoButton);
            }
        }
        
    }
}
