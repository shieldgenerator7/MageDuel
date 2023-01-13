

using UnityEngine;

public class HealSpellEffect : SpellEffect
{
    public override void activate()
    {
        int heal = spellContext.getAttribute(getParameter(0));
        Debug.Log($"Heal effect: {heal}, {spellContext.target}");
        spellContext.target.heal(heal);
    }
}
