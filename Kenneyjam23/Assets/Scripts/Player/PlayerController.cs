using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private Animator _animator = null;

    [SerializeField] private LayerMask _aimableMask;

    private CharacterController _cc = null;

    private Vector2 _inputVec = Vector2.zero;
    private bool _hasGun = false;

    private float _speed = 7f;

    private float _gravity = 0;

    [SerializeField] private float _rotationSpeed = 5f;

    [SerializeField] private float _cameraRotSpeed = 10f;

    private float _cameraRotation = 0;
    private bool _canRotate = false;
    private float _cameraRotDir;

    public float CameraRotation { get { return _cameraRotation; } }

    public UnityEvent<bool> WeaponChange;

    /// <summary>
    /// Code for moving the player based on input from the keyboard
    /// </summary>

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _cc = GetComponent<CharacterController>();

        _gravity = Physics.gravity.y;
    }

    private void LateUpdate()
    {
        Vector3 movementDir = Camera.main.transform.TransformDirection(_inputVec);
        movementDir = new Vector3(movementDir.x, 0f, movementDir.z).normalized;

        Vector3 movementVec = new Vector3(movementDir.x * _speed, _gravity, movementDir.z * _speed);

        _cc?.Move(movementVec * Time.deltaTime);

        RotatePlayer();

        RotateCamera();

        _animator?.SetBool("IsMoving", _inputVec != Vector2.zero);
        _animator?.SetBool("HasGun", _hasGun);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        _inputVec = context.ReadValue<Vector2>();
    }

    public void OnShift(InputAction.CallbackContext context)
    {
        _hasGun = context.ReadValueAsButton();

        if(context.performed || context.canceled)
        {
            WeaponChange.Invoke(_hasGun);
        }
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        _cameraRotDir = context.ReadValue<float>();

        _canRotate = context.performed;
    }

    private void RotatePlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) == false) return;

        Vector3 worldMousePos = hit.point;


        Vector2 dir = new Vector2(worldMousePos.x - transform.position.x, worldMousePos.z - transform.position.z);

        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void RotateCamera()
    {
        if(_canRotate)
        {
            _cameraRotation += _cameraRotDir * Time.deltaTime * _cameraRotSpeed;

            _cameraRotation = (_cameraRotation + 360) % 360;
        }
    }
}
