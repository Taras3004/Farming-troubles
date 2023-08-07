using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealthPoint healthPoint;
    [SerializeField] private PlayerHealth player;

    private List<PlayerHealthPoint> healthPointsList = new();

    private void Awake()
    {
        healthPoint.gameObject.SetActive(false);
    }

    private void Start()
    {
        CreateVisual();
        LevelReloader.Instance.OnLevelReloaded += LevelLoader_OnLevelReloaded;
        player.OnHealthIncreased += Player_OnHealthIncreased;
        player.OnHealthDecreased += Player_OnHealthDecreased;
    }

    private void LevelLoader_OnLevelReloaded(object sender, EventArgs e)
    {
        CreateVisual();
    }

    private void Player_OnHealthDecreased(object sender, EventArgs e)
    {
        DestroyHealthPoint();
    }

    private void Player_OnHealthIncreased(object sender, EventArgs e)
    {
        AddHealthPoint();
    }

    private void CreateVisual()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if(child == healthPoint.transform)
                continue;
            Destroy(child.gameObject);
        }
        for (int i = 0; i < player.CurrentHealth(); i++)
            CreateHealthPoint(out PlayerHealthPoint playerHealthPoint);
    }

    private void CreateHealthPoint(out PlayerHealthPoint playerHealthPoint)
    {
        PlayerHealthPoint tempHealthPoint = Instantiate(healthPoint, transform);
        playerHealthPoint = tempHealthPoint;
        healthPointsList.Add(tempHealthPoint);
        tempHealthPoint.gameObject.SetActive(true);
    }

    private void AddHealthPoint()
    {
        CreateHealthPoint(out PlayerHealthPoint playerHealthPoint);
        playerHealthPoint.Initialize();
    }

    private void DestroyHealthPoint()
    {
        if (healthPointsList == null && player.CurrentHealth() <= 0)
            return;
        PlayerHealthPoint lastHealthPoint = healthPointsList[healthPointsList.Count - 1];
        healthPointsList.Remove(lastHealthPoint);
        lastHealthPoint.Destroy();
    }
}