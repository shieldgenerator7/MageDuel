using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MageHoodCoinDisplayer : PlayerDisplayUI
{
    public Sprite eyeClosed;
    public Sprite eyeOpened;
    public Image imgEyes;
    public Image imgHood;

    protected override void _registerDelegates(bool register)
    {
        if (player)
        {
            player.onStateChanged -= updateState;
        }
        if (register)
        {
            player.onStateChanged += updateState;
        }
    }

    public override void forceUpdate()
    {
        imgHood.color = player.color;
        updateState(player.State);
    }

    private void updateState(Player.PlayState playState)
    {
        imgEyes.sprite = (playState == Player.PlayState.CASTING) ?eyeOpened : eyeClosed;
    }
}
