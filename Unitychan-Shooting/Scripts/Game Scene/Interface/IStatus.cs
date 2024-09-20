using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatus
{
    //Property
    public int Hp { get; }

    public bool IsAlive { get; }
}