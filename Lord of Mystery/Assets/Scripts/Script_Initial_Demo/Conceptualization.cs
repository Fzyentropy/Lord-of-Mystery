using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conceptualization : CardMachine
{
    public override IEnumerator EndOfProgress()
    {
        Reduce_PhysicalStrength(1);
        Reduce_Putrefaction(1);
        yield return new WaitForSeconds(1f);
        Produce_Godhood(1);

    }
}
