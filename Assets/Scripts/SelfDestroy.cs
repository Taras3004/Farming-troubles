using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public void Setup(float destroyTime)
    {
        Destroy(gameObject, destroyTime);
    }
}
