using System;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnLookLeft;
    public event EventHandler OnLookRight;

    public event EventHandler OnFireStartAction;
    public event EventHandler OnFireFinishAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnJerkAction;
    public event EventHandler OnDropWeaponStartAction;
    public event EventHandler OnDropWeaponFinishAction;
    public event EventHandler OnPickupWeaponAction;
    public event EventHandler OnReloadWeaponAction;

    private PlayerInputActions playerInputActions;
    private float aimAngle;

    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();

        playerInputActions.Enable();

        playerInputActions.Player.Fire.performed += Fire_performed;
        playerInputActions.Player.Fire.canceled += Fire_canceled;
        playerInputActions.Player.Pause.performed += Pause_performed;
        playerInputActions.Player.Jerk.performed += Jerk_performed;
        playerInputActions.Player.DropWeapon.performed += DropWeapon_performed;
        playerInputActions.Player.DropWeapon.canceled += DropWeapon_canceled;
        playerInputActions.Player.PickupWeapon.performed += PickupWeapon_performed;
        playerInputActions.Player.ReloadWeapon.performed += ReloadWeapon_performed;
    }



    private void OnDestroy()
    {
        playerInputActions.Player.Fire.performed -= Fire_performed;
        playerInputActions.Player.Fire.canceled -= Fire_canceled;
        playerInputActions.Player.Pause.performed -= Pause_performed;
        playerInputActions.Player.Jerk.performed -= Jerk_performed;
        playerInputActions.Player.DropWeapon.performed -= DropWeapon_performed;
        playerInputActions.Player.DropWeapon.canceled -= DropWeapon_canceled;
        playerInputActions.Player.PickupWeapon.performed -= PickupWeapon_performed;
        playerInputActions.Player.ReloadWeapon.performed -= ReloadWeapon_performed;

        playerInputActions.Dispose();
    }
    private void Update()
    {
        CheckCursorPosition();
    }
    #region InputActions
    private void Jerk_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJerkAction?.Invoke(this, EventArgs.Empty);
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void Fire_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnFireStartAction?.Invoke(this, EventArgs.Empty);
    }

    private void Fire_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnFireFinishAction?.Invoke(this, EventArgs.Empty);
    }

    private void PickupWeapon_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPickupWeaponAction?.Invoke(this, EventArgs.Empty);
    }

    private void DropWeapon_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnDropWeaponFinishAction?.Invoke(this, EventArgs.Empty);
    }

    private void DropWeapon_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnDropWeaponStartAction?.Invoke(this, EventArgs.Empty);
    }
    private void ReloadWeapon_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnReloadWeaponAction?.Invoke(this, EventArgs.Empty);
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
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 aimDirection = (mousePosition - Player.Instance.transform.position).normalized;
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
