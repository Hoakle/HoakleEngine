using System;
using System.Collections;
using System.Collections.Generic;

namespace HoakleEngine.Core.Services.PlayServices
{
    public interface IPlayServicesTPA : ThirdPartyActor
    {
        public void Synchronize(List<string> achievementKeys);
        public void UnlockAchievement(string key);
        public void DisplayAchievements();
        
        //Leaderboard
        public Action<LeaderboardData> OnScoreLoaded { get; set; }
        public void UpdateScore(string key, long score);
        public void LoadScore(string key, bool isPlayerCentered);
        public void DisplayLeaderboards();
        
        //Review
        public Action OnReviewInfoReady { get; set; }
        public IEnumerator PrepareReview();
        public IEnumerator LaunchReview();
    }
}
