using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO: make this class abstract and more general,
//then make a subclass for animating Image.fillAmount
public class Animation
{
    private float startValue = 0;
    private float endValue = 0;
    private Image img;

    public Animation(Image img, float start, float end)
    {
        this.startValue = start;
        this.endValue = end;
        this.img = img;
    }

    public void update(float percent)
    {
        img.fillAmount = Mathf.Lerp(startValue, endValue, percent);
    }
}
