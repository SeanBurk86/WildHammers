
using TMPro;
using UnityCore.Menu;
using UnityCore.Scene;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using WildHammers.Player;
using WildHammers.Team;
using WildHammers.UI;

namespace WildHammers
{
    namespace Match
    {
        public class MatchController : MonoBehaviour
        {
            public bool debug;
            
            public static MatchController instance;

            private MatchInfo m_MatchInfo = new MatchInfo();

            private TeamController.MatchTeam teamWest, teamEast;

            public TMP_Text player1WestText, player2WestText, player1EastText, player2EastText, 
                teamWestScoreBox, teamEastScoreBox, roundTimer;

            public Image teamWestPanel, teamEastPanel;

            public bool hasMatchStarted = false, areTeamsPicked = false, areSettingsSet = false, areSettingsAccepted = false;

            [SerializeField] private TMP_Text m_WestTeamName, m_EastTeamName;
            [SerializeField] private Image m_WestTeamBanner, m_EastTeamBanner;
            [SerializeField] private MatchSettingsPanel m_MatchSettingsPanel;

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
                if (!hasMatchStarted)
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

                    if (!areTeamsPicked && !PageController.instance.PageIsOn(PageType.TeamSelect)
                                        && PlayerJoinController.instance.areAllPlayersEntered)
                    {
                        PageController.instance.TurnPageOn(PageType.TeamSelect);
                    } 
                    else if (teamWest != null && teamEast != null && !areTeamsPicked)
                    {
                        areTeamsPicked = true;
                        PageController.instance.TurnPageOff(PageType.TeamSelect, PageType.MatchSettings);
                    }
                    else if (areTeamsPicked && !areSettingsSet && !areSettingsAccepted)
                    {
                        PageController.instance.TurnPageOn(PageType.MatchSettings);
                    }
                    else if (areTeamsPicked && !areSettingsSet && areSettingsAccepted)
                    {
                        SetMatchInfo();
                    } 
                    else if (areTeamsPicked && areSettingsSet)
                    {
                        StartMatch();
                    }
                    
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
                ConfigureScoreBoard();
                hasMatchStarted = true;
            }

            public MatchInfo PackageMatchInfo()
            {
                return m_MatchInfo;
            }

            public void FlushMatchSettings()
            {
                m_MatchInfo = new MatchInfo(teamWest,teamEast);
            }
            
            public void FlushTeamSettings()
            {
                teamWest = null;
                teamEast = null;
                m_MatchInfo = new MatchInfo();
            }
            
            #endregion

            #region Private Functions

            private void Configure()
            {
                instance = this;
                m_MatchSettingsPanel.acceptSettingsEvent += AcceptSettings;
                DontDestroyOnLoad(gameObject);
            }

            private void Dispose()
            {
                m_MatchSettingsPanel.acceptSettingsEvent -= AcceptSettings;
            }

            private void AcceptSettings()
            {
                areSettingsAccepted = true;
            }

            private void AddTeamWestToMatch(TeamController.MatchTeam _matchTeam)
            {
                teamWest = _matchTeam;
            }
            
            private void AddTeamEastToMatch(TeamController.MatchTeam _matchTeam)
            {
                teamEast = _matchTeam;
            }

            private void ConfigureScoreBoard()
            {
                m_WestTeamName.text = teamWest.teamInfo.city + " " + teamWest.teamInfo.name;
                m_WestTeamBanner.sprite = teamWest.teamInfo.livery;
                m_EastTeamName.text = teamEast.teamInfo.city + " " + teamEast.teamInfo.name;
                m_EastTeamBanner.sprite = teamEast.teamInfo.livery;
            }
            

            private void SetMatchInfo()
            {
                m_MatchInfo = new MatchInfo(teamWest, teamEast,
                    m_MatchSettingsPanel.matchSettings.numberOfBalls, m_MatchSettingsPanel.matchSettings.winningScore,
                    m_MatchSettingsPanel.matchSettings.roundTimeLength);
                areSettingsSet = true;
            }

            private void Log(string _msg)
            {
                if(debug)
                    Debug.Log("[MatchController]: "+_msg);
            }

            #endregion
        }
    }
}

