using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

//Player.Instance ←検索用
public class SearchRange : MonoBehaviour
{
    [SerializeField] ReferenceDistance referenceDistance;

    //Field
    Vector3 direction;
    Vector3 rangeDistance = new (10, 10, 10);
    readonly float interval = 5f;

    //Cache
    Transform transformCache;
    int LayerPlayer;
    int LayerBackGround;

    //Property
    public bool IsVisible { get; private set; }

    public bool IsObstacle { get; private set; }

    private void OnEnable()
    {
        IsVisible = false;
        IsObstacle = false;
    }

    void Start()
    {
        transformCache = this.transform;
        LayerPlayer = LayerMask.NameToLayer("Player");
        LayerBackGround = LayerMask.NameToLayer("BackGround");
    }

    void Update()
    {
        if (rangeDistance.sqrMagnitude < referenceDistance.CalcDistance()) return;

        direction = Player.Instance.transform.localPosition - transformCache.localPosition;
        direction.Normalize();

        if (Time.frameCount % interval == 0f)
        {
            FOV();
            JudgeObstacle();
        }
    }

    /// <summary>
    ///視野範囲
    /// </summary>
    void FOV()
    {
        //平行視野範囲
        var angleHorizontalRange = 60f;

        //視野計算
        var nowAngle = Vector3.Angle(transformCache.forward, direction);

        //angle(45度)未満ならtrue
        if (nowAngle < angleHorizontalRange)
        {
            //垂直視野範囲
            var angleVerticalRange = 20f;

            if (nowAngle > angleVerticalRange)
            {
                IsVisible = false;
                return;
            }

            IsVisible = true;
        }

        else
        {
            IsVisible = false;
        }
    }

    /// <summary>
    ///障害物判定
    /// </summary>
    void JudgeObstacle()
    {
        //rayの高さ調節
        //yが0だと斜めの地形で反応してしまうことがあるため
        var pos = transformCache.localPosition;
        pos.y += 0.5f;
        var ray = new Ray(pos, direction);
        var maxDistance = 15f;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            if (hit.collider.gameObject.layer == LayerPlayer)
            {
                IsObstacle = false;
            }

            else if (hit.collider.gameObject.layer == LayerBackGround)
            {
                IsObstacle = true;
            }
        }
    }
}
