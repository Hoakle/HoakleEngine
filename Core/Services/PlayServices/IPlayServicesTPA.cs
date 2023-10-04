using System;

namespace HoakleEngine.Core.Services.PlayServices
{
    public interface IPlayServicesTPA : ThirdPartyActor
    {
        public void SignIn();
        public void ManualSignIn();

        public void UnlockAchievement(string key);
        public void DisplayAchievements();
        
        //Leaderboard
        public Action<LeaderboardData> OnScoreLoaded { get; set; }
        public void UpdateScore(string key, long score);
        public void LoadScore(string key, bool isPlayerCentered);
        public void DisplayLeaderboards();
    }
}
