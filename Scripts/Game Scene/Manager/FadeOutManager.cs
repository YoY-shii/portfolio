using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutManager : MonoBehaviour
{
    //Field
    [SerializeField] Image fadeOutBackGround;

    //Cache
    WaitForSeconds waitForSecondsCache;
    readonly float delayTime = 0.002f;

    private void Awake()
    {
        fadeOutBackGround.enabled = false;
        waitForSecondsCache = new WaitForSeconds(delayTime);
    }

    void Start()
    {
        fadeOutBackGround.enabled = true;
        StartCoroutine(FadeOut());
    }

    /// <summary>
    ///クリア後に画面を徐々に暗くするメソッド
    /// </summary>
    IEnumerator FadeOut()
    {
        for (var i = 0f; i <= 255f; i += 0.001f)
        {
            fadeOutBackGround.color = new Color(0f, 0f, 0f, i);
            yield return waitForSecondsCache;
        }
    }
}
