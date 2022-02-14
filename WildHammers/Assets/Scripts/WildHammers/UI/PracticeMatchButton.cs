
using UnityCore.Menu;
using UnityCore.Scene;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
            }
        }
    }
}

