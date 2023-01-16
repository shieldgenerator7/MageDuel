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

    private Queue<Animation> animationQueue = new Queue<Animation>();

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
        //Show damage anim
        if (percent < healthBar.fillAmount)
        {
            //Instant update
            healthBar.fillAmount = percent;
            //Animation
            Animation healthAnim = new Animation(imgHealthDiff, percent);
            queueAnimation(healthAnim);
        }
        //Show heal anim
        else
        {
            //Instant update
            imgHealthDiff.fillAmount = percent;
            //Animation
            Animation healthAnim = new Animation(healthBar, percent);
            queueAnimation(healthAnim);
        }
    }

    private void updateAuraBar(int aura)
    {
        float percent = (float)aura / (float)player.aura.maxValue; //Show damage anim
        //Show damage anim
        if (percent < healthBar.fillAmount)
        {
            //Instant update
            auraBar.fillAmount = percent;
            //Animation
            Animation auraAnim = new Animation(imgAuraDiff, percent);
            queueAnimation(auraAnim);
        }
        //Show heal anim
        else
        {
            //Instant update
            imgAuraDiff.fillAmount = percent;
            //Animation
            Animation auraAnim = new Animation(auraBar, percent);
            queueAnimation(auraAnim);
        }
    }

    private void queueAnimation(Animation anim)
    {
        //Start anim after prev anim finishes
        if (animationQueue.Count > 0)
        {
            animationQueue.Peek().onFinished += () =>
                Managers.Animation.startAnimation(anim, 1);
        }
        //Start anim now
        else
        {
            Managers.Animation.startAnimation(anim, 1);
        }
        //Put anim in queue
        animationQueue.Enqueue(anim);
        anim.onFinished += () => animationQueue.Dequeue();//assumes this anim is at start of queue
    }

    private void updateFocusBar(int focus)
    {
        focusBar.fillAmount = (float)focus / (float)player.focus.maxValue;
    }

}
