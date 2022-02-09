
using UnityCore.Menu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace WildHammers
{
    namespace UI
    {
        public class SettingsButton : Button
        {
            public override void OnSubmit(BaseEventData eventData)
            {
                Debug.Log("Hitting the Setting Button with player "+eventData.currentInputModule.transform.gameObject.GetComponent<PlayerInput>().playerIndex);
                base.OnSubmit(eventData);
                PageController.instance.TurnPageOff(PageType.StartMenu, PageType.ConfigSettings);
            }
        }
        
    }
}
