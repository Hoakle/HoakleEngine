using System;
using System.Linq;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

namespace HoakleEngine.Core.Services.PlayServices
{
    public class GooglePlayServicesTPA : IPlayServicesTPA
    {
        public Action<ActorError> OnError { get; set; }
        public Action<LeaderboardData> OnScoreLoaded { get; set; }
        public void Init()
        {
            SignIn();
        }

        public void SignIn()
        {
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }

        public void ManualSignIn()
        {
            PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
        }
        
        private void ProcessAuthentication(SignInStatus status) {
            if (status == SignInStatus.Success) {
                //Success
            } else {
                OnError?.Invoke(new PlayServicesError(PlayServicesErrorType.AuthenticationError, (int) status, "Authentication error: Status = " + status));
            }
        }

        public void UnlockAchievement(string key)
        {
            PlayGamesPlatform.Instance.UnlockAchievement(key, success =>
            {
                if(!success)
                    OnError?.Invoke(new PlayServicesError(PlayServicesErrorType.UnlockAchievementError, -1 , "Unlock achievement error: " + key));
            });
        }

        public void DisplayAchievements()
        {
            PlayGamesPlatform.Instance.ShowAchievementsUI();
        }

        public void UpdateScore(string key, long score)
        {
            PlayGamesPlatform.Instance.ReportScore(score, key, success => 
            {
                if(!success)
                    OnError?.Invoke(new PlayServicesError(PlayServicesErrorType.UpdateScoreError, -1 , "Update leaderboard error: " + key));
            });
        }

        public void LoadScore(string key, bool isPlayerCentered)
        {
            PlayGamesPlatform.Instance.LoadScores(key, isPlayerCentered ? LeaderboardStart.PlayerCentered : LeaderboardStart.TopScores, 10, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime,
                scoreData =>
                {
                    if(scoreData.Status == ResponseStatus.Success)
                    {
                        LeaderboardData data = new LeaderboardData(
                            scoreData.Title,
                            GetDataFromIScore(scoreData.PlayerScore),
                            scoreData.Scores.ToList().Select(GetDataFromIScore).ToList());
                        
                        OnScoreLoaded?.Invoke(data);
                    }
                    else
                    {
                        OnError?.Invoke(new PlayServicesError(PlayServicesErrorType.LoadScoreError, (int) scoreData.Status , "Load score error: " + scoreData.Status));
                    }
                    
                });
        }

        private ScoreData GetDataFromIScore(IScore score)
        {
            return new ScoreData(score.userID, score.rank, score.value);
        }
        public void DisplayLeaderboards()
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        }
    }
}
