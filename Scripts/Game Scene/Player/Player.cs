using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerUI))]
[RequireComponent(typeof(DetectSlope))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Search))]
[RequireComponent(typeof(BombOrientation))]

public class Player : MonoBehaviour, IInjurer, IStatus
{
    public static Player Instance;

    [SerializeField] CalcScore calcScore;
    [SerializeField] GameManager gameManager;
    [SerializeField] BombOrientation bombOrientation;

    //Field
    Vector3 direction; //ノックバック時に使用
    float time = 0;

    //Status
    [SerializeField] int maxHp;
    bool isKnockBack = false;
    bool isInvincibility = false;

    //Cache
    Rigidbody rb;
    Animator animator;
    Transform transformCache;
    WaitForSeconds WaitForSecondsCache;
    readonly float interval = 3f;
    int damagedCache;
    int damagedTagCache;
    int isDeathCache;
    int LayerEnemy;

    //Property
    [field: SerializeField] public int Hp { get; private set; }

    public bool IsAlive { get; private set; }

    private void Awake()
    {
        if (ReferenceEquals(gameManager, null))
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }

        if (ReferenceEquals(calcScore, null))
        {
            calcScore = FindAnyObjectByType<CalcScore>();
        }

        TryGetComponent(out bombOrientation);
        TryGetComponent(out rb);
        TryGetComponent(out animator);
        transformCache = this.transform;
        WaitForSecondsCache = new WaitForSeconds(interval);
        LayerEnemy = LayerMask.NameToLayer("Enemy");

        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        Hp = maxHp;
        IsAlive = true;
        isInvincibility = true;
        time = 0;

        damagedCache = Animator.StringToHash("Damaged");
        damagedTagCache = Animator.StringToHash("Damaged");
        isDeathCache = Animator.StringToHash("IsDeath");

        StartCoroutine(Attack());
    }

    private void FixedUpdate()
    {
        if (isKnockBack) OccurKnockBack();
    }

    void Update()
    {
        if (!IsAlive) return;
        if (isInvincibility) MakeisInvincibility();
    }

    /// <summary>
    ///攻撃のクールタイムとしてコルーチンを実装
    /// </summary>
    IEnumerator Attack()
    {
        while (IsAlive)
        {
            if (gameManager.IsEnding) yield return WaitForSecondsCache;
            if (animator.GetCurrentAnimatorStateInfo(0).tagHash == damagedTagCache) yield return WaitForSecondsCache;

            if (Input.GetMouseButtonDown(0))
            {
                bombOrientation.BombOrient();

                yield return WaitForSecondsCache;
            }

            yield return null;
        }

        StopCoroutine(Attack());
    }

    //以下ダメージ処理
    void IInjurer.Damage(int damage)
    {
        //下記3つの早期returnをメソッドにまとめると挙動が変わってしまうのでまとめない
        if (gameManager.IsEnding) return;
        if (!IsAlive) return;
        if (isInvincibility) return;

        Hp -= damage;
        animator.SetTrigger(damagedCache);
        isInvincibility = true;

        if (Hp <= 0)
        {
            Hp = 0;
            IsAlive = false;
            calcScore.CheckAlive(this);
            animator.SetTrigger(isDeathCache);
            StartCoroutine(Revival());
        }
    }

    /// <summary>
    ///無敵化
    /// </summary>
    void MakeisInvincibility()
    {
        time += Time.deltaTime;

        if (time >= 5f)
        {
            //無敵化終了
            isInvincibility = false;
            time = 0;
        }
    }

    /// <summary>
    ///collisionでノックバック相手の情報を取得
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (gameManager.IsEnding) return;
        if (!IsAlive) return;
        if (isInvincibility) return;

        if (collision.gameObject.layer == LayerEnemy)
        {
            //自身と敵の向き取得
            direction = transformCache.position - collision.transform.position;
            direction.Normalize();
            isKnockBack = true;
        }
    }

    /// <summary>
    ///ノックバックメソッド
    /// </summary>
    void OccurKnockBack()
    {
        //後方にノックバック
        var knockBackPower = 500f;
        rb.AddForce(direction * knockBackPower * Time.deltaTime, ForceMode.VelocityChange);

        time += Time.deltaTime;

        if (time > 0.5f)
        {
            isKnockBack = false;
            time = 0;
        }
    }

    /// <summary>
    ///復活処理
    /// </summary>
    IEnumerator Revival()
    {
        yield return WaitForSecondsCache;

        Hp = maxHp;
        IsAlive = true;
        isKnockBack = false;
        isInvincibility = true;
        time = 0;
        StartCoroutine(Attack());
        StopCoroutine(Revival());
        yield return null;
    }
}