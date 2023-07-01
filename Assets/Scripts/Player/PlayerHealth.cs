using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;
    
    public event EventHandler OnHealthDecreased;
    public event EventHandler OnHealthIncreased;
    public event EventHandler OnDie;
    
    [SerializeField] private int maxHealth;
    private int currHealth;
    public int CurrentHealth()
    {
        return currHealth;
    }

    private void Awake()
    {
        Instance = this;
        currHealth = maxHealth;
    }

    private void Start()
    {
        OnDie += OnOnDie;
    }

    private void OnOnDie(object sender, EventArgs e)
    {
        GameInput.Instance.OnRespawnAction += InstanceOnOnRespawnAction;
    }

    private void InstanceOnOnRespawnAction(object sender, EventArgs e)
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Hit()
    {
        if(currHealth <= 0)
            return;
        
        currHealth--;
        OnHealthDecreased?.Invoke(this, EventArgs.Empty);

        if (currHealth <= 0)
        {
            OnDie?.Invoke(this, EventArgs.Empty);
        }
    }
    public void Heal()
    {
        currHealth++;
        OnHealthIncreased?.Invoke(this, EventArgs.Empty);
    }
}
