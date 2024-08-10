using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Player.Instance ←検索用
public class CalcScore : MonoBehaviour
{
    //Field
    [SerializeField] TextMeshProUGUI totalScoreText;

    int totalScore = 0;
    public int KilledCount { get; set; }
    public int DeathCount { get; set; }
    public bool isAliveBoss { get; set; }

    void Start()
    {
        KilledCount = 0;
        DeathCount = 0;
        isAliveBoss = true;

        totalScoreText.text = "Score " + totalScore.ToString();
    }

    public void CheckAlive(IStatus status)
    {
        //下記の二つのifはリスコフの置換原則に違反
        if (status is Player && !status.IsAlive)
        {
            DeathCount++;
            return;
        }

        if (status is Boss && !status.IsAlive)
        {
            isAliveBoss = status.IsAlive;
        }
    }

    /// <summary>
    ///Scoreを計算するメソッド
    /// </summary>
    public void GetScore(IStatus status, IHaveScore score)
    {
        //2体以上まとめて倒すとtotalScoreとKilledCountがバグったのでIsAliveで判定後にカウントする方式に変更
        if (!status.IsAlive)
        {
            totalScore += score.Score;
            KilledCount++;
        }

        if (totalScore <= 0)
        {
            totalScore = 0;
        }

        totalScoreText.text = "Score " + totalScore.ToString();
    }

    public int ResultScore()
    {
        const int PENALTYSCORE = 5000;
        var finallScore = totalScore + Player.Instance.Hp * 100 - DeathCount * PENALTYSCORE;

        if (finallScore <= 0)
        {
            finallScore = 0;
        }

        return finallScore;
    }
}

//まとめる前のCheckAliveメソッド
//public void CheckAlivePlayer(Player player)
//{
//    if (!player.IsAlive)
//    {
//        DeathCount++;
//    }
//}

//public void CheckAliveBoss(Boss boss) => isAliveBoss = boss.IsAlive;