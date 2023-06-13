using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    public Transform target;
    //private float velocity;
    private void Update()
    {
        Vector3 aimDirection = Vector2.Lerp(transform.position ,target.position - transform.position, 200f);
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        //float lerp = Mathf.SmoothDamp(transform.eulerAngles.z, angle, ref velocity, 0.2f);
        //float lerp = Mathf.Lerp(transform.eulerAngles.z, angle, 5f * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, 0, angle);
        //Debug.Log(angle + " " + transform.eulerAngles.z + " " + lerp);
    }
    /*
    Vector3 aimDirection = Player.Instance.transform.position - transform.position;
    float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
    float angle = Vector3.Angle(aimDirection, transform.forward);
    transform.rotation = Quaternion.LookRotation();
    Debug.Log(angle);
    float lerp = Mathf.Lerp(rr.eulerAngles.z, aimAngle, 5f * Time.deltaTime);
    rr.eulerAngles = new Vector3(0, 0, lerp);
    Debug.Log(aimAngle + " " + lerp);
    */
}
