using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Field
    [SerializeField] AudioSource audioSource;

    //Cache
    WaitForSeconds waitForSecondsCache;
    readonly float delayTime = 0.002f;

    private void Awake()
    {
        waitForSecondsCache = new WaitForSeconds(delayTime);
    }

    void Start()
    {
        TryGetComponent(out audioSource);

        StartCoroutine(VolumeDown());
    }

    /// <summary>
    ///クリア後にBGMの音量を徐々に下げる関数
    /// </summary>
    IEnumerator VolumeDown()
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= 0.001f;
            yield return waitForSecondsCache;
        }
    }
}
