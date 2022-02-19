
using System;
using TMPro;
using UnityCore.Audio;
using UnityCore.Menu;
using UnityCore.Scene;
using UnityEngine;
using WildHammers.Match;
using WildHammers.Player;
using WildHammers.Round;
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
            }

            public void MatchSettings()
            {
                MatchController.instance.hasMatchStarted = false;
                MatchController.instance.areSettingsSet = false;
                MatchController.instance.areSettingsAccepted = false;
                MatchController.instance.FlushMatchSettings();
                AudioController.instance.PlayAudio(AudioType.ST_01);
                PageController.instance.TurnOffAllPages();
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
                PageController.instance.TurnOffAllPages();
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
                PageController.instance.TurnOffAllPages();
                SceneController.instance.Load(SceneType.MainMenu,false,PageType.Loading);
            }
        }
        
    }
}
