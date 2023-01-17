using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public Deck defaultDeck;

    public List<string> playerNames;
    public List<Placemat> placemats;

    public TargetArrowDisplayer targetArrowDisplayer;

    private Game game;
    private UIVariables uiVars;

    // Start is called before the first frame update
    void Start()
    {
        //Game
        game = new Game();
        for (int i = 0; i < playerNames.Count; i++)
        {
            Player p = new Player(playerNames[i]);
            p.deck ??= defaultDeck;
            game.players.Add(p);
        }
        game.onPhaseChanged += onPhaseChanged;
        game.onSubPhaseChanged += onSubPhaseChanged;
        game.startGame();
        //uiVars
        uiVars = new UIVariables(game);
        for (int i = 0; i < game.players.Count; i++)
        {
            Player p = game.players[i];
            placemats[i].setPlayer(p, uiVars);
        }
        //Target Arrow Displayer
        targetArrowDisplayer.uiVars = uiVars;
    }

    void onPhaseChanged(Game.GamePhase phase)
    {
        //depending on phase, go directly into the next phase
        switch (phase)
        {
            case Game.GamePhase.READYUP:
                Managers.Timer.startTimer(3, game.checkNextPhase);
                break;
            case Game.GamePhase.LINEUP:
                break;
            case Game.GamePhase.MATCHUP:
                break;
            case Game.GamePhase.CLEANUP:
                Managers.Timer.startTimer(3, game.checkNextPhase);
                break;
            default:
                Debug.LogError($"Unknown game phase! phase: {phase}");
                break;
        }
    }

    private void onSubPhaseChanged(Game.GameSubPhase subphase)
    {
        if (subphase == Game.GameSubPhase.PROCESSING)
        {
            Timer lastTimer = null;
            int queueCount = game.CastingQueue.Count;
            Func<Timer> startTimerFunc = () =>
                Managers.Timer.startTimer(1, game.processQueue);
            for (int i = 0; i < queueCount; i++)
            {

                if (lastTimer == null)
                {
                    lastTimer = startTimerFunc();
                }
                else if (lastTimer != null)
                {
                    Timer timer = startTimerFunc();
                    timer.stop();
                    lastTimer.onTimerFinished += timer.reset;
                    lastTimer = timer;
                }
            }
            if (lastTimer != null)
            {
                Timer timer = Managers.Timer.startTimer(1, game.moveToNextMatchUp);
                timer.stop();
                lastTimer.onTimerFinished += timer.reset;
                lastTimer = timer;
            }
            else
            {
                game.moveToNextMatchUp();
            }
        }
    }
}
