using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Limb : MonoBehaviour
{
    public abstract void LevelUp();
    public int stage { get; set; }
}
