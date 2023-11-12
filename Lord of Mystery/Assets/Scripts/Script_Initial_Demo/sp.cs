using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sp : CardMachine
{
    public override IEnumerator EndOfProgress()
    {
        Produce_Soul(3);
        return base.EndOfProgress();
    }
}
