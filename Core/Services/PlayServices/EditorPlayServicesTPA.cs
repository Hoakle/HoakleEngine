using System;
using System.Collections.Generic;
using System.Linq;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

namespace HoakleEngine.Core.Services.PlayServices
{
    public class EditorPlayServicesTPA : IPlayServicesTPA
    {
        public Action<ActorError> OnError { get; set; }
        public Action<LeaderboardData> OnScoreLoaded { get; set; }

        private bool _IsSignedIn;
        public void Init()
        {
            SignIn();
        }

        public void SignIn()
        {
            _IsSignedIn = true;
        }

        public void ManualSignIn()
        {
            _IsSignedIn = true;
        }

        public void UnlockAchievement(string key)
        {
            Debug.LogError("UnlockAchievement not implemented in editor");
        }

        public void DisplayAchievements()
        {
            Debug.LogError("DisplayAchievements not implemented in editor");
        }

        public void UpdateScore(string key, long score)
        {
            Debug.LogError("UpdateScore not implemented in editor");
        }

        public void LoadScore(string key, bool isPlayerCentered)
        {
            List<ScoreData> fakeScores = new List<ScoreData>();
            ScoreData playerScore = new ScoreData("Hoakle", 15, 10000);
            
            if (isPlayerCentered)
            {
                fakeScores.Add(new ScoreData("Kimberley", 11, 50000));
                fakeScores.Add(new ScoreData("Venom", 12, 45000));
                fakeScores.Add(new ScoreData("Liv", 13, 40000));
                fakeScores.Add(new ScoreData("Leon", 14, 35000));
                fakeScores.Add(playerScore);
                fakeScores.Add(new ScoreData("Mary", 16, 25000));
                fakeScores.Add(new ScoreData("Tyler", 17, 20000));
                fakeScores.Add(new ScoreData("Shirley", 18, 15000));
                fakeScores.Add(new ScoreData("Munson", 19, 10000));
                fakeScores.Add(new ScoreData("Anthony", 20, 5000));
            }
            else
            {
                fakeScores.Add(new ScoreData("SidoPiou", 1, 50000));
                fakeScores.Add(new ScoreData("Venom", 2, 45000));
                fakeScores.Add(new ScoreData("Liv", 3, 40000));
                fakeScores.Add(new ScoreData("Leon", 4, 35000));
                fakeScores.Add(new ScoreData("PapiPoule", 5, 30000));
                fakeScores.Add(new ScoreData("Mary", 6, 25000));
                fakeScores.Add(new ScoreData("Tyler", 7, 20000));
                fakeScores.Add(new ScoreData("Shirley", 8, 15000));
                fakeScores.Add(new ScoreData("Munson", 9, 10000));
                fakeScores.Add(new ScoreData("Anthony", 10, 5000));
            }
            
            
            LeaderboardData data = new LeaderboardData("Classement", playerScore, fakeScores);
            
            OnScoreLoaded?.Invoke(data);
        }

        private ScoreData GetDataFromIScore(IScore score)
        {
            return new ScoreData(score.userID, score.rank, score.value);
        }
        
        public void DisplayLeaderboards()
        {
            Debug.LogError("DisplayLeaderboards not implemented in editor");
        }

        public Action OnReviewInfoReady { get; set; }

        public void PrepareReview()
        {
            OnReviewInfoReady?.Invoke();
        }

        public void LaunchReview()
        {
            
        }
    }
}
