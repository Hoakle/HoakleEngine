using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using HoakleEngine.Core.Services;
using HoakleEngine.Core.Services.PlayServices;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace HoakleEngine
{
    public class PlayServicesTP : ThirdPartyService
    {
        private IPlayServicesTPA _Actor;
        public override void Init()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            _Actor = new GooglePlayServicesTPA();
#endif
#if UNITY_EDITOR
            _Actor = new EditorPlayServicesTPA();
#endif
            _Actor.OnError += OnError;
            _Actor.OnScoreLoaded += (leaderboardData) => OnScoreLoaded?.Invoke(leaderboardData);
            _Actor.Init();
        }
        
#region Authentication
        public void SignIn()
        {
            _Actor.SignIn();
        }

        public void ManualSignIn()
        {
            _Actor.ManualSignIn();
        }
        

#endregion

#region Achievements

        public void UnlockAchievement(string achievementKey)
        {
            _Actor.UnlockAchievement(achievementKey);
        }

        public void DisplayAchievements()
        {
            _Actor.DisplayAchievements();
        }
        
#endregion
        
#region Leaderboards

        public Action<LeaderboardData> OnScoreLoaded { get; set; }

        public void UpdateScore(string leaderboardKey, long score)
        {
            _Actor.UpdateScore(leaderboardKey, score);
        }

        public void LoadScore(string leaderboardKey, bool isPlayerCentered)
        {
            _Actor.LoadScore(leaderboardKey, isPlayerCentered);
        }
        
        public void DisplayLeaderboards()
        {
            _Actor.DisplayLeaderboards();
        }
        
#endregion
        
#region Error handling
        private void OnError(ActorError error)
        {
            StringBuilder str = new StringBuilder();
            str.Append("Play Services Error: " + ((PlayServicesError) error).Type);
            str.AppendLine("    - Code: " + error.Code);
            str.AppendLine("    - Message: " + error.Message);
                    
            Debug.LogError(str.ToString());
        }
        
#endregion

    }

    public class PlayServicesError : ActorError
    {
        public PlayServicesErrorType Type;

        public PlayServicesError(PlayServicesErrorType type, int code, string message)
        {
            Type = type;
            Code = code;
            Message = message;
        }
    }

    public enum PlayServicesErrorType
    {
        AuthenticationError = 0,
        UnlockAchievementError = 1,
        UpdateScoreError = 2,
        LoadScoreError
    }

    public class LeaderboardData
    {
        public string Title;
        public ScoreData UserData;
        public List<ScoreData> Scores;
        public LeaderboardData(string scoreDataTitle, ScoreData scoreDataPlayerScore, List<ScoreData> scoreDataScores)
        {
            Title = scoreDataTitle;
            UserData = scoreDataPlayerScore;
            Scores = scoreDataScores;
        }
    }

    public class ScoreData
    {
        public string UserId;
        public int Rank;
        public long Score;
        
        public ScoreData(string userId, int rank, long score)
        {
            UserId = userId;
            Rank = rank;
            Score = score;
        }
    }
}
