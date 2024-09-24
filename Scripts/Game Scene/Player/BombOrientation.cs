using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombOrientation : MonoBehaviour
{
    [SerializeField] BombPool bombPool;
    [SerializeField] Search search;

    //Field
    Vector3 rangeDistance = new(5, 5, 5);

    //Cache
    Camera mainCam;
    Animator animator;
    Transform transformCameraCache;
    Transform transformCache;
    int LayerBackGround;
    int magicAttackCache;

    private void Awake()
    {
        if (ReferenceEquals(bombPool, null))
        {
            bombPool = FindAnyObjectByType<BombPool>();
        }

        TryGetComponent(out search);
        TryGetComponent(out animator);
    }

    void Start()
    {
        mainCam = Camera.main;
        transformCameraCache = mainCam.transform;
        transformCache = this.transform;
        LayerBackGround = LayerMask.NameToLayer("BackGround");
        magicAttackCache = Animator.StringToHash("MagicAttack");
    }

    /// <summary>
    ///方向を管理しBombの位置を定めるメソッド
    /// </summary>
    public void BombOrient()
    {
        if (!IsRange()) return;

        //ObjectPoolから再利用
        var bombMagic = bombPool.MakeEffect();

        //ワールド座標に変換
        //Vector3.Distanceではないと座標がずれる
        var distance = Vector3.Distance(search.SearchObj.transform.position, transformCameraCache.position);
        var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        var currentPosition = mainCam.ScreenToWorldPoint(mousePosition);

        //魔法の生成場所
        bombMagic.transform.position = currentPosition;

        //発動した魔法の方向へプレイヤーを向ける
        var orientMagic = Quaternion.LookRotation(bombMagic.transform.position - transformCache.position);
        orientMagic.x = 0;
        orientMagic.z = 0;
        transformCache.localRotation = orientMagic;
        animator.SetTrigger(magicAttackCache);
    }

    /// <summary>
    ///Bombの射程を管理
    /// </summary>
    public bool IsRange()
    {
        var ray = mainCam.ScreenPointToRay(Input.mousePosition);
        var maxDistance = 10f;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            //Groundより下にBombが生成されないようにするため
            if (hit.collider.gameObject.layer == LayerBackGround) return false;
        }

        //距離が射程距離外なら早期return
        var magnitude = Vector3.SqrMagnitude(transformCache.localPosition - search.SearchObj.transform.localPosition);
        return magnitude < rangeDistance.sqrMagnitude;
    }

    /// <summary>
    ///魔法着地点
    /// </summary>
    //void OnDrawGizmos()
    //{
    //    if (currentPosition != Vector3.zero)
    //    {
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawSphere(currentPosition, 0.5f);
    //    }
    //}
}