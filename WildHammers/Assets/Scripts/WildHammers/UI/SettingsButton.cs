
using UnityCore.Menu;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WildHammers
{
    namespace UI
    {
        public class SettingsButton : Button
        {
            public override void OnSubmit(BaseEventData eventData)
            {
                base.OnSubmit(eventData);
                PageController.instance.TurnPageOff(PageType.StartMenu, PageType.ConfigSettings);
            }
        }
        
    }
}
