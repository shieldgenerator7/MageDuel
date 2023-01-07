using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPoolDisplayer : PlayerDisplayUI
{
    public Image healthBar;
    public Image auraBar;

    protected override void _registerDelegates(bool register)
    {
        player.health.onValueChanged -= updateHealthBar;
        player.aura.onValueChanged -= updateAuraBar;
        if (register)
        {
            player.health.onValueChanged += updateHealthBar;
            player.aura.onValueChanged += updateAuraBar;
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

}
