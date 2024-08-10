using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] FadeOutManager fadeOutManager;
    [SerializeField] AudioManager audioManager;
    [SerializeField] DisplayResult displayResult;

    //Field
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI timeLimitText;
    [SerializeField] TextMeshProUGUI IsEndingText;

    float timeLimit;
    bool isCalledAllManager = false;
    float time = 0;

    //Property
    /// <value>Playerクラスへ</value>
    public bool IsEnding { get; private set; }

    void Awake()
    {
        if (ReferenceEquals(fadeOutManager, null))
        {
            fadeOutManager = FindAnyObjectByType<FadeOutManager>();
        }

        if (ReferenceEquals(audioManager, null))
        {
            audioManager = FindAnyObjectByType<AudioManager>();
        }

        if (ReferenceEquals(displayResult, null))
        {
            displayResult = FindAnyObjectByType<DisplayResult>();
        }

        IsEndingText.enabled = false;

        var gameTime = 3f;
        const float MINUTES = 1f;
        const float SECONDS = 60f;
        timeLimit = gameTime * MINUTES * SECONDS; //180秒

        IsEnding = false;

        fadeOutManager.enabled = false;
        audioManager.enabled = false;
        isCalledAllManager = false;
    }

    void Update()
    {
        if (isCalledAllManager) return;

        //時間の管理
        timeLimit -= Time.deltaTime;
        var span = new TimeSpan(0, 0, (int)timeLimit);
        timeLimitText.text = span.ToString(@"mm\:ss");

        if (timeLimit <= 0)
        {
            timeLimit = 0;
            IsEndingText.enabled = true;
            IsEndingText.text = "ゲーム終了";
            IsEnding = true;

            time += Time.deltaTime;
            if (time > 3f)
            {
                canvas.enabled = false;

                fadeOutManager.enabled = true;
                audioManager.enabled = true;
                displayResult.Invoke(nameof(displayResult.EvaluateResultText), 3f);
                isCalledAllManager = true;
            }
        }
    }
}

//Debug.unityLogger.logEnabled = false;