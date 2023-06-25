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
    }

    private void PlayerHealth_OnDie(object sender, EventArgs e)
    {
        SetPanel(UIPanels.DeathPanel);
    }

    public void SetPanel(UIPanels panel)
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

    public enum UIPanels
    {
        GamePanel,
        DeathPanel
    }
}