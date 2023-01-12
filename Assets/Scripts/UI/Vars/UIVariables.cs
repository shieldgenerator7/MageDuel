using System.Collections;
using System.Collections.Generic;

public class UIVariables
{
    public Game game { get; private set; }
    public List<PlayerUIVariables> playerVars { get; private set; }

    public UIVariables(Game game)
    {
        this.game = game;
        playerVars = game.players.ConvertAll(player => new PlayerUIVariables(player));
    }

    public PlayerUIVariables getPlayerVariables(Player player)
    {
        return playerVars.Find(vars => vars.player == player);
    }
}