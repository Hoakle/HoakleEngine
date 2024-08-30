using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Play.Common;
using Google.Play.Review;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using Range = UnityEngine.SocialPlatforms.Range;

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

        public void Synchronize(List<string> achievementKeys)
        {
            PlayGamesPlatform.Instance.LoadAchievements(callback =>
            {
                
            });
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
            if(isPlayerCentered)
                PlayGamesPlatform.Instance.LoadScores(key, LoadScoreCallback);
            PlayGamesPlatform.Instance.LoadScores(key, isPlayerCentered ? LeaderboardStart.PlayerCentered : LeaderboardStart.TopScores, 10, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime,
                scoreData =>
                {
                    if(scoreData.Status == ResponseStatus.Success)
                    {
                        string[] list = scoreData.Scores.Append(scoreData.PlayerScore).Select(p => p != null ? p.userID : "").ToArray();
                        PlayGamesPlatform.Instance.LoadUsers(list, profiles =>
                        {
                            LeaderboardData data = new LeaderboardData(
                                scoreData.Title,
                                GetDataFromIScore(scoreData.PlayerScore, profiles.First(p => p.id == scoreData.PlayerScore.userID)),
                                scoreData.Scores.ToList().Select(score => GetDataFromIScore(score, profiles.First(p => p.id == score.userID))).ToList());
                            
                            OnScoreLoaded?.Invoke(data);
                        });
                    }
                    else
                    {
                        OnError?.Invoke(new PlayServicesError(PlayServicesErrorType.LoadScoreError, (int) scoreData.Status , "Load score error: " + scoreData.Status));
                    }
                    
                });
        }

        private void LoadScoreCallback(IScore[] scores)
        {
            
        }
        private ScoreData GetDataFromIScore(IScore score, IUserProfile profile)
        {
            if (score == null)
                return new ScoreData("Unknown", 0, 0);
            
            return new ScoreData(profile.userName, score.rank, score.value);
        }
        
        public void DisplayLeaderboards()
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        }

        private ReviewManager _ReviewManager;
        private PlayReviewInfo _PlayReviewInfo;
        public Action OnReviewInfoReady { get; set; }
        public async void PrepareReview()
        {
            _ReviewManager = new ReviewManager();
            PlayAsyncOperation<PlayReviewInfo,ReviewErrorCode> requestFlowOperation = await new Task<PlayAsyncOperation<PlayReviewInfo,ReviewErrorCode>>(() => _ReviewManager.RequestReviewFlow());
            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            { 
                OnError?.Invoke(new PlayServicesError(PlayServicesErrorType.ReviewError, (int) requestFlowOperation.Error , "Request Flow Operation Error: " + requestFlowOperation.Error));
                return;
            }
            
            _PlayReviewInfo = await new Task<PlayReviewInfo>(() => requestFlowOperation.GetResult());
            OnReviewInfoReady?.Invoke();
        }

        public async void LaunchReview()
        {
            PlayAsyncOperation<VoidResult,ReviewErrorCode> launchFlowOperation = await new Task<PlayAsyncOperation<VoidResult, ReviewErrorCode>>(() => _ReviewManager.LaunchReviewFlow(_PlayReviewInfo));
            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                OnError?.Invoke(new PlayServicesError(PlayServicesErrorType.ReviewError, (int) launchFlowOperation.Error , "Launch Flow Operation Error: " + launchFlowOperation.Error));
            }
        }
    }
}
