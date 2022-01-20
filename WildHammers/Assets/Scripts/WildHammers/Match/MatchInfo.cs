
using WildHammers.Team;

namespace WildHammers
{
    namespace Match
    {
        public class MatchInfo
        {
            public TeamController.MatchTeam teamWest;
            public TeamController.MatchTeam teamEast;

            public MatchInfo(TeamController.MatchTeam teamWest, TeamController.MatchTeam teamEast)
            {
                this.teamWest = teamWest;
                this.teamEast = teamEast;
            }
        }
        
    }
}

