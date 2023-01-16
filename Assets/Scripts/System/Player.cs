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
    public Deck deck;
    /// <summary>
    /// The spells that the player has in the lineup
    /// </summary>
    private List<SpellContext> lineup = new List<SpellContext>();
    public int castingSpeed = 5;//how many spells they can cast per round

    public Game game;
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

    public void lineupSpell(Spell spell, int index = -1)
    {
        //Put spell in lineup
        SpellContext context = (spell != null) ? new SpellContext(spell, this) : null;
        //Insert
        if (index >= 0)
        {
            lineup.Insert(index, context);
        }
        //Append
        else
        {
            lineup.Add(context);
        }
        //Remove extra spells at end
        while (lineup.Count > castingSpeed)
        {
            lineup.RemoveAt(lineup.Count - 1);
        }
        //Delegate
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

    public void targetPlayer(Player player, SpellContext spellContext)
    {
        this.onTargetingPlayer?.Invoke(player, spellContext);
        player.onTargetedByPlayer?.Invoke(this, spellContext);
    }
    public delegate void OnTargetPlayer(Player player, SpellContext spellContext);
    public event OnTargetPlayer onTargetingPlayer;
    public event OnTargetPlayer onTargetedByPlayer;

    public void readyUp()
    {
        State = PlayState.FOCUSING;
        focus.refill();
        aura.refill();
    }

    public static implicit operator bool(Player player) => player != null;
    public override string ToString()
    {
        return name;
    }
}
