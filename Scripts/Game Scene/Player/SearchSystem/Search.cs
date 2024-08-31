using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search : MonoBehaviour
{
    [SerializeField] EnemyPool enemyPool;
    [SerializeField] DetectObstacle detectObstacle;

    //Field
    [SerializeField] GameObject[] targets = new GameObject[16];
    readonly float interval = 5f;

    //Cache
    Transform transformCache;

    //Property
    /// <value>BombOrientationクラス、EstimateRangeクラス、およびLockOnCameraクラスへ</value>
    public GameObject SearchObj { get; private set; }

    private void Awake()
    {
        if (ReferenceEquals(enemyPool, null))
        {
            enemyPool = FindAnyObjectByType<EnemyPool>();
        }

        if (ReferenceEquals(detectObstacle, null))
        {
            detectObstacle = FindAnyObjectByType<DetectObstacle>();
        }
    }

    void Start()
    {
        transformCache = this.transform;
    }

    void Update()
    {
        if (Time.frameCount % interval == 0f)
        {
            InsertionSort();
            detectObstacle.JudgeObstacle(this);
        }
    }

    /// <summary>
    ///最も近い敵を索敵
    /// </summary>
    void InsertionSort()
    {
        targets = enemyPool.Targets;
        var distanceI = 0f;
        var distanceJ = 0f;

        for (int i = 1; i < targets.Length; i++)
        {
            distanceI = Vector3.SqrMagnitude(transformCache.position - targets[i].transform.position);
            distanceJ = Vector3.SqrMagnitude(transformCache.position - targets[i - 1].transform.position);

            if (distanceI < distanceJ)
            {
                var j = i;

                while (0 < j && distanceI < distanceJ)
                {
                    (targets[j], targets[j - 1]) = (targets[j - 1], targets[j]);
                    j--;
                }

                SearchObj = targets[j];
            }
        }
    }
}
