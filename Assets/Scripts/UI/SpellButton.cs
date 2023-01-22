using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class SpellButton : PlayerDisplayUI
{
    [Tooltip("The index into the deck for this button")]
    public int index = 0;

    public PageControllerUI pageController;
    public SpellDisplayer spellDisplayer;

    private Spell spell;

    protected override void _registerDelegates(bool register)
    {
        player.onDeckChanged -= onDeckChanged;
        if (register)
        {
            player.onDeckChanged += onDeckChanged;
        }
    }

    public override void forceUpdate()
    {
        onDeckChanged(player.Deck);
    }
    private void onDeckChanged(Deck deck)
    {
        spell = player.Deck[index];
        pageController.init(spell);
        spellDisplayer.init(spell);
    }
}
