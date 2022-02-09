
using UnityCore.Audio;
using UnityCore.Menu;
using UnityCore.Scene;
using UnityEngine;
using WildHammers.Match;
using WildHammers.Player;
using AudioType = UnityCore.Audio.AudioType;

namespace WildHammers
{
    namespace UI
    {
        public class VictoryPanel : MonoBehaviour
        {
            public void RestartMatch()
            {
                SceneController.instance.ReloadScene();
            }

            public void MatchSettings()
            {
                MatchController.instance.hasMatchStarted = false;
                MatchController.instance.areSettingsSet = false;
                MatchController.instance.areSettingsAccepted = false;
                MatchController.instance.FlushMatchSettings();
                AudioController.instance.PlayAudio(AudioType.ST_01);
                SceneController.instance.Load(SceneType.MainMenu,false,PageType.Loading);
            }

            public void ChangeTeams()
            {
                MatchController.instance.hasMatchStarted = false;
                MatchController.instance.areSettingsSet = false;
                MatchController.instance.areSettingsAccepted = false;
                MatchController.instance.areTeamsPicked = false;
                MatchController.instance.FlushTeamSettings();
                AudioController.instance.PlayAudio(AudioType.ST_01);
                SceneController.instance.Load(SceneType.MainMenu,false,PageType.Loading);
            }

            public void ChangePlayers()
            {
                MatchController.instance.hasMatchStarted = false;
                MatchController.instance.areSettingsSet = false;
                MatchController.instance.areSettingsAccepted = false;
                MatchController.instance.areTeamsPicked = false;
                MatchController.instance.FlushTeamSettings();
                AudioController.instance.PlayAudio(AudioType.ST_01);
                PlayerJoinController.instance.ResetJoinPanel();
                SceneController.instance.Load(SceneType.MainMenu,false,PageType.Loading);
            }
        }
        
    }
}
