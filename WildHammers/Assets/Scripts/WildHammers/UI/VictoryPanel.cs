
using TMPro;
using UnityCore.Audio;
using UnityCore.Menu;
using UnityCore.Scene;
using UnityEngine;
using WildHammers.Match;
using WildHammers.Player;
using WildHammers.Round;
using WildHammers.ScreenFlow;
using AudioType = UnityCore.Audio.AudioType;

namespace WildHammers
{
    namespace UI
    {
        public class VictoryPanel : MonoBehaviour
        {
            [SerializeField] private TMP_Text m_VictoryMsg;

            private void OnEnable()
            {
                string _resultText;
                if (GameRoundController.instance.winningTeam == "Draw") _resultText = "Draw";
                else _resultText = "Victory!!! \n"+GameRoundController.instance.winningTeam;
                m_VictoryMsg.text = _resultText;
            }

            public void RestartMatch()
            {
                SceneController.instance.ReloadScene();
                ScreenFlowController.instance.Flow(ScreenPoseType.Game);
            }

            public void MatchSettings()
            {
                MatchController.instance.hasMatchStarted = false;
                MatchController.instance.areSettingsSet = false;
                MatchController.instance.areSettingsAccepted = false;
                MatchController.instance.FlushMatchSettings();
                AudioController.instance.PlayAudio(AudioType.ST_01);
                SceneController.instance.Load(SceneType.MainMenu,false,PageType.Loading);
                ScreenFlowController.instance.Flow(ScreenPoseType.MatchSettings, false);
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
                ScreenFlowController.instance.Flow(ScreenPoseType.TeamSelect, false);
            }

            public void ChangePlayers()
            {
                MatchController.instance.hasMatchStarted = false;
                MatchController.instance.areSettingsSet = false;
                MatchController.instance.areSettingsAccepted = false;
                MatchController.instance.areTeamsPicked = false;
                MatchController.instance.FlushTeamSettings();
                PlayerJoinController.instance.ResetJoinPanel();
                AudioController.instance.PlayAudio(AudioType.ST_01);
                SceneController.instance.Load(SceneType.MainMenu,false,PageType.Loading);
                if(PlayerJoinController.instance.maxPlayerCount == 4)
                    ScreenFlowController.instance.Flow(ScreenPoseType.FourPlayerJoin, false);
                else ScreenFlowController.instance.Flow(ScreenPoseType.TwoPlayerJoin, false);
            }
        }
        
    }
}
