using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealthPoint healthPoint;
    [SerializeField] private PlayerHealth player;

    private List<PlayerHealthPoint> healthPointsList = new List<PlayerHealthPoint>();

    private void Awake()
    {
        healthPoint.gameObject.SetActive(false);
    }
    private void Start()
    {
        CreateVisual();
        player.OnHealthIncreased += Player_OnHealthIncreased;
        player.OnHealthDecreased += Player_OnHealthDecreased;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            DestroyHealthPoint();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            AddHealthPoint();
        }
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
        for (int i = 0; i < player.CurrentHealth(); i++)
        {
            CreateHealthPoint(out PlayerHealthPoint playerHealthPoint);
        }
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
        if (healthPointsList == null)
            return;
        PlayerHealthPoint lastHealthPoint = healthPointsList[healthPointsList.Count - 1];
        healthPointsList.Remove(lastHealthPoint);
        lastHealthPoint.Destroy();
    }
}
