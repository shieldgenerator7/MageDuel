

using UnityEngine;

public class SpellContext
{
    public Spell spell;
    public Player target;
    public Player caster;
    private int focusSpent;
    private int auraSpent;

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

    public Spell Spell => spell;

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

    public Element element => spell.element;

    public string Description
    {
        get
        {
            string desc = this.spell.description;
            foreach (SpellAttribute attr in spell.attributes)
            {
                string attrName = attr.name;
                desc = desc.Replace($"[{attrName}]", $"{getAttribute(attrName)}");
            }
            return desc;
        }
    }

    public int getAttribute(string attrName)
    {
        SpellAttribute attr = spell.getAttribute(attrName);
        int value = attr?.value ?? 0;
        if (attr?.rampable ?? false)
        {
            value += auraSpent;
        }
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