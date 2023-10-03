using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    [SerializeField] public float runSpeedRate;
    private Vector2 _curMovementInput;
    public float jumpForce;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;

    public float minXLook;
    public float maxXLook;
    private float _camCurXRot;
    public float lookSensitivity;

    [Header("Attack")]
    [SerializeField] private float AttackDelay;
    [SerializeField] private bool isAttackReady;
    [SerializeField] private Weapon _equipWeapon;
    
    private Vector2 _mouseDelta;

    [HideInInspector]
    public bool canLook = true;

    private Rigidbody _rigidbody;
    private PlayerConditions _conditions;
    private Animator _animator;
    
    private static PlayerController Instance;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _rigidbody = GetComponent<Rigidbody>();
        _conditions = GetComponent<PlayerConditions>();
    }

    private void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
        Attack();
    }


    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * _curMovementInput.y + transform.right * _curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;
        _rigidbody.velocity = dir;
    }
    private void CameraLook()
    {
        _camCurXRot += _mouseDelta.y * lookSensitivity;
        _camCurXRot = Mathf.Clamp(_camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-_camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, _mouseDelta.x * lookSensitivity, 0);
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        _mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _curMovementInput = context.ReadValue<Vector2>();
            _animator.SetBool("Move", true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _curMovementInput = Vector2.zero;
            _animator.SetBool("Move", false);
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (IsGrounded())
            {
                _animator.SetBool("Run", false);
                _animator.SetBool("Move", false);
                _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    public void OnRunInput(InputAction.CallbackContext context)
    {
        if (_conditions.stamina.curValue > _conditions.stamina.decayRage
            && context.phase == InputActionPhase.Started)
        {
            _animator.SetBool("Run", true);
            moveSpeed *= runSpeedRate;
            _conditions.useRunStamina = true;
        }
        else if (_conditions.useRunStamina
                 && context.phase == InputActionPhase.Canceled)
        {
            _animator.SetBool("Run", false);
            moveSpeed /= runSpeedRate;
            _conditions.useRunStamina = false;
        }
    }
    
    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + transform.forward * 0.2f + Vector3.up * 0.01f, Vector3.down),
            new Ray(transform.position - transform.forward * 0.2f + Vector3.up * 0.01f, Vector3.down),
            new Ray(transform.position + transform.right * 0.2f + Vector3.up * 0.01f, Vector3.down),
            new Ray(transform.position - transform.forward * 0.2f + Vector3.up * 0.01f, Vector3.down)
        };
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.3f,groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + (transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (transform.right * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.2f), Vector3.down);
    }
    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    public void Attack()
    {
        if (_equipWeapon == null)
        {
            return;
        }

        AttackDelay += Time.deltaTime;
        isAttackReady = _equipWeapon.rate < AttackDelay;
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (_conditions.stamina.curValue >= _conditions.attackStamina && context.phase == InputActionPhase.Started)
        {
            _equipWeapon.Use();
            _animator.SetTrigger("Attack");
            AttackDelay = 0;
            _conditions.UseStamina(_conditions.attackStamina);
        }
    }
    
}
