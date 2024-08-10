using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    ObjectPool<GameObject> pool;

    //Field
    [SerializeField] GameObject[] targets = new GameObject[16];//Original設置
    [SerializeField] GameObject enemyPrefab;
    readonly float interval = 5f;

    //Cache
    [SerializeField] Transform transformCache;

    //Property
    /// <value>SearchScriptクラスへ</value>
    public GameObject[] Targets => targets;

    private void Awake()
    {
        pool = new ObjectPool<GameObject>(
        OnCreatePoolObject,
        OnTakeFromPool,
        OnReturnedToPool,
        OnDestroyPoolObject
        );
    }

    void Start()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] != null) continue;

            targets[i] = pool.Get();
            targets[i].name = $"enemy{i}";
        }
    }

    void Update()
    {
        if (Time.frameCount % interval == 0f)
        {
            ManageEnemy();
        }
    }

    void ManageEnemy()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null)
            {
                targets[i] = pool.Get();
            }

            if (!targets[i].activeSelf)
            {
                var obj = targets[i];
                pool.Get(out targets[i]);
                pool.Release(obj);
            }
        }
    }

    GameObject OnCreatePoolObject()
    {
        var enemyCache = Instantiate(enemyPrefab, transformCache);

        return enemyCache;
    }

    void OnTakeFromPool(GameObject target) => target.SetActive(true);

    void OnReturnedToPool(GameObject target) => target.SetActive(false);

    void OnDestroyPoolObject(GameObject target) => Destroy(target);

}
