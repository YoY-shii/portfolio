using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable, IStatus, IHaveScore
{
    CalcScore calcScore;
    [SerializeField] SearchRange searchRange;
    [SerializeField] ReferenceDistance referenceDistance;

    //Field
    Vector3 range = new (5, 5, 5);
    float terrainX = 40f;
    float terrainY = 0f;
    float terrainZ = 40f;

    //Status
    [SerializeField] int maxHp;
    int damage = 5;

    //Cache
    NavMeshAgent nav;
    Transform transformCache;
    WaitForSeconds waitForSecondsCache;
    WaitForSeconds waitForSecondsCacheAttack;
    readonly float interval = 2f;
    readonly float intervalAttack = 0.5f;

    //Property
    public int Hp { get; private set; }

    public bool IsAlive { get; private set; }

    public int Score { get; private set; }

    private void Awake()
    {
        if (ReferenceEquals(calcScore, null))
        {
            calcScore = FindAnyObjectByType<CalcScore>();
        }

        TryGetComponent(out nav);
        transformCache = this.transform;
        waitForSecondsCache = new WaitForSeconds(interval);
        waitForSecondsCacheAttack = new WaitForSeconds(intervalAttack);
        Score = 100;
    }

    private void OnEnable()
    {
        //全回復しないことが稀にあるため
        if (Hp != maxHp)
        {
            Hp = maxHp;
        }

        IsAlive = true;

        nav.isStopped = false;

        StartCoroutine(Attack());

        //ランダムに出現
        transformCache.position = new Vector3(
            Random.Range(-terrainX, terrainX),
            terrainY,
            Random.Range(-terrainZ, terrainZ)
            );
    }

    IEnumerator Attack()
    {
        while (IsAlive)
        {
            yield return null;

            if (referenceDistance.CalcDistance() < range.sqrMagnitude &&
                searchRange.IsVisible &&
                !searchRange.IsObstacle)
            {
                nav.isStopped = true;
                
                int randomAttack = Random.Range(1, 4);

                for (int i = 0; i < randomAttack; i++)
                {
                    //ObjectPoolから取得
                    var effect = EffectPool.Instance.MakeEffect();
                    var pos = new Vector3(transformCache.position.x, transformCache.position.y + 1f, transformCache.position.z);
                    effect.transform.position = pos;

                    yield return waitForSecondsCacheAttack;
                }

                yield return waitForSecondsCache;

                nav.isStopped = false;
                
                yield return null;
            }
        }
    }

    //以下ダメージ処理
    void IDamageable.Damage(int damage)
    {
        Hp -= damage;

        if (Hp <= 0)
        {
            Hp = 0;
            IsAlive = false;
            nav.isStopped = true;
            Invoke(nameof(HideEnemy), 0.75f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var iInjurer = collision.gameObject.GetComponent<IInjurer>();

        if (iInjurer != null)
        {
            iInjurer.Damage(damage);
        }
    }

    void HideEnemy() => this.gameObject.SetActive(false);

    private void OnDisable()
    {
        calcScore.GetScore(this,this);
    }
}

//[field: SerializeField]