using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //Status
    int damage = 10;

    //Cache
    [SerializeField] AudioClip bombSound;
    AudioSource audioSource;

    WaitForSeconds waitForSecondsCache;
    readonly float interval = 1.5f;

    private void Awake()
    {
        waitForSecondsCache = new WaitForSeconds(interval);
        TryGetComponent(out audioSource);
    }

    private void OnEnable()
    {
        StartCoroutine(Return());
        audioSource.PlayOneShot(bombSound);
    }

    IEnumerator Return()
    {
        while (true)
        {
            if (this.gameObject.activeSelf)
            {
                yield return waitForSecondsCache;
                this.gameObject.SetActive(false);
            }

            yield return null;
        }
    }

    private void OnParticleCollision(GameObject collision)
    {
        var iDamageable = collision.gameObject.GetComponent<IDamageable>();

        if (iDamageable != null)
        {
            iDamageable.Damage(damage);
        }
    }
}
