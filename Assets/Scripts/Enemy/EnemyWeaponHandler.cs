using System;
using Pathfinding;
using UnityEngine;

public class EnemyWeaponHandler : MonoBehaviour
{
    [SerializeField] private Transform weaponObject;
    [SerializeField] private EnemyWeapon weapon;
    [SerializeField] private float weaponOffset = 0.2f;

    private EnemyBehaviourHandler enemyBehaviourHandler;
    private EnemyHealthHandler enemyHealth;
    private Transform target;
    
    private bool isDied = false;
    private bool isPlayerFinded = false;
    private float aimAngle;

    private bool CanShoot()
    {
        return isDied == false && isPlayerFinded == true && target != null;
    }
    public Vector2 DirToTarget()
    {
        return target.position - transform.position;
    }
    private void Awake()
    {
        weaponObject.gameObject.SetActive(false);
        enemyHealth = GetComponent<EnemyHealthHandler>();
        enemyBehaviourHandler = GetComponent<EnemyBehaviourHandler>();
    }
    private void Start()
    {
        target = enemyBehaviourHandler.Target();
        enemyHealth.OnDieEvent += EnemyHealth_OnDieEvent;
        enemyBehaviourHandler.OnFindPlayerAction += EnemyBehaviourHandlerOnOnFindPlayerAction;
    }

    private void EnemyBehaviourHandlerOnOnFindPlayerAction(object sender, EventArgs e)
    {
        weaponObject.gameObject.SetActive(true);
        isPlayerFinded = true;
    }

    private void EnemyHealth_OnDieEvent(object sender, System.EventArgs e)
    {
        isDied = true;
    }

    private void Update()
    {
        if (CanShoot() == false) 
            return;

        HandleShooting();
        HandleWeaponPosition();
        HandleWeaponAim();
    }
    private void HandleShooting()
    {
        if (CanShootTarget() == true)
        {
            Shoot();
        }
    }
    protected virtual bool CanShootTarget()
    {
        bool isReachedEndDistance = enemyBehaviourHandler.IsReachedEndDistance();

        if (isReachedEndDistance == false)
            return false;

        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, DirToTarget(), Mathf.Infinity, enemyBehaviourHandler.ObstaclesLayer());
        if (raycastHit.collider == null)
            return false;
        if (raycastHit.collider.gameObject == target.gameObject)
        {
            return true;
        }

        return false;
    }
    private void HandleWeaponPosition()
    {
        if (target.transform.position.x >= transform.position.x)
        {
            weaponObject.localScale = new Vector3(1f, 1f, 1f);

            float weaponPositionOffsetRight = weaponOffset;
            weaponObject.localPosition = new Vector2(weaponPositionOffsetRight, weaponObject.localPosition.y);
        }
        else if (target.transform.position.x <= transform.position.x)
        {
            weaponObject.localScale = new Vector3(1f, -1f, 1f);

            float weaponPositionOffsetLeft = -weaponOffset;
            weaponObject.localPosition = new Vector2(weaponPositionOffsetLeft, weaponObject.localPosition.y);
        }
    }
    private void HandleWeaponAim()
    {
        weaponObject.right = target.position - transform.position;
    }
    public void Shoot()
    {
        weapon.TryShoot();
    }
}
