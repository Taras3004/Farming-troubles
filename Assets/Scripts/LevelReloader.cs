using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class LevelReloader : MonoBehaviour
{
    public static LevelReloader Instance { get; private set; }

    public event EventHandler OnLevelReloaded; 

    private IEnumerable<IResetable> resetables;

    private void Awake()
    {
        Instance = this;
        resetables = FindObjectsOfType<Object>().OfType<IResetable>();
    }

    private void Start()
    {
        PlayerHealth.Instance.OnDie += PlayerHealth_OnDie;
    }

    private void PlayerHealth_OnDie(object sender, EventArgs e)
    {
        GameInput.Instance.OnRespawnAction += GameInput_OnRespawnAction;
    }

    private void GameInput_OnRespawnAction(object sender, EventArgs e)
    {
        foreach (IResetable resettable in resetables)
            resettable.Reset();
        OnLevelReloaded?.Invoke(this, EventArgs.Empty);
        GameInput.Instance.OnRespawnAction -= GameInput_OnRespawnAction;
    }
}