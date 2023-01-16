using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPoolDisplayer : PlayerDisplayUI
{
    public Image healthBar;
    public Image imgHealthDiff;
    public Image auraBar;
    public Image imgAuraDiff;
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
        float percent = (float)health / (float)player.health.maxValue;
        //Instant update
        healthBar.fillAmount = percent;
        //Animation
        Animation healthAnim = new Animation(imgHealthDiff, percent);
        Animation auraAnim = Managers.Animation.getAnimation(imgAuraDiff);
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
        float percent = (float)aura / (float)player.aura.maxValue;
        //Instant update
        auraBar.fillAmount = percent;
        //Animation
        Animation auraAnim = new Animation(imgAuraDiff, percent);
        Animation healthAnim = Managers.Animation.getAnimation(imgHealthDiff);
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
