using System;
using System.Data;

public class HealthSystem
{
    private int maxHealth               = 100;
	private int health_ammount          = 50;

    public EntityEvents HealthEvents { get; private set; }

    private int fire_defense            = 0; // updated from inventory equiped slots 
    private int physical_defense        = 0;
    private int magic_defense           = 0;



    private int lives_ammount           = 2;




    private bool bleed_effect           = false;    // bleed effects - bleed out over time    // room or action event 
    private int bleed_effect_time       = 0;
    private int bleed_effect_maxTime    = 5;
    private int bleed_effect_amt        = 1;

    private bool potion_effect          = false;   // potion increases health over time 
    private int potion_effect_time      = 0;
    private int potion_effect_maxTime   = 5;
    private int potion_effect_amt       = 2;


    public HealthSystem(EntityEvents events)
    {
        HealthEvents                    = events;
        HealthEvents.OnHealRequested    += reginerate;   // healing event
        HealthEvents.OnHitRequested     += Hit;          // hit event
    }

    public void  UpdateDefenseStats(int p_,int f_,int m_) 
    {
        physical_defense    = p_;
        fire_defense        = f_;
        magic_defense       = m_;
    }

    /// <summary>
    /// Internal update function - handles time based effects like bleed and potion effects, should be called in player update loop
    /// </summary>
    public void update()
    {
        bleed();
        potion();
    }

    /// <summary>
    /// Gets the current health status as a string, can be used for UI display or other purposes.
    /// </summary>
    /// <returns>The current health amount as a string.</returns>
    public string GetHealthStatus()
    {
        return health_ammount.ToString();
    }

    /// <summary>
    /// sets the max health of the player, can be used for leveling up or equipping items that increase max health,
    /// also updates current health to match new max health if current health exceeds it.
    /// </summary>
    /// <param name="amount">The new maximum health value.</param>
    /// <returns>The updated maximum health value.</returns>
    public int SetMaxHealth(int amount)
    {
        maxHealth = amount;
        return maxHealth;
    }

    /// <summary>
    /// Adds a specified amount of lives to the player's current life count.
    /// This can be used for power-ups, rewards, or other game mechanics that grant extra lives.
    /// </summary>
    /// <param name="amt"></param>
    public void addLives(int amt)
    {
        lives_ammount += amt;
    }

    /// <summary>
    /// Uses one life if the player has any remaining. If a life is used, it decrements the life count and returns true.
    /// </summary>
    /// <returns>True if a life was used; otherwise, false.</returns>
    public bool useLife()
    {
        if (lives_ammount > 0)
        {
            lives_ammount -= 1;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Has lives checks if the player has any remaining lives. 
    /// It returns true if the life count is greater than zero, indicating that the player can continue playing or respawn after death. 
    /// </summary>
    /// <returns>True if the player has any remaining lives; otherwise, false.</returns>
    public bool hasLives()
    {
        if (lives_ammount > 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Respawn resets the player's health to the maximum health value.
    /// This can be used when the player dies and needs to respawn with full health.
    /// </summary>
    public void respawn()
    {
        health_ammount = maxHealth;
    }

    /// <summary>
    /// Checks if the player is alive by verifying if the current health amount is greater than zero.
    /// </summary>
    /// <returns>True if the player is alive; otherwise, false.</returns>
    public bool isAlive()
    {
        if (health_ammount >0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Hit reduces the player's health by a specified amount. If the damage exceeds the current health, it sets health to zero. 
    /// </summary>
    /// <param name="decriment_ammount ">The amount of damage to apply.</param>
	public void Hit(int decriment_ammount)
	{
		if (health_ammount > decriment_ammount)
		{
            health_ammount -= decriment_ammount;
		}
		else
		{
            health_ammount = 0; 
        }
	}

    /// <summary>
    /// Reginerate increases the player's health by a specified amount. If the healing exceeds the maximum health, it sets health to the maximum value.
    /// </summary>
    /// <param name="incriment_ammount">The amount of health to restore.</param>
    public void reginerate(int incriment_ammount)
    {
        if (health_ammount < maxHealth)
        {
            health_ammount += incriment_ammount;
        }
        else
        {
            health_ammount = maxHealth;
        }
    }

    /// <summary>
    /// Potion settings configures the parameters for a potion effect that increases health over time.
    /// </summary>
    /// <param name="maxTime">The maximum duration of the potion effect.</param>
    /// <param name="amt">The amount of health to restore per tick.</param>
    public void potion_settings(int maxTime, int amt)
    {
        potion_effect_maxTime = maxTime;
        potion_effect_amt = amt;
    }
    /// <summary>
    /// Bleed settings configures the parameters for a bleed effect that decreases health over time.
    /// </summary>
    /// <param name="maxTime">The maximum duration of the bleed effect.</param>
    /// <param name="amt">The amount of health to decrease per tick.</param>
    public void bleed_settings(int maxTime, int amt)
    {
        bleed_effect_maxTime = maxTime;
        bleed_effect_amt = amt;
    }


    /// <summary>
    /// Bleed applies a bleed effect that decreases the player's health over time. It checks if the bleed effect is active and increments the bleed effect timer.
    /// If the timer is within the maximum duration, it applies damage to the player. Once the timer exceeds the maximum duration, it deactivates the bleed effect.
    /// </summary>
    private void bleed()
    {
        if (bleed_effect)
        {
            bleed_effect_time++;
            if (bleed_effect_time < bleed_effect_maxTime)
            {
                Hit(bleed_effect_amt);
            }
            else
            {
                bleed_effect = false;
            }
        }
    }

    /// <summary>
    /// potion applies a potion effect that increases the player's health over time. It checks if the potion effect is active and increments the potion effect timer.
    /// </summary>
    private void potion()
    {
        if (potion_effect)
        {
            potion_effect_time++;
            if (potion_effect_time < potion_effect_maxTime)
            {
                reginerate(potion_effect_amt);
            }
            else
            {
                potion_effect = false;
            }
        }
    }


}
