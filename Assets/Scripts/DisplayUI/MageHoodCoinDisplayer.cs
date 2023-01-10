using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MageHoodCoinDisplayer : PlayerDisplayUI
{
    public Sprite eyeClosed;
    public Sprite eyeOpened;
    public Image imgEyes;

    protected override void _registerDelegates(bool register)
    {
        player.onStateChanged -= updateState;
        if (register)
        {
            player.onStateChanged += updateState;
        }
    }

    public override void forceUpdate()
    {
        updateState(player.State);
    }

    private void updateState(Player.PlayState playState)
    {
        imgEyes.sprite = (playState == Player.PlayState.CASTING) ?eyeOpened : eyeClosed;
    }
}
