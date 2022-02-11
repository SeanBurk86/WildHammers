
using UnityCore.Audio;
using UnityCore.Menu;
using UnityCore.Scene;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WildHammers.Match;
using WildHammers.Player;
using WildHammers.Round;

namespace WildHammers
{
    namespace UI
    {
        public class BackToStartButton : Button
        {
            public override void OnSubmit(BaseEventData eventData)
            {
                base.OnSubmit(eventData);
                if (PageController.instance.PageIsOn(PageType.ConfigSettings))
                {
                    PageController.instance.TurnPageOff(PageType.ConfigSettings, PageType.StartMenu);
                }
                else if (PageController.instance.PageIsOn(PageType.PauseMenu))
                {
                    SceneController.instance.Load(SceneType.MainMenu,false,PageType.Loading);
                    MatchController.instance.hasMatchStarted = false;
                    MatchController.instance.areSettingsSet = false;
                    MatchController.instance.areSettingsAccepted = false;
                    MatchController.instance.areTeamsPicked = false;
                    MatchController.instance.FlushTeamSettings();
                    AudioController.instance.PlayAudio(AudioType.ST_01);
                    GameRoundController.instance.HandlePauseInput();
                    PlayerJoinController.instance.ResetJoinPanel();
                    PageController.instance.TurnPageOn(PageType.StartMenu);
                }
            }
        }
        
    }
}
