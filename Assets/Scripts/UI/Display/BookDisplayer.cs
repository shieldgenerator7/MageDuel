using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookDisplayer : PlayerDisplayUI
{
    public List<Image> imgColors;
    public TMP_Text txtName;
    public TMP_Text txtDescription;

    private Spell spell;
    private PlayerUIVariables playerVars;

    protected override void _registerDelegates(bool register)
    {
        playerVars = uiVars.getPlayerVariables(player);
        playerVars.onSpellBookPaged -= onPageTurned;
        if (register)
        {
            playerVars.onSpellBookPaged += onPageTurned;
        }
    }

    public override void forceUpdate()
    {
        onPageTurned(playerVars.SpellBookPage);
    }

    public void onPageTurned(int index)
    {
        spell = player.Deck[playerVars.SpellBookPage];
        imgColors.ForEach(img => img.color = spell.element.color);
        txtName.text = spell.name;
        txtDescription.text = spell.Description;
    }

}
