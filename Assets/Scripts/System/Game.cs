using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game
{
    public List<Player> players = new List<Player>();

    public int roundNumber = 1;
    public int matchupIndex = 0;

    public enum GamePhase
    {
        READYUP,
        LINEUP,
        MATCHUP,
        CLEANUP,
    }
    private GamePhase phase;
    public GamePhase Phase
    {
        get => phase;
        private set
        {
            phase = value;
            onPhaseChanged.Invoke(phase);
        }
    }
    public delegate void OnPhase(GamePhase phase);
    public event OnPhase onPhaseChanged;

    public void startGame()
    {
        players.ForEach(player =>
        {
            player.game = this;
            player.opponent = getOpponent(player);
            player.onStateChanged += onPlayStateChanged;
            player.onLineupChanged += onLineupChanged;
        });
        roundNumber = 1;
        matchupIndex = 0;
        Phase = GamePhase.READYUP;
    }

    private void nextPhase(GamePhase phase)
    {
        this.phase = phase;
        switch (phase)
        {
            case GamePhase.READYUP:
                roundNumber++;
                players.ForEach(player =>
                {
                    player.readyUp();
                });
                break;
            case GamePhase.LINEUP:
                players.ForEach(player =>
                {
                    player.State = Player.PlayState.FOCUSING;
                });
                break;
            case GamePhase.MATCHUP:
                matchupIndex = 0;
                players.ForEach(player =>
                {
                    player.State = Player.PlayState.CASTING;
                });
                setSpellsBinDran();
                break;
            case GamePhase.CLEANUP:
                players.ForEach(player =>
                {
                    player.clearLineup();
                });
                break;
            default:
                Debug.LogError($"Unknown phase: {phase}");
                break;
        }
        onPhaseChanged?.Invoke(phase);
    }

    public void checkNextPhase()
    {
        switch (phase)
        {
            case GamePhase.READYUP:
                nextPhase(GamePhase.LINEUP);
                break;
            case GamePhase.LINEUP:
                //if all players are done lining spells up,
                if (players.All(player => player.State == Player.PlayState.CASTING))
                {
                    //move to matchup phase
                    nextPhase(GamePhase.MATCHUP);
                }
                break;
            case GamePhase.MATCHUP:
                //if all players are done casting spells,
                if (players.All(player => player.Lineup.All(
                    spell => spell == null || spell.Processed
                    )))
                {
                    //move to cleanup phase
                    nextPhase(GamePhase.CLEANUP);
                }
                break;
            case GamePhase.CLEANUP:
                nextPhase(GamePhase.READYUP);
                break;
            default:
                Debug.LogError($"Unknown phase: {phase}");
                break;
        }
    }

    private void onPlayStateChanged(Player.PlayState playState)
    {
        if (phase == GamePhase.LINEUP)
        {
            checkNextPhase();
        }
    }
    private void onLineupChanged(List<SpellContext> spellContexts = null)
    {
        if (phase == GamePhase.LINEUP)
        {
            spellContexts.ForEach(spell =>
            {
                spell.OnSpellResolved -= OnSpellResolved;
                spell.OnSpellResolved += OnSpellResolved;
            });
        }
    }

    private void OnSpellResolved(SpellContext spellContext)
    {
        bool endPhase = false;
        bool moveToNextMatchup = players.All(player =>
            player.Lineup.Count <= matchupIndex
            || player.Lineup[matchupIndex] == null
            || player.Lineup[matchupIndex].Processed
            );
        while (moveToNextMatchup)
        {
            matchupIndex++;
            endPhase = players.All(player => matchupIndex >= player.castingSpeed);
            if (endPhase)
            {
                break;
            }
            moveToNextMatchup = players.All(player =>
                player.Lineup.Count <= matchupIndex
                || player.Lineup[matchupIndex] == null
                || player.Lineup[matchupIndex].Processed
                );
        }
        if (endPhase)
        {
            checkNextPhase();
        }
        else
        {
            setSpellsBinDran();
        }
    }

    private void setSpellsBinDran()
    {
        List<SpellContext> currentSpells = new List<SpellContext>();
        foreach (Player player in players)
        {
            List<SpellContext> spells = player.Lineup;
            for (int i = 0; i < spells.Count; i++)
            {
                SpellContext spell = spells[i];
                if (spell == null) { continue; }
                bool binDran = !spell.Processed
                    && i == matchupIndex;
                spell.BinDran = binDran;
                if (binDran)
                {
                    currentSpells.Add(spell);
                }
            }
        }
        //Resolve ties
        if (currentSpells.Count > 1)
        {
            currentSpells.ForEach(spell => spell.BinDran = false);
            //Flash spells faster
            if (currentSpells.Count > 1)
            {
                List<SpellContext> flashSpells = currentSpells.FindAll(spell => spell.spell.keywords.Contains(Spell.Keyword.FLASH));
                if (flashSpells.Count > 0)
                {
                    currentSpells = flashSpells;
                }
            }
            //Higher spell speed faster
            if (currentSpells.Count > 1)
            {
                int maxSpeed = currentSpells.Max(spell => spell.spell.speed);
                currentSpells = currentSpells.FindAll(spell => spell.spell.speed == maxSpeed);
            }
            //Lower Focus faster
            if (currentSpells.Count > 1)
            {
                int lowFocus = currentSpells.Min(spell => spell.Focus);
                currentSpells = currentSpells.FindAll(spell => spell.Focus == lowFocus);
            }
            //Higher Aura faster
            if (currentSpells.Count > 1)
            {
                int maxAura = currentSpells.Max(spell => spell.Aura);
                currentSpells = currentSpells.FindAll(spell => spell.Aura == maxAura);
            }
            //Fizzle clashing spells
            if (currentSpells.Count > 1)
            {
                currentSpells.ForEach(spell => spell.fizzle());
            }
            //Found final spell
            else if (currentSpells.Count == 1)
            {
                currentSpells.ForEach(spell => spell.BinDran = true);
            }
            else
            {
                Debug.LogError($"Should not be possible to get here! currentSpell count: {currentSpells.Count}");
            }
        }
    }


    public Player getOpponent(Player p)
    {
        int index = players.IndexOf(p);
        if (index < 0)
        {
            throw new System.ArgumentException($"Player {p} is not in the game! index: {index}");
        }
        int oppIndex = (index + 1) % players.Count;
        return players[oppIndex];
    }
}
