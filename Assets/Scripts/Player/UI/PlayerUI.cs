using System;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private RectTransform gamePanel;
    [SerializeField] private RectTransform deathPanel;

    private void Awake()
    {
        SetPanel(UIPanels.GamePanel);
    }

    private void Start()
    {
        PlayerHealth.Instance.OnDie += PlayerHealth_OnDie;
        LevelReloader.Instance.OnLevelReloaded += LevelReloader_OnLevelReloaded;
    }

    private void LevelReloader_OnLevelReloaded(object sender, EventArgs e)
    {
        SetPanel(UIPanels.GamePanel);
    }

    private void PlayerHealth_OnDie(object sender, EventArgs e)
    {
        SetPanel(UIPanels.DeathPanel);
    }

    private void SetPanel(UIPanels panel)
    {
        gamePanel.gameObject.SetActive(false);
        deathPanel.gameObject.SetActive(false);
        
        if (panel == UIPanels.GamePanel)
        {
            gamePanel.gameObject.SetActive(true);
        }
        else if (panel == UIPanels.DeathPanel)
        {
            deathPanel.gameObject.SetActive(true);
        }
    }

    private enum UIPanels
    {
        GamePanel,
        DeathPanel
    }
}