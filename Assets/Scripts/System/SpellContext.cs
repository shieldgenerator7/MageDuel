

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellContext : Target
{
    public Spell spell;
    public Player target;
    public Player caster;
    private int focusSpent;
    private int auraSpent;
    private bool binDran = false;//"ich bin dran" -> "it's my turn"; true: it is now this spell's turn to be cast

    public enum State
    {
        LINEDUP,//in the lineup, waiting for its turn
        CASTING,//selected for casting and/or in the process of being casted
        RESOLVED,//processed and casted
        FIZZLED,//processed but not casted
    }
    private State _state = State.LINEDUP;
    public State state
    {
        get => _state;
        set
        {
            _state = value;
            onStateChanged?.Invoke(_state);
        }
    }
    public delegate void OnStateChanged(State state);
    public event OnStateChanged onStateChanged;

    private AttributeSet variables;
    private Dictionary<string, SpellContext> spellTargets = new Dictionary<string, SpellContext>();
    public List<SpellContext> SpellTargets => new List<SpellContext>(spellTargets.Values);

    private List<SpellEffect> spellEffects;

    private List<SpellEffectMod> mods = new List<SpellEffectMod>();

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
        variables = new AttributeSet(spell.attributes);
    }

    public Spell Spell => spell;

    public int Focus
    {
        get => focusSpent;
        set
        {
            focusSpent = Mathf.Clamp(value, 0, caster.focus.maxValue);
            onFocusChanged?.Invoke(focusSpent);
        }
    }
    public int Aura
    {
        get => auraSpent;
        set
        {
            auraSpent = Mathf.Clamp(value, 0, caster.aura.maxValue); ;
            onAuraChanged?.Invoke(auraSpent);
        }
    }
    public delegate void OnChargeChanged(int value);
    public event OnChargeChanged onFocusChanged;
    public event OnChargeChanged onAuraChanged;

    public bool FocusPaid => focusSpent >= spell.cost;

    public Element element => spell.element;

    public string Description
    {
        get
        {
            string desc = this.spell.description;
            //Find vars in desc
            List<string> vars = new List<string>();
            int startBrak = desc.IndexOf('[');
            while (startBrak > 0)
            {
                int endBrak = desc.IndexOf(']', startBrak);
                vars.Add(desc.Substring(startBrak + 1, endBrak - startBrak - 1));
                startBrak = desc.IndexOf('[', endBrak);
            }
            foreach (string attrName in vars)
            {
                desc = desc.Replace($"[{attrName}]", $"{getAttribute(attrName)}");
            }
            string keywordStr = "";
            foreach (Spell.Keyword keyword in spell.keywords)
            {
                keywordStr += $"<b>{keyword}</b> ";
            }
            if (!string.IsNullOrEmpty(keywordStr))
            {
                keywordStr += "\n";
            }
            desc = keywordStr + desc;
            return desc;
        }
    }

    public int Speed => getModdedValue("speed", spell.speed);

    public int getAttribute(string attrName)
    {
        if (string.IsNullOrWhiteSpace(attrName))
        {
            return 0;
        }
        SpellAttribute attr = variables.getAttribute(attrName);
        if (attr == null)
        {
            Debug.LogError($"Spell {spell.name} doesnt have attribute {attrName}!");
            return 0;
        }
        int value = attr.value;
        if (attr.rampable)
        {
            value += auraSpent;
        }
        //TODO: modifications from other effects
        return value;
    }
    public void setAttribute(string attrName, int value)
    {
        variables.set(attrName, value);
    }

    public bool hasTarget(string name)
    {
        return spellTargets.ContainsKey(name);
    }
    public SpellContext getTarget(string name)
    {
        SpellContext target = (hasTarget(name)) ? spellTargets[name] : null;
        if (target == null)
        {
            Debug.LogError($"Spell {spell.name} doesnt have target {name}!");
        }
        return target;
    }
    private void setTarget(string name, SpellContext spellContext)
    {
        spellTargets[name] = spellContext;
        onTargetChanged?.Invoke(SpellTargets);
    }
    public delegate void OnTargetChanged(List<SpellContext> targets);
    public event OnTargetChanged onTargetChanged;
    public bool hasAllTargets()
    {
        return spell.spellTargets.All(target => hasTarget(target.name));
    }
    public void acceptTargetAsNext(SpellContext spellContext)
    {
        SpellTarget target = spell.spellTargets.Find(target => !hasTarget(target.name));
        setTarget(target.name, spellContext);
    }
    public bool isTargetingSpell(SpellContext spellContext)
    {
        return spellTargets.ContainsValue(spellContext);
    }
    public void clearTargets()
    {
        spellTargets.Clear();
    }
    public void redirect(Player newTarget)
    {
        Player oldTarget = target;
        target = newTarget;
        //Redirect spells
        List<SpellContext> oldLineup = oldTarget.Lineup;
        List<SpellContext> newLineup = newTarget.Lineup;
        List<string> targetNames = spellTargets.Keys.ToList();
        foreach (string targetName in targetNames)
        {
            SpellContext spell = spellTargets[targetName];
            int index = oldLineup.IndexOf(spell);
            //TODO: check to make sure new spell target is a valid target
            setTarget(targetName, newLineup[index]);
        }
    }

    public void addMod(SpellEffectMod mod, bool add)
    {
        if (add)
        {
            if (!mods.Contains(mod))
            {
                mods.Add(mod);
            }
        }
        else
        {
            mods.Remove(mod);
        }
    }
    private int getModdedValue(string name, int value)
    {
        int ret = value;
        mods.ForEach(mod => ret = mod.modVariable(name, ret));
        return ret;
    }

    public bool BinDran
    {
        get => binDran;
        set
        {
            binDran = value;
            onBinDran?.Invoke(binDran);
        }
    }
    public delegate void OnBinDran(bool binDran);
    public event OnBinDran onBinDran;

    public bool canBeCastNext
        => !Processed && (binDran || Flash);

    public bool Flash => spell.keywords.Contains(Spell.Keyword.FLASH);

    public void acceptCompile(List<SpellEffect> spellEffects)
    {
        this.spellEffects = spellEffects;
    }

    public void activate()
    {
        //Early exit: Not enough focus
        if (!FocusPaid)
        {
            fizzle();
            return;
        }
        //Target player
        if (target)
        {
            caster.targetPlayer(target, this);
        }
        //Activate spell
        spellEffects.ForEach(effect =>
        {
            effect.init(this);
            effect.activate();
        });
        //Mark this spell resolved
        state = State.RESOLVED;
        OnSpellProcessed?.Invoke(this);
    }
    public delegate void OnSpell(SpellContext sc);
    public event OnSpell OnSpellProcessed;

    public bool Processed => (new State[] {
        State.RESOLVED,
        State.FIZZLED,
    }).Contains(state);

    public void fizzle()
    {
        state = State.FIZZLED;
        OnSpellProcessed?.Invoke(this);
    }
}