
using System.Collections;
using UnityEngine;
using WildHammers.Player;

namespace WildHammers
{
    namespace Team
    {
        public class TeamController : MonoBehaviour
        {
            public static TeamController instance;
            
            public bool debug;

            public TeamInfo[] teams;

            private Hashtable m_TeamInfoTable = new Hashtable(); //table for registering teams
            
            [System.Serializable]
            public class TeamInfo
            {
                public TeamCity city;
                public TeamName name;
                public Color[] colors;
                public Sprite livery;
            }

            public class MatchTeam
            {
                public TeamInfo teamInfo;
                public PlayerInfo[] teamRoster;

                public MatchTeam(TeamInfo _teamInfo, PlayerInfo[] _teamRoster)
                {
                    this.teamInfo = _teamInfo;
                    this.teamRoster = _teamRoster;
                }
            }

            #region Unity Functions

            private void Awake()
            {
                if (!instance)
                {
                    Configure();
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            #endregion

            #region Public Functions

            public PlayerInfo[] MatchPlayers(PlayerInfo _player1, PlayerInfo _player2)
            {
                return new PlayerInfo[] {_player1, _player2};
            }

            public MatchTeam MatchTeamToPlayers(TeamInfo _teamInfo, PlayerInfo[] _roster)
            {
                for (int i = 0; i < _roster.Length; i++)
                {
                    _roster[i].SetHammerColor(_teamInfo.colors[i]);
                }
                return new MatchTeam(_teamInfo, _roster);
            }

            public TeamInfo GetTeamInfo(TeamName _name)
            {
                foreach (TeamInfo _teamInfo in m_TeamInfoTable)
                {
                    if (_teamInfo.name == _name)
                    {
                        return _teamInfo;
                    }
                }

                return null;
            }

            #endregion
            
            #region Private Functions

            private void Configure()
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                GenerateTeamTable();
            }

            private void GenerateTeamTable()
            {
                foreach (TeamInfo _teamInfo in teams)
                {
                    if (m_TeamInfoTable.ContainsKey(_teamInfo.name))
                    {
                        LogWarning("You are trying to register team ["+_teamInfo.name+"] that has already been registered.");
                    } else if (_teamInfo.colors.Length != 2)
                    {
                        LogWarning("You are trying to register team ["+_teamInfo.name+"] with an incorrect number of colors");
                    }
                    else
                    {
                        m_TeamInfoTable.Add(_teamInfo.name, _teamInfo);
                    }
                }
            }

            private void Log(string _msg)
            {
                Debug.Log("[TeamController]: "+_msg);
            }
            
            private void LogWarning(string _msg)
            {
                Debug.LogWarning("[TeamController]: "+_msg);
            }
            

            #endregion
        }
        
    }
}
