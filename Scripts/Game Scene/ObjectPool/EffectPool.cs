using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class EffectPool : MonoBehaviour
{
    //自作オブジェクトプール擬き
    public static EffectPool Instance;

    //Field
    [SerializeField] FireMagic effectPrefab;
    [SerializeField] List<GameObject> pool = new List<GameObject>(32);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        var defaultMax = 16;

        //poolにキャッシュ
        for (int i = 0; i < defaultMax; i++)
        {
            var fireMagic = Instantiate(effectPrefab.gameObject, this.transform);
            pool.Add(fireMagic);
            fireMagic.SetActive(false);
        }
    }

    /// <summary>
    ///Bombを表示する関数
    /// </summary>
    public GameObject MakeEffect()
    {
        //poolから再利用
        var reUsed = pool
            .FirstOrDefault(i => !i.activeSelf);

        if (ReferenceEquals(reUsed, null))
        {
            reUsed = Instantiate(effectPrefab.gameObject, this.transform);
            pool.Add(reUsed);
        }

        reUsed.SetActive(true);

        return reUsed;
    }
}
