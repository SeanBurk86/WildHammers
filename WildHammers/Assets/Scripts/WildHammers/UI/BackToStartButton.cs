
using UnityCore.Audio;
using UnityCore.Game;
using UnityCore.Menu;
using UnityCore.Scene;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WildHammers.Match;
using WildHammers.Player;

namespace WildHammers
{
    namespace UI
    {
        public class BackToStartButton : Button
        {
            public override void OnSubmit(BaseEventData eventData)
            {
                base.OnSubmit(eventData);
                GameController.instance.isHeadingToStart = true;
                if (PageController.instance.PageIsOn(PageType.ConfigSettings))
                {
                    PageController.instance.TurnPageOff(PageType.ConfigSettings, PageType.StartMenu);
                }
                else if (PageController.instance.PageIsOn(PageType.PauseMenu) 
                         || PageController.instance.PageIsOn(PageType.Victory))
                {
                    GameController.instance.HandlePauseInput();
                    PageController.instance.TurnOffAllPages();
                    SceneController.instance.Load(SceneType.MainMenu,false,PageType.Loading);
                    MatchController.instance.hasMatchStarted = false;
                    MatchController.instance.areSettingsSet = false;
                    MatchController.instance.areSettingsAccepted = false;
                    MatchController.instance.areTeamsPicked = false;
                    MatchController.instance.FlushTeamSettings();
                    AudioController.instance.PlayAudio(AudioType.ST_01);
                    PlayerJoinController.instance.ResetJoinPanel();
                    PageController.instance.TurnPageOn(PageType.StartMenu);
                }
            }
        }
        
    }
}
