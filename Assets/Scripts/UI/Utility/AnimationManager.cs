using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{

    private List<Animation> animations = new List<Animation>();

    public void startAnimation(Image img, float end, float duration)
    {
        float start = img.fillAmount;
        startAnimation(new Animation(img, start, end), duration);
    }

    public void startAnimation(Animation animation, float duration)
    {
        animations.Add(animation);
        Timer timer = Managers.Timer.startTimer(
            duration,
            () => animations.Remove(animation)
            );
        timer.onTimerProgressPercent += animation.update;
    }
}
