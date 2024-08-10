using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    IStatus statusInterface;
    [SerializeField] SearchRange searchRange;
    [SerializeField] ReferenceDistance referenceDistance;

    //Field
    Vector3 range = new(5, 5, 5);
    float terrainX = 40f;
    float terrainY = 0f;
    float terrainZ = 40f;

    //Cache
    Animator animator;
    UnityEngine.AI.NavMeshAgent nav;
    Transform transformCache;
    WaitForSeconds waitForSecondsCache;
    WaitForSeconds waitForSecondsCacheAttack;
    readonly float interval = 2f;
    readonly float intervalAttack = 0.5f;
    int magicAttackCache;

    private void Awake()
    {
        TryGetComponent(out animator);
        TryGetComponent(out nav);
    }

    //statusInterfaceがnullになるため、IEnumerator Start()に変更
    //Awakeでも失敗する
    IEnumerator Start()
    {
        //↓必ず使用すること、statusInterfaceがnullになる
        yield return null;

        TryGetComponent(out statusInterface);
        transformCache = this.transform;
        waitForSecondsCache = new WaitForSeconds(interval);
        waitForSecondsCacheAttack = new WaitForSeconds(intervalAttack);
        magicAttackCache = Animator.StringToHash("MagicAttack");

        //ランダムに出現
        transformCache.position = new Vector3(
            Random.Range(-terrainX, terrainX),
            terrainY,
            Random.Range(-terrainZ, terrainZ)
            );

        yield return Attack();
    }

    IEnumerator Attack()
    {
        while (statusInterface.IsAlive)
        {
            yield return null;

            if (referenceDistance.CalcDistance() < range.sqrMagnitude &&
                searchRange.IsVisible &&
                !searchRange.IsObstacle)
            {
                animator.SetTrigger(magicAttackCache);
                int randomAttack = Random.Range(1, 10);

                for (int i = 0; i < randomAttack; i++)
                {
                    //ObjectPoolから取得
                    var effect = EffectPool.Instance.MakeEffect();
                    var pos = new Vector3(transformCache.position.x, transformCache.position.y + 1f, transformCache.position.z);
                    effect.transform.position = pos;

                    yield return waitForSecondsCacheAttack;
                }

                yield return waitForSecondsCache;

                Warp();

                yield return null;
            }
        }
    }

    void Warp()
    {
        transformCache.position = new Vector3(
            Random.Range(-terrainX, terrainX),
            terrainY,
            Random.Range(-terrainZ, terrainZ)
            );

        nav.Warp(transformCache.position);
    }
}
