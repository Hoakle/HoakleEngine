using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Google.Play.Review;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using ModestTree;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Zenject;

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
                if(callback == null || callback.Length == 0)
                    return;

                var keys = callback.Where(a => a.completed).Select(a => a.id).ToList();
                foreach (var key in achievementKeys)
                {
                    if(!keys.Contains(key))
                        UnlockAchievement(key);
                }
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
            Debug.LogError("Load score for localUser: " + PlayGamesPlatform.Instance.localUser.userName + " - isPlayerCentered ? " + isPlayerCentered);

            var leaderboardStart = isPlayerCentered ? LeaderboardStart.PlayerCentered : LeaderboardStart.TopScores;
            PlayGamesPlatform.Instance.LoadScores(
                key, 
                leaderboardStart, 
                10, 
                LeaderboardCollection.Public, 
                LeaderboardTimeSpan.AllTime, 
                LoadScoreCallback);
        }

        private void LoadScoreCallback(LeaderboardScoreData scoreData)
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
        }
        
        private ScoreData GetDataFromIScore(IScore score, IUserProfile profile)
        {
            if (score == null)
                return new ScoreData("Unknown", 0, 0, new Texture2D(50,50));
            
            return new ScoreData(profile.userName, score.rank, score.value, profile.image);
        }
        
        public void DisplayLeaderboards()
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        }
        
        private ReviewManager _ReviewManager;
        private PlayReviewInfo _PlayReviewInfo;
        public Action OnReviewInfoReady { get; set; }
        public IEnumerator PrepareReview()
        {
            if (_ReviewManager == null) _ReviewManager = new ReviewManager();
            var requestFlowOperation = _ReviewManager.RequestReviewFlow();
            yield return requestFlowOperation;
            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                OnError?.Invoke(new PlayServicesError(PlayServicesErrorType.ReviewError, (int) requestFlowOperation.Error , "Request Flow Operation Error: " + requestFlowOperation.Error));
                yield break;
            }
            
            _PlayReviewInfo = requestFlowOperation.GetResult();
            OnReviewInfoReady?.Invoke();
        }

        public IEnumerator LaunchReview()
        {
            Debug.LogError("LaunchReview - LaunchReviewFlow");
            var launchFlowOperation = _ReviewManager.LaunchReviewFlow(_PlayReviewInfo);
            yield return launchFlowOperation;
            Debug.LogError("LaunchReview - launchFlowOperation = " + launchFlowOperation);
            _PlayReviewInfo = null;
            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                Debug.LogError("LaunchReview - DirectlyOpen - Error = " + launchFlowOperation.Error);
                DirectlyOpen();
            }
        }
        
        private void DirectlyOpen() { Application.OpenURL($"https://play.google.com/store/apps/details?id={Application.identifier}"); }
    }
}
