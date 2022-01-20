
using TMPro;
using UnityCore.Menu;
using UnityCore.Scene;
using UnityEngine;
using UnityEngine.UI;
using WildHammers.Team;

namespace WildHammers
{
    namespace Match
    {
        public class MatchController : MonoBehaviour
        {
            public static MatchController instance;

            private TeamController.MatchTeam teamWest, teamEast;

            public TMP_Text player1WestText, player2WestText, player1EastText, player2EastText;

            public Image teamWestPanel, teamEastPanel;

            public bool hasMatchStarted;

            #region Unity Functions

            private void Awake()
            {
                if(!instance) Configure();
            }

            private void OnDisable()
            {
                Dispose();
            }

            private void Update()
            {
                if (teamWest != null)
                {
                    player1WestText.text = teamWest.teamRoster[0].playerInitials+" the "+teamWest.teamRoster[0].zodiacSign;
                    player2WestText.text = teamWest.teamRoster[1].playerInitials+" the "+teamWest.teamRoster[1].zodiacSign;
                    teamWestPanel.sprite = teamWest.teamInfo.livery;
                }
                
                if (teamEast != null)
                {
                    player1EastText.text = teamEast.teamRoster[0].playerInitials+" the "+teamEast.teamRoster[0].zodiacSign;
                    player2EastText.text = teamEast.teamRoster[1].playerInitials+" the "+teamEast.teamRoster[1].zodiacSign;
                    teamEastPanel.sprite = teamEast.teamInfo.livery;
                }

                if (teamWest != null && teamEast != null && !hasMatchStarted)
                {
                    StartMatch();
                }
            }

            #endregion

            #region Public Functions

            public void AddTeamToMatch(TeamController.MatchTeam _matchTeam)
            {
                if (teamWest != null)
                {
                    AddTeamEastToMatch(_matchTeam);
                    return;
                }
                AddTeamWestToMatch(_matchTeam);
            }

            public void StartMatch()
            {
                PageController.instance.TurnOffAllPages();
                SceneController.instance.Load(SceneType.MainGame, false, PageType.Loading);
                hasMatchStarted = true;
            }
            
            public MatchInfo PackageMatchInfo()
            {
                return new MatchInfo(teamWest, teamEast);
            }
            
            #endregion

            #region Private Functions

            private void Configure()
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            private void Dispose()
            {
                
            }

            private void AddTeamWestToMatch(TeamController.MatchTeam _matchTeam)
            {
                teamWest = _matchTeam;
            }
            
            private void AddTeamEastToMatch(TeamController.MatchTeam _matchTeam)
            {
                teamEast = _matchTeam;
            }

            #endregion
        }
    }
}

