using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWholeMagazineWeaponVisual : MonoBehaviour
{
    [SerializeField] private MMF_Player reloadFeedback;
    [SerializeField] private Image reloadProgressSprite;

    private EnemyWholeMagazineWeapon weapon;
    
    private void Awake()
    {
        weapon = GetComponent<EnemyWholeMagazineWeapon>();
    }

    private void Start()
    {
        weapon.EnemyHealth().OnDieEvent += EnemyHealthOnOnDieEvent;
        weapon.OnReloadAction += WeaponOnOnReloadAction;
        weapon.OnWeaponReloadProgress += WeaponOnOnWeaponReloadProgress;
    }

    private void EnemyHealthOnOnDieEvent(object sender, EventArgs e)
    {
        reloadProgressSprite.gameObject.SetActive(false);
    }

    private void WeaponOnOnReloadAction(object sender, EventArgs e)
    {
        reloadFeedback.PlayFeedbacks();
    }

    private void WeaponOnOnWeaponReloadProgress(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        reloadProgressSprite.fillAmount = e.progressNormalized;
        if (e.progressNormalized >= 0.98 || e.progressNormalized == 0)
        {
            reloadProgressSprite.gameObject.SetActive(false);
        }
        else
        {
            reloadProgressSprite.gameObject.SetActive(true);
        }
    }
}
