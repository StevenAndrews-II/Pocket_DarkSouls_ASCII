using System;
using System.Data;

public class HealthSystem
{
    private int maxHealth               = 100;
	private int health_ammount          = 100;
    private int lives_ammount           = 2;

    private bool bleed_effect           = false;    // bleed effects - bleed out over time    // room or action event 
    private int bleed_effect_time       = 0;
    private int bleed_effect_maxTime    = 5;
    private int bleed_effect_amt        = 1;

    private bool potion_effect          = false;   // potion increases health over time 
    private int potion_effect_time      = 0;
    private int potion_effect_maxTime   = 5;
    private int potion_effect_amt       = 2;


    public HealthSystem()
	{
	}




    public void update()
    {
        bleed();
        potion();
    }


    public string GetHealthStatus()
    {
        return health_ammount.ToString();
    }


    public int SetMaxHealth(int amount)
    {
        maxHealth = amount;
        return maxHealth;
    }


    public void addLives(int amt)
    {
        lives_ammount += amt;
    }


    public bool useLife()
    {
        if (lives_ammount > 0)
        {
            lives_ammount -= 1;
            return true;
        }
        return false;
    }


    public bool hasLives()
    {
        if (lives_ammount > 0)
        {
            return true;
        }
        return false;
    }


    public void respawn()
    {
        health_ammount = maxHealth;
    }


    public bool isAlive()
    {
        if (health_ammount >0)
        {
            return true;
        }
        return false;
    }


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


    public void potion_settings(int maxTime, int amt)
    {
        potion_effect_maxTime = maxTime;
        potion_effect_amt = amt;
    }

    public void bleed_settings(int maxTime, int amt)
    {
        bleed_effect_maxTime = maxTime;
        bleed_effect_amt = amt;
    }


    // Helper functions - time based effects 
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
