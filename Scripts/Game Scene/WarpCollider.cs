using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpCollider : MonoBehaviour
{
    //Field
    float terrainX = 0;
    float terrainY = 2f;
    float terrainZ = 0;

    //Status
    int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.position = new Vector3(terrainX,terrainY,terrainZ);

            var iDamager = other.gameObject.GetComponent<IInjurer>();

            if (iDamager != null)
            {
                iDamager.Damage(damage);
            }
        }
    }
}
