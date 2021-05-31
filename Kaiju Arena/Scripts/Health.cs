using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public float maxHealth;
    private float health;
    private int deathCount = 0;
    public int maxDeath;
    public HealthBar healthBar;

    private float deathTimer = 0;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if(deathCount == maxDeath)
        {
            die();
        }
    }
    private void softDie()
    {
        deathCount++;
        maxHealth *= 0.75f;
        health = maxHealth;
        if(healthBar != null)
            healthBar.setMaxHealth(health);
        Vector3 scale = GetComponent<Transform>().localScale;
        scale *= 0.75f;
        GetComponent<Transform>().localScale = scale;
    }

    private void die()
    {
        if (deathTimer >= 2f)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(0);
        }
        else deathTimer += Time.deltaTime;
    }

    public void decreaseHealth(float amount)
    {
        health -= amount;
        if (health <= 0) softDie();
    }

    public int getDeaths()
    {
        return deathCount;
    }

    public float getHealth()
    {
        return health;
    }

}
