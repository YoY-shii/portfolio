using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    IStatus statusInterface;
    [SerializeField] ReferenceDistance referenceDistance;

    //Field
    [SerializeField] Canvas enemyUICanvas;
    [SerializeField] Slider hpSlider;
    Vector3 range = new (10, 10, 10);

    //Cache
    Transform transformCameraCache;

    private void Awake()
    {
        TryGetComponent(out statusInterface);
    }

    void Start()
    {
        hpSlider.value = statusInterface.Hp;
        transformCameraCache = Camera.main.transform;
    }

    void Update()
    {
        hpSlider.value = statusInterface.Hp;

        enemyUICanvas.enabled = referenceDistance.CalcDistance() < range.sqrMagnitude ? true : false;
    }

    private void LateUpdate()
    {
        if (!enemyUICanvas.enabled) return;
        enemyUICanvas.transform.LookAt(transformCameraCache);
    }
}
