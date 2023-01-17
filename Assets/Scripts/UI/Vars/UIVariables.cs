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

    private SpellContext currentlyCastingSpell = null;
    public SpellContext CurrentTargetingSpell
    {
        get => currentlyCastingSpell;
        set
        {
            currentlyCastingSpell = value;
            onCurrentCastingSpellChanged?.Invoke(currentlyCastingSpell);
        }
    }
    public delegate void OnCurrentCastingSpellChanged(SpellContext spellContext);
    public event OnCurrentCastingSpellChanged onCurrentCastingSpellChanged;

    private List<Target> validTargets = null;
    public List<Target> ValidTargets
    {
        get => validTargets;
        set
        {
            validTargets = value;
            onValidTargetsChanged?.Invoke(validTargets);
        }
    }
    public delegate void OnValidTargetsChanged(List<Target> targets);
    public event OnValidTargetsChanged onValidTargetsChanged;
}
