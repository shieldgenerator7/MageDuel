using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageHoodController : PlayerControlUI
{
    public override void activate()
    {
        switch (player.State)
        {
            case Player.PlayState.READYING:
            case Player.PlayState.FOCUSING:
                player.State = Player.PlayState.CASTING;
                break;
            case Player.PlayState.CASTING:
                player.State = Player.PlayState.FOCUSING;
                break;
            default:
                Debug.LogError($"Unknown state! {player.State}");
                break;
        }
    }
}
