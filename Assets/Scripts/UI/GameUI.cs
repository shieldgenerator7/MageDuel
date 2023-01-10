using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public List<string> playerNames;
    public List<Placemat> placemats;

    public Game game;
    private UIVariables uiVars;

    // Start is called before the first frame update
    void Start()
    {
        //Game
        game = new Game();
        for (int i = 0; i < playerNames.Count; i++)
        {
            Player p = new Player(playerNames[i]);
            game.players.Add(p);
        }
        game.onPhaseChanged += onPhaseChanged;
        game.startGame();
        //uiVars
        uiVars = new UIVariables(game);
        for (int i = 0; i < game.players.Count; i++)
        {
            Player p = game.players[i];
            placemats[i].setPlayer(p, uiVars);
        }
    }

    void onPhaseChanged(Game.GamePhase phase)
    {
        //depending on phase, go directly into the next phase
        switch(phase)
        {
            case Game.GamePhase.READYUP:
                game.checkNextPhase();
                break;
            case Game.GamePhase.LINEUP:
                break;
            case Game.GamePhase.MATCHUP:
                break;
            case Game.GamePhase.CLEANUP:
                game.checkNextPhase();
                break;
            default:
                Debug.LogError($"Unknown game phase! phase: {phase}");
                break;
        }
    }
}
