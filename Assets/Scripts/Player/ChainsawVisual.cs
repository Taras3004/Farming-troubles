using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainsawVisual : MonoBehaviour
{
    [SerializeField] private AudioClip cutting;
    [SerializeField] private AudioClip idle;
    [SerializeField] private ParticleSystem hitParticles;

    private Chainsaw chainsaw;
    private AudioSource audioSource;

    private void Awake()
    {
        chainsaw = GetComponent<Chainsaw>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        chainsaw.OnStartedDamaging += Chainsaw_OnStartedDamaging;
        chainsaw.OnFinishedDamaging += Chainsaw_OnFinishedDamaging;
    }

    private void Chainsaw_OnFinishedDamaging(object sender, System.EventArgs e)
    {
        FinishDamaging();
    }

    private void Chainsaw_OnStartedDamaging(object sender, System.EventArgs e)
    {
        StartDamaging();
    }

    private void StartDamaging()
    {
        audioSource.clip = cutting;
        audioSource.Play();
        hitParticles.Play();

        var main = hitParticles.main;
        main.startColor = new Color(chainsaw.ColorOfParticles().r, chainsaw.ColorOfParticles().g, chainsaw.ColorOfParticles().b, 255);
    }
    private void FinishDamaging()
    {
        audioSource.clip = idle;
        audioSource.Play();
        hitParticles.Stop();
    }
}
