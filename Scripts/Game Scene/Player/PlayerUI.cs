using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    IStatus statusInterface;

    //Field
    [SerializeField] Slider hpSlider;

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
    }

    private void LateUpdate()
    {
        //常にUIをカメラの方向にする
        //hpSlider.transform.rotation = transformCameraCache.rotation; //Left to Right
        hpSlider.transform.LookAt(transformCameraCache); //Right to Left
    }
}
