using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishInput : MonoBehaviour
{
    public bool ForcedTermination()
    {
        return Input.GetKeyDown("f");
    }
}
