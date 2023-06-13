using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public void Setup(float time)
    {
        Destroy(gameObject, time);
    }
}
