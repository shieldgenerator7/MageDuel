

using UnityEngine;

public class HealSpellEffect : SpellEffect
{
    public override void activate()
    {
        //Early exit: no target
        if (!checkTarget()) { return; }
        //Heal target
        int heal = Parameter0;
        Debug.Log($"Heal effect: {heal}, {spellContext.target}");
        spellContext.target.heal(heal);
    }
}
