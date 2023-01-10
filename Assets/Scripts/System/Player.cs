using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Player : Entity
{
    public string name;
    public Pool focus = new Pool(7);
    /// <summary>
    /// The spells that the player knows
    /// </summary>
    public List<Spell> spellList = new List<Spell>();
    /// <summary>
    /// The spells that the player has in the lineup
    /// </summary>
    private List<SpellContext> lineup = new List<SpellContext>();

    public Player opponent;//TODO: remove this (assuming it doesnt need it in the future)

    public enum PlayState
    {
        READYING,
        FOCUSING,
        CASTING,
    }
    private PlayState playState = PlayState.FOCUSING;
    public PlayState State
    {
        get => playState;
        set
        {
            playState = value;
            onStateChanged?.Invoke(playState);
            //prevent softlocks (TEST CODE)
            if (playState == PlayState.CASTING && lineup.Count == 0)
            {
                State = PlayState.FOCUSING;
            }
        }
    }
    public delegate void OnPlayState(PlayState state);
    public event OnPlayState onStateChanged;


    public Player(string name) : base(3, 5)
    {
        this.name = name;
    }

    public void focusSpell(SpellContext spellContext, int focus, int aura = 0, bool toSpell = true)
    {
        if (focus < 0)
        {
            Debug.LogError($"Focus must be 0 or greater! focus: {focus}");
            return;
        }
        if (aura < 0)
        {
            Debug.LogError($"Aura must be 0 or greater! aura: {aura}");
            return;
        }
        //Focus
        if (toSpell)
        {
            if (this.focus >= focus)
            {
                this.focus.Value -= focus;
                spellContext.Focus += focus;
            }
        }
        else
        {
            if (spellContext.Focus >= focus)
            {
                spellContext.Focus -= focus;
                this.focus.Value += focus;
            }
        }
        //Aura
        if (toSpell)
        {
            if (this.aura >= aura)
            {
                this.aura.Value -= aura;
                spellContext.Aura += aura;
            }
        }
        else
        {
            if (spellContext.Aura >= aura)
            {
                spellContext.Aura -= aura;
                this.aura.Value += aura;
            }
        }
    }

    public void lineupSpell(Spell spell)
    {
        SpellContext context = new SpellContext(spell, this);
        lineup.Add(context);
        context.OnSpellResolved += removeSpellFromLineup;
        onLineupChanged?.Invoke(lineup);
    }
    public delegate void OnLineupChanged(List<SpellContext> spellList);
    public event OnLineupChanged onLineupChanged;

    public List<SpellContext> Lineup => lineup.ToList();

    public void removeSpellFromLineup(SpellContext sc)
    {
        lineup.Remove(sc);
        onLineupChanged?.Invoke(lineup);
    }

    public void clearLineup()
    {
        lineup.Clear();
        onLineupChanged?.Invoke(lineup);
    }

    public void readyUp()
    {
        State = PlayState.FOCUSING;
        focus.refill();
        aura.refill();
    }

    public static implicit operator bool(Player player) => player != null;
}
