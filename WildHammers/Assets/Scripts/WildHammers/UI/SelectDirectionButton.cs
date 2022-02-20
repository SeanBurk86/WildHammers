
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WildHammers
{
    namespace UI
    {
        public class SelectDirectionButton : Button
        {
            public delegate void PressedDelegate();

            public event PressedDelegate pressed;
            
            public override void OnSubmit(BaseEventData eventData)
            {
                base.OnSubmit(eventData);
                pressed();
            }
        }
    }
}

