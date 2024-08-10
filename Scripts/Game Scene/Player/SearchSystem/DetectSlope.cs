using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectSlope : MonoBehaviour
{
    readonly float interval = 5f;
    Transform transformCache;

    //Property
    /// <value>Movementクラスへ</value>
    public bool IsSlope { get; private set; }

    void Start()
    {
        transformCache = this.transform;
    }

    void Update()
    {
        if (Time.frameCount % interval == 0f)
        {
            Detect();
        }
    }

    /// <summary>
    ///坂検知
    /// </summary>
    void Detect()
    {
        var ray = new Ray(transformCache.localPosition, transformCache.forward);
        var maxDistance = 1f; //pos使用するなら3f

        if (!Physics.Raycast(ray, maxDistance))
        {
            IsSlope = false;
            return;
        }

        // 坂道の検出範囲
        var slopeRange = 0.5f;

        if (Physics.Raycast(transformCache.position, Vector3.down, out RaycastHit hit, slopeRange))
        {
            // 坂道の法線ベクトルを取得
            var slopeNormal = hit.normal;

            // 坂道の角度を計算
            var slopeAngle = Vector3.Angle(Vector3.up, slopeNormal);

            IsSlope = 35f <= slopeAngle ? true : false;
        }
    }
}

