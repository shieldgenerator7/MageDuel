using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIVariables
{
    public Player player { get; private set; }

    private int spellBookPage = 0;
    public int SpellBookPage
    {
        get => spellBookPage;
        set
        {
            spellBookPage = Mathf.Clamp(value, 0, player.Deck.spellList.Count - 1);
            onSpellBookPaged?.Invoke(spellBookPage);
        }
    }
    public delegate void OnSpellBookPaged(int page);
    public event OnSpellBookPaged onSpellBookPaged;

    public PlayerUIVariables(Player player)
    {
        this.player = player;
    }
}
