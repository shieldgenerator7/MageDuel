

using UnityEngine;

public class HealSpellEffect : SpellEffect
{
    public override void activate()
    {
        int heal = spellContext.getAttribute("heal");
        int healPerFocus = spellContext.getAttribute("healPerFocus");
        Debug.Log($"Heal effect: {heal}, {healPerFocus}, {spellContext.target}");
        spellContext.target.heal(heal + healPerFocus * spellContext.Focus);
    }
}
