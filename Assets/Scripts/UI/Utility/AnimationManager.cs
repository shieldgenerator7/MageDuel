using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{

    private List<Animation> animations = new List<Animation>();

    public void startAnimation(Image img, float end, float duration)
    {
        startAnimation(new Animation(img, end), duration);
    }

    public void startAnimation(Animation animation, float duration)
    {
        animations.Add(animation);
        Timer timer = Managers.Timer.startTimer(
            duration,
            () => removeAnimation(animation)
            );
        timer.onTimerProgressPercent += animation.update;
    }

    private void removeAnimation(Animation animation)
    {
        animation.finish();
        animations.Remove(animation);
    }

    public Animation getAnimation(Image img)
    {
        return animations.Find(anim => anim.Image == img);
    }
}
