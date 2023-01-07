public class Entity
{
    public Pool health;
    public Pool aura;

    public Entity(int maxHealth, int maxAura)
    {
        health = new Pool(maxHealth);
        aura = new Pool(maxAura);
    }

    public void takeDamage(int damage)
    {
        if (aura > 0)
        {
            int leftOverDamage = damage - aura;
            aura.Value -= damage;
            if (leftOverDamage > 0)
            {
                health.Value -= leftOverDamage;
            }
        }
        else
        {
            health.Value -= damage;
        }
    }

    public void heal(int healing)
    {
        if (health < health.maxValue)
        {
            int leftOverHealing = healing - (health.maxValue - health);
            health.Value += healing;
            if (leftOverHealing > 0)
            {
                aura.Value += leftOverHealing;
            }
        }
        else
        {
            aura.Value += healing;
        }
    }
}