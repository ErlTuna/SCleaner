using System;
using UnityEngine;

public class AwesomeTest : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int damage = 10;
    public static event Action<int> OnDamageTaken;


    void Start()
    {
        currentHealth = maxHealth;
    }
    public void DoDamage()
    {
        int newHealth = currentHealth - damage;

        if (newHealth < 0) newHealth = 0;
        else currentHealth = newHealth;

        OnDamageTaken?.Invoke(currentHealth);
    }
}
