using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event EventHandler OnHealthDecreased;
    public event EventHandler OnHealthIncreased;
    
    [SerializeField] private int maxHealth;
    private int currHealth;
    public int CurrentHealth()
    {
        return currHealth;
    }

    private void Awake()
    {
        currHealth = maxHealth;
    }
    public void Hit()
    {
        currHealth--;
        OnHealthDecreased?.Invoke(this, EventArgs.Empty);
    }
    public void Heal()
    {
        currHealth++;
        OnHealthIncreased?.Invoke(this, EventArgs.Empty);
    }
}
