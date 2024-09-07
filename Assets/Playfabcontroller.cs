using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Text�R���|�[�l���g�p

public class PlayFabController : MonoBehaviour
{
    public Text leaderboardText;  // Unity��Inspector����Text���A�^�b�`
    const string STATISTICS_NAME = "HighScore";

    //int resultScore = MergePuzzleSceneDirector,score;

    // MergePuzzleSceneDirector�̃C���X�^���X���擾
    //MergePuzzleSceneDirector director = GameObject.FindObjectOfType<MergePuzzleSceneDirector>();
    //int resultScore = director.score;


    void Start()
    {
        PlayFabClientAPI.LoginWithCustomID(
            new LoginWithCustomIDRequest { CustomId = "Test3ID", CreateAccount = true },
            result => Debug.Log("���O�C�������I"),
            error => Debug.Log("���O�C�����s"));
            
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
                Debug.Log("�X�R�A���M");
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
                    x => leaderboardOutput += string.Format("{0}��: {1} �X�R�A: {2} \r\n", x.Position + 1, x.DisplayName, x.StatValue)
                );

                // ���[�_�[�{�[�h�̌��ʂ�Text UI�ɕ\��
                leaderboardText.text = leaderboardOutput;
            },
            error =>
            {
                Debug.Log(error.GenerateErrorReport());
            }
        );
    }
}
