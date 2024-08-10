using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Player.Instance ←検索用
public class FireMagic : MonoBehaviour
{
    //Status
    float effectSpeed = 25f;
    int damage = 10;

    //Cache
    Rigidbody rb;
    Transform transformCache;
    WaitForSeconds waitForSecondsCache;
    readonly float interval = 2f;

    private void Awake()
    {
        TryGetComponent(out rb);
        transformCache = this.transform;
        waitForSecondsCache = new WaitForSeconds(interval);
    }

    private void OnEnable()
    {
        StartCoroutine(Return());
    }

    private void FixedUpdate()
    {
        var direction = Player.Instance.transform.localPosition - transformCache.localPosition;
        rb.AddForce(direction * effectSpeed * Time.deltaTime);
    }

    /// <summary>
    ///エフェクト非表示関数
    /// </summary>
    IEnumerator Return()
    {
        yield return null;

        if (this.gameObject.activeSelf)
        {
            yield return waitForSecondsCache;
            this.gameObject.SetActive(false);
        }
    }

    private void OnParticleCollision(GameObject collision)
    {
        var iInjurer = collision.gameObject.GetComponent<IInjurer>();

        if (iInjurer != null)
        {
            iInjurer.Damage(damage);
        }
    }
}
