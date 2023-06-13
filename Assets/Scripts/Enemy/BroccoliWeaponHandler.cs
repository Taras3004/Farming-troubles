using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroccoliWeaponHandler : EnemyWeaponHandler
{
    [SerializeField] private LineRenderer lineOfSight;

    private float amplitude = 20;
    private float currAngle;

    //private float rotationSpeed = 35;
    private float visionDistance = 5;

    private void Start()
    {
        lineOfSight.useWorldSpace = false;
        currAngle = amplitude;
    }
    private void Update()
    {
        HandleAngleChange();
    }
    private void HandleAngleChange()
    {
        //float factor = 10f;

        if (currAngle >= amplitude)
        {

        }
    }
    private IEnumerator DecreaseValue()
    {
        float factor = 10f;
        currAngle -= Time.deltaTime * factor;
        yield return new WaitUntil(() => currAngle <= -amplitude);
        Debug.Log("Done!");
    }
    protected override bool CanShootTarget()
    {
        //lineOfSight.SetPosition(0, transform.position);

        lineOfSight.transform.rotation = new Quaternion(0, 0, currAngle, 0);
        
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, visionDistance);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.TryGetComponent(out PlayerHealth playerHealth))
            {
                return true;
            }
        }
        else
        {
            lineOfSight.SetPosition(1, transform.position + transform.right * visionDistance);
        }
        return false;
    }
}
