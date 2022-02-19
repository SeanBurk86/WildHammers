
using System;
using UnityEngine;

namespace UnityCore
{
    namespace Data
    {
        public class DataController : MonoBehaviour
        {
            public static DataController instance = null;
            
            private static readonly string MASTER_VOLUME = "mastervolume";
            private static readonly string MUSIC_VOLUME = "musicvolume";
            private static readonly string SFX_VOLUME = "sfxvolume";
            private static readonly string TOTAL_GOALS_SUFFIX = "_totalgoals";
            private static readonly string GOALS_SCORED_FOR_TEAM_SUFFIX = "_goals_scored_forteam";
            private static readonly string GOALS_SCORED_ON_SELF_SUFFIX = "_goals_scored_onself";
            private static readonly string MATCHES_WON_SUFFIX = "_matches_won";
            private static readonly string MATCHES_LOST_SUFFIX = "_matches_lost";
            private static readonly string MATCHES_DRAWN_SUFFIX = "_matches_drawn";
            private static readonly string MATCH_MUSIC = "matchmusic";
            private static readonly int DEFAULT_INT = 0;

            #region Properties
            public int MasterVolume
            {
                get
                {
                    return GetInt(MASTER_VOLUME);
                }
                set
                {
                    SaveInt(MASTER_VOLUME, value);
                }
            }

            public int MusicVolume
            {
                get
                {
                    return GetInt(MUSIC_VOLUME);
                }
                set
                {
                    SaveInt(MUSIC_VOLUME, value);
                }
            }
            
            

            public int SfxVolume
            {
                get
                {
                    return GetInt(SFX_VOLUME);
                }
                set
                {
                    SaveInt(SFX_VOLUME, value);
                }
            }

            public int MatchMusic
            {
                get
                {
                    return GetInt(MATCH_MUSIC);
                }
                set
                {
                    SaveInt(MATCH_MUSIC, value);
                }
            }

            #endregion
                
            #region Unity Functions

            private void Awake()
            {
                if (!instance) Configure();
                else Destroy(gameObject);
            }

            #endregion

            #region Public Functions

            public int GetPlayerTotalGoals(string _playerInfoString)
            {
                return GetInt(_playerInfoString + TOTAL_GOALS_SUFFIX);
            }

            public void IncrementPlayerTotalGoals(string _playerInfoString, int _points)
            {
                SaveInt(_playerInfoString+TOTAL_GOALS_SUFFIX, GetPlayerTotalGoals(_playerInfoString)+_points);
            }
            
            public int GetPlayerGoalsScoredOnSelf(string _playerInfoString)
            {
                return GetInt(_playerInfoString + GOALS_SCORED_ON_SELF_SUFFIX);
            }

            public void IncrementPlayerGoalsScoredOnSelf(string _playerInfoString, int _points)
            {
                SaveInt(_playerInfoString+GOALS_SCORED_ON_SELF_SUFFIX, GetPlayerGoalsScoredOnSelf(_playerInfoString)+_points);
            }
            
            public int GetPlayerGoalsScoredForTeam(string _playerInfoString)
            {
                return GetInt(_playerInfoString + GOALS_SCORED_FOR_TEAM_SUFFIX);
            }

            public void IncrementPlayerGoalsScoredForTeam(string _playerInfoString, int _points)
            {
                SaveInt(_playerInfoString+GOALS_SCORED_FOR_TEAM_SUFFIX, GetPlayerGoalsScoredForTeam(_playerInfoString)+_points);
            }
            
            public int GetPlayerMatchesWon(string _playerInfoString)
            {
                return GetInt(_playerInfoString + MATCHES_WON_SUFFIX);
            }

            public void IncrementPlayerMatchesWon(string _playerInfoString)
            {
                SaveInt(_playerInfoString+MATCHES_WON_SUFFIX, GetPlayerMatchesWon(_playerInfoString)+1);
            }
            
            public int GetPlayerMatchesLost(string _playerInfoString)
            {
                return GetInt(_playerInfoString + MATCHES_LOST_SUFFIX);
            }

            public void IncrementPlayerMatchesLost(string _playerInfoString)
            {
                SaveInt(_playerInfoString+MATCHES_LOST_SUFFIX, GetPlayerMatchesLost(_playerInfoString)+1);
            }
            
            public int GetPlayerMatchesDrawn(string _playerInfoString)
            {
                return GetInt(_playerInfoString + MATCHES_DRAWN_SUFFIX);
            }

            public void IncrementPlayerMatchesDrawn(string _playerInfoString)
            {
                SaveInt(_playerInfoString+MATCHES_DRAWN_SUFFIX, GetPlayerMatchesDrawn(_playerInfoString)+1);
            }

            #endregion
            
            #region Private Functions

            private void SaveInt(string _data, int _value)
            {
                PlayerPrefs.SetInt(_data, _value);
            }

            private int GetInt(string _data)
            {
                return PlayerPrefs.GetInt(_data, DEFAULT_INT);
            }

            private void Configure()
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            #endregion
        }
    }
}