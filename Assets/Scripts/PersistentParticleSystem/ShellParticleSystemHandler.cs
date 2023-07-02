using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShellParticleSystemHandler : MonoBehaviour
{
    public static ShellParticleSystemHandler Instance { get; private set; }

    private MeshParticleSystem meshParticleSystem;
    private List<SingleShell> singleShellsList;

    private void Awake()
    {
        Instance = this;
        meshParticleSystem = GetComponent<MeshParticleSystem>();
        singleShellsList = new List<SingleShell>();
    }

    private void Update()
    {
        foreach (SingleShell singleShell in singleShellsList)
        {
            singleShell.Update();
            if (singleShell.IsMovementComplete())
            {
                singleShellsList.Remove(singleShell);
                break;
            }
        }
    }

    public void SpawnShell(Vector3 position, Vector3 direction, ShellUVIndex shellUVIndex)
    {
        singleShellsList.Add(new SingleShell(meshParticleSystem, position, direction, shellUVIndex));
    }

    private class SingleShell
    {
        private MeshParticleSystem meshParticleSystem;
        private Vector3 position;
        private Vector3 direction;
        private int quadIndex;
        private Vector3 quadSize;
        private float rotation;
        private float moveSpeed;
        private int shellUVIndexInt;

        public SingleShell(MeshParticleSystem meshParticleSystem, Vector3 position, Vector3 direction, ShellUVIndex shellUVIndex)
        {
            this.meshParticleSystem = meshParticleSystem;
            this.position = position;
            this.direction = direction;

            quadSize = new Vector3(0.1f, 0.1f);
            rotation = Random.Range(0, 360f);
            moveSpeed = Random.Range(1.5f, 4f);
            shellUVIndexInt = (int)shellUVIndex;
            
            quadIndex = meshParticleSystem.AddQuad(position, rotation, quadSize, true, shellUVIndexInt);
        }

        public void Update()
        {
            position += direction * moveSpeed * Time.deltaTime;
            rotation += 360f * (moveSpeed / 10f) * Time.deltaTime;
            
            meshParticleSystem.UpdateQuad(quadIndex, position, rotation, quadSize, true, shellUVIndexInt, Color.white);

            float slowDownFactor = 3.5f;
            moveSpeed -= moveSpeed * slowDownFactor * Time.deltaTime;
        }

        public bool IsMovementComplete()
        {
            return moveSpeed < 0.1f;
        }
    }

    public enum ShellUVIndex
    {
        Shotgun,
        Mp5,
        Ak,
        M4
    }
}
