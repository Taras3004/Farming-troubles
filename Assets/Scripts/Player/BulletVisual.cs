using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Bullet))]
public class BulletVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem obstacleHitParticle;

    private Bullet bullet;


    private void Start()
    {
        bullet.OnObstacleHit += Bullet_OnObstacleHit;
    }

    private void Awake()
    {
        bullet = GetComponent<Bullet>();
    }

    private void Bullet_OnObstacleHit(object sender, EventArgs e)
    {
        ParticleSystem particle = Instantiate(obstacleHitParticle, transform.position, Quaternion.identity);
        particle.AddComponent<SelfDestroy>().Setup(particle.main.duration);
    }
}