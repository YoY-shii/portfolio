using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//Player.Instance ←検索用
public class DisplayResult : MonoBehaviour
{
    [SerializeField] CalcScore calcScore;
    [SerializeField] Boss boss;

    //Field
    [SerializeField] TextMeshProUGUI resultText;

    void Start()
    {
        resultText.enabled = false;
    }

    /// <summary>
    ///条件次第でクリア画面変更するメソッド
    /// </summary>
    public void EvaluateResultText()
    {
        resultText.enabled = true;

        var result = "結果";
        var score = "スコア:";
        var title = "称号:";
        var name = "";

        if (Player.Instance.Hp == 100 &&
            calcScore.DeathCount == 0 &&
            !boss.IsAlive)
        {
            name = "暇神";
        }

        else if (!boss.IsAlive)
        {
            name = "ボスキラー";
        }

        else if (calcScore.KilledCount > 15)
        {
            name = "雑魚狩りマスター！";
        }

        else
        {
            title = "";
            name = "I hope you enjoyed it!";
        }

        resultText.text =
            $"{result}" +
            $"\n{score}{calcScore.ResultScore()}" +
            $"\n{title}{name}";

        Invoke(nameof(ReturnStartScene), 5f);
    }

    void ReturnStartScene() => SceneManager.LoadScene("Start Scene");

}
