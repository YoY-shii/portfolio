using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LockOnTextManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lockOnText;
    [SerializeField] Search search;
    Vector3 lockOnRange = new(5.5f, 5.5f, 5.5f);
    Transform transformCache;

    void Start()
    {
        lockOnText.enabled = false;
        transformCache = Player.Instance.transform;
    }

    void Update()
    {
        if (ReferenceEquals(search.SearchObj, null)) return;

        var distance = Vector3.SqrMagnitude(transformCache.position - search.SearchObj.transform.position);

        lockOnText.enabled =  distance < lockOnRange.sqrMagnitude ? true : false;
    }
}
