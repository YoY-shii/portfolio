using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    [SerializeField] DetectSlope detectSlope;

    //Field
    Vector3 moveDirection;

    //Status
    float currentSpeed;
    readonly float groundSpeed = 5f;
    readonly float slopeSpeed = 0;
    readonly float interval = 10f;

    //Cache
    Animator animator;
    Transform transformCameraCache;
    Transform transformCache;
    int moveSpeedCache;
    int attackTagCache;
    int damagedTagCache;
    int isDeathTagCache;

    private void Awake()
    {
        TryGetComponent(out detectSlope);
        TryGetComponent(out animator);
    }

    void Start()
    {
        currentSpeed = groundSpeed;
        transformCameraCache = Camera.main.transform;
        transformCache = this.transform;
        moveSpeedCache = Animator.StringToHash("MoveSpeed");
        attackTagCache = Animator.StringToHash("Attack");
        damagedTagCache = Animator.StringToHash("Damaged");
        isDeathTagCache = Animator.StringToHash("IsDeathTag");
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).tagHash == isDeathTagCache) return;
        if (animator.GetCurrentAnimatorStateInfo(0).tagHash == damagedTagCache) return;
        if (animator.GetCurrentAnimatorStateInfo(0).tagHash == attackTagCache) return;

        if (Time.frameCount % interval == 0f)
        {
            currentSpeed = detectSlope.Detect() ? slopeSpeed : groundSpeed;
        }

        InputDirection();
        Rotation();
        Move();
    }

    /// <summary>
    ///プレイヤー移動入力操作
    /// </summary>
    void InputDirection()
    {
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.z = Input.GetAxisRaw("Vertical");
        moveDirection.Normalize();
    }

    /// <summary>
    ///回転
    /// </summary>
    void Rotation()
    {
        var rotaition = Quaternion.AngleAxis(transformCameraCache.eulerAngles.y, Vector3.up);
        moveDirection = rotaition * new Vector3(moveDirection.x, 0, moveDirection.z);
    }

    /// <summary>
    ///プレイヤーの方向および移動
    /// </summary>
    void Move()
    {
        animator.SetFloat(moveSpeedCache, moveDirection.sqrMagnitude);

        //十字キー方向にキャラが向くコード
        var rotationSpeed = 100 * Time.deltaTime;
        var direction = transformCache.localPosition + moveDirection * rotationSpeed;
        transformCache.LookAt(direction);

        transformCache.localPosition += moveDirection * currentSpeed * Time.deltaTime;
    }
}