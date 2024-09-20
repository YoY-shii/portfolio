using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

//Player.Instance ←検索用
public class LockOnCamera : MonoBehaviour
{
    [SerializeField] Search search;
    [SerializeField] DetectObstacle detectObstacle;

    //Field
    [SerializeField] CinemachineVirtualCamera freeCVCam;
    [SerializeField] CinemachineVirtualCamera lockOnCVCam;
    [SerializeField] GameObject lockOnTarget;
    Vector3 rangeDistance = new (5, 5, 5);
    Vector3 closeRangeDistance = new (4, 4, 4);

    //Cache
    Transform transformCache;

    private void Awake()
    {
        if (ReferenceEquals(search, null))
        {
            search = FindAnyObjectByType<Search>();
        }

        if (ReferenceEquals(detectObstacle, null))
        {
            detectObstacle = FindAnyObjectByType<DetectObstacle>();
        }
    }

    void Start()
    {
        freeCVCam.enabled = true;
        lockOnCVCam.enabled = false;

        transformCache = Player.Instance.transform;
    }

    void Update()
    {
        lockOnTarget = search.SearchObj;

        if (lockOnTarget == null) return;

        var distance = Vector3.SqrMagnitude(transformCache.position - lockOnTarget.transform.position);

        if (!lockOnTarget.activeSelf ||
            rangeDistance.sqrMagnitude < distance ||
            detectObstacle.IsObstacle)
        {
            freeCVCam.enabled = true;
            lockOnCVCam.enabled = false;
        }

        if (Input.GetMouseButton(1))
        {
            freeCVCam.enabled = false;
            lockOnCVCam.enabled = true;
            lockOnCVCam.LookAt = lockOnTarget.transform;
        }

        if (closeRangeDistance.sqrMagnitude < distance)
        {
            lockOnCVCam.LookAt = transformCache;
        }
    }
}
