using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : Limb
{
    // Start is called before the first frame update
    public override int[] xpThresholds { get; } = { 100, 200, 300 }; // Example values

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void LevelUp()
    {
        throw new System.NotImplementedException();
    }
}
