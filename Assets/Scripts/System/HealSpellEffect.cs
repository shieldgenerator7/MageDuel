

using UnityEngine;

public class HealSpellEffect : SpellEffect
{
    public override void activate(SpellContext context)
    {
        int heal = context.getAttribute("heal");
        int healPerFocus = context.getAttribute("healPerFocus");
        Debug.Log($"Heal effect: {heal}, {healPerFocus}, {context.target}");
        context.target.heal(heal + healPerFocus * context.Focus);
    }
}
