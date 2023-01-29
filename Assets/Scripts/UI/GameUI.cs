using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameUI : MonoBehaviour
{
    public GameSettings gameSettings;

    public float castProcessingDelay = 2;

    public List<Placemat> placemats;

    public TargetArrowManager targetArrowManager;

    private Game game;
    private UIVariables uiVars;

    private Timer processQueueTimer;

    // Start is called before the first frame update
    void Start()
    {
        //Game
        game = new Game();
        for (int i = 0; i < gameSettings.playerNames.Count; i++)
        {
            Player p = new Player(gameSettings.playerNames[i]);
            p.color = gameSettings.playerColors[i];
            p.Deck ??= ((gameSettings.decks.Count > 0)
                    ? gameSettings.decks[Random.Range(0, gameSettings.decks.Count)]
                    : null
                )
                ?? gameSettings.defaultDeck;
            game.players.Add(p);
        }
        game.onPhaseChanged += onPhaseChanged;
        game.onSubPhaseChanged += onSubPhaseChanged;
        game.startGame();
        //uiVars
        uiVars = new UIVariables(game);
        uiVars.gameSettings = gameSettings;
        for (int i = 0; i < game.players.Count; i++)
        {
            Player p = game.players[i];
            placemats[i].setPlayer(p, uiVars);
        }
        //Target Arrow Displayer
        targetArrowManager.uiVars = uiVars;
    }

    void onPhaseChanged(Game.GamePhase phase)
    {
        //depending on phase, go directly into the next phase
        switch (phase)
        {
            case Game.GamePhase.READYUP:
                Managers.Timer.startTimer(1, game.checkNextPhase);
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
            Managers.Timer.startTimer(castProcessingDelay, this.processQueue);
        }
    }
    private void processQueue()
    {
        int queueCount = game.CastingQueue.Count;
        if (queueCount > 0)
        {
            game.processQueue();
            processQueueTimer = Managers.Timer.startTimer(castProcessingDelay, this.processQueue);
        }
        else
        {
            game.moveToNextMatchUp();
        }
    }

    private void Update()
    {
        if (game.Phase == Game.GamePhase.MATCHUP && game.SubPhase == Game.GameSubPhase.PROCESSING)
        {
            //TEST CODE: press space to immediately process the queue
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (processQueueTimer != null)
                {
                    processQueueTimer.canceled = true;
                    processQueueTimer = null;
                }
                processQueue();
            }
        }
        if (Input.GetKeyUp(KeyCode.Escape) && !Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Quitting");
            Application.Quit(0);
        }
    }
}
