using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Player.Instance ←検索用
public class EnemyMovement : MonoBehaviour
{
    IStatus statusInterface;
    [SerializeField] SearchRange searchRange;

    //Status
    float randomWalkNum = 5f;
    float time = 0;

    //Cache
    NavMeshAgent nav;
    Transform transformCache;

    private void Awake()
    {
        TryGetComponent(out statusInterface);
        TryGetComponent(out searchRange);
    }

    void Start()
    {
        TryGetComponent(out nav);
        transformCache = this.transform;
    }

    void Update()
    {
        if (!statusInterface.IsAlive) return;

        if (searchRange.IsVisible && !searchRange.IsObstacle)
        {
            Chase();
            return;
        }

        RandomWalk();
    }

    void RandomWalk()
    {
        nav.speed = 2f;
        var numX = Random.Range(-randomWalkNum, randomWalkNum);
        var numZ = Random.Range(-randomWalkNum, randomWalkNum);
        var randomPos = transformCache.position + new Vector3(numX, 0, numZ);

        time += Time.deltaTime;

        if (time > 3f)
        {
            nav.SetDestination(randomPos);
            time = 0;
        }
    }

    void Chase()
    {
        nav.speed = 3f;
        nav.SetDestination(Player.Instance.transform.position);
    }
}