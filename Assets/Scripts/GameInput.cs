using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnLookLeft;
    public event EventHandler OnLookRight;

    public event EventHandler OnFireStartAction;
    public event EventHandler OnFireFinishAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnDashAction;
    public event EventHandler OnDropWeaponStartAction;
    public event EventHandler OnDropWeaponFinishAction;
    public event EventHandler OnPickupWeaponAction;
    public event EventHandler OnReloadWeaponAction;
    public event EventHandler OnRespawnAction;

    private PlayerInputActions playerInputActions;
    private float aimAngle;
    private Camera cam;

    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();

        playerInputActions.Enable();

        EnablePlayerActions();
    }

    private void Start()
    {
        cam = Camera.main;
    }


    private void OnDestroy()
    {
        DisablePlayerActions();

        playerInputActions.Dispose();
    }

    private void Update()
    {
        CheckCursorPosition();
    }

    public void EnablePlayerActions()
    {
        playerInputActions.Player.Fire.performed += Fire_performed;
        playerInputActions.Player.Fire.canceled += Fire_canceled;
        playerInputActions.Player.Pause.performed += Pause_performed;
        playerInputActions.Player.Dash.performed += Dash_performed;
        playerInputActions.Player.DropWeapon.performed += DropWeapon_performed;
        playerInputActions.Player.DropWeapon.canceled += DropWeapon_canceled;
        playerInputActions.Player.PickupWeapon.performed += PickupWeapon_performed;
        playerInputActions.Player.ReloadWeapon.performed += ReloadWeapon_performed;
        playerInputActions.Player.Respawn.performed += Respawn_performed;
    }

    public void DisablePlayerActions()
    {
        playerInputActions.Player.Fire.performed -= Fire_performed;
        playerInputActions.Player.Fire.canceled -= Fire_canceled;
        playerInputActions.Player.Pause.performed -= Pause_performed;
        playerInputActions.Player.Dash.performed -= Dash_performed;
        playerInputActions.Player.DropWeapon.performed -= DropWeapon_performed;
        playerInputActions.Player.DropWeapon.canceled -= DropWeapon_canceled;
        playerInputActions.Player.PickupWeapon.performed -= PickupWeapon_performed;
        playerInputActions.Player.ReloadWeapon.performed -= ReloadWeapon_performed;
        playerInputActions.Player.Respawn.performed -= Respawn_performed;
    }

    #region InputActions

    private void Dash_performed(InputAction.CallbackContext obj)
    {
        OnDashAction?.Invoke(this, EventArgs.Empty);
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void Fire_performed(InputAction.CallbackContext obj)
    {
        OnFireStartAction?.Invoke(this, EventArgs.Empty);
    }

    private void Fire_canceled(InputAction.CallbackContext obj)
    {
        OnFireFinishAction?.Invoke(this, EventArgs.Empty);
    }

    private void PickupWeapon_performed(InputAction.CallbackContext obj)
    {
        OnPickupWeaponAction?.Invoke(this, EventArgs.Empty);
    }

    private void DropWeapon_canceled(InputAction.CallbackContext obj)
    {
        OnDropWeaponFinishAction?.Invoke(this, EventArgs.Empty);
    }

    private void DropWeapon_performed(InputAction.CallbackContext obj)
    {
        OnDropWeaponStartAction?.Invoke(this, EventArgs.Empty);
    }

    private void ReloadWeapon_performed(InputAction.CallbackContext obj)
    {
        OnReloadWeaponAction?.Invoke(this, EventArgs.Empty);
    }

    private void Respawn_performed(InputAction.CallbackContext obj)
    {
        OnRespawnAction?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    public Vector2 GetPlayerMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    private void CheckCursorPosition()
    {
        if (cam == null) 
            return;

        PlayerWeaponHandler player = PlayerMovement.Instance.GetComponent<PlayerWeaponHandler>();

        Vector3 aimCenterPosition = new Vector3(player.transform.position.x,
            player.GetWeaponSpawnTransform().transform.position.y);
        
        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 aimDirection = (mousePosition - aimCenterPosition).normalized;
        aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if (aimAngle > 90 || aimAngle < -90)
        {
            OnLookLeft?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnLookRight?.Invoke(this, EventArgs.Empty);
        }
    }
    public float GetAimDirectionAngle()
    {
        return aimAngle;
    }
    public Vector2 GetAimDirectionVector()
    {
        Vector2 dir = new Vector2
            (Mathf.Sin((aimAngle + 90) * Mathf.Deg2Rad), -Mathf.Cos((aimAngle + 90) * Mathf.Deg2Rad));
        return dir;
    }
}
