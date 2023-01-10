using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game
{
    public List<Player> players = new List<Player>();

    public int roundNumber = 1;

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
        set
        {
            phase = value;
            onPhaseChanged.Invoke(phase);
        }
    }
    public delegate void OnPhase(GamePhase phase);
    public event OnPhase onPhaseChanged;

    public void startGame()
    {
        phase = GamePhase.READYUP;
        players.ForEach(player =>
        {
            players.ForEach(p => p.opponent = getOpponent(p));
            player.onStateChanged += onPlayStateChanged;
            player.onLineupChanged += onLineupChanged;
        });
    }

    private void nextPhase(GamePhase phase)
    {
        this.Phase = phase;
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
                players.ForEach(player =>
                {
                    player.State = Player.PlayState.CASTING;
                });
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
    }

    public void checkNextPhase()
    {
        switch(phase)
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
                if (players.All(player => player.Lineup.Count == 0))
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

    public void onPlayStateChanged(Player.PlayState playState)
    {
        if (phase == GamePhase.LINEUP)
        {
            checkNextPhase();
        }
    }
    public void onLineupChanged(List<SpellContext> spellContexts = null)
    {
        if (phase == GamePhase.MATCHUP)
        {
            checkNextPhase();
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
