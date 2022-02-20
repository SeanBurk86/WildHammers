
using UnityCore.Menu;
using UnityCore.Scene;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WildHammers.ScreenFlow;

namespace WildHammers
{
    namespace UI
    {
        public class PracticeMatchButton : Button
        {
            public override void OnSubmit(BaseEventData eventData)
            {
                base.OnSubmit(eventData);
                SceneController.instance.Load(SceneType.Practice, false, PageType.Loading);
                ScreenFlowController.instance.Flow(ScreenPoseType.Practice);
            }
        }
    }
}

