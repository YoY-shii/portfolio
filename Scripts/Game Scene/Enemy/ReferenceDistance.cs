using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Player.Instance ←検索用
public class ReferenceDistance : MonoBehaviour
{
    Transform transformCache;

    void Awake()
    {
        transformCache = this.transform;
    }

    public float CalcDistance()
    {
        return Vector3.SqrMagnitude(transformCache.position - Player.Instance.transform.position);
    }
}
