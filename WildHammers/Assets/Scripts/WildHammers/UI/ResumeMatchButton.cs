
using UnityCore.Game;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WildHammers
{
    namespace GameplayObjects
    {
        public class ResumeMatchButton : Button
        {
            public override void OnSubmit(BaseEventData eventData)
            {
                base.OnSubmit(eventData);
                GameController.instance.HandlePauseInput();
            }
        }
    }
}
