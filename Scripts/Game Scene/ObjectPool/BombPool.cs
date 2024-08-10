using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombPool : MonoBehaviour
{
    //自作オブジェクトプール擬き
    [SerializeField] Bomb bombPrefab;

    //Field
    [SerializeField] List<GameObject> pool = new List<GameObject>(1);

    void Start()
    {
        //poolにキャッシュ
        var bombMagic = Instantiate(bombPrefab.gameObject, this.transform);
        pool.Add(bombMagic);
        bombMagic.SetActive(false);
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
            reUsed = Instantiate(bombPrefab.gameObject, this.transform);
            pool.Add(reUsed);
        }

        reUsed.SetActive(true);

        return reUsed;
    }
}