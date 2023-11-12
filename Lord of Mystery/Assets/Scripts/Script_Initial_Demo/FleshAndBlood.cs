using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleshAndBlood : CardMachine 
{
    public override IEnumerator EndOfProgress()
    {
        Reduce_PhysicalStrength(1);
        return base.EndOfProgress();
    }
}
