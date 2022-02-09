
using WildHammers.Team;

namespace WildHammers
{
    namespace Match
    {
        public class MatchInfo
        {
            public TeamController.MatchTeam teamWest;
            public TeamController.MatchTeam teamEast;
            public int numberOfBalls;
            public int winningScore;
            public float roundTimeLength;

            public MatchInfo()
            {
                this.teamWest = null;
                this.teamEast = null;
                this.numberOfBalls = 0;
                this.winningScore = 0;
                this.roundTimeLength = 0f;
            }

            public MatchInfo(TeamController.MatchTeam _TeamWest, TeamController.MatchTeam _TeamEast)
            {
                this.teamWest = _TeamWest;
                this.teamEast = _TeamEast;
                this.numberOfBalls = 5;
                this.winningScore = 10;
                this.roundTimeLength = 300f;
            }

            public MatchInfo(TeamController.MatchTeam _TeamWest, TeamController.MatchTeam _TeamEast, 
                int _numberOfBalls, int _WinningScore, float _RoundTimeLength)
            {
                this.teamWest = _TeamWest;
                this.teamEast = _TeamEast;
                this.numberOfBalls = _numberOfBalls;
                this.winningScore = _WinningScore;
                this.roundTimeLength = _RoundTimeLength;
            }
        }
        
    }
}

