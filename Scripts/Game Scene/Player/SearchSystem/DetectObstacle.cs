using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Player.Instance ←検索用
public class DetectObstacle : MonoBehaviour
{
    //Cache
    Transform transformCache;
    int LayerEnemy;
    int LayerBackGround;

    /// <value>LockOnCameraクラスへ</value>
    public bool IsObstacle { get; private set; }

    void Start()
    {
        transformCache = Player.Instance.transform;
        LayerEnemy = LayerMask.NameToLayer("Enemy");
        LayerBackGround = LayerMask.NameToLayer("BackGround");
    }

    /// <summary>
    ///障害物を判定
    /// </summary>
    public void JudgeObstacle(Search Search)
    {
        //方向を取得
        var direction = Search.SearchObj.transform.localPosition - transformCache.localPosition;
        direction.Normalize();

        //rayの高さ調節
        //yが0だと斜めの地形で反応してしまうことがあるため
        var pos = transformCache.localPosition;
        pos.y += 0.5f;
        var ray = new Ray(pos, direction);

        var maxDistance = 10f;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            if (hit.collider.gameObject.layer == LayerEnemy)
            {
                IsObstacle = false;
            }

            else if (hit.collider.gameObject.layer == LayerBackGround)
            {
                IsObstacle = true;
                //Debug.DrawRay(pos, ray.direction * 5f, Color.red, maxDistance);
                //Debug.Log(hit.collider.name);
            }
        }
    }
}
