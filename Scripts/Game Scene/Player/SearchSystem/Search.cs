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
            ShakerSort();
            detectObstacle.JudgeObstacle(this);
        }
    }

    //挿入ソートではロックオン機能にバグが生じるため変更
    /// <summary>
    ///最も近い敵を索敵
    /// </summary>
    void ShakerSort()
    {
        //直接書き込むのは冗長的になるのとinspectorで確認できるようにtargetsへ代入
        targets = enemyPool.Targets;
        var distanceI = 0f;
        var distanceJ = 0f;
        var left = 0;
        var right = targets.Length - 1;

        while (left < right)
        {
            for (int i = left; i < right; i++)
            {
                distanceI = Vector3.SqrMagnitude(transformCache.position - targets[i].transform.position);
                distanceJ = Vector3.SqrMagnitude(transformCache.position - targets[i + 1].transform.position);

                if (distanceJ < distanceI)
                {
                    //プレイヤーから最も近い敵をtarget[0]に寄せ、SearchObjに代入
                    (targets[i], targets[i + 1]) = (targets[i + 1], targets[i]);
                }
            }

            left += 1;

            for (int i = right; left < i; i--)
            {
                distanceI = Vector3.SqrMagnitude(transformCache.position - targets[i].transform.position);
                distanceJ = Vector3.SqrMagnitude(transformCache.position - targets[i - 1].transform.position);

                if (distanceI < distanceJ)
                {
                    //プレイヤーから最も近い敵をtarget[0]に寄せ、SearchObjに代入
                    (targets[i], targets[i - 1]) = (targets[i - 1], targets[i]);
                }
            }

            right -= 1;
        }

        SearchObj = targets[0];
    }


}
