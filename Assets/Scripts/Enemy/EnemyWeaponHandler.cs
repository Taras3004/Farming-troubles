using Pathfinding;
using UnityEngine;

public class EnemyWeaponHandler : MonoBehaviour
{
    [SerializeField] private Transform weaponObject;
    [SerializeField] private EnemyWeapon weapon;
    [SerializeField] private float weaponOffset = 0.2f;

    private EnemyPathfinder enemyPathfinder;
    private EnemyHealthHandler enemyHealth;
    private Transform target;
    private bool canShoot = true;

    private float aimAngle;

    public Vector2 DirToTarget()
    {
        return target.position - transform.position;
    }
    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealthHandler>();
        enemyPathfinder = GetComponent<EnemyPathfinder>();
    }
    private void Start()
    {
        target = enemyPathfinder.Target();
        enemyHealth.OnDieEvent += EnemyHealth_OnDieEvent;
    }

    private void EnemyHealth_OnDieEvent(object sender, System.EventArgs e)
    {
        canShoot = false;
    }

    private void Update()
    {
        if (canShoot == false || target == null) 
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
        bool isReachedEndDistance = enemyPathfinder.IsReachedEndDistance();

        if (isReachedEndDistance == false)
            return false;

        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, DirToTarget(), Mathf.Infinity, enemyPathfinder.ObstaclesLayer());
        if (raycastHit.collider == null)
            return false;
        if (raycastHit.collider.gameObject == target.gameObject)
        {
            return true;
        }
        else
        {
            return false;
        }
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
