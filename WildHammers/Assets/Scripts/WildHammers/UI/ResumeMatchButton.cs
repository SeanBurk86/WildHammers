
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WildHammers.Round;

namespace WildHammers
{
    namespace GameplayObjects
    {
        public class ResumeMatchButton : Button
        {
            public override void OnSubmit(BaseEventData eventData)
            {
                base.OnSubmit(eventData);
                GameRoundController.instance.HandlePauseInput();
            }
        }
    }
}
