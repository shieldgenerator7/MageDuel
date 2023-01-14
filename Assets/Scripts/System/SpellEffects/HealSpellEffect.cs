

using UnityEngine;

public class HealSpellEffect : SpellEffect
{
    public override void activate()
    {
        //Early exit: no target
        if (spellContext.target == null)
        {
            Debug.Log($"Can't heal target {spellContext.target}");
            return;
        }
        //Heal target
        int heal = spellContext.getAttribute(getParameter(0));
        Debug.Log($"Heal effect: {heal}, {spellContext.target}");
        spellContext.target.heal(heal);
    }
}
