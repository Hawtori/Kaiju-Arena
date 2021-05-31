using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina
{
    public bool replenishStamina;

    private float maxStamina;
    private float stamina;
    private float staminaWaitTime;
    private float staminaWaitTimer;

    public void setStaminaWait(float t)
    {
        staminaWaitTime = t;
    }

    public void useStamina(float s)
    {
        stamina -= s;
    }

    public void addStamina()
    {
        stamina += 5;
    }

    public void setMaxStamina(float s)
    {
        maxStamina = s;
    }

    public void setStamina(float s)
    {
        stamina = s;
    }

    public float getStamina()
    {
        return stamina;
    }

    public float getMaxStamina()
    {
        return maxStamina;
    }

    /// <summary>
    /// Creates an object for class stamina
    /// </summary>
    /// <param name="t"> Set stamina wait time </param>
    /// <param name="s"> Set max stamina, gives 100% stamina start </param>
    public Stamina(float t, float s)
    {
        replenishStamina = false;
        setStaminaWait(t);
        staminaWaitTimer = 0;
        setMaxStamina(s);
        setStamina(s);
    }
}
