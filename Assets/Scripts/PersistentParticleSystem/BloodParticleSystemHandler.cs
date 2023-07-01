using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class BloodParticleSystemHandler : MonoBehaviour
{
    public static BloodParticleSystemHandler Instance { get; private set; }

    private MeshParticleSystem meshParticleSystem;
    private List<Single> singleList;

    private void Awake() {
        Instance = this;
        meshParticleSystem = GetComponent<MeshParticleSystem>();
        singleList = new List<Single>();
    }

    private void Update() {
        foreach (Single singleShell in singleList)
        {
            singleShell.Update();
            if (singleShell.IsParticleComplete())
            {
                singleList.Remove(singleShell);
                break;
            }
        }
    }

    public void SpawnBlood(Vector3 position, Vector3 direction) {
        float bloodParticleCount = 3;
        for (int i = 0; i < bloodParticleCount; i++) {
            singleList.Add(new Single(position, UtilsClass.ApplyRotationToVector(direction, Random.Range(-15f, 15f)), meshParticleSystem));
        }
    }
    
    private class Single {

        private MeshParticleSystem meshParticleSystem;
        private Vector3 position;
        private Vector3 direction;
        private int quadIndex;
        private Vector3 quadSize;
        private float moveSpeed;
        private float rotation;
        private int uvIndex;

        public Single(Vector3 position, Vector3 direction, MeshParticleSystem meshParticleSystem) {
            this.position = position;
            this.direction = direction;
            this.meshParticleSystem = meshParticleSystem;

            quadSize = new Vector3(0.2f, 0.2f);
            rotation = Random.Range(0, 360f);
            moveSpeed = Random.Range(5f, 7f);
            uvIndex = Random.Range(0, 8);

            quadIndex = meshParticleSystem.AddQuad(position, rotation, quadSize, false, uvIndex);
        }

        public void Update() {
            position += direction * moveSpeed * Time.deltaTime;
            rotation += 360f * (moveSpeed / 10f) * Time.deltaTime;

            meshParticleSystem.UpdateQuad(quadIndex, position, rotation, quadSize, false, uvIndex);

            float slowDownFactor = 3.5f;
            moveSpeed -= moveSpeed * slowDownFactor * Time.deltaTime;
        }

        public bool IsParticleComplete() {
            return moveSpeed < .1f;
        }

    }
}
