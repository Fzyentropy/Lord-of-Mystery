using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence9 : CardMachine
{
    public override IEnumerator EndOfProgress()
    {
        Produce_PhysicalStrength(1);
        Produce_Spirit(1);
        Produce_Putrefaction(1);
        return base.EndOfProgress();
    }
}
