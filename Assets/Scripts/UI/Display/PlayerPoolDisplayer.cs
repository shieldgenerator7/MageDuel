using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPoolDisplayer : PlayerDisplayUI
{
    public Image healthBar;
    public Image auraBar;
    public Image focusBar;

    protected override void _registerDelegates(bool register)
    {
        player.health.onValueChanged -= updateHealthBar;
        player.aura.onValueChanged -= updateAuraBar;
        player.focus.onValueChanged -= updateFocusBar;
        if (register)
        {
            player.health.onValueChanged += updateHealthBar;
            player.aura.onValueChanged += updateAuraBar;
            player.focus.onValueChanged += updateFocusBar;
        }
    }

    public override void forceUpdate()
    {
        updateHealthBar(player.health);
        updateAuraBar(player.aura);
    }

    private void updateHealthBar(int health)
    {
        healthBar.fillAmount = (float)health / (float)player.health.maxValue;
    }

    private void updateAuraBar(int aura)
    {
        auraBar.fillAmount = (float)aura / (float)player.aura.maxValue;
    }

    private void updateFocusBar(int focus)
    {
        focusBar.fillAmount = (float)focus / (float)player.focus.maxValue;
    }

}