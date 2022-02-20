
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WildHammers.ScreenFlow;

namespace WildHammers
{
    namespace UI
    {
        public class SettingsButton : Button
        {
            public override void OnSubmit(BaseEventData eventData)
            {
                base.OnSubmit(eventData);
                ScreenFlowController.instance.Flow(ScreenPoseType.SoundSettings);
            }
        }
        
    }
}
