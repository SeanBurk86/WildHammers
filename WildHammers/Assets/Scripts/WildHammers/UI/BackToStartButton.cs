
using UnityCore.Game;
using UnityCore.Menu;
using UnityCore.Scene;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WildHammers.Match;
using WildHammers.Player;
using WildHammers.ScreenFlow;

namespace WildHammers
{
    namespace UI
    {
        public class BackToStartButton : Button
        {
            public override void OnSubmit(BaseEventData eventData)
            {
                base.OnSubmit(eventData);
                if (ScreenFlowController.instance.currentPoseType == ScreenPoseType.Pause
                         || ScreenFlowController.instance.currentPoseType == ScreenPoseType.Victory)
                {
                    GameController.instance.HandlePauseInput();
                    SceneController.instance.Load(SceneType.MainMenu,false,PageType.Loading);
                    MatchController.instance.hasMatchStarted = false;
                    MatchController.instance.areSettingsSet = false;
                    MatchController.instance.areSettingsAccepted = false;
                    MatchController.instance.areTeamsPicked = false;
                    MatchController.instance.FlushTeamSettings();
                    PlayerJoinController.instance.ResetJoinPanel();
                }
                ScreenFlowController.instance.Flow(ScreenPoseType.StartMenu,false);
            }
        }
        
    }
}
