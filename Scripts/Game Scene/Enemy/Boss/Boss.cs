using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour, IDamageable, IStatus, IHaveScore
{
    [SerializeField] CalcScore calcScore;

    //Status
    [SerializeField] int maxHp;
    int damage = 5;

    //Cache
    Animator animator;
    Transform transformCamCache;
    Transform transformCache;
    NavMeshAgent nav;
    int isDeathCache;

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

        TryGetComponent(out animator);
        TryGetComponent(out nav);
    }

    void Start()
    {
        transformCache = this.transform;
        transformCamCache = Camera.main.transform;
        isDeathCache = Animator.StringToHash("IsDeath");
        Hp = maxHp;
        IsAlive = true;
        Score = 5000;
        nav.isStopped = false;
    }

    private void LateUpdate()
    {
        transformCache.LookAt(transformCamCache);
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
            animator.SetTrigger(isDeathCache);
            Invoke(nameof(Death), 1f);
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

    void Death() => Destroy(this.gameObject);

    private void OnDestroy()
    {
        calcScore.GetScore(this,this);
        calcScore.CheckAlive(this);
    }
}