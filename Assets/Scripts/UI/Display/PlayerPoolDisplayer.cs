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
        Animation healthAnim = new Animation(
            healthBar,
            (float)health / (float)player.health.maxValue
            );
        Animation auraAnim = Managers.Animation.getAnimation(auraBar);
        //Start the health anim now
        if (auraAnim == null)
        {
            Managers.Animation.startAnimation(healthAnim, 1);
        }
        //Start the health anim after the aura anim
        else
        {
            auraAnim.onFinished += () =>
            {
                Managers.Animation.startAnimation(healthAnim, 1);
            };
        }
    }

    private void updateAuraBar(int aura)
    {
        Animation auraAnim = new Animation(
            auraBar,
            (float)aura / (float)player.aura.maxValue
            );
        Animation healthAnim = Managers.Animation.getAnimation(healthBar);
        //Start the aura anim now
        if (healthAnim == null)
        {
            Managers.Animation.startAnimation(auraAnim, 1);
        }
        //Start the aura anim after the health anim finishes
        else
        {
            healthAnim.onFinished += () =>
            {
                Managers.Animation.startAnimation(auraAnim, 1);
            };
        }

    }

    private void updateFocusBar(int focus)
    {
        focusBar.fillAmount = (float)focus / (float)player.focus.maxValue;
    }

}
