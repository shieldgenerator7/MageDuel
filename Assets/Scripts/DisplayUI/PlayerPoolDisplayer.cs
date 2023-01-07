using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPoolDisplayer : MonoBehaviour
{
    public Player player;
    public Image healthBar;
    public Image auraBar;

    // Start is called before the first frame update
    void OnEnable()
    {
        player = new Player("player1");//TEST
        registerDelegates(true);
        updateHealthBar(player.health);
        updateAuraBar(player.aura);
    }
    private void OnDisable()
    {
        registerDelegates(false);
    }

    void registerDelegates(bool register)
    {
        player.health.onValueChanged -= updateHealthBar;
        player.aura.onValueChanged -= updateAuraBar;
        if (register)
        {
            player.health.onValueChanged += updateHealthBar;
            player.aura.onValueChanged += updateAuraBar;
        }
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
