using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soriee : CardMachine
{
   public override IEnumerator EndOfProgress()
   {
      Produce_Spirit(1);
      Produce_Knowledge(1);
      Produce_Madness(1);
      yield return null;
   }
}
