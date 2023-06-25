using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Obstacle
{
    private const string SHAKE_TRIGGER = "Hit";
    
    [SerializeField] private Animator animator;
    public override void Interact()
    {
        animator.SetTrigger(SHAKE_TRIGGER);
    }
}
