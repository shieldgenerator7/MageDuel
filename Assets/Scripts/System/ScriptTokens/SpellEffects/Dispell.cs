using System.Collections;
using UnityEngine;

public class Dispell : SpellEffect
{
    public override void activate()
    {
        if (!checkTarget(true)) { return; }

        SpellContext target = Target0;
        target.dispell();
    }
}