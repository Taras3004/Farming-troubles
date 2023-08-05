using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public virtual void Interact()
    {
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}