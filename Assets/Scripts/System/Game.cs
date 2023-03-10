using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game
{
    public List<Player> players = new List<Player>();

    public int roundNumber = 1;

    private int matchupIndex = 0;
    public int MatchupIndex
    {
        get => matchupIndex;
        set
        {
            matchupIndex = Mathf.Clamp(value, 0, players.Max(player => player.castingSpeed));
            onMatchUpChanged?.Invoke(matchupIndex);
        }
    }
    public delegate void OnMatchUpChanged(int index);
    public event OnMatchUpChanged onMatchUpChanged;

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
            onPhaseChanged?.Invoke(phase);
        }
    }
    public delegate void OnPhase(GamePhase phase);
    public event OnPhase onPhaseChanged;

    public enum GameSubPhase
    {
        NONE,
        CASTING,
        PROCESSING,
    }
    private GameSubPhase subphase;
    public GameSubPhase SubPhase
    {
        get => subphase;
        private set
        {
            subphase = value;
            onSubPhaseChanged?.Invoke(subphase);
        }
    }
    public delegate void OnSubPhase(GameSubPhase subphase);
    public event OnSubPhase onSubPhaseChanged;

    private List<SpellContext> castingQueue = new List<SpellContext>();
    public List<SpellContext> CastingQueue => castingQueue.ToList();

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
        MatchupIndex = 0;
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
                MatchupIndex = 0;
                players.ForEach(player =>
                {
                    player.State = Player.PlayState.CASTING;
                });
                SubPhase = GameSubPhase.CASTING;
                setSpellsBinDran();
                break;
            case GamePhase.CLEANUP:
                players.ForEach(player =>
                {
                    player.clearLineup();
                });
                SubPhase = GameSubPhase.NONE;
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
                spell.onStateChanged -= OnSpellStateChanged;
                spell.onStateChanged += OnSpellStateChanged;
            });
        }
        else if (phase == GamePhase.MATCHUP)
        {
            if (subphase == GameSubPhase.PROCESSING)
            {
                setSpellsBinDran();
                castingQueue.FindAll(spell => !spell.canBeCastNext)
                    .ForEach(spell => queueSpell(spell, false));
            }
        }
    }

    private void OnSpellStateChanged(SpellContext.State state)
    {
        if (phase == GamePhase.MATCHUP)
        {
            if (subphase == GameSubPhase.CASTING)
            {
                bool readyToProcess = players.All(player =>
                {
                    List<SpellContext> lineup = player.Lineup;
                    if (matchupIndex >= lineup.Count)
                    {
                        return true;
                    }
                    SpellContext spell = lineup[matchupIndex];
                    if (spell == null)
                    {
                        return true;
                    }
                    return spell.state != SpellContext.State.LINEDUP;
                });
                if (readyToProcess)
                {
                    SubPhase = GameSubPhase.PROCESSING;
                }
            }
            else if (subphase == GameSubPhase.PROCESSING)
            {
                bool finishedProcessing = players.All(player =>
                {
                    List<SpellContext> lineup = player.Lineup;
                    if (matchupIndex >= lineup.Count)
                    {
                        return true;
                    }
                    SpellContext spell = lineup[matchupIndex];
                    if (spell == null)
                    {
                        return true;
                    }
                    return spell.Processed;
                });
                if (finishedProcessing)
                {
                    SubPhase = GameSubPhase.CASTING;
                }
            }
        }
    }

    public void moveToNextMatchUp()
    {
        bool endPhase = false;
        bool moveToNextMatchup = players.All(player =>
            player.Lineup.Count <= matchupIndex
            || player.Lineup[matchupIndex] == null
            || player.Lineup[matchupIndex].Processed
            );
        while (moveToNextMatchup)
        {
            MatchupIndex++;
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
        foreach (Player player in players)
        {
            List<SpellContext> spells = player.Lineup;
            for (int i = 0; i < spells.Count; i++)
            {
                SpellContext spell = spells[i];
                if (spell != null)
                {
                    spell.BinDran = !spell.Processed
                        && i == matchupIndex;
                }
            }
        }
    }

    public void queueSpell(SpellContext spellContext, bool queue)
    {
        if (queue)
        {
            if (!castingQueue.Contains(spellContext))
            {
                castingQueue.Add(spellContext);
                castingQueue = castingQueue.OrderBy(spell => (spell.Flash) ? 0 : 1)
                    .ThenByDescending(spell => spell.Speed)
                    .ThenBy(spell => spell.Focus)
                    .ThenByDescending(spell => spell.Aura).ToList();
                onQueueChanged?.Invoke(castingQueue);
            }
            spellContext.state = SpellContext.State.CASTING;
        }
        else
        {
            if (castingQueue.Contains(spellContext))
            {
                castingQueue.Remove(spellContext);
                onQueueChanged?.Invoke(castingQueue);
            }
            spellContext.state = SpellContext.State.LINEDUP;
        }
    }

    //TEST METHOD
    public void clearQueue()
    {
        Debug.LogWarning("Game.clearQueue() called!");
        castingQueue.Clear();
        onQueueChanged?.Invoke(castingQueue);
    }

    /// <summary>
    /// Processes one spell from the front of the queue
    /// </summary>
    public void processQueue()
    {
        if (castingQueue.Count > 0)
        {
            SpellContext firstSpell = castingQueue[0];
            firstSpell.activate();
            castingQueue.Remove(firstSpell);
            onQueueChanged?.Invoke(castingQueue);
        }
    }
    public delegate void OnQueueChanged(List<SpellContext> queue);
    public event OnQueueChanged onQueueChanged;


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
