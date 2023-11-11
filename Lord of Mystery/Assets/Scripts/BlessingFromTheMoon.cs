using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlessingFromTheMoon : CardMachine
{
    public override IEnumerator EndOfProgress()
    {
        
        Produce_Soul(1);
        Produce_Putrefaction(1);
        Reduce_Knowledge(3);
        
        return base.EndOfProgress();
    }
}
