using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public List<string> playerNames;
    public List<Placemat> placemats;

    public Game game;

    // Start is called before the first frame update
    void Start()
    {
        game = new Game();
        for (int i = 0; i < playerNames.Count; i++)
        {
            Player p = new Player(playerNames[i]);
            game.players.Add(p);
            placemats[i].setPlayer(p);
        }
        game.startGame();
    }
}
