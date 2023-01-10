

using UnityEngine;

public class SpellContext
{
    public Spell spell;
    public Player target;
    public Player caster;
    private int focusSpent;
    public int auraSpent;

    public SpellContext(Spell spell, Player caster)
    {
        this.spell = spell;
        this.caster = caster;
        if (spell.autoTargetEnemy)
        {
            this.target = caster.opponent;
        }
        else if (spell.autoTargetSelf)
        {
            this.target = caster;
        }
    }

    public int Focus
    {
        get => focusSpent;
        set
        {
            focusSpent = value;
            onFocusChanged?.Invoke(focusSpent);
        }
    }
    public int Aura
    {
        get => auraSpent;
        set
        {
            auraSpent = value;
            onAuraChanged?.Invoke(auraSpent);
        }
    }
    public delegate void OnChargeChanged(int value);
    public event OnChargeChanged onFocusChanged;
    public event OnChargeChanged onAuraChanged;

    public int getAttribute(string attrName)
    {
        int value = spell.getAttribute(attrName);
        //TODO: modifications from other effects
        return value;
    }

    public void activate()
    {
        spell.effects = SpellScriptCompiler.compile(spell.script);
        spell.effects.ForEach(effect =>
        {
            effect.activate(this);
        });
        OnSpellResolved?.Invoke(this);
    }
    public delegate void OnSpell(SpellContext sc);
    public event OnSpell OnSpellResolved;
}