using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Textコンポーネント用

public class PlayFabController : MonoBehaviour
{
    public Text leaderboardText;  // UnityのInspectorからTextをアタッチ
    const string STATISTICS_NAME = "HighScore";

    //int resultScore = MergePuzzleSceneDirector,score;

    // MergePuzzleSceneDirectorのインスタンスを取得
    //MergePuzzleSceneDirector director = GameObject.FindObjectOfType<MergePuzzleSceneDirector>();
    //int resultScore = director.score;


    void Start()
    {
        PlayFabClientAPI.LoginWithCustomID(
            new LoginWithCustomIDRequest { CustomId = "Test3ID", CreateAccount = true },
            result => Debug.Log("ログイン成功！"),
            error => Debug.Log("ログイン失敗"));
            
    }

    void Update()
    {
       
       // Debug.Log("score" + resultScore);
        //SubmitScore(resultscore);
        

        RequestLeaderBoard();


    }

    void SubmitScore(int playerScore)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(
            new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>()
                {
                    new StatisticUpdate
                    {
                        StatisticName = STATISTICS_NAME,
                        Value = playerScore
                    }
                }
            },
            result =>
            {
                Debug.Log("スコア送信");
            },
            error =>
            {
                Debug.Log(error.GenerateErrorReport());
            }
        );
    }

    void RequestLeaderBoard()
    {
        PlayFabClientAPI.GetLeaderboard(
            new GetLeaderboardRequest
            {
                StatisticName = STATISTICS_NAME,
                StartPosition = 0,
                MaxResultsCount = 3
            },
            result =>
            {
                string leaderboardOutput = "";
                result.Leaderboard.ForEach(
                    x => leaderboardOutput += string.Format("{0}位: {1} スコア: {2} \r\n", x.Position + 1, x.DisplayName, x.StatValue)
                );

                // リーダーボードの結果をText UIに表示
                leaderboardText.text = leaderboardOutput;
            },
            error =>
            {
                Debug.Log(error.GenerateErrorReport());
            }
        );
    }
}
