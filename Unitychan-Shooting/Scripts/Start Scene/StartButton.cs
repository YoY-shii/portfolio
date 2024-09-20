using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    //Field
    AsyncOperation async;
    [SerializeField] Canvas buttonCanvas;
    [SerializeField] Canvas lordingCanvas;
    [SerializeField] Slider lordingSlider;

    private void Start()
    {
        lordingCanvas.enabled = false;
    }

    public void OnPressed()
    {
        StartCoroutine(Load());
        buttonCanvas.enabled = false;
        lordingCanvas.enabled = true;
    }

    IEnumerator Load()
    {
        // シーンの読み込み
        async = SceneManager.LoadSceneAsync("Game Scene");

        //　読み込みスライダー
        while (!async.isDone)
        {
            var _progressVal = Mathf.Clamp01(async.progress / 0.9f);
            lordingSlider.value = _progressVal;
            yield return null;
        }

        StopCoroutine(Load());
    }
}
